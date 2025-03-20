using SM_ProyectoWeb.Models;

namespace SM_ProyectoWeb.Dependencias
{
    public interface IUtilitarios
    {
        List<ComentarioModel> ConsultarInfoComentario(long Id_Comentario);

    }
}
