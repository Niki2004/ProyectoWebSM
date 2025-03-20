using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SM_ProyectoWeb.Dependencias;
using SM_ProyectoWeb.Models;
using System.Net.Http.Headers;

namespace SM_ProyectoWeb.Controllers
{
    public class MisRecetasController : Controller
    {

        private readonly IHttpClientFactory _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IUtilitarios _utilitarios;

        public MisRecetasController(IHttpClientFactory httpClient, IConfiguration configuration, IUtilitarios utilitarios)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _utilitarios = utilitarios;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RegistrarReceta()
        {
            var datosResult = _utilitarios.ConsultarInfoRecetas(0);
            CargarComentariosCombo();
            return View(datosResult);
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


        [HttpGet]
        public IActionResult RegistrarComentario()
        {
            return View();
        }


        [HttpPost]
        public IActionResult RegistrarComentario(ComentarioModel model)
        {
            using (var api = _httpClient.CreateClient())
            {
                var url = _configuration.GetSection("Variables:urlApi").Value + "MisRecetas/RegistrarComentario";

                api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                var result = api.PostAsJsonAsync(url, model).Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("RegistrarComentario", "MisRecetas");
                }
            }

            return View();
        }

        private void CargarComentariosCombo()
        {
            var datosResult = _utilitarios.ConsultarInfoRecetas(0);
            var recetasSelect = new List<SelectListItem>();

            recetasSelect.Add(new SelectListItem { Value = string.Empty, Text = "-- Seleccione --" });
            foreach (var item in datosResult)
            {
                recetasSelect.Add(new SelectListItem { Value = item.Id_Receta.ToString(), Text = item.Titulo });
            }

            ViewBag.ListaRecetas = recetasSelect;
        }

    }
}
