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

------------------------ RegistrarReceta ----------------------

CREATE PROCEDURE RegistrarReceta
    @Id_Categoria BIGINT,
    @Titulo NVARCHAR(255),
    @Descripcion NVARCHAR(255),
    @PlatoReciente BIT,
    @PlatoDestacada BIT,
    @Ingrediente NVARCHAR(255)
AS
BEGIN
    INSERT INTO [dbo].[Receta] (
        Id_Categoria, Titulo, Descripcion, Fecha_Publicacion, 
        PlatoReciente, PlatoDestacada, Ingrediente
    )
    VALUES (
        @Id_Categoria, @Titulo, @Descripcion, GETDATE(), 
        @PlatoReciente, @PlatoDestacada, @Ingrediente
    );
END;


------------------------ ModificarReceta ----------------------


CREATE PROCEDURE ModificarReceta
    @Id_Receta BIGINT,
    @Id_Categoria BIGINT,
    @Titulo NVARCHAR(255),
    @Descripcion NVARCHAR(255),
    @PlatoReciente BIT,
    @PlatoDestacada BIT,
    @Ingrediente NVARCHAR(255)
AS
BEGIN

    UPDATE [dbo].[Receta]
    SET
        Id_Categoria = @Id_Categoria,
        Titulo = @Titulo,
        Descripcion = @Descripcion,
        PlatoReciente = @PlatoReciente,
        PlatoDestacada = @PlatoDestacada,
        Ingrediente = @Ingrediente
    WHERE Id_Receta = @Id_Receta;

END;

------------------------ EliminarReceta ----------------------


CREATE PROCEDURE EliminarReceta
    @Id_Receta BIGINT
AS
BEGIN
    -- Eliminar la receta correspondiente al Id_Receta
    DELETE FROM [dbo].[Receta]
    WHERE Id_Receta = @Id_Receta;
END;



------------------------ RegistrarComentario ----------------------

CREATE PROCEDURE RegistrarComentario
    @Id_Usuario BIGINT,
    @Id_Receta BIGINT,
    @Contenido NVARCHAR(255)
AS
BEGIN
    INSERT INTO [dbo].[Comentario] (
        Id_Usuario, Id_Receta, Contenido, Fecha_Comentario
    )
    VALUES (
        @Id_Usuario, @Id_Receta, @Contenido, GETDATE()
    );
END;


------------------------ ModificarComentario ----------------------

CREATE PROCEDURE ModificarComentario
    @Id_Comentario BIGINT,
    @Id_Usuario BIGINT,
    @Id_Receta BIGINT,
    @Contenido NVARCHAR(255)
AS
BEGIN
    UPDATE [dbo].[Comentario]
    SET
        Id_Usuario = @Id_Usuario,
        Id_Receta = @Id_Receta,
        Contenido = @Contenido,
        Fecha_Comentario = GETDATE()  
    WHERE Id_Comentario = @Id_Comentario;
END;


------------------------ EliminarComentario ----------------------

CREATE PROCEDURE EliminarComentario
    @Id_Comentario BIGINT
AS
BEGIN
    -- Eliminar el comentario correspondiente al Id_Comentario
    DELETE FROM [dbo].[Comentario]
    WHERE Id_Comentario = @Id_Comentario;
END;

