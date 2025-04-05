using Microsoft.AspNetCore.Mvc;
using SM_ProyectoWeb.Models;
using System;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace SM_ProyectoWeb.Controllers
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IConfiguration _configuration;

        public LoginController(IHttpClientFactory httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public IActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]
        public IActionResult IniciarSesion(UsuarioModel model)
        {
            try
            {
                model.Contrasenia = Encrypt(model.Contrasenia!);

                using (var api = _httpClient.CreateClient())
                {
                    var url = _configuration.GetSection("Variables:urlApi").Value + "Login/IniciarSesion";
                    var response = api.PostAsJsonAsync(url, model).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

                        if (result != null && result.Indicador)
                        {

                            using (var api2 = _httpClient.CreateClient())
                            {
                                var url2 = _configuration.GetSection("Variables:urlApi").Value + "Login/ObtenerUsuarioPorCorreoYContrasenia";

                                var response2 = api2.PostAsJsonAsync(url2, model).Result; 

                                if (response2.IsSuccessStatusCode)
                                {
                                    var result2 = response2.Content.ReadFromJsonAsync<RespuestaModel>().Result;

                                    if (result2 != null && result2.Indicador)
                                    {
                                        var datosResult2 = JsonSerializer.Deserialize<UsuarioModel>((JsonElement)result2.Datos!);
                                        HttpContext!.Session.SetString("Token", datosResult2.Token!);
                                        HttpContext!.Session.SetString("Id_Usuario", datosResult2.Id_Usuario.ToString());
                                        HttpContext!.Session.SetString("Nombre", datosResult2.Nombre ?? "");
                                        HttpContext!.Session.SetString("Id_Rol", datosResult2.Id_Rol.ToString());

                                        Debug.WriteLine($"Nombre User: {datosResult2.Nombre}");
                                    }
                                }
                            }

                            return RedirectToAction("Principal", "Login");
                        }
                        else
                        {
                            TempData["Error"] = "Credenciales inválidas. Por favor, intente nuevamente.";
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Error al comunicarse con el servidor. Por favor, intente nuevamente.";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al iniciar sesión: {ex.Message}");
                TempData["Error"] = "Ocurrió un error inesperado. Por favor, intente nuevamente.";
            }

            return View();
        }

        [FiltroSeguridadSesion] //Fitro para que no puedas ir a las vistas sin estar logueado
        [HttpGet]
        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("IniciarSesion", "Login");
        }

        [HttpGet]
        public IActionResult RegistrarUsuario()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegistrarUsuario(UsuarioModel model)
        {
            model.Contrasenia = Encrypt(model.Contrasenia!);

            using (var api = _httpClient.CreateClient())
            {
                var url = _configuration.GetSection("Variables:urlApi").Value + "Login/RegistrarUsuario";
                var result = api.PostAsJsonAsync(url, model).Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("IniciarSesion", "Login");
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult RecuperarContrasenna()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Perfil()
        {
            UsuarioModel? usuario = null;
            RolModel? rol = null;

            try
            {
                using (var api = _httpClient.CreateClient())
                {
                    var url = _configuration.GetSection("Variables:urlApi").Value + $"Login/ObtenerUsuarioPorId/{HttpContext.Session.GetString("Id_Usuario")}";
                    var response = api.GetAsync(url).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

                        if (result != null && result.Indicador)
                        {
                            usuario = JsonSerializer.Deserialize<UsuarioModel>((JsonElement)result.Datos!);


                            if (usuario != null)
                            {

                                ViewBag.Nombre = usuario.Nombre;
                                ViewBag.Email = usuario.Email;
                                ViewBag.Id_Usuario = usuario.Id_Usuario;
                            }
                        }
                        else
                        {
                            TempData["Error"] = "No se pudo obtener la información del usuario.";
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Hubo un problema al conectarse con la API.";
                    }
                }

                using (var api2 = _httpClient.CreateClient())
                {
                    var url2 = _configuration.GetSection("Variables:urlApi").Value + $"Rol/ObtenerRolePorId/{usuario.Id_Rol}";
                    api2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                    var response2 = api2.GetAsync(url2).Result;

                    if (response2.IsSuccessStatusCode)
                    {
                        var result2 = response2.Content.ReadFromJsonAsync<RespuestaModel>().Result;

                        if (result2 != null && result2.Indicador)
                        {
                            rol = JsonSerializer.Deserialize<RolModel>((JsonElement)result2.Datos!);


                            if (rol != null)
                            {
                                ViewBag.Rol = rol.Rol;
                                ViewBag.Id_Rol = rol.Id_Rol;
                            }
                   
                        }
                        else
                        {
                            TempData["Error"] = "No se pudo obtener la información del Rol.";
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Hubo un problema al conectarse con la API.";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Ocurrió un error inesperado: {ex.Message}";
            }

            return View(usuario);
        }


        [FiltroSeguridadSesion]
        public IActionResult Principal()
        {
            ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
            ViewBag.IdUsuario = HttpContext.Session.GetString("Id_Usuario");
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
