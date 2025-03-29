using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SM_ProyectoApi.Models;

namespace SM_ProyectoApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public RolController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        [Route("ObtenerRolePorId/{id}")]
        public IActionResult ObtenerRolePorId(long id)
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
            {

                var result = context.QueryFirstOrDefault<RolModel>("ObtenerRolePorId",
                    new { Id_Role = id }, commandType: System.Data.CommandType.StoredProcedure);

                var respuesta = new RespuestaModel();

                if (result != null)
                {
                    respuesta.Indicador = true;
                    respuesta.Mensaje = "Rol encontrado correctamente.";
                    respuesta.Datos = result;
                }
                else
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "Rol no encontrado.";
                }


                return Ok(respuesta);
            }
        }
    }
}
