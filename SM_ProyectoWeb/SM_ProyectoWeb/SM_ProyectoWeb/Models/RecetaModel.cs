namespace SM_ProyectoWeb.Models
{
    public class RecetaModel
    {
        public long Id_Receta { get; set; }

        public long Id_Categoria { get; set; }

        public string? Titulo { get; set; }

        public string? Descripcion { get; set; }

        public DateTime Fecha_Publicacion { get; set; }

        public bool PlatoReciente { get; set; }

        public bool PlatoDestacada { get; set; }

        public string? Ingrediente { get; set; }

        public string Imagen { get; set; }
    }
}
