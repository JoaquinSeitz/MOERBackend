using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoerEstetica.API.Data;
using MoerEsteticaBackend.DTOs.Request;
using MoerEsteticaBackend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MoerEsteticaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public UsuariosController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")] // Asegurate de que el nombre del rol coincida con tu tabla Rol
        public async Task<IActionResult> ObtenerUsuarios()
        {
            try
            {
                // Usamos un tipo anónimo en lugar de UsuarioResponseDTO por ahora para simplificar
                var usuarios = await _context.Usuarios
                    .Include(u => u.Rol)
                    .Select(u => new
                    {
                        u.IdUsuario,
                        u.Email,
                        Rol = u.Rol != null ? u.Rol.Nombre : "Sin Rol"
                    })
                    .ToListAsync();

                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error recuperando los usuarios.", error = ex.Message });
            }
        }

        [HttpPost("registro")]
        [AllowAnonymous]
        public async Task<IActionResult> Registrar([FromBody] RegistroUsuarioDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Contrasenia))
                return BadRequest(new { mensaje = "Datos de registro incompletos." });

            try
            {
                if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
                    return BadRequest(new { mensaje = "El email ya está registrado." });

                // 1. Asignamos por defecto el rol "Cliente". Debe existir previamente en la tabla Rol (Id = 2 por ej.)
                var rolCliente = await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == "Cliente");
                if (rolCliente == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "El rol 'Cliente' no existe en la BD. Ejecuta el Seeding." });

                // 2. Alta de Usuario (Credenciales)
                var nuevoUsuario = new Usuario
                {
                    Email = dto.Email,
                    Contrasenia = dto.Contrasenia,
                    IdRol = rolCliente.IdRol
                };

                _context.Usuarios.Add(nuevoUsuario);
                await _context.SaveChangesAsync(); // Se genera el IdUsuario

                // 3. Alta de Cliente (Datos Personales)
                var nuevoCliente = new Cliente
                {
                    Nombre = dto.Nombre,
                    Apellido = dto.Apellido,
                    Telefono = dto.Telefono,
                    IdUsuario = nuevoUsuario.IdUsuario
                };

                _context.Clientes.Add(nuevoCliente);
                await _context.SaveChangesAsync();

                return Ok(new { mensaje = "Usuario registrado exitosamente.", idUsuario = nuevoUsuario.IdUsuario, idCliente = nuevoCliente.IdCliente });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error al registrar el usuario.", error = ex.Message });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            if (!ModelState.IsValid)
            {
                var errores = string.Join(" ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(new { mensaje = errores });
            }

            try
            {
                var usuario = await _context.Usuarios
                    .Include(u => u.Rol)
                    .FirstOrDefaultAsync(u => u.Email == login.Email);

                if (usuario == null || usuario.Contrasenia != login.Contrasenia)
                {
                    return Unauthorized(new { mensaje = "El correo electrónico o la contraseña son incorrectos." });
                }

                // Buscamos el Cliente asociado para enviarle el IdCliente al frontend
                var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.IdUsuario == usuario.IdUsuario);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim(ClaimTypes.Role, usuario.Rol?.Nombre ?? "Cliente")
                };

                if (cliente != null)
                {
                    claims.Add(new Claim("IdCliente", cliente.IdCliente.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, $"{cliente.Nombre} {cliente.Apellido}"));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(2),
                    signingCredentials: creds
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new
                {
                    token = tokenString,
                    idUsuario = usuario.IdUsuario,
                    idCliente = cliente?.IdCliente,
                    email = usuario.Email,
                    rol = usuario.Rol?.Nombre
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error interno del servidor.", error = ex.Message });
            }
        }
    }
}