﻿namespace SM_ProyectoApi.Models
{
    public class UsuarioModel
    {
        public long Id_Usuario { get; set; }

        public long Id_Estado { get; set; }

        public string? Nombre { get; set; }

        public string? Email { get; set; }

        public string? Contrasenia { get; set; }

        public DateTime Fecha_Registro { get; set; }

        public string? Rol { get; set; }

        public string? Token { get; set; }



    }
}
