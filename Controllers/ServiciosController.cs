using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoerEstetica.API.Data;
using MoerEsteticaBackend.DTOs.Request;
using MoerEsteticaBackend.DTOs.Response;
using MoerEsteticaBackend.Models;

namespace MoerEsteticaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiciosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ServiciosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetServicios()
        {
            try
            {
                var servicios = await _context.Servicios
                    .Include(s => s.Categoria_Servicio)
                    .Select(s => new ServicioResponseDTO
                    {
                        IdServicio = s.IdServicio,
                        Nombre = s.Nombre,
                        Descripcion = s.Descripcion,
                        Precio = s.Precio,
                        IdCategoriaServicio = s.IdCategoriaServicio,
                        CategoriaNombre = s.Categoria_Servicio != null ? s.Categoria_Servicio.Nombre : "Sin Categoría"
                    })
                    .ToListAsync();

                return Ok(servicios);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error recuperando el catálogo de tratamientos.", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetServicio(int id)
        {
            try
            {
                var servicio = await _context.Servicios
                    .Include(s => s.Categoria_Servicio)
                    .Where(s => s.IdServicio == id)
                    .Select(s => new ServicioResponseDTO
                    {
                        IdServicio = s.IdServicio,
                        Nombre = s.Nombre,
                        Descripcion = s.Descripcion,
                        Precio = s.Precio,
                        IdCategoriaServicio = s.IdCategoriaServicio,
                        CategoriaNombre = s.Categoria_Servicio != null ? s.Categoria_Servicio.Nombre : "Sin Categoría"
                    })
                    .FirstOrDefaultAsync();

                if (servicio == null)
                    return NotFound(new { mensaje = "Tratamiento no encontrado." });

                return Ok(servicio);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error al obtener el tratamiento.", error = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CrearServicio([FromBody] ServicioCreateDTO dto)
        {
            if (dto == null) return BadRequest(new { mensaje = "Los datos del tratamiento son nulos." });

            try
            {
                var nuevoServicio = new Servicio
                {
                    Nombre = dto.Nombre,
                    Descripcion = dto.Descripcion,
                    Precio = dto.Precio,
                    IdCategoriaServicio = dto.IdCategoriaServicio
                };

                _context.Servicios.Add(nuevoServicio);
                await _context.SaveChangesAsync();

                return Ok(new { mensaje = "Tratamiento creado exitosamente.", IdServicio = nuevoServicio.IdServicio });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error al crear el tratamiento.", error = ex.InnerException?.Message ?? ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ActualizarServicio(int id, [FromBody] ServicioCreateDTO dto)
        {
            if (dto == null) return BadRequest(new { mensaje = "Los datos del tratamiento son nulos." });

            try
            {
                var servicioExistente = await _context.Servicios.FindAsync(id);
                if (servicioExistente == null)
                    return NotFound(new { mensaje = "Tratamiento no encontrado." });

                servicioExistente.Nombre = dto.Nombre;
                servicioExistente.Descripcion = dto.Descripcion;
                servicioExistente.Precio = dto.Precio;
                servicioExistente.IdCategoriaServicio = dto.IdCategoriaServicio;

                await _context.SaveChangesAsync();

                return Ok(new { mensaje = "Tratamiento actualizado exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error al actualizar el tratamiento.", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> EliminarServicio(int id)
        {
            try
            {
                var servicio = await _context.Servicios.FindAsync(id);
                if (servicio == null) return NotFound(new { mensaje = "Tratamiento no encontrado." });

                _context.Servicios.Remove(servicio);
                await _context.SaveChangesAsync();

                return Ok(new { mensaje = "Tratamiento eliminado correctamente." });
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { mensaje = "No se puede eliminar el tratamiento porque existen turnos históricos o pendientes asociados a él." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error al eliminar el tratamiento.", error = ex.Message });
            }
        }
    }
}