using System;

namespace SM_ProyectoApi.Models
{
    public class ValoracionModel
    {
        public long Id_Valoracion { get; set; }

        public long Id_Usuario { get; set; }

        public long Id_Receta { get; set; }

        public int Puntuacion { get; set; }

        public string Nombre_Receta { get; set; }

    }
}
