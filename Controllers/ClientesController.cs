using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoerEstetica.API.Data;
using MoerEsteticaBackend.Models;
using System.Security.Claims;

namespace MoerEsteticaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Clientes (Listado para el Dashboard del Administrador)
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetClientes()
        {
            try
            {
                var clientes = await _context.Clientes
                    .Select(c => new
                    {
                        c.IdCliente,
                        c.Nombre,
                        c.Apellido,
                        c.Telefono,
                        Email = c.Usuario != null ? c.Usuario.Email : "Sin Email"
                    })
                    .ToListAsync();

                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error recuperando el listado de clientes.", error = ex.Message });
            }
        }

        // GET: api/Clientes/mi-perfil (El cliente consulta sus propios datos)
        [HttpGet("mi-perfil")]
        public async Task<IActionResult> MiPerfil()
        {
            try
            {
                var idClienteClaim = User.Claims.FirstOrDefault(c => c.Type == "IdCliente")?.Value;
                if (string.IsNullOrEmpty(idClienteClaim))
                    return Unauthorized(new { mensaje = "No se pudo identificar el cliente actual." });

                int idCliente = int.Parse(idClienteClaim);

                var cliente = await _context.Clientes
                    .Include(c => c.Usuario)
                    .FirstOrDefaultAsync(c => c.IdCliente == idCliente);

                if (cliente == null) return NotFound(new { mensaje = "Cliente no encontrado." });

                return Ok(new
                {
                    cliente.IdCliente,
                    cliente.Nombre,
                    cliente.Apellido,
                    cliente.Telefono,
                    Email = cliente.Usuario?.Email
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error recuperando tu perfil.", error = ex.Message });
            }
        }

        // PUT: api/Clientes (El cliente modifica sus propios datos)
        [HttpPut]
        public async Task<IActionResult> ModificarMiPerfil([FromBody] Cliente clienteActualizado)
        {
            try
            {
                var idClienteClaim = User.Claims.FirstOrDefault(c => c.Type == "IdCliente")?.Value;
                if (string.IsNullOrEmpty(idClienteClaim))
                    return Unauthorized(new { mensaje = "No se pudo identificar el cliente actual." });

                int idCliente = int.Parse(idClienteClaim);

                var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.IdCliente == idCliente);
                if (cliente == null) return NotFound(new { mensaje = "Cliente no encontrado." });

                cliente.Nombre = clienteActualizado.Nombre;
                cliente.Apellido = clienteActualizado.Apellido;
                cliente.Telefono = clienteActualizado.Telefono;

                await _context.SaveChangesAsync();
                return Ok(new { mensaje = "Perfil actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error al modificar el perfil.", error = ex.Message });
            }
        }
    }
}