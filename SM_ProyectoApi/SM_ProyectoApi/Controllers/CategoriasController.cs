using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SM_ProyectoApi.Models;

namespace SM_ProyectoApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CategoriasController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //Desayunos
        [HttpGet]
        [Route("DesayunosDestacados")]
        public IActionResult DesayunosDestacados()
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
            {
                var desayunos = context.Query<RecetaModel>("DesayunosDestacados");

                var respuesta = new RespuestaModel();

                if (desayunos != null && desayunos.Any())
                {
                    respuesta.Indicador = true;
                    respuesta.Mensaje = "Desayunos destacados obtenidos correctamente";
                    respuesta.Datos = desayunos;
                }
                else
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "No se encontraron desayunos destacados";
                }

                return Ok(respuesta);
            }
        }

        [HttpGet]
        [Route("DesayunosRecientes")]
        public IActionResult DesayunosRecientes()
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
            {
                var desayunos = context.Query<RecetaModel>("DesayunosRecientes");

                var respuesta = new RespuestaModel();

                if (desayunos != null && desayunos.Any())
                {
                    respuesta.Indicador = true;
                    respuesta.Mensaje = "Desayunos recientes obtenidos correctamente";
                    respuesta.Datos = desayunos;
                }
                else
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "No se encontraron desayunos recientemente";
                }

                return Ok(respuesta);
            }
        }

        //Entradas
        [HttpGet]
        [Route("EntradasDestacados")]
        public IActionResult EntradasDestacados()
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
            {
                var entradas = context.Query<RecetaModel>("EntradasDestacados");

                var respuesta = new RespuestaModel();

                if (entradas != null && entradas.Any())
                {
                    respuesta.Indicador = true;
                    respuesta.Mensaje = "Entradas destacadas obtenidas correctamente";
                    respuesta.Datos = entradas;
                }
                else
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "No se encontraron entradass destacadas";
                }

                return Ok(respuesta);
            }
        }

        [HttpGet]
        [Route("EntradasRecientes")]
        public IActionResult EntradasRecientes()
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
            {
                var entradas = context.Query<RecetaModel>("EntradasRecientes");

                var respuesta = new RespuestaModel();

                if (entradas != null && entradas.Any())
                {
                    respuesta.Indicador = true;
                    respuesta.Mensaje = "Entradas recientes obtenidas correctamente";
                    respuesta.Datos = entradas;
                }
                else
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "No se encontraron entradas recientemente";
                }

                return Ok(respuesta);
            }
        }

        //PlatosFuertes
        [HttpGet]
        [Route("PlatosFuertesDestacados")]
        public IActionResult PlatosFuertesDestacados()
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
            {
                var platosFuertes = context.Query<RecetaModel>("PlatosFuertesDestacados");

                var respuesta = new RespuestaModel();

                if (platosFuertes != null && platosFuertes.Any())
                {
                    respuesta.Indicador = true;
                    respuesta.Mensaje = "Platos fuertes destacadas obtenidas correctamente";
                    respuesta.Datos = platosFuertes;
                }
                else
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "No se encontraron entradas destacadas";
                }

                return Ok(respuesta);
            }
        }

        [HttpGet]
        [Route("PlatosFuertesRecientes")]
        public IActionResult PlatosFuertesRecientes()
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
            {
                var platosFuertes = context.Query<RecetaModel>("PlatosFuertesRecientes");

                var respuesta = new RespuestaModel();

                if (platosFuertes != null && platosFuertes.Any())
                {
                    respuesta.Indicador = true;
                    respuesta.Mensaje = "Platos fuertes recientes obtenidas correctamente";
                    respuesta.Datos = platosFuertes;
                }
                else
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "No se encontraron Platos fuertes recientemente";
                }

                return Ok(respuesta);
            }
        }

        //Postres 
        [HttpGet]
        [Route("PostresDestacados")]
        public IActionResult PostresDestacados()
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
            {
                var postres = context.Query<RecetaModel>("PostresDestacados");

                var respuesta = new RespuestaModel();

                if (postres != null && postres.Any())
                {
                    respuesta.Indicador = true;
                    respuesta.Mensaje = "Postres destacados obtenidos correctamente";
                    respuesta.Datos = postres;
                }
                else
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "No se encontraron postres destacados";
                }

                return Ok(respuesta);
            }
        }

        [HttpGet]
        [Route("PostresRecientes")]
        public IActionResult PostresRecientes()
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
            {
                var postres = context.Query<RecetaModel>("PostresRecientes");

                var respuesta = new RespuestaModel();

                if (postres != null && postres.Any())
                {
                    respuesta.Indicador = true;
                    respuesta.Mensaje = "Postres recientes obtenidos correctamente";
                    respuesta.Datos = postres;
                }
                else
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "No se encontraron postres recientemente";
                }

                return Ok(respuesta);
            }
        }


    }
}
