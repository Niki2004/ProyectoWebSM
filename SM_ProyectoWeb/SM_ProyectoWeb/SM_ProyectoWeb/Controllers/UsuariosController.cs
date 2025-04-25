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


        [HttpDelete]
        public IActionResult EliminarUsuario(long id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
                {
                    return Json(new { success = false, message = "No hay sesión activa. Por favor, inicie sesión nuevamente." });
                }

                Console.WriteLine($"\n=== INICIO ELIMINACIÓN DE USUARIO ===");
                Console.WriteLine($"Token: {HttpContext.Session.GetString("Token")}");
                Console.WriteLine($"Id_Usuario: {id}");

                using (var api = _httpClient.CreateClient())
                {
                    var url = _configuration.GetSection("Variables:urlApi").Value + $"Usuarios/EliminarUsuario/{id}";
                    Console.WriteLine($"URL del API: {url}");

                    api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                    var result = api.DeleteAsync(url).Result;

                    Console.WriteLine($"Código de respuesta: {result.StatusCode}");
                    var responseContent = result.Content.ReadAsStringAsync().Result;
                    Console.WriteLine($"Contenido de la respuesta: {responseContent}");

                    if (result.IsSuccessStatusCode)
                    {
                        var response = JsonSerializer.Deserialize<RespuestaModel>(responseContent);
                        if (response != null && response.Indicador)
                        {
                            Console.WriteLine("Usuario eliminado exitosamente");
                            return Json(new { success = true, message = "Usuario eliminado exitosamente" });
                        }
                        else
                        {
                            Console.WriteLine($"Error en la respuesta: {response?.Mensaje}");
                            return Json(new { success = false, message = response?.Mensaje ?? "Error al eliminar el usuario" });
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error del API: {responseContent}");
                        return Json(new { success = false, message = "Error al comunicarse con el servidor. Por favor, intente nuevamente." });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el usuario: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return Json(new { success = false, message = "Ocurrió un error inesperado. Por favor, intente nuevamente." });
            }
        }


    }
}

  
       
