namespace SM_ProyectoApi.Models
{
    public class Receta_IngredienteModel
    {
        public long Id_Receta_Ingrediente { get; set; }

        public long Id_Receta { get; set; }

        public long Id_Ingrediente { get; set; }

        public decimal Cantidad { get; set; }
 
    }
}
