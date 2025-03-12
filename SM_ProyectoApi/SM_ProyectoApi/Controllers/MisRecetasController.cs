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



        [HttpPut("modificar/{id}")]
        public async Task<IActionResult> ModificarReceta(long id, [FromBody] RecetaModel receta)
        {
            if (receta == null || id <= 0)
            {
                return BadRequest("Datos inválidos para la actualización.");
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("ModificarReceta", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Id_Receta", id);
                        cmd.Parameters.AddWithValue("@Id_Categoria", receta.Id_Categoria);
                        cmd.Parameters.AddWithValue("@Titulo", receta.Titulo);
                        cmd.Parameters.AddWithValue("@Descripcion", receta.Descripcion);
                        cmd.Parameters.AddWithValue("@PlatoReciente", receta.PlatoReciente);
                        cmd.Parameters.AddWithValue("@PlatoDestacada", receta.PlatoDestacada);
                        cmd.Parameters.AddWithValue("@Ingrediente", receta.Ingrediente);

                        int filasAfectadas = await cmd.ExecuteNonQueryAsync();

                        if (filasAfectadas == 0)
                        {
                            return NotFound(new { message = "No se encontró la receta a modificar." });
                        }
                    }
                }

                return Ok(new { message = "Receta modificada exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocurrió un error al modificar la receta.", details = ex.Message });
            }
        }

    }
}

