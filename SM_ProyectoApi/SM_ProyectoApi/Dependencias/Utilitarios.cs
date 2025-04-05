using System.Security.Claims;

namespace SM_ProyectoApi.Dependencias
{
    public class Utilitarios : IUtilitarios
    {
        public long ObtenerUsuarioFromToken(IEnumerable<Claim> valores)
        {
            if (valores.Any())
            {
                var Id_Usuario = valores.FirstOrDefault(x => x.Type == "Id_Usuario")?.Value;
                return long.Parse(Id_Usuario!);
            }

            return 0;
        }
        
        public bool ValidarUsuarioAdministradorFromToken(IEnumerable<Claim> valores)
        {
            if (valores.Any())
            {
                var Id_Perfil = valores.FirstOrDefault(x => x.Type == "Id_Perfil")?.Value;
                return Id_Perfil == "2";
            }

            return false;
        }
    }
}
