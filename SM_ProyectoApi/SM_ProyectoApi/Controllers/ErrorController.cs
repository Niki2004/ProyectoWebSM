using Dapper;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SM_ProyectoApi.Dependencias;
using SM_ProyectoApi.Models;

namespace SM_ProyectoApi.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUtilitarios _utilitarios;
        public ErrorController(IConfiguration configuration, IUtilitarios utilitarios)
        {
            _configuration = configuration;
            _utilitarios = utilitarios;
        }

        [Route("CapturarError")]
        public IActionResult CapturarError()
        {
            var ex = HttpContext.Features.Get<IExceptionHandlerFeature>();

            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
            {
                var Id_Usuario = _utilitarios.ObtenerUsuarioFromToken(User.Claims);
                var Descripcion = ex!.Error.Message;
                var Origen = ex.Path;

                context.Execute("RegistrarErrores",
                new { Id_Usuario, Descripcion, Origen });
            }

            var respuesta = new RespuestaModel();
            respuesta.Indicador = false;
            respuesta.Mensaje = "Se presentó un problema en el sistema";

            return BadRequest(respuesta);
        }
    }
}
