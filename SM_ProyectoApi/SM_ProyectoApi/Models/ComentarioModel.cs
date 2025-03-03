namespace SM_ProyectoApi.Models
{
    public class ComentarioModel
    {
        public long Id_Comentario { get; set; }

        public long Id_Usuario { get; set; }

        public long Id_Receta { get; set; }

        public string? Contenido { get; set; }

        public DateTime Fecha_Comentario { get; set; }
      
    }
}
