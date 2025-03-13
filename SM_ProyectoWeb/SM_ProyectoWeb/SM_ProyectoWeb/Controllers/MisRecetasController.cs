    using Microsoft.AspNetCore.Mvc;
using SM_ProyectoWeb.Models;
using System.Net.Http.Headers;

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
        public IActionResult RegistrarReceta(RecetaModel model)
        {
            if (model == null)
            {
                TempData["Error"] = "Los datos de la receta son inválidos.";
                return RedirectToAction("RegistrarReceta");
            }

            try
            {
                using (var api = _httpClient.CreateClient())
                {
                    var url = _configuration.GetSection("Variables:urlApi").Value + "RegistrarReceta";

                    api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));

                    var response = api.PostAsJsonAsync(url, model).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

                        if (result != null && result.Indicador)
                        {
                            TempData["Mensaje"] = "La receta se ha registrado correctamente.";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Error"] = result?.Mensaje ?? "Hubo un problema al registrar la receta.";
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Error en la comunicación con el servidor.";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al registrar la receta: " + ex.Message;
            }

            return RedirectToAction("RegistrarReceta");
        }
    }
}
