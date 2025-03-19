namespace SM_ProyectoApi.Models
{
    public class ComentarioModel
    {
        public long Id_Comentario { get; set; }

        public long Id_Usuario { get; set; }

        public long Id_Receta { get; set; }

        public string? Contenido { get; set; }

        public string? Titulo { get; set; } //Esto debería ser un campo en la tabla de recetas

        public DateTime Fecha_Comentario { get; set; }
      
    }
}
