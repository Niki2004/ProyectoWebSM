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
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
            {
                return RedirectToAction("IniciarSesion", "Login");
            }
            CargarRecetasCombo();
            return View();
        }


        [HttpPost]
        public IActionResult RegistrarComentario(ComentarioModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
                {
                    TempData["Error"] = "No hay sesión activa. Por favor, inicie sesión nuevamente.";
                    return RedirectToAction("IniciarSesion", "Login");
                }

                // Obtener el ID del usuario de la sesión
                var idUsuarioStr = HttpContext.Session.GetString("Id_Usuario");
                if (string.IsNullOrEmpty(idUsuarioStr))
                {
                    TempData["Error"] = "No se encontró el ID del usuario en la sesión. Por favor, inicie sesión nuevamente.";
                    return RedirectToAction("IniciarSesion", "Login");
                }

                // Validar que el ID del usuario sea un número válido
                if (!int.TryParse(idUsuarioStr, out int idUsuario))
                {
                    TempData["Error"] = "El ID del usuario no es válido. Por favor, inicie sesión nuevamente.";
                    return RedirectToAction("IniciarSesion", "Login");
                }

                // Asignar el ID del usuario al modelo
                model.Id_Usuario = idUsuario;

                if (!ModelState.IsValid)
                {
                    foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        TempData["Error"] = modelError.ErrorMessage;
                    }
                    CargarRecetasCombo();
                    return View(model);
                }

                Console.WriteLine($"Intentando registrar comentario - Id_Usuario: {model.Id_Usuario}, Id_Receta: {model.Id_Receta}");
                Console.WriteLine($"Contenido: {model.Contenido}");
                Console.WriteLine($"Fecha: {model.Fecha_Comentario}");

                using (var api = _httpClient.CreateClient())
                {
                    var url = _configuration.GetSection("Variables:urlApi").Value + "MisRecetas/RegistrarComentario";
                    Console.WriteLine($"URL del API: {url}");
                    Console.WriteLine($"Token: {HttpContext.Session.GetString("Token")}");

                    api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                    var result = api.PostAsJsonAsync(url, model).Result;

                    Console.WriteLine($"Código de respuesta: {result.StatusCode}");
                    var responseContent = result.Content.ReadAsStringAsync().Result;
                    Console.WriteLine($"Contenido de la respuesta: {responseContent}");

                    if (result.IsSuccessStatusCode)
                    {
                        var response = JsonSerializer.Deserialize<RespuestaModel>(responseContent);
                        if (response != null && response.Indicador)
                        {
                            TempData["Mensaje"] = "Comentario registrado exitosamente";
                            return RedirectToAction("ConsultarComentario", "MisRecetas");
                        }
                        else
                        {
                            TempData["Error"] = response?.Mensaje ?? "Error al registrar el comentario";
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error del API: {responseContent}");
                        TempData["Error"] = "Error al comunicarse con el servidor. Por favor, intente nuevamente.";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar comentario: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                TempData["Error"] = "Ocurrió un error inesperado. Por favor, intente nuevamente.";
            }

            CargarRecetasCombo();
            return View(model);
        }

        private void CargarRecetasCombo()
        {
            using (var api = _httpClient.CreateClient())
            {
                var url = _configuration.GetSection("Variables:urlApi").Value + "MisRecetas/ConsultarRecetas";
                api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                var response = api.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;
                    if (result != null && result.Indicador)
                    {
                        var recetas = JsonSerializer.Deserialize<List<RecetaModel>>((JsonElement)result.Datos!);
                        var recetasSelect = new List<SelectListItem>
                        {
                            new SelectListItem { Value = "", Text = "-- Seleccione --" }
                        };

                        foreach (var receta in recetas)
                        {
                            recetasSelect.Add(new SelectListItem { Value = receta.Id_Receta.ToString(), Text = receta.Titulo });
                        }

                        ViewBag.ListaRecetas = recetasSelect;
                    }
                }
            }
        }


       public IActionResult ConsultarRecetas(RecetaModel model)
        {
            using (var api = _httpClient.CreateClient())
         {

                var url = _configuration.GetSection("Variables:urlApi").Value + "MisRecetas/ConsultarRecetas";

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

        [HttpGet]
        public IActionResult ModificarComentario(long id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
                {
                    return RedirectToAction("IniciarSesion", "Login");
                }

                // Obtener el comentario por ID
                var comentario = _utilitarios.ConsultarInfoComentario(id).FirstOrDefault();
                if (comentario == null)
                {
                    TempData["Error"] = "No se encontró el comentario.";
                    return RedirectToAction("ConsultarComentario", "MisRecetas");
                }

                // Verificar que el usuario actual sea el propietario del comentario
                var idUsuarioActual = HttpContext.Session.GetString("Id_Usuario");
                if (comentario.Id_Usuario.ToString() != idUsuarioActual)
                {
                    TempData["Error"] = "No tiene permisos para editar este comentario.";
                    return RedirectToAction("ConsultarComentario", "MisRecetas");
                }

                CargarRecetasCombo();
                return View(comentario);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar el comentario: {ex.Message}");
                TempData["Error"] = "Ocurrió un error al cargar el comentario.";
                return RedirectToAction("ConsultarComentario", "MisRecetas");
            }
        }

        [HttpPost]
        public IActionResult ModificarComentario(long id, ComentarioModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
                {
                    TempData["Error"] = "No hay sesión activa. Por favor, inicie sesión nuevamente.";
                    return RedirectToAction("IniciarSesion", "Login");
                }

                // Obtener el ID del usuario de la sesión
                var idUsuarioStr = HttpContext.Session.GetString("Id_Usuario");
                if (string.IsNullOrEmpty(idUsuarioStr))
                {
                    TempData["Error"] = "No se encontró el ID del usuario en la sesión. Por favor, inicie sesión nuevamente.";
                    return RedirectToAction("IniciarSesion", "Login");
                }

                // Validar que el ID del usuario sea un número válido
                if (!int.TryParse(idUsuarioStr, out int idUsuario))
                {
                    TempData["Error"] = "El ID del usuario no es válido. Por favor, inicie sesión nuevamente.";
                    return RedirectToAction("IniciarSesion", "Login");
                }

                // Asignar el ID del usuario al modelo
                model.Id_Usuario = idUsuario;

                if (!ModelState.IsValid)
                {
                    foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        TempData["Error"] = modelError.ErrorMessage;
                    }
                    CargarRecetasCombo();
                    return View(model);
                }

                Console.WriteLine($"Intentando modificar comentario - Id: {id}, Id_Usuario: {model.Id_Usuario}, Id_Receta: {model.Id_Receta}");
                Console.WriteLine($"Contenido: {model.Contenido}");

                using (var api = _httpClient.CreateClient())
                {
                    var url = _configuration.GetSection("Variables:urlApi").Value + $"MisRecetas/ModificarComentario/{id}";
                    Console.WriteLine($"URL del API: {url}");

                    api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                    var result = api.PutAsJsonAsync(url, model).Result;

                    Console.WriteLine($"Código de respuesta: {result.StatusCode}");
                    var responseContent = result.Content.ReadAsStringAsync().Result;
                    Console.WriteLine($"Contenido de la respuesta: {responseContent}");

                    if (result.IsSuccessStatusCode)
                    {
                        var response = JsonSerializer.Deserialize<RespuestaModel>(responseContent);
                        if (response != null && response.Indicador)
                        {
                            TempData["Mensaje"] = "Comentario modificado exitosamente";
                            return RedirectToAction("ConsultarComentario", "MisRecetas");
                        }
                        else
                        {
                            TempData["Error"] = response?.Mensaje ?? "Error al modificar el comentario";
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error del API: {responseContent}");
                        TempData["Error"] = "Error al comunicarse con el servidor. Por favor, intente nuevamente.";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al modificar comentario: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                TempData["Error"] = "Ocurrió un error inesperado. Por favor, intente nuevamente.";
            }

            CargarRecetasCombo();
            return View(model);
        }

    }
}
