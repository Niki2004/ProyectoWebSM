using Microsoft.AspNetCore.Mvc;
using SM_ProyectoWeb.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SM_ProyectoWeb.Controllers
{
    [FiltroSeguridadSesion]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class CategoriasController : Controller
    {

        private readonly IHttpClientFactory _httpClient;
        private readonly IConfiguration _configuration;

        public CategoriasController(IHttpClientFactory httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

        }

        //Desayunos
        [HttpGet]
        public IActionResult DesayunosDestacados()
        {
            ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
            ViewBag.IdUsuario = HttpContext.Session.GetString("Id_Usuario");
            using (var api = _httpClient.CreateClient())
            {
                var url = _configuration.GetSection("Variables:urlApi").Value + "Categorias/DesayunosDestacados";

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

        public IActionResult DesayunosRecientes()
        {
            ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
            ViewBag.IdUsuario = HttpContext.Session.GetString("Id_Usuario");
            using (var api = _httpClient.CreateClient())
            {
                var url = _configuration.GetSection("Variables:urlApi").Value + "Categorias/DesayunosRecientes";

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

        //Entradas
        public IActionResult EntradasDestacados()
        {
            ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
            ViewBag.IdUsuario = HttpContext.Session.GetString("Id_Usuario");
            using (var api = _httpClient.CreateClient())
            {
                var url = _configuration.GetSection("Variables:urlApi").Value + "Categorias/EntradasDestacados";

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

        public IActionResult EntradasRecientes()
        {
            ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
            ViewBag.IdUsuario = HttpContext.Session.GetString("Id_Usuario");
            using (var api = _httpClient.CreateClient())
            {
                var url = _configuration.GetSection("Variables:urlApi").Value + "Categorias/EntradasRecientes";

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

        //PlatosFuertes
        public IActionResult PlatosFuertesDestacados()
        {
            ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
            ViewBag.IdUsuario = HttpContext.Session.GetString("Id_Usuario");
            using (var api = _httpClient.CreateClient())
            {
                var url = _configuration.GetSection("Variables:urlApi").Value + "Categorias/PlatosFuertesDestacados";

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

        public IActionResult PlatosFuertesRecientes()
        {
            ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
            ViewBag.IdUsuario = HttpContext.Session.GetString("Id_Usuario");
            using (var api = _httpClient.CreateClient())
            {
                var url = _configuration.GetSection("Variables:urlApi").Value + "Categorias/PlatosFuertesRecientes";

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

        //Postres 
        public IActionResult PostresDestacados()
        {
            ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
            ViewBag.IdUsuario = HttpContext.Session.GetString("Id_Usuario");
            using (var api = _httpClient.CreateClient())
            {
                var url = _configuration.GetSection("Variables:urlApi").Value + "Categorias/PostresDestacados";

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

        public IActionResult PostresRecientes()
        {
            ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
            ViewBag.IdUsuario = HttpContext.Session.GetString("Id_Usuario");
            using (var api = _httpClient.CreateClient())
            {
                var url = _configuration.GetSection("Variables:urlApi").Value + "Categorias/PostresRecientes";

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
