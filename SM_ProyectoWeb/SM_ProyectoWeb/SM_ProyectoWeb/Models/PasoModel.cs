namespace SM_ProyectoWeb.Models
{
    public class PasoModel
    {
        public long Id_Paso { get; set; }

        public long Id_Receta { get; set; }

        public string? Descripcion { get; set; }
       
        public int Numero_Paso { get; set; }

    }
}
