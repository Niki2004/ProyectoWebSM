using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SM_ProyectoApi.Dependencias;
using SM_ProyectoApi.Models;
using System.Data;

namespace SM_ProyectoApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUtilitarios _utilitarios;
        public UsuariosController(IConfiguration configuration, IUtilitarios utilitarios)
        {
            _configuration = configuration;
            _utilitarios = utilitarios;
        }

        [HttpGet]
        [Route("ListaUsuarios")]
        public IActionResult ListaUsuarios()
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
            {
                var usuarios = context.Query<UsuarioModel>("ObtenerUsuariosRegistrados", commandType: System.Data.CommandType.StoredProcedure);

                var respuesta = new RespuestaModel();

                if (usuarios != null && usuarios.Any())
                {
                    respuesta.Indicador = true;
                    respuesta.Mensaje = "Usuarios registrados obtenidos correctamente";
                    respuesta.Datos = usuarios;
                }
                else
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "No se encontraron usuarios registrados";
                    respuesta.Datos = null;
                }

                return Ok(respuesta);
            }
        }


        [HttpDelete]
        [Route("EliminarUsuario/{idUsuario}")]
        public IActionResult EliminarUsuario(long idUsuario)
        {
            var respuesta = new RespuestaModel();
            try
            {
                using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
                {

                    var result = context.Execute("EliminarUsuario", new { Id_Usuario = idUsuario });

                    if (result > 0)
                    {
                        respuesta.Indicador = true;
                        respuesta.Mensaje = "El usuario ha sido eliminado correctamente.";
                    }
                    else
                    {
                        respuesta.Indicador = false;
                        respuesta.Mensaje = "No se pudo eliminar el usuario. Verifique que el usuario exista.";
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta.Indicador = false;
                respuesta.Mensaje = $"Error: {ex.Message}";
            }

            return Ok(respuesta);
        }

        [HttpPut]
        [Route("ActualizarContrasenia")]
        public IActionResult ActualizarContrasenia(UsuarioModel model)
        {
            var Id_Usuario = _utilitarios.ObtenerUsuarioFromToken(User.Claims);

            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
            {
                var result = context.Execute("ActualizarContrasenia",
                    new { Id_Usuario, model.Contrasenia });

                var respuesta = new RespuestaModel();

                if (result > 0)
                {
                    respuesta.Indicador = true;
                    respuesta.Mensaje = "Información actualizada";
                }
                else
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "No se actualizó la información correctamente";
                }

                return Ok(respuesta);
            }
        }
    }
}

