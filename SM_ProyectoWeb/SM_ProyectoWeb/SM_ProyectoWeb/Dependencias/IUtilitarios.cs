using SM_ProyectoWeb.Models;

namespace SM_ProyectoWeb.Dependencias
{
    public interface IUtilitarios
    {
        List<RecetaModel> ConsultarInfoRecetas(long Id_Receta);

    }
}
