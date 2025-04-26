using Microsoft.AspNetCore.Mvc;
using SM_ProyectoWeb.Models;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
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

        [HttpGet]
        public IActionResult ActualizarContrasenna()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ActualizarContrasenna(UsuarioModel model)
        {
            if (model.Contrasenia == null)
            {
                TempData["Error"] = "Debe confirmar correctamente su nueva contraseña";
                return View();
            }

            using (var api = _httpClient.CreateClient())
            {
                var url = _configuration.GetSection("Variables:urlApi").Value + "Usuarios/ActualizarContrasenia";

                model.Contrasenia = Encrypt(model.Contrasenia!);
                api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                var response = api.PutAsJsonAsync(url, model).Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

                    if (result != null && result.Indicador)
                    {
                        return RedirectToAction("Principal", "Login");
                    }
                    else
                        TempData["Error"] = result!.Mensaje;
                }
                else
                    TempData["Error"] = "No se pudo completar su petición";
            }

            return View();
        }

        private string Encrypt(string texto)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_configuration.GetSection("Variables:llaveCifrado").Value!);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(texto);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }
    }
}

  
       
