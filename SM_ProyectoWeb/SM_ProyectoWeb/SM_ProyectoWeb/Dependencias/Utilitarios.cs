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

        public List<RecetaModel> ConsultarInfoRecetas(long Id_Receta)
        {
            using (var api = _httpClient.CreateClient())
            {
                var url = _configuration.GetSection("Variables:urlApi").Value + "MisRecetas/RegistrarComentarios?Id=" + Id_Receta;

                api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessor.HttpContext!.Session.GetString("Token"));
                var response = api.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

                    if (result != null && result.Indicador)
                    {
                        return JsonSerializer.Deserialize<List<RecetaModel>>((JsonElement)result.Datos!)!;
                    }
                }
            }

            return new List<RecetaModel>();
        }
    }
}
