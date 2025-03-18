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

        [HttpGet]
        public IActionResult RegistrarReceta()
        {
            return View();
        }


        [HttpPost]
        public IActionResult RegistrarReceta(RecetaModel model)
        {
            using (var api = _httpClient.CreateClient())
            {
                var url = _configuration.GetSection("Variables:urlApi").Value + "MisRecetas/RegistrarReceta";

                api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                var result = api.PostAsJsonAsync(url, model).Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Recetas", "MisRecetas");
                }
            }

            return View();
        }

    }
}
