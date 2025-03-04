using Microsoft.AspNetCore.Mvc;
using SM_ProyectoWeb.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace SM_ProyectoWeb.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IConfiguration _configuration;

        public LoginController(IHttpClientFactory httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        //Inicio de sesión  
        public IActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]
        public IActionResult IniciarSesion(UsuarioModel model)
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
                        var datosResult = JsonSerializer.Deserialize<UsuarioModel>((JsonElement)result.Datos!);

                        return RedirectToAction("Principal", "Login");
                    }


                }
            }

            return View();
        }

        //Registro de usuario 
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

        //Recuperar contraseña
        [HttpGet]
        public IActionResult RecuperarContrasenna()
        {
            return View();
        }

        //Clase principal
        public IActionResult Principal()
        {
            return View();
        }

        //Cifrado de contraseña
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
