using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Modelos;
using System.Collections.Generic;
using System.Linq;

namespace proyecto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private AppDbContext context;
        public ProductosController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductos()
        {
            return Ok(await context.Productos.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await context.Productos.FirstOrDefaultAsync(p => p.Id == id);
            if (producto == null)
                return NotFound();
            return Ok(producto);
        }

        [HttpPost]
        public async Task<IActionResult> CrearProducto(Producto p)
        {
            var producto = await (from prod in context.Productos
                           where prod.Nombre == p.Nombre
                           select prod).FirstOrDefaultAsync();
            if (producto != null)
                return BadRequest("El producto ya existe");
            await context.Productos.AddAsync(p);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProducto), new { id = p.Id }, p);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarProducto(int id, Producto producto)
        {
            var existing = await context.Productos.FirstOrDefaultAsync(p => p.Id == id);
            if (existing == null)
                return NotFound();
            existing.Nombre = producto.Nombre;
            existing.Precio = producto.Precio;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            var producto = await context.Productos.FirstOrDefaultAsync(p => p.Id == id);
            if (producto == null)
                return NotFound();
            context.Productos.Remove(producto);
            return NoContent();
        }
    }
}