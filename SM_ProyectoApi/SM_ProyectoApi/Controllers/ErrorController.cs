using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SM_ProyectoApi.Models;

namespace SM_ProyectoApi.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {

        [HttpPost]
        [Route("CapturarError")]
        public IActionResult CapturarError()
        {
            var ex = HttpContext.Features.Get<IExceptionHandlerFeature>();

            var respuesta = new RespuestaModel();

            respuesta.Indicador = false;
            respuesta.Mensaje = "Se presento un problema en el sistema";

            return Ok(respuesta);

        }
    }
}
