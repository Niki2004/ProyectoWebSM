﻿using Dapper;
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
    public class MisRecetasController : Controller
    {

        private readonly IConfiguration _configuration;
        private readonly IUtilitarios _utilitarios;

        public MisRecetasController(IConfiguration configuration, IUtilitarios utilitarios)
        {
            _configuration = configuration;
            _utilitarios = utilitarios;
        }

        [HttpPost]
        [Route("RegistrarReceta")]
        public IActionResult RegistrarReceta(RecetaModel model)
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
            {
                var result = context.Execute("RegistrarReceta",
                    new { model.Id_Categoria, model.Titulo, model.Descripcion, model.PlatoReciente, model.PlatoDestacada, model.Ingrediente, model.Imagen });

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
                        model.Ingrediente,
                        model.Imagen
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

        [HttpDelete]
        [Route("EliminarReceta/{idReceta}")]
        public IActionResult EliminarReceta(long idReceta)
        {
            var respuesta = new RespuestaModel();
            try
            {
                using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
                {

                    var result = context.Execute("EliminarReceta", new { Id_Receta = idReceta });

                    if (result > 0)
                    {
                        respuesta.Indicador = true;
                        respuesta.Mensaje = "La receta ha sido eliminada correctamente.";
                    }
                    else
                    {
                        respuesta.Indicador = false;
                        respuesta.Mensaje = "No se pudo eliminar la receta. Verifique que la receta exista.";
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

        [HttpPost]
        [Route("RegistrarComentario")]
        public IActionResult RegistrarComentario(ComentarioModel model)
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
            {
                var result = context.Execute("RegistrarComentario",
                    new { model.Id_Usuario, model.Id_Receta, model.Contenido });

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
        [Route("ModificarComentario/{id}")]
        public IActionResult ModificarComentario(long id, ComentarioModel model)
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
            {
                var result = context.Execute("ModificarComentario",
                    new
                    {
                        Id_Comentario = id,
                        model.Id_Usuario,
                        model.Id_Receta,
                        model.Contenido
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

        [HttpDelete]
        [Route("EliminarComentario")]
        public IActionResult EliminarComentario(long idComentario)
        {
            var respuesta = new RespuestaModel();
            try
            {
                using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
                {
                    // Llamada al procedimiento almacenado EliminarComentario
                    var result = context.Execute("EliminarComentario", new { Id_Comentario = idComentario });

                    if (result > 0)
                    {
                        respuesta.Indicador = true;
                        respuesta.Mensaje = "El comentario ha sido eliminado correctamente.";
                    }
                    else
                    {
                        respuesta.Indicador = false;
                        respuesta.Mensaje = "No se pudo eliminar el comentario. Verifique el Id.";
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

        [HttpGet]
        [Route("ConsultarComentario")]
        public IActionResult ConsultarComentario(long Id_Comentario)
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
            {
                var result = context.Query<ComentarioModel>("ConsultarComentario",
                    new { Id_Comentario });

                var respuesta = new RespuestaModel();

                if (result != null)
                {
                    respuesta.Indicador = true;
                    respuesta.Mensaje = "Información consultada";
                    respuesta.Datos = result;
                }
                else
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "No hay información registrada en este momento";
                }

                return Ok(respuesta);
            }

        }

        [HttpGet]
        [Route("ConsultarRecetas")]
        public IActionResult ConsultarRecetas()
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
            {
                var desayunos = context.Query<RecetaModel>("ConsultarRecetas");

                var respuesta = new RespuestaModel();

                if (desayunos != null && desayunos.Any())
                {
                    respuesta.Indicador = true;
                    respuesta.Mensaje = "Recetas existentes obtenidos correctamente";
                    respuesta.Datos = desayunos;
                }
                else
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "No se encontraron recetas existentes";
                }

                return Ok(respuesta);
            }
        }

        [HttpPost]
        [Route("RegistrarValoracion")]
        public IActionResult RegistrarValoracion(ValoracionModel model)
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
            {
                var result = context.Execute("RegistrarValoracion",
                   new { model.Id_Usuario,model.Id_Receta, model.Puntuacion });

                var respuesta = new RespuestaModel();

                if (result > 0)
                {
                    respuesta.Indicador = true;
                    respuesta.Mensaje = "Valoración registrada exitosamente.";
                }
                else
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "Error al registrar la valoración.";
                }

                return Ok(respuesta);
            }
        }

        [HttpGet]
        [Route("ConsultarValoraciones/{idUsuario}")]
        public IActionResult ConsultarValoraciones(long idUsuario)
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
            {
                var valoraciones = context.Query<ValoracionModel>("ConsultarValoraciones",
                    new { Id_Usuario = idUsuario });

                var respuesta = new RespuestaModel();

                if (valoraciones != null && valoraciones.Any())
                {
                    respuesta.Indicador = true;
                    respuesta.Mensaje = "Valoraciones obtenidas correctamente";
                    respuesta.Datos = valoraciones;
                }
                else
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "No se encontraron valoraciones para este usuario";
                }

                return Ok(respuesta);
            }
        }

        [HttpGet]
        [Route("ConsultarComentarioAdmin")]
        public IActionResult ConsultarComentarioAdmin(long Id_Comentario)
        {
            var respuesta = new RespuestaModel();

            //Esto es para que no pueda acceder un usuario a la vista
            //if (_utilitarios.ValidarUsuarioAdministradorFromToken(User.Claims))
            //{
            //    respuesta.Indicador = true;
            //    respuesta.Mensaje = "No tiene permisos para acceder a esta información";
            //    return Ok(respuesta);
            //}
            return View();
        }

        [HttpGet]
        [Route("EliminarComentarioAdmin")]
        public IActionResult EliminarComentarioAdmin()
        {
            var respuesta = new RespuestaModel();

            if (_utilitarios.ValidarUsuarioAdministradorFromToken(User.Claims))
            {
                respuesta.Indicador = true;
                respuesta.Mensaje = "No tiene permisos para acceder a esta información";
                return Ok(respuesta);
            }

            return View();
        }

    }
}

