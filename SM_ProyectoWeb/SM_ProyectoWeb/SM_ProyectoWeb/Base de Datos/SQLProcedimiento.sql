Use ProyectoWebAvanzada;

------------------------ RegistrarUsuario ----------------------

CREATE PROCEDURE [dbo].[RegistrarUsuario]
    @Id_Estado bigint = 1,
    @Id_Rol bigint = 1,
    @Nombre NVARCHAR(50),
    @Email NVARCHAR(255),
    @Contrasenia NVARCHAR(50)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Usuario WHERE Email = @Email)
    BEGIN
        INSERT INTO dbo.Usuario (Id_Estado, Id_Rol, Nombre, Email, Contrasenia)
        VALUES (@Id_Estado, @Id_Rol, @Nombre, @Email, @Contrasenia)
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
            U.Id_Rol AS Roles
    FROM    dbo.Usuario U
    WHERE   U.Email = @Email
        AND U.Contrasenia = @Contrasenia
        AND U.Id_Estado = 1
END
GO

------------------------ DesayunosDestacados ----------------------

CREATE PROCEDURE DesayunosDestacados
AS
BEGIN
    SELECT Id_Categoria, Titulo, Ingrediente, Descripcion, Fecha_Publicacion,
	       PlatoReciente, PlatoDestacada
    FROM Receta
    WHERE PlatoDestacada = 0 AND Id_Categoria = 1;
END;
GO

------------------------ DesayunosRecientes ----------------------

CREATE PROCEDURE DesayunosRecientes
AS
BEGIN
    SELECT Id_Categoria, Titulo, Ingrediente, Descripcion, Fecha_Publicacion,
	       PlatoReciente, PlatoDestacada
    FROM Receta
    WHERE PlatoReciente = 1 AND Id_Categoria = 1;
END;
GO


------------------------ EntradasDestacados ----------------------

CREATE PROCEDURE EntradasDestacados
AS
BEGIN
    SELECT Id_Categoria, Titulo, Ingrediente, Descripcion, Fecha_Publicacion,
	       PlatoReciente, PlatoDestacada
    FROM Receta
    WHERE PlatoDestacada = 0 AND Id_Categoria = 2;
END;
GO

------------------------ EntradasRecientes ----------------------

CREATE PROCEDURE EntradasRecientes
AS
BEGIN
    SELECT Id_Categoria, Titulo, Ingrediente, Descripcion, Fecha_Publicacion,
	       PlatoReciente, PlatoDestacada
    FROM Receta
    WHERE PlatoReciente = 1 AND Id_Categoria = 2;
END;
GO

------------------------ PlatosFuertesDestacados ----------------------

CREATE PROCEDURE PlatosFuertesDestacados
AS
BEGIN
    SELECT Id_Categoria, Titulo, Ingrediente, Descripcion, Fecha_Publicacion,
	       PlatoReciente, PlatoDestacada
    FROM Receta
    WHERE PlatoDestacada = 0 AND Id_Categoria = 3;
END;
GO

------------------------ PlatosFuertesRecientes ----------------------

CREATE PROCEDURE PlatosFuertesRecientes
AS
BEGIN
    SELECT Id_Categoria, Titulo, Ingrediente, Descripcion, Fecha_Publicacion,
	       PlatoReciente, PlatoDestacada
    FROM Receta
    WHERE PlatoReciente = 1 AND Id_Categoria = 3;
END;
GO

------------------------ PostresDestacados ----------------------

CREATE PROCEDURE PostresDestacados
AS
BEGIN
    SELECT Id_Categoria, Titulo, Ingrediente, Descripcion, Fecha_Publicacion,
	       PlatoReciente, PlatoDestacada
    FROM Receta
    WHERE PlatoDestacada = 0 AND Id_Categoria = 4;
END;
GO

------------------------ PostresRecientes ----------------------

CREATE PROCEDURE PostresRecientes
AS
BEGIN
    SELECT Id_Categoria, Titulo, Ingrediente, Descripcion, Fecha_Publicacion,
	       PlatoReciente, PlatoDestacada
    FROM Receta
    WHERE PlatoReciente = 1 AND Id_Categoria = 4;
END;
GO










