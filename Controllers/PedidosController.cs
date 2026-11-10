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
    public class PedidosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PedidosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> CrearPedido([FromBody] PedidoCreateDTO dto)
        {
            var clienteIdClaim = User.Claims.FirstOrDefault(c => c.Type == "IdCliente")?.Value;
            if (string.IsNullOrEmpty(clienteIdClaim) || !int.TryParse(clienteIdClaim, out int idCliente))
                return Unauthorized(new { mensaje = "No se pudo identificar al cliente." });

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var pedido = new Pedido
                {
                    IdCliente = idCliente,
                    Fecha = DateTime.Now,
                    IdEstadoPedido = 1
                };
                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();

                foreach (var item in dto.Detalles)
                {
                    var producto = await _context.Productos.FindAsync(item.IdProducto);
                    if (producto == null || !producto.Activo)
                        throw new Exception($"El producto con ID {item.IdProducto} no existe o no está activo.");

                    if (producto.StockActual < item.Cantidad)
                        throw new Exception($"Stock insuficiente para el producto {producto.Nombre}. Disponible: {producto.StockActual}.");

                    var detalle = new Detalle_Pedido
                    {
                        IdPedido = pedido.IdPedido,
                        IdProducto = item.IdProducto,
                        Cantidad = item.Cantidad,
                        PrecioUnitario = producto.Precio
                    };
                    _context.Detalles_Pedidos.Add(detalle);
                    producto.StockActual -= item.Cantidad;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { mensaje = "Pedido generado con éxito.", idPedido = pedido.IdPedido });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("mis-pedidos")]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> GetMisPedidos()
        {
            var clienteIdClaim = User.Claims.FirstOrDefault(c => c.Type == "IdCliente")?.Value;
            if (string.IsNullOrEmpty(clienteIdClaim) || !int.TryParse(clienteIdClaim, out int idCliente))
                return Unauthorized();

            var pedidos = await _context.Pedidos
                .Include(p => p.Estado_Pedido)
                .Include(p => p.Detalles)
                    .ThenInclude(dp => dp.Producto)
                .Where(p => p.IdCliente == idCliente)
                .OrderByDescending(p => p.Fecha)
                .Select(p => new PedidoResponseDTO
                {
                    IdPedido = p.IdPedido,
                    Fecha = p.Fecha,
                    Estado = p.Estado_Pedido.Nombre,
                    Total = p.Detalles.Sum(dp => dp.Cantidad * dp.PrecioUnitario),
                    Detalles = p.Detalles.Select(dp => new DetallePedidoResponseDTO
                    {
                        Producto = dp.Producto.Nombre,
                        Cantidad = dp.Cantidad,
                        PrecioUnitario = dp.PrecioUnitario
                    }).ToList()
                }).ToListAsync();

            return Ok(pedidos);
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetAllPedidos()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Estado_Pedido)
                .Include(p => p.Cliente)
                .Include(p => p.Detalles)
                    .ThenInclude(dp => dp.Producto)
                .OrderByDescending(p => p.Fecha)
                .Select(p => new PedidoResponseDTO
                {
                    IdPedido = p.IdPedido,
                    Fecha = p.Fecha,
                    Estado = p.Estado_Pedido.Nombre,
                    ClienteNombre = $"{p.Cliente.Nombre} {p.Cliente.Apellido}",
                    ClienteTelefono = p.Cliente.Telefono,
                    Total = p.Detalles.Sum(dp => dp.Cantidad * dp.PrecioUnitario),
                    Detalles = p.Detalles.Select(dp => new DetallePedidoResponseDTO
                    {
                        Producto = dp.Producto.Nombre,
                        Cantidad = dp.Cantidad,
                        PrecioUnitario = dp.PrecioUnitario
                    }).ToList()
                }).ToListAsync();

            return Ok(pedidos);
        }

        [HttpPatch("{id}/estado")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ActualizarEstadoPedido(int id, [FromBody] PedidoEstadoUpdateDTO dto)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null) return NotFound(new { mensaje = "Pedido no encontrado." });

            pedido.IdEstadoPedido = dto.IdEstadoPedido;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Estado del pedido actualizado." });
        }
    }
}