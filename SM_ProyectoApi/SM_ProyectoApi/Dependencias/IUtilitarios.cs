using System.Security.Claims;

namespace SM_ProyectoApi.Dependencias
{
    public interface IUtilitarios
    {
        long ObtenerUsuarioFromToken(IEnumerable<Claim> valores);
     
    }
}
