using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using SM_ProyectoApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SM_ProyectoApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;  
        }

        [HttpPost] 
        [Route("RegistrarUsuario")]
        public IActionResult RegistrarUsuario(UsuarioModel model)
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
            {
                var result = context.Execute("RegistrarUsuario",
                    new { model.Nombre, model.Email, model.Contrasenia, model.Fecha_Registro });

                var respuesta = new RespuestaModel();

                if (result > 0)
                {
                    respuesta.Indicador = true;
                    respuesta.Mensaje = "El usuario se registro correctamente";
                }

                else
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "Error al registrar el usuario";
                }

                return Ok(respuesta);
            }

        }

        [HttpPost]
        [Route("IniciarSesion")]
        public IActionResult IniciarSesion(UsuarioModel model)
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
            {
               var result = context.QueryFirstOrDefault<UsuarioModel>("IniciarSesion",
                    new { model.Email, model.Contrasenia });

                var respuesta = new RespuestaModel();

                if (result != null)
                {
                    result.Token = GenerarToken(result.Id_Usuario);
                    respuesta.Indicador = true;
                    respuesta.Mensaje = "Su información se ha validado correctamente";
                    respuesta.Datos = result;
                }

                else
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "Su información no se ha validado correctamente";
                }

                return Ok(respuesta);
            }
        }

        private string GenerarToken(long Id_Usuario)
        {
            string SecretKey = _configuration.GetSection("Variables:llaveToken").Value!;

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("Id_Usuario", Id_Usuario.ToString()));
            

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(20),
                signingCredentials: cred);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
