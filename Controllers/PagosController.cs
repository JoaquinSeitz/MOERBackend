using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoerEstetica.API.Data;
using MoerEstetica.API.DTOs.Request;
using MoerEsteticaBackend.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MoerEstetica.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrador")]
    public class PagosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PagosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarPago([FromBody] PagoCreateDTO dto)
        {
            if (dto.IdPedido == null && dto.IdTurno == null)
                return BadRequest(new { mensaje = "Debe asociar el pago a un pedido o a un turno." });

            decimal montoTotal = 0;

            if (dto.IdPedido.HasValue)
            {
                var pedido = await _context.Pedidos
                    .Include(p => p.Detalles)
                    .FirstOrDefaultAsync(p => p.IdPedido == dto.IdPedido.Value);

                if (pedido == null) return NotFound(new { mensaje = "Pedido no encontrado." });

                montoTotal += pedido.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario);
                pedido.IdEstadoPedido = 3;
            }

            if (dto.IdTurno.HasValue)
            {
                var turno = await _context.Turnos
                    .Include(t => t.Servicio)
                    .FirstOrDefaultAsync(t => t.IdTurno == dto.IdTurno.Value);

                if (turno == null) return NotFound(new { mensaje = "Turno no encontrado." });

                montoTotal += turno.Servicio.Precio;
                turno.IdEstadoTurno = 2;
            }

            var pago = new Pago
            {
                Fecha = DateTime.Now,
                MontoTotal = montoTotal,
                IdMetodoPago = dto.IdMetodoPago,
                IdPedido = dto.IdPedido,
                IdTurno = dto.IdTurno
            };

            _context.Pagos.Add(pago);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Pago registrado exitosamente.", idPago = pago.IdPago, monto = montoTotal });
        }

        [HttpGet("metodos")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMetodosPago()
        {
            var metodos = await _context.Metodos_Pago
                .Where(m => m.Activo)
                .Select(m => new { m.IdMetodoPago, m.Nombre })
                .ToListAsync();

            return Ok(metodos);
        }
    }
}