using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SM_ProyectoWeb.Dependencias;
using SM_ProyectoWeb.Models;
using System.Net.Http.Headers;
using System.Text.Json;

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
                    return RedirectToAction("RegistrarReceta", "MisRecetas");
                }
            }

            return View();
        }


        [HttpGet]
        public IActionResult ConsultarComentario()
        {
            var datosResult = _utilitarios.ConsultarInfoComentario(0);
            Console.WriteLine("Cantidad de comentarios: " + datosResult.Count);
            return View(datosResult);
        }


        [HttpGet]
        public IActionResult RegistrarComentario()
        {
            CargarComentariosCombo();
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
            var datosResult = _utilitarios.ConsultarInfoComentario(0);
            var recetasSelect = new List<SelectListItem>();

            recetasSelect.Add(new SelectListItem { Value = string.Empty, Text = "-- Seleccione --" });
            foreach (var item in datosResult)
            {
                recetasSelect.Add(new SelectListItem { Value = item.Id_Receta.ToString(), Text = item.Titulo });
            }

            ViewBag.ListaRecetas = recetasSelect;
        }


       public IActionResult ConsultarRecetas()
{
    using (var api = _httpClient.CreateClient())
    {
        var url = _configuration.GetSection("Variables:urlApi").Value + "MisRecetas/ConsultarRecetass";

        api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
        var response = api.GetAsync(url).Result;

        if (response.IsSuccessStatusCode)
        {
            var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

            if (result != null && result.Indicador)
            {
                var datosResult = JsonSerializer.Deserialize<List<RecetaModel>>((JsonElement)result.Datos!);
                return View(datosResult);
            }
        }
    }

    return View(new List<RecetaModel>());
}



    }
}
