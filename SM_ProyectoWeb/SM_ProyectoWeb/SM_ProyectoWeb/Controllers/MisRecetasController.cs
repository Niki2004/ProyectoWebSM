using Microsoft.AspNetCore.Mvc;
using SM_ProyectoWeb.Models;

namespace SM_ProyectoWeb.Controllers
{
    public class MisRecetasController : Controller
    {

        private readonly IHttpClientFactory _httpClient;
        private readonly IConfiguration _configuration;

        public MisRecetasController(IHttpClientFactory httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }


        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [Route("RegistrarReceta")]
        public IActionResult RegistrarRecetaWeb(RecetaModel model)
        {
            if (model == null)
            {
                TempData["MensajeError"] = "Los datos de la receta no son válidos.";
                return RedirectToAction("Index");
            }

            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
            {
                var result = context.Execute("RegistrarReceta",
                    new
                    {
                        model.Id_Categoria,
                        model.Titulo,
                        model.Descripcion,
                        model.PlatoReciente,
                        model.PlatoDestacada,
                        model.Ingrediente
                    });

                if (result > 0)
                {
                    TempData["MensajeExito"] = "La receta se ha registrado correctamente.";
                }
                else
                {
                    TempData["MensajeError"] = "Hubo un error al registrar la receta.";
                }
            }

            return RedirectToAction("Index");
        }
    }
}
