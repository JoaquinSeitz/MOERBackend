using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoerEstetica.API.Data;
using MoerEsteticaBackend.Models;
using System;
using System.Threading.Tasks;

namespace MoerEstetica.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // CATEGORÍAS DE PRODUCTOS
        // ==========================================

        [HttpGet("productos")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategoriasProductos()
        {
            try
            {
                var categorias = await _context.Categorias_Productos.ToListAsync();
                return Ok(categorias);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error recuperando categorías de productos.", error = ex.Message });
            }
        }

        [HttpPost("productos")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CrearCategoriaProducto([FromBody] Categoria_Producto categoria)
        {
            if (categoria == null) return BadRequest();
            try
            {
                _context.Categorias_Productos.Add(categoria);
                await _context.SaveChangesAsync();
                return Ok(new { mensaje = "Categoría de producto creada exitosamente.", categoria });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error creando categoría.", error = ex.Message });
            }
        }

        [HttpPut("productos/{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ActualizarCategoriaProducto(int id, [FromBody] Categoria_Producto categoriaActualizada)
        {
            if (id != categoriaActualizada.IdCategoriaProducto) return BadRequest();
            try
            {
                var categoria = await _context.Categorias_Productos.FindAsync(id);
                if (categoria == null) return NotFound();

                categoria.Nombre = categoriaActualizada.Nombre;
                categoria.Descripcion = categoriaActualizada.Descripcion;

                await _context.SaveChangesAsync();
                return Ok(new { mensaje = "Categoría de producto actualizada.", categoria });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error actualizando categoría.", error = ex.Message });
            }
        }

        [HttpDelete("productos/{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> EliminarCategoriaProducto(int id)
        {
            try
            {
                var categoria = await _context.Categorias_Productos.FindAsync(id);
                if (categoria == null) return NotFound();

                _context.Categorias_Productos.Remove(categoria);
                await _context.SaveChangesAsync();
                return Ok(new { mensaje = "Categoría eliminada." });
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { mensaje = "No se puede eliminar la categoría porque hay productos asignados a ella." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error al eliminar.", error = ex.Message });
            }
        }

        // ==========================================
        // CATEGORÍAS DE SERVICIOS
        // ==========================================

        [HttpGet("servicios")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategoriasServicios()
        {
            try
            {
                var categorias = await _context.Categorias_Servicios.ToListAsync();
                return Ok(categorias);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error recuperando categorías de servicios.", error = ex.Message });
            }
        }

        [HttpPost("servicios")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CrearCategoriaServicio([FromBody] Categoria_Servicio categoria)
        {
            if (categoria == null) return BadRequest();
            try
            {
                _context.Categorias_Servicios.Add(categoria);
                await _context.SaveChangesAsync();
                return Ok(new { mensaje = "Categoría de servicio creada exitosamente.", categoria });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error creando categoría.", error = ex.Message });
            }
        }

        [HttpPut("servicios/{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ActualizarCategoriaServicio(int id, [FromBody] Categoria_Servicio categoriaActualizada)
        {
            if (id != categoriaActualizada.IdCategoriaServicio) return BadRequest();
            try
            {
                var categoria = await _context.Categorias_Servicios.FindAsync(id);
                if (categoria == null) return NotFound();

                categoria.Nombre = categoriaActualizada.Nombre;
                categoria.Descripcion = categoriaActualizada.Descripcion;

                await _context.SaveChangesAsync();
                return Ok(new { mensaje = "Categoría de servicio actualizada.", categoria });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error actualizando categoría.", error = ex.Message });
            }
        }

        [HttpDelete("servicios/{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> EliminarCategoriaServicio(int id)
        {
            try
            {
                var categoria = await _context.Categorias_Servicios.FindAsync(id);
                if (categoria == null) return NotFound();

                _context.Categorias_Servicios.Remove(categoria);
                await _context.SaveChangesAsync();
                return Ok(new { mensaje = "Categoría eliminada." });
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { mensaje = "No se puede eliminar la categoría porque hay servicios (tratamientos) asignados a ella." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error al eliminar.", error = ex.Message });
            }
        }
    }
}