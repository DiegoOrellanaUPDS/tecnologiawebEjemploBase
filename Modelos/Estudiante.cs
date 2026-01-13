using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class Estudiante
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Ci { get; set; }
        public string Estado { get; set; } = "Activo";
    }
}

