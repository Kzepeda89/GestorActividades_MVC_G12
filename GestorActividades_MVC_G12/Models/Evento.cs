namespace GestorActividades_MVC_G12.Models
{
    public class Evento
    {
        public int IdEvento { get; set; }
        public string NombreEvento { get; set; }
        public string NombreCategoria { get; set; } // Para mostrar el nombre del JOIN
        public int IdCategoria { get; set; }
        public int CuposDisponibles { get; set; }
    }
}