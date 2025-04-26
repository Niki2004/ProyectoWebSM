using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using SM_ProyectoApi.Models;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SM_ProyectoApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;  
        }

        [HttpPost] 
        [Route("RegistrarUsuario")]
        public IActionResult RegistrarUsuario(UsuarioModel model)
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
            {
                var result = context.Execute("RegistrarUsuario",
                    new { model.Nombre, model.Email, model.Contrasenia});

                var respuesta = new RespuestaModel();

                if (result > 0)
                {
                    respuesta.Indicador = true;
                    respuesta.Mensaje = "El usuario se registro correctamente";
                }

                else
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "Error al registrar el usuario";
                }

                return Ok(respuesta);
            }

        }

        [HttpPost]
        [Route("IniciarSesion")]
        public IActionResult IniciarSesion(UsuarioModel model)
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
            {
               var result = context.QueryFirstOrDefault<UsuarioModel>("IniciarSesion",
                    new { model.Email, model.Contrasenia });

                var respuesta = new RespuestaModel();

                if (result != null)
                {
                    result.Token = GenerarToken(result.Id_Usuario, result.Id_Rol);
                    respuesta.Indicador = true;
                    respuesta.Mensaje = "Su información se ha validado correctamente";
                    respuesta.Datos = result;
                }

                else
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "Su información no se ha validado correctamente";
                }

                return Ok(respuesta);
            }
        }

        [HttpPost]  
        [Route("ObtenerUsuarioPorCorreoYContrasenia")]
        public IActionResult ObtenerUsuarioPorCorreoYContrasenia([FromBody] UsuarioModel model)
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
            {
                var result = context.QueryFirstOrDefault<UsuarioModel>("ObtenerUsuarioPorCorreoYContrasenia",
                    new { model.Email, model.Contrasenia }, commandType: System.Data.CommandType.StoredProcedure);

                var respuesta = new RespuestaModel();

                if (result != null)
                {
                    result.Token = GenerarToken(result.Id_Usuario, result.Id_Rol);
                    respuesta.Indicador = true;
                    respuesta.Mensaje = "Su información se ha validado correctamente";
                    respuesta.Datos = result;
                }
                else
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "Su información no se ha validado correctamente";
                }

                return Ok(respuesta);
            }
        }


        [HttpGet]
        [Route("ObtenerUsuarioPorId/{id}")]
        public IActionResult ObtenerUsuarioPorId(long id)
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
            {

                    var result = context.QueryFirstOrDefault<UsuarioModel>("ObtenerUsuarioPorId",
                        new { Id_Usuario = id }, commandType: System.Data.CommandType.StoredProcedure);

                     var respuesta = new RespuestaModel();

                if (result != null)
                    {
                        respuesta.Indicador = true;
                        respuesta.Mensaje = "Usuario encontrado correctamente.";
                        respuesta.Datos = result;
                    }
                    else
                    {
                        respuesta.Indicador = false;
                        respuesta.Mensaje = "Usuario no encontrado.";
                    }
                

                return Ok(respuesta);
            }
        }

        [HttpPost]
        [Route("RecuperarContrasenia")]
        public IActionResult RecuperarContrasenia(UsuarioModel model)
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
            {
                //buscar al usuario
                var result = context.QueryFirstOrDefault<UsuarioModel>("ValidarUsuarioCorreo",
                    new { model.Email });

                var respuesta = new RespuestaModel();

                if (result != null)
                {
                    //Autogenerar contraseña
                    var Codigo = GenerarCodigo();
                    var Contrasenia = Encrypt(Codigo);

                    //Actualizar contraseña
                    context.Execute("ActualizarContrasenia", new { result.Id_Usuario, Contrasenia });

                    //Enviar correo
                    var Contenido = "Hola " + result.Nombre + ". Se ha generado el siguiente código de seguridad: " + Codigo;

                    EnviarCorreo(result.Email!, "Acceso al sistema", Contenido);

                    respuesta.Indicador = true;
                    respuesta.Mensaje = "Su información se ha validado correctamente";
                    respuesta.Datos = result;
                }
                else
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "Su información no se ha validado correctamente";
                }

                return Ok(respuesta);
            }
        }

        private string GenerarCodigo()
        {
            int length = 8;
            const string valid = "ABCDEFGHIJKLMNOPQRSTUVWXYZ012456789";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
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

        private string GenerarToken(long Id_Usuario, long Id_Perfil)
        {
            string SecretKey = _configuration.GetSection("Variables:llaveToken").Value!;

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("Id_Usuario", Id_Usuario.ToString()));
            claims.Add(new Claim("Id_Perfil", Id_Perfil.ToString()));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(20),
                signingCredentials: cred);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private void EnviarCorreo(string destino, string asunto, string contenido)
        {
            string cuenta = _configuration.GetSection("Variables:CorreoEmail").Value!;
            string contrasenia = _configuration.GetSection("Variables:ClaveEmail").Value!;

            MailMessage message = new MailMessage();
            message.From = new MailAddress(cuenta);
            message.To.Add(new MailAddress(destino));
            message.Subject = asunto;
            message.Body = contenido;
            message.Priority = MailPriority.Normal;
            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential(cuenta, contrasenia);
            client.EnableSsl = true;

            //Esto es para que no se intente enviar el correo si no hay una contraseña
            if (!string.IsNullOrEmpty(contrasenia))
            {
                client.Send(message);
            }
        }
    }
}
