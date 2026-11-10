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
    public class ProductosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductosController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [HttpGet("admin")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetTodosLosProductosAdmin()
        {
            var productos = await _context.Productos
                 .Include(p => p.Categoria_Producto)
                 .Select(p => new ProductoResponseDTO
                 {
                     IdProducto = p.IdProducto,
                     Nombre = p.Nombre,
                     Descripcion = p.Descripcion,
                     Precio = p.Precio,
                     StockActual = p.StockActual,
                     Imagen = p.Imagen,
                     Activo = p.Activo, // Exponemos el estado
                     IdCategoriaProducto = p.IdCategoriaProducto,
                     CategoriaNombre = p.Categoria_Producto != null ? p.Categoria_Producto.Nombre : "Sin Categoría"
                 })
                 .ToListAsync();

            return Ok(productos);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductos([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var productos = await _context.Productos
                     .Include(p => p.Categoria_Producto)
                     .Where(p => p.Activo)
                     .Skip((pageNumber - 1) * pageSize)
                     .Take(pageSize)
                     .Select(p => new ProductoResponseDTO
                     {
                         IdProducto = p.IdProducto,
                         Nombre = p.Nombre,
                         Descripcion = p.Descripcion,
                         Precio = p.Precio,
                         StockActual = p.StockActual,
                         Imagen = p.Imagen,
                         IdCategoriaProducto = p.IdCategoriaProducto,
                         CategoriaNombre = p.Categoria_Producto != null ? p.Categoria_Producto.Nombre : "Sin Categoría"
                     })
                     .ToListAsync();

                return Ok(productos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error recuperando el catálogo de productos.", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProducto(int id)
        {
            try
            {
                var producto = await _context.Productos
                    .Include(p => p.Categoria_Producto)
                    .Where(p => p.IdProducto == id && p.Activo)
                    .Select(p => new ProductoResponseDTO
                    {
                        IdProducto = p.IdProducto,
                        Nombre = p.Nombre,
                        Descripcion = p.Descripcion,
                        Precio = p.Precio,
                        StockActual = p.StockActual,
                        Imagen = p.Imagen,
                        IdCategoriaProducto = p.IdCategoriaProducto,
                        CategoriaNombre = p.Categoria_Producto != null ? p.Categoria_Producto.Nombre : "Sin Categoría"
                    })
                    .FirstOrDefaultAsync();

                if (producto == null)
                    return NotFound(new { mensaje = "Producto no encontrado o inactivo." });

                return Ok(producto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error al obtener el producto.", error = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CrearProducto([FromBody] ProductoCreateDTO dto)
        {
            if (dto == null) return BadRequest(new { mensaje = "Los datos del producto son nulos." });

            try
            {
                var nuevoProducto = new Producto
                {
                    Nombre = dto.Nombre,
                    Descripcion = dto.Descripcion,
                    Precio = dto.Precio,
                    StockActual = dto.StockActual, // Nace con el stock indicado
                    Imagen = dto.Imagen,
                    IdCategoriaProducto = dto.IdCategoriaProducto,
                    Activo = true
                };

                _context.Productos.Add(nuevoProducto);
                await _context.SaveChangesAsync();

                return Ok(new { mensaje = "Producto creado exitosamente.", IdProducto = nuevoProducto.IdProducto });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error al crear el producto.", error = ex.InnerException?.Message ?? ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ActualizarProducto(int id, [FromBody] ProductoCreateDTO dto)
        {
            if (dto == null) return BadRequest(new { mensaje = "Los datos del producto son nulos." });

            try
            {
                var productoExistente = await _context.Productos.FindAsync(id);

                if (productoExistente == null || !productoExistente.Activo)
                    return NotFound(new { mensaje = "Producto no encontrado o inactivo." });

                productoExistente.Nombre = dto.Nombre;
                productoExistente.Descripcion = dto.Descripcion;
                productoExistente.Precio = dto.Precio;
                productoExistente.IdCategoriaProducto = dto.IdCategoriaProducto;
                // El stock y la imagen no se actualizan mediante este DTO por regla de negocio

                await _context.SaveChangesAsync();

                return Ok(new { mensaje = "Producto actualizado exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error al actualizar el producto.", error = ex.Message });
            }
        }

        [HttpPatch("{id}/estado")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CambiarEstadoProducto(int id)
        {
            try
            {
                var producto = await _context.Productos.FindAsync(id);
                if (producto == null) return NotFound(new { mensaje = "Producto no encontrado." });

                producto.Activo = !producto.Activo; // Toggle: si estaba activo lo desactiva, y viceversa
                await _context.SaveChangesAsync();

                string estadoTexto = producto.Activo ? "reactivado" : "dado de baja lógicamente";
                return Ok(new { mensaje = $"Producto {estadoTexto}." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error al cambiar el estado del producto.", error = ex.Message });
            }
        }

        // Endpoint multipart/form-data para subir imágenes físicas
        [HttpPost("{id}/imagen")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> SubirImagenReal(int id, IFormFile archivo)
        {
            try
            {
                if (archivo == null || archivo.Length == 0)
                    return BadRequest(new { mensaje = "No se envió ninguna imagen." });

                if (archivo.Length > 10 * 1024 * 1024)
                    return BadRequest(new { mensaje = "La imagen es muy pesada. Máximo 10MB." });

                var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var extension = Path.GetExtension(archivo.FileName).ToLower();

                if (!extensionesPermitidas.Contains(extension))
                    return BadRequest(new { mensaje = "Formato no permitido. Usá JPG, PNG o WEBP." });

                var producto = await _context.Productos.FindAsync(id);
                if (producto == null || !producto.Activo)
                    return NotFound(new { mensaje = "Producto no encontrado o inactivo." });

                string webRootPath = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
                string carpetaUploads = Path.Combine(webRootPath, "uploads");

                if (!Directory.Exists(carpetaUploads))
                    Directory.CreateDirectory(carpetaUploads);

                string nombreArchivo = $"prod_{id}_{Guid.NewGuid()}{extension}";
                string rutaFisica = Path.Combine(carpetaUploads, nombreArchivo);

                using (var stream = new FileStream(rutaFisica, FileMode.Create))
                {
                    await archivo.CopyToAsync(stream);
                }

                producto.Imagen = $"/uploads/{nombreArchivo}";
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    mensaje = "Imagen subida exitosamente.",
                    ruta = producto.Imagen
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    mensaje = "Error fatal al guardar la imagen.",
                    error = ex.Message
                });
            }
        }
    }
}