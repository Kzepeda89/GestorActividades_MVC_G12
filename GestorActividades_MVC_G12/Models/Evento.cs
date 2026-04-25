using System.ComponentModel.DataAnnotations;

namespace GestorActividades_MVC_G12.Models
{
    public class Evento
    {
        [Key]
        public int IdEvento { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string NombreEvento { get; set; }

        public int IdCategoria { get; set; }

        // Esta es la propiedad que te está dando el error CS1061 y CS0117
        public string NombreCategoria { get; set; }

        [Range(1, 500)]
        public int CuposDisponibles { get; set; }
    }
}