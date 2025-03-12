using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SM_ProyectoApi.Models;
using System.Data;

namespace SM_ProyectoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MisRecetasController : Controller
    {

        private readonly IConfiguration _configuration;

        public MisRecetasController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpPost]
        [Route("RegistrarReceta")]
        public IActionResult RegistrarReceta(RecetaModel model)
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
            {
                var result = context.Execute("RegistrarReceta",
                    new {model.Id_Categoria, model.Titulo, model.Descripcion, model.PlatoReciente, model.PlatoDestacada, model.Ingrediente });

                var respuesta = new RespuestaModel();

                if (result > 0)
                {
                    respuesta.Indicador = true;
                    respuesta.Mensaje = "Su información se ha registrado correctamente";
                }
                else
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "Su información no ha registrado correctamente";
                }

                return Ok(respuesta);
            }
        }

        [HttpPut]
        [Route("ModificarReceta/{id}")]
        public IActionResult ModificarReceta(long id, RecetaModel model)
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
            {
                var result = context.Execute("ModificarReceta",
                    new
                    {
                        Id_Receta = id,
                        model.Id_Categoria,
                        model.Titulo,
                        model.Descripcion,
                        model.PlatoReciente,
                        model.PlatoDestacada,
                        model.Ingrediente
                    });

                var respuesta = new RespuestaModel();
                if (result > 0)
                {
                    respuesta.Indicador = true;
                    respuesta.Mensaje = "Su información se ha modificado correctamente";
                }
                else
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "Su información no ha modificado correctamente";
                }
                return Ok(respuesta);
            }
        }
    }
}

