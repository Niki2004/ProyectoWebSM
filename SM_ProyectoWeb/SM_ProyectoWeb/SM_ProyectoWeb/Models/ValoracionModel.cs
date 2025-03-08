namespace SM_ProyectoWeb.Models
{
    public class ValoracionModel
    {
        public long Id_Valoracion { get; set; }

        public long Id_Usuario { get; set; }

        public long Id_Receta { get; set; }

        public int Puntuacion { get; set; }

    }
}
