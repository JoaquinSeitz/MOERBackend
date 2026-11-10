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
    [Authorize(Roles = "Administrador")]
    public class IngresoProductosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public IngresoProductosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetIngresos([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var ingresos = await _context.Ingresos_Stock
                    .Include(i => i.Detalles)
                        .ThenInclude(d => d.Producto)
                    .OrderByDescending(i => i.Fecha)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(i => new IngresoProductoResponseDTO
                    {
                        IdIngreso = i.IdIngreso,
                        Fecha = i.Fecha,
                        Detalles = i.Detalles.Select(d => new DetalleIngresoResponseDTO
                        {
                            IdProducto = d.IdProducto,
                            ProductoNombre = d.Producto != null ? d.Producto.Nombre : "Desconocido",
                            Cantidad = d.Cantidad,
                            PrecioCompraUnitario = d.PrecioCompraUnitario
                        }).ToList()
                    })
                    .ToListAsync();

                return Ok(ingresos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error recuperando el historial de ingresos.", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetIngreso(int id)
        {
            try
            {
                var ingreso = await _context.Ingresos_Stock
                    .Include(i => i.Detalles)
                        .ThenInclude(d => d.Producto)
                    .FirstOrDefaultAsync(i => i.IdIngreso == id);

                if (ingreso == null) return NotFound(new { mensaje = "Ingreso no encontrado." });

                var response = new IngresoProductoResponseDTO
                {
                    IdIngreso = ingreso.IdIngreso,
                    Fecha = ingreso.Fecha,
                    Detalles = ingreso.Detalles.Select(d => new DetalleIngresoResponseDTO
                    {
                        IdProducto = d.IdProducto,
                        ProductoNombre = d.Producto != null ? d.Producto.Nombre : "Desconocido",
                        Cantidad = d.Cantidad,
                        PrecioCompraUnitario = d.PrecioCompraUnitario
                    }).ToList()
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error recuperando el ingreso.", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarIngreso([FromBody] IngresoProductoRequestDTO dto)
        {
            if (dto == null || dto.Detalles == null || !dto.Detalles.Any())
            {
                return BadRequest(new { mensaje = "El ingreso debe contener al menos un producto en el detalle." });
            }

            // Usamos una transacción para asegurar que el stock y el historial se guarden juntos o fallen juntos
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var nuevoIngreso = new Ingreso_Stock
                {
                    Fecha = DateTime.Now,
                    Detalles = new List<Detalle_Ingreso>()
                };

                foreach (var item in dto.Detalles)
                {
                    var producto = await _context.Productos.FindAsync(item.IdProducto);
                    if (producto == null || !producto.Activo)
                        return NotFound(new { mensaje = $"Producto con ID {item.IdProducto} no encontrado o inactivo." });

                    // Aumentamos el stock actual del producto en el catálogo
                    producto.StockActual += item.Cantidad;

                    // Agregamos el detalle al historial de ingresos
                    nuevoIngreso.Detalles.Add(new Detalle_Ingreso
                    {
                        IdProducto = item.IdProducto,
                        Cantidad = item.Cantidad,
                        PrecioCompraUnitario = item.PrecioCompraUnitario
                    });
                }

                _context.Ingresos_Stock.Add(nuevoIngreso);

                // SaveChangesAsync guarda el ingreso, sus detalles, y los updates a la tabla Producto simultáneamente
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return Ok(new { mensaje = "Ingreso registrado y stock actualizado exitosamente.", IdIngreso = nuevoIngreso.IdIngreso });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error registrando el ingreso de stock.", error = ex.Message });
            }
        }
    }
}