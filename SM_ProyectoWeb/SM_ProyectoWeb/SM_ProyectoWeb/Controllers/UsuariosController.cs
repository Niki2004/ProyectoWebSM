using Microsoft.AspNetCore.Mvc;
using SM_ProyectoWeb.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SM_ProyectoWeb.Controllers
{
    [FiltroSeguridadSesion]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class UsuariosController : Controller
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IConfiguration _configuration;

        public UsuariosController(IHttpClientFactory httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

        }

        [HttpGet]
        public IActionResult ListaUsuarios()
        {
            ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
            ViewBag.IdUsuario = HttpContext.Session.GetString("Id_Usuario");
            using (var api = _httpClient.CreateClient())
            {
                var url = _configuration.GetSection("Variables:urlApi").Value + "Usuarios/ListaUsuarios";

                api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                var response = api.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

                    if (result != null && result.Indicador)
                    {
                        var datosResult = JsonSerializer.Deserialize<List<UsuarioModel>>((JsonElement)result.Datos!);
                        return View(datosResult);
                    }
                }
            }

            return View(new List<UsuarioModel>());
        }





    }
}

  
       
