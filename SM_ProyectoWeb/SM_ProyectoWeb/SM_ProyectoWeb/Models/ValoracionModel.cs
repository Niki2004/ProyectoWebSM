using System.ComponentModel.DataAnnotations;

namespace SM_ProyectoWeb.Models
{
    public class ValoracionModel
    {
        public long Id_Valoracion { get; set; }

        public long Id_Usuario { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una receta")]
        public long Id_Receta { get; set; }

        [Required(ErrorMessage = "Debe asignar una puntuación")]
        [Range(1, 5, ErrorMessage = "La puntuación debe estar entre 1 y 5 estrellas")]
        public int Puntuacion { get; set; }

        public string? Nombre_Receta { get; set; }
    }
}
