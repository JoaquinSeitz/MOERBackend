using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoerEstetica.API.Data;
using MoerEstetica.API.DTOs.Request;
using MoerEstetica.API.DTOs.Response;
using MoerEsteticaBackend.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MoerEstetica.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TurnosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TurnosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> CrearTurno([FromBody] TurnoCreateDTO dto)
        {
            var clienteIdClaim = User.Claims.FirstOrDefault(c => c.Type == "IdCliente")?.Value;
            if (string.IsNullOrEmpty(clienteIdClaim) || !int.TryParse(clienteIdClaim, out int idCliente))
                return Unauthorized(new { mensaje = "No se pudo identificar al cliente." });

            bool inhabilitado = await _context.Horarios_Inhabilitados
                .AnyAsync(h => h.Fecha.Date == dto.Fecha.Date
                            && h.HoraInicio <= dto.HoraInicio
                            && h.HoraFin > dto.HoraInicio);
            if (inhabilitado)
                return BadRequest(new { mensaje = "El horario seleccionado está inhabilitado." });

            bool ocupado = await _context.Turnos
                .AnyAsync(t => t.Fecha.Date == dto.Fecha.Date
                            && t.HoraInicio == dto.HoraInicio
                            && t.IdEstadoTurno != 3);
            if (ocupado)
                return BadRequest(new { mensaje = "El horario seleccionado ya está ocupado." });

            var turno = new Turno
            {
                Fecha = dto.Fecha.Date,
                HoraInicio = dto.HoraInicio,
                IdCliente = idCliente,
                IdServicio = dto.IdServicio,
                IdEstadoTurno = 1
            };

            _context.Turnos.Add(turno);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Turno agendado con éxito.", idTurno = turno.IdTurno });
        }

        [HttpGet("mis-turnos")]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> GetMisTurnos()
        {
            var clienteIdClaim = User.Claims.FirstOrDefault(c => c.Type == "IdCliente")?.Value;
            if (string.IsNullOrEmpty(clienteIdClaim) || !int.TryParse(clienteIdClaim, out int idCliente))
                return Unauthorized(new { mensaje = "No se pudo identificar al cliente." });

            var turnos = await _context.Turnos
                .Include(t => t.Servicio)
                .Include(t => t.Estado_Turno)
                .Where(t => t.IdCliente == idCliente)
                .OrderByDescending(t => t.Fecha).ThenBy(t => t.HoraInicio)
                .Select(t => new TurnoResponseDTO
                {
                    IdTurno = t.IdTurno,
                    Fecha = t.Fecha,
                    HoraInicio = t.HoraInicio,
                    Estado = t.Estado_Turno.Nombre,
                    Servicio = t.Servicio.Nombre,
                    Precio = t.Servicio.Precio
                }).ToListAsync();

            return Ok(turnos);
        }

        [HttpGet("agenda")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetAgenda([FromQuery] DateTime? fecha)
        {
            var queryFecha = fecha?.Date ?? DateTime.Today;

            var turnos = await _context.Turnos
                .Include(t => t.Servicio)
                .Include(t => t.Estado_Turno)
                .Include(t => t.Cliente)
                .Where(t => t.Fecha.Date == queryFecha)
                .OrderBy(t => t.HoraInicio)
                .Select(t => new TurnoResponseDTO
                {
                    IdTurno = t.IdTurno,
                    Fecha = t.Fecha,
                    HoraInicio = t.HoraInicio,
                    Estado = t.Estado_Turno.Nombre,
                    Servicio = t.Servicio.Nombre,
                    Precio = t.Servicio.Precio,
                    ClienteNombre = $"{t.Cliente.Nombre} {t.Cliente.Apellido}",
                    ClienteTelefono = t.Cliente.Telefono
                }).ToListAsync();

            return Ok(turnos);
        }

        [HttpPatch("{id}/estado")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ActualizarEstadoTurno(int id, [FromBody] TurnoEstadoUpdateDTO dto)
        {
            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null) return NotFound(new { mensaje = "Turno no encontrado." });

            turno.IdEstadoTurno = dto.IdEstadoTurno;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Estado del turno actualizado." });
        }

        [HttpPatch("{id}/cancelar")]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> CancelarMiTurno(int id)
        {
            var clienteIdClaim = User.Claims.FirstOrDefault(c => c.Type == "IdCliente")?.Value;
            if (string.IsNullOrEmpty(clienteIdClaim) || !int.TryParse(clienteIdClaim, out int idCliente))
                return Unauthorized();

            var turno = await _context.Turnos.FirstOrDefaultAsync(t => t.IdTurno == id && t.IdCliente == idCliente);
            if (turno == null) return NotFound(new { mensaje = "Turno no encontrado." });

            if (turno.IdEstadoTurno != 1)
                return BadRequest(new { mensaje = "Solo se pueden cancelar turnos en estado pendiente." });

            turno.IdEstadoTurno = 3;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Turno cancelado exitosamente." });
        }
    }
}