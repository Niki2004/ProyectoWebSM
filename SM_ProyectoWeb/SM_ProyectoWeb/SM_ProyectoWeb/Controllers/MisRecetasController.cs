using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SM_ProyectoWeb.Dependencias;
using SM_ProyectoWeb.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SM_ProyectoWeb.Controllers
{
    [FiltroSeguridadSesion]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
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
            ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
            ViewBag.IdUsuario = HttpContext.Session.GetString("Id_Usuario");
            return View();
        }

        [HttpPost]
        public IActionResult RegistrarReceta(RecetaModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
                {
                    TempData["Error"] = "No hay sesión activa. Por favor, inicie sesión nuevamente.";
                    return RedirectToAction("IniciarSesion", "Login");
                }

                Console.WriteLine("\n=== INICIO REGISTRO DE RECETA ===");
                Console.WriteLine($"Token: {HttpContext.Session.GetString("Token")}");
                Console.WriteLine($"Id_Categoria: {model.Id_Categoria}");
                Console.WriteLine($"Titulo: {model.Titulo}");
                Console.WriteLine($"Descripción: {model.Descripcion}");
                Console.WriteLine($"Plato Reciente: {model.PlatoReciente}");
                Console.WriteLine($"Plato Destacado: {model.PlatoDestacada}");
                Console.WriteLine($"Ingredientes: {model.Ingrediente}");

                // Validaciones adicionales
                if (model.Id_Categoria <= 0)
                {
                    TempData["Error"] = "Debe seleccionar una categoría válida";
                    return View(model);
                }

                if (string.IsNullOrEmpty(model.Titulo))
                {
                    TempData["Error"] = "El título es obligatorio";
                    return View(model);
                }

                if (string.IsNullOrEmpty(model.Descripcion))
                {
                    TempData["Error"] = "La descripción es obligatoria";
                    return View(model);
                }

                if (string.IsNullOrEmpty(model.Ingrediente))
                {
                    TempData["Error"] = "Debe agregar al menos un ingrediente";
                    return View(model);
                }

                // Procesar la imagen si existe
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    if (file != null && file.Length > 0)
                    {
                        Console.WriteLine("\n=== Procesando imagen ===");
                        Console.WriteLine($"Nombre del archivo: {file.FileName}");
                        Console.WriteLine($"Tipo de contenido: {file.ContentType}");
                        Console.WriteLine($"Tamaño del archivo: {file.Length} bytes");
                        
                        // Validar el tipo de imagen
                        if (!file.ContentType.StartsWith("image/"))
                        {
                            Console.WriteLine($"Error: El archivo no es una imagen válida. Tipo: {file.ContentType}");
                            TempData["Error"] = "El archivo debe ser una imagen válida";
                            return View(model);
                        }

                        // Crear directorio para imágenes si no existe
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        // Generar nombre único para el archivo
                        var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Guardar el archivo
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        // Guardar la ruta relativa en el modelo
                        model.Imagen = $"/uploads/{uniqueFileName}";
                        Console.WriteLine($"Imagen guardada en: {model.Imagen}");
                    }
                    else
                    {
                        Console.WriteLine("Error: El archivo está vacío");
                        TempData["Error"] = "La imagen es obligatoria";
                        return View(model);
                    }
                }
                else
                {
                    Console.WriteLine("Error: No se encontró ningún archivo en la solicitud");
                    TempData["Error"] = "La imagen es obligatoria";
                    return View(model);
                }

                // Obtener los ingredientes del formulario
                var ingredientes = Request.Form["Ingrediente"].ToList();
                model.Ingrediente = string.Join(", ", ingredientes.Where(i => !string.IsNullOrEmpty(i)));
                Console.WriteLine($"\nIngredientes procesados: {model.Ingrediente}");

                // Procesar los valores de los checkboxes
                model.PlatoReciente = Request.Form.ContainsKey("PlatoReciente");
                model.PlatoDestacada = Request.Form.ContainsKey("PlatoDestacada");
                Console.WriteLine($"Checkboxes - PlatoReciente: {model.PlatoReciente}, PlatoDestacada: {model.PlatoDestacada}");

                using (var api = _httpClient.CreateClient())
                {
                    var url = _configuration.GetSection("Variables:urlApi").Value + "MisRecetas/RegistrarReceta";
                    Console.WriteLine($"\nURL del API: {url}");

                    api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                    
                    // Crear un objeto anónimo con los datos necesarios
                    var recetaData = new
                    {
                        model.Id_Categoria,
                        model.Titulo,
                        model.Descripcion,
                        model.PlatoReciente,
                        model.PlatoDestacada,
                        model.Ingrediente,
                        model.Imagen
                    };

                    // Serializar los datos para verificar
                    var jsonData = JsonSerializer.Serialize(recetaData);
                    Console.WriteLine($"Datos a enviar al API: {jsonData}");

                    Console.WriteLine("\nEnviando datos al API...");
                    var result = api.PostAsJsonAsync(url, recetaData).Result;

                    Console.WriteLine($"Código de respuesta: {result.StatusCode}");
                    var responseContent = result.Content.ReadAsStringAsync().Result;
                    Console.WriteLine($"Contenido de la respuesta: {responseContent}");

                    if (result.IsSuccessStatusCode)
                    {
                        var response = JsonSerializer.Deserialize<RespuestaModel>(responseContent);
                        if (response != null && response.Indicador)
                        {
                            Console.WriteLine("Receta registrada exitosamente");
                            TempData["Mensaje"] = "Receta registrada exitosamente";
                            return RedirectToAction("ConsultarRecetas", "MisRecetas");
                        }
                        else
                        {
                            Console.WriteLine($"Error en la respuesta: {response?.Mensaje}");
                            TempData["Error"] = response?.Mensaje ?? "Error al registrar la receta";
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
                Console.WriteLine($"Error al registrar receta: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                TempData["Error"] = "Ocurrió un error inesperado. Por favor, intente nuevamente.";
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ConsultarComentario()
        {
            ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
            ViewBag.IdUsuario = HttpContext.Session.GetString("Id_Usuario");
            var datosResult = _utilitarios.ConsultarInfoComentario(0);
            Console.WriteLine("Cantidad de comentarios: " + datosResult.Count);
            return View(datosResult);
        }

        [HttpGet]
        public IActionResult RegistrarComentario()
        {
            ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
            ViewBag.IdUsuario = HttpContext.Session.GetString("Id_Usuario");
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
            ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
            ViewBag.IdUsuario = HttpContext.Session.GetString("Id_Usuario");
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
            ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
            ViewBag.IdUsuario = HttpContext.Session.GetString("Id_Usuario");
            try
            {
                Console.WriteLine("=== INICIO CONSULTA DE RECETAS ===");
                Console.WriteLine($"Token: {HttpContext.Session.GetString("Token")}");

                using (var api = _httpClient.CreateClient())
                {
                    var url = _configuration.GetSection("Variables:urlApi").Value + "MisRecetas/ConsultarRecetas";
                    Console.WriteLine($"URL del API: {url}");

                    api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                    var response = api.GetAsync(url).Result;

                    Console.WriteLine($"Código de respuesta: {response.StatusCode}");
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine($"Contenido de la respuesta: {responseContent}");

                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonSerializer.Deserialize<RespuestaModel>(responseContent);
                        if (result != null && result.Indicador)
                        {
                            if (result.Datos != null)
                            {
                                var datosResult = JsonSerializer.Deserialize<List<RecetaModel>>((JsonElement)result.Datos);
                                Console.WriteLine($"Cantidad de recetas encontradas: {datosResult?.Count ?? 0}");
                                
                                if (datosResult != null)
                                {
                                    foreach (var receta in datosResult)
                                    {
                                        Console.WriteLine($"\n=== Receta encontrada ===");
                                        Console.WriteLine($"ID: {receta.Id_Receta}");
                                        Console.WriteLine($"Título: {receta.Titulo}");
                                        
                                        if (!string.IsNullOrEmpty(receta.Imagen))
                                        {
                                            Console.WriteLine($"Imagen presente - Longitud: {receta.Imagen.Length}");
                                            Console.WriteLine($"Primeros caracteres de la imagen: {receta.Imagen.Substring(0, Math.Min(50, receta.Imagen.Length))}");
                                            
                                            // Verificar el formato de la imagen
                                            if (receta.Imagen.StartsWith("/9j/"))
                                            {
                                                Console.WriteLine("Formato de imagen: JPEG");
                                            }
                                            else if (receta.Imagen.StartsWith("iVBORw0KGgo"))
                                            {
                                                Console.WriteLine("Formato de imagen: PNG");
                                            }
                                            else
                                            {
                                                Console.WriteLine("ADVERTENCIA: Formato de imagen no reconocido");
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("No hay imagen en esta receta");
                                        }
                                    }
                                }
                                
                                return View(datosResult ?? new List<RecetaModel>());
                            }
                            else
                            {
                                Console.WriteLine("No hay datos en la respuesta");
                                TempData["Mensaje"] = "No hay recetas registradas";
                                return View(new List<RecetaModel>());
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Error en la respuesta: {result?.Mensaje}");
                            TempData["Error"] = result?.Mensaje ?? "Error al consultar las recetas";
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
                Console.WriteLine($"Error al consultar recetas: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                TempData["Error"] = "Ocurrió un error inesperado. Por favor, intente nuevamente.";
            }

            return View(new List<RecetaModel>());
        }

        [HttpGet]
        public IActionResult ModificarComentario(long id)
        {
            ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
            ViewBag.IdUsuario = HttpContext.Session.GetString("Id_Usuario");
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

        [HttpPost]
        public IActionResult EliminarComentario(long id)
        {

            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
                {
                    TempData["Error"] = "No hay sesión activa. Por favor, inicie sesión nuevamente.";
                    return RedirectToAction("IniciarSesion", "Login");
                }

                // Obtener el comentario por ID para verificar el propietario
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
                    TempData["Error"] = "No tiene permisos para eliminar este comentario.";
                    return RedirectToAction("ConsultarComentario", "MisRecetas");
                }

                using (var api = _httpClient.CreateClient())
                {
                    var url = _configuration.GetSection("Variables:urlApi").Value + "MisRecetas/EliminarComentario";
                    Console.WriteLine($"URL del API: {url}");

                    api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                    var result = api.DeleteAsync($"{url}?idComentario={id}").Result;

                    Console.WriteLine($"Código de respuesta: {result.StatusCode}");
                    var responseContent = result.Content.ReadAsStringAsync().Result;
                    Console.WriteLine($"Contenido de la respuesta: {responseContent}");

                    if (result.IsSuccessStatusCode)
                    {
                        var response = JsonSerializer.Deserialize<RespuestaModel>(responseContent);
                        if (response != null && response.Indicador)
                        {
                            TempData["Mensaje"] = "Comentario eliminado exitosamente";
                        }
                        else
                        {
                            TempData["Error"] = response?.Mensaje ?? "Error al eliminar el comentario";
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
                Console.WriteLine($"Error al eliminar comentario: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                TempData["Error"] = "Ocurrió un error inesperado. Por favor, intente nuevamente.";
            }

            return RedirectToAction("ConsultarComentario", "MisRecetas");
        }

        [HttpGet]
        public IActionResult RegistrarValoracion()
        {
            ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
            ViewBag.IdUsuario = HttpContext.Session.GetString("Id_Usuario");
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
            {
                return RedirectToAction("IniciarSesion", "Login");
            }
            CargarRecetasCombo();
            return View();
        }

        [HttpPost]
        public IActionResult RegistrarValoracion(ValoracionModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
                {
                    TempData["Error"] = "No hay sesión activa. Por favor, inicie sesión nuevamente.";
                    return RedirectToAction("IniciarSesion", "Login");
                }

                var idUsuarioStr = HttpContext.Session.GetString("Id_Usuario");
                if (string.IsNullOrEmpty(idUsuarioStr) || !long.TryParse(idUsuarioStr, out long idUsuario))
                {
                    TempData["Error"] = "ID de usuario no válido. Inicie sesión nuevamente.";
                    return RedirectToAction("IniciarSesion", "Login");
                }

                model.Id_Usuario = idUsuario;

                if (!ModelState.IsValid)
                {
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        TempData["Error"] = error.ErrorMessage;
                    }
                    return View(model);
                }

                Console.WriteLine($"Registrando valoración - Usuario: {model.Id_Usuario}, Receta: {model.Id_Receta}, Puntuación: {model.Puntuacion}");

                using (var api = _httpClient.CreateClient())
                {
                    var url = _configuration.GetSection("Variables:urlApi").Value + "MisRecetas/RegistrarValoracion";
                    api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));

                    var result = api.PostAsJsonAsync(url, model).Result;
                    var responseContent = result.Content.ReadAsStringAsync().Result;

                    Console.WriteLine($"Código de respuesta: {result.StatusCode}, Contenido: {responseContent}");

                    if (result.IsSuccessStatusCode)
                    {
                        var response = JsonSerializer.Deserialize<RespuestaModel>(responseContent);
                        if (response?.Indicador == true)
                        {
                            TempData["Mensaje"] = "Valoración registrada exitosamente.";
                            return RedirectToAction("ConsultarValoraciones", "MisRecetas");
                        }
                        else
                        {
                            TempData["Error"] = response?.Mensaje ?? "Error al registrar la valoración.";
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Error de comunicación con el servidor. Intente nuevamente.";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar valoración: {ex.Message}\n{ex.StackTrace}");
                TempData["Error"] = "Ocurrió un error inesperado. Intente nuevamente.";
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ConsultarValoraciones()
        {
            ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
            ViewBag.IdUsuario = HttpContext.Session.GetString("Id_Usuario");
            try
            {
                var idUsuarioStr = HttpContext.Session.GetString("Id_Usuario");
                if (string.IsNullOrEmpty(idUsuarioStr) || !long.TryParse(idUsuarioStr, out long idUsuario))
                {
                    TempData["Error"] = "ID de usuario no válido. Inicie sesión nuevamente.";
                    return RedirectToAction("IniciarSesion", "Login");
                }

                using (var api = _httpClient.CreateClient())
                {
                    var url = $"{_configuration.GetSection("Variables:urlApi").Value}MisRecetas/ConsultarValoraciones?idUsuario={idUsuario}";
                    api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                    var response = api.GetAsync(url).Result;
                    var responseContent = response.Content.ReadAsStringAsync().Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonSerializer.Deserialize<RespuestaModel>(responseContent);
                        if (result?.Indicador == true)
                        {
                            var valoraciones = JsonSerializer.Deserialize<List<ValoracionModel>>((JsonElement)result.Datos!);
                            return View(valoraciones ?? new List<ValoracionModel>());
                        }
                        else
                        {
                            TempData["Error"] = result?.Mensaje ?? "Error al consultar las valoraciones";
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
                Console.WriteLine($"Error al consultar valoraciones: {ex.Message}\n{ex.StackTrace}");
                TempData["Error"] = "Ocurrió un error inesperado. Por favor, intente nuevamente.";
            }

            return View(new List<ValoracionModel>());
        }

    }
}
