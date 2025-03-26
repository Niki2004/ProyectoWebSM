namespace SM_ProyectoWeb.Models
{
    public class ErroresModel
    {
        public long Id_Errores { get; set; }
        public long Id_Usuario { get; set; }
        public DateTime Fecha_Publicacion { get; set; }
        public string? Descripcion { get; set; }
        public string? Origen { get; set; }
    }
}
