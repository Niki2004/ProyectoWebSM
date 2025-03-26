using Microsoft.Extensions.Configuration;
using SM_ProyectoWeb.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SM_ProyectoWeb.Dependencias
{
     public class Utilitarios : IUtilitarios
     {
        private readonly IHttpClientFactory _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _accessor;

        public Utilitarios(IHttpClientFactory httpClient, IConfiguration configuration, IHttpContextAccessor accessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _accessor = accessor;
        }

        public List<ComentarioModel> ConsultarInfoComentario(long Id_Comentario)
        {
            using (var api = _httpClient.CreateClient())
            {
                var url = _configuration.GetSection("Variables:urlApi").Value + "MisRecetas/ConsultarComentario?Id=" + Id_Comentario;

                api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessor.HttpContext!.Session.GetString("Token"));
                var response = api.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

                    if (result != null && result.Indicador)
                    {
                        return JsonSerializer.Deserialize<List<ComentarioModel>>((JsonElement)result.Datos!)!;
                    }
                }
            }

            return new List<ComentarioModel>();
        }

        public List<PasoModel> ConsultarInfoPasos(long Id_Pasos)
        {
            using (var api = _httpClient.CreateClient())
            {
                var url = _configuration.GetSection("Variables:urlApi").Value + "MisRecetas/MostrarPasos?Id=" + Id_Pasos;

                api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessor.HttpContext!.Session.GetString("Token"));
                var response = api.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

                    if (result != null && result.Indicador)
                    {
                        return JsonSerializer.Deserialize<List<PasoModel>>((JsonElement)result.Datos!)!;
                    }
                }
            }

            return new List<PasoModel>();
        }

    }
}
