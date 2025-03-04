Use ProyectoWebAvanzada;

------------------------  PROCEDIMIENTOS  ----------------------

------------------------ RegistrarUsuario ----------------------

CREATE PROCEDURE [dbo].[RegistrarUsuario]
    @Id_Estado bigint = 1,
    @Nombre NVARCHAR(50),
    @Email NVARCHAR(255),
    @Contrasenia NVARCHAR(50),
    @Rol NVARCHAR(50) = 'User'
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Usuario WHERE Email = @Email)
    BEGIN
        INSERT INTO dbo.Usuario (Id_Estado, Nombre, Email, Contrasenia, Rol)
        VALUES (@Id_Estado, @Nombre, @Email, @Contrasenia, @Rol)
    END
END
GO

------------------------ IniciarSesion ----------------------

CREATE PROCEDURE [dbo].[IniciarSesion]
    @Email NVARCHAR(255),
    @Contrasenia NVARCHAR(50)
AS
BEGIN
    SELECT  U.Id_Usuario AS Id,
            U.Email AS Identificacion,
            U.Nombre AS NombreUsuario,
            U.Email AS Correo,
            U.Id_Estado AS Estado,
            U.Rol
    FROM    dbo.Usuario U
    WHERE   U.Email = @Email
        AND U.Contrasenia = @Contrasenia
        AND U.Id_Estado = 1
END
GO






