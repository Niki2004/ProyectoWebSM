Use ProyectoWebAvanzada;

------------------------  PROCEDIMIENTOS  ----------------------

------------------------ RegistrarUsuario ----------------------

CREATE PROCEDURE [dbo].[RegistrarUsuario]
    @Id_Estado bigint = 1,
    @Nombre NVARCHAR(50),
    @Email NVARCHAR(255),
    @Contrasenia NVARCHAR(50),
    @Fecha_Registro DATETIME,
    @Rol NVARCHAR(50) = 'User'
AS
BEGIN
    -- Si no se pasa la fecha, se asigna la fecha actual
    IF @Fecha_Registro IS NULL
    BEGIN
        SET @Fecha_Registro = GETDATE();
    END

    IF NOT EXISTS (SELECT 1 FROM Usuario WHERE Email = @Email)
    BEGIN
        INSERT INTO dbo.Usuario (Id_Estado, Nombre, Email, Contrasenia, Fecha_Registro, Rol)
        VALUES (@Id_Estado, @Nombre, @Email, @Contrasenia, @Fecha_Registro, @Rol)
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






