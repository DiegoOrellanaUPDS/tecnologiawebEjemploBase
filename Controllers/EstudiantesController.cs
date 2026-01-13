using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace proyecto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstudiantesController : ControllerBase
    {
        private AppDbContext context;
        public EstudiantesController(AppDbContext context)
        {
            this.context = context;
        }
        private static List<Estudiante> estudiantes = new List<Estudiante>();


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estudiante>>> GetEstudiantes()
        {
            return Ok(await context.Estudiantes.ToListAsync());
        }

        [HttpGet("{ci}")]
        public async Task<IActionResult> GetEstudiante(int ci)
        {
            var estudiante = await (from est in context.Estudiantes
                                   where est.Ci == ci && est.Estado != "Borrado"
                                   select est).FirstOrDefaultAsync();
            if (estudiante == null)
            {
                return NotFound();
            }
            
            return Ok(estudiante);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEstudiante(Estudiante estudiante)
        {
            var e = await (from est in context.Estudiantes
                           where est.Ci == estudiante.Ci && est.Estado != "Borrado"
                           select est).FirstOrDefaultAsync();
            if (e != null)
            {
                return BadRequest("El estudiante ya existe.");
            }
            await context.Estudiantes.AddAsync(estudiante);
            await context.SaveChangesAsync();
            return Ok(estudiante);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEstudiante(int id, [FromBody] Estudiante estudiante)
        {
            var existing = estudiantes.FirstOrDefault(e => e.Id == id);
            if (existing == null)
                return NotFound();

            existing.Nombre = estudiante.Nombre;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstudiante(int ci)
        {
            var estudiante = await (from est in context.Estudiantes
                                   where est.Ci == ci && est.Estado != "Borrado"
                                   select est).FirstOrDefaultAsync();
            if (estudiante == null)
                return NotFound();

            estudiante.Estado = "Borrado";
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}