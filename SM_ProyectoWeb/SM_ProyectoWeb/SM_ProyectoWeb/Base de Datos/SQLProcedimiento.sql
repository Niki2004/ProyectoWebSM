Use ProyectoWebAvanzada;

------------------------ Registrar Usuario ----------------------

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

------------------------ Iniciar Sesion ----------------------

CREATE PROCEDURE [dbo].[IniciarSesion]
    @Email NVARCHAR(255),
    @Contrasenia NVARCHAR(50)
AS
BEGIN
    SELECT  U.Id_Usuario,
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

------------------------ Desayunos Destacados ----------------------

CREATE PROCEDURE DesayunosDestacados
AS
BEGIN
    SELECT Id_Categoria, Titulo, Ingrediente, Descripcion, Fecha_Publicacion,
	       PlatoReciente, PlatoDestacada, Imagen 
    FROM Receta
    WHERE PlatoDestacada = 1 AND Id_Categoria = 1;
END;
GO

------------------------ Desayunos Recientes ----------------------

CREATE PROCEDURE DesayunosRecientes
AS
BEGIN
    SELECT Id_Categoria, Titulo, Ingrediente, Descripcion, Fecha_Publicacion,
	       PlatoReciente, PlatoDestacada, Imagen
    FROM Receta
    WHERE PlatoReciente = 1 AND Id_Categoria = 1;
END;
GO

------------------------ Entradas Destacados ----------------------

CREATE PROCEDURE EntradasDestacados
AS
BEGIN
    SELECT Id_Categoria, Titulo, Ingrediente, Descripcion, Fecha_Publicacion,
	       PlatoReciente, PlatoDestacada, Imagen
    FROM Receta
    WHERE PlatoDestacada = 1 AND Id_Categoria = 2;
END;
GO

------------------------ Entradas Recientes ----------------------

CREATE PROCEDURE EntradasRecientes
AS
BEGIN
    SELECT Id_Categoria, Titulo, Ingrediente, Descripcion, Fecha_Publicacion,
	       PlatoReciente, PlatoDestacada, Imagen
    FROM Receta
    WHERE PlatoReciente = 1 AND Id_Categoria = 2;
END;
GO

------------------------ Platos Fuertes Destacados ----------------------

CREATE PROCEDURE PlatosFuertesDestacados
AS
BEGIN
    SELECT Id_Categoria, Titulo, Ingrediente, Descripcion, Fecha_Publicacion,
	       PlatoReciente, PlatoDestacada, Imagen
    FROM Receta
    WHERE PlatoDestacada = 1 AND Id_Categoria = 3;
END;
GO

------------------------ Platos Fuertes Recientes ----------------------

CREATE PROCEDURE PlatosFuertesRecientes
AS
BEGIN
    SELECT Id_Categoria, Titulo, Ingrediente, Descripcion, Fecha_Publicacion,
	       PlatoReciente, PlatoDestacada, Imagen
    FROM Receta
    WHERE PlatoReciente = 1 AND Id_Categoria = 3;
END;
GO

------------------------ Postres Destacados ----------------------

CREATE PROCEDURE PostresDestacados
AS
BEGIN
    SELECT Id_Categoria, Titulo, Ingrediente, Descripcion, Fecha_Publicacion,
	       PlatoReciente, PlatoDestacada, Imagen
    FROM Receta
    WHERE PlatoDestacada = 1 AND Id_Categoria = 4;
END;
GO

------------------------ Postres Recientes ----------------------

CREATE PROCEDURE PostresRecientes
AS
BEGIN
    SELECT Id_Categoria, Titulo, Ingrediente, Descripcion, Fecha_Publicacion,
	       PlatoReciente, PlatoDestacada, Imagen
    FROM Receta
    WHERE PlatoReciente = 1 AND Id_Categoria = 4;
END;
GO

------------------------ Registrar Receta ----------------------

CREATE PROCEDURE RegistrarReceta
    @Id_Categoria BIGINT,
    @Titulo NVARCHAR(255),
    @Descripcion NVARCHAR(255),
    @PlatoReciente BIT,
    @PlatoDestacada BIT,
    @Ingrediente NVARCHAR(255),
    @Imagen NVARCHAR(255)
AS
BEGIN
    INSERT INTO [dbo].[Receta] (
        Id_Categoria, Titulo, Descripcion, Fecha_Publicacion, 
        PlatoReciente, PlatoDestacada, Ingrediente, Imagen
    )
    VALUES (
        @Id_Categoria, @Titulo, @Descripcion, GETDATE(), 
        @PlatoReciente, @PlatoDestacada, @Ingrediente, @Imagen
    );
END;


------------------------ Modificar Receta ----------------------

CREATE PROCEDURE ModificarReceta
    @Id_Receta BIGINT,
    @Id_Categoria BIGINT,
    @Titulo NVARCHAR(255),
    @Descripcion NVARCHAR(255),
    @PlatoReciente BIT,
    @PlatoDestacada BIT,
    @Ingrediente NVARCHAR(255),
	@Imagen NVARCHAR(255)
AS
BEGIN

    UPDATE [dbo].[Receta]
    SET
        Id_Categoria = @Id_Categoria,
        Titulo = @Titulo,
        Descripcion = @Descripcion,
        PlatoReciente = @PlatoReciente,
        PlatoDestacada = @PlatoDestacada,
        Ingrediente = @Ingrediente,
		Imagen = @Imagen
    WHERE Id_Receta = @Id_Receta;

END;

------------------------ Eliminar Receta ----------------------

CREATE PROCEDURE EliminarReceta
    @Id_Receta BIGINT
AS
BEGIN
    -- Eliminar la receta correspondiente al Id_Receta
    DELETE FROM [dbo].[Receta]
    WHERE Id_Receta = @Id_Receta;
END;



------------------------ Registrar Comentario ----------------------

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


------------------------ Modificar Comentario ----------------------

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


------------------------ Eliminar Comentario ----------------------

CREATE PROCEDURE EliminarComentario
    @Id_Comentario BIGINT
AS
BEGIN
    DELETE FROM [dbo].[Comentario]
    WHERE Id_Comentario = @Id_Comentario;
END;


------------------------ Consultar Comentario ----------------------

CREATE PROCEDURE [dbo].[ConsultarComentario]
    @Id_Comentario BIGINT,
    @Id_Usuario BIGINT = NULL 
AS
BEGIN

    IF(@Id_Comentario = 0) 
        SET @Id_Comentario = NULL

    SELECT C.Id_Comentario, C.Id_Usuario, C.Id_Receta, R.Titulo, C.Contenido, C.Fecha_Comentario
    FROM Comentario C
    INNER JOIN Receta R ON C.Id_Receta = R.Id_Receta
    WHERE (C.Id_Comentario = @Id_Comentario OR @Id_Comentario IS NULL)
    AND (C.Id_Usuario = @Id_Usuario OR @Id_Usuario IS NULL)
    ORDER BY C.Fecha_Comentario DESC

END


------------------------ Consultar Recetas ----------------------
CREATE PROCEDURE [dbo].[ConsultarRecetas]
AS
BEGIN
    SELECT 
        R.Id_Receta,
        R.Id_Categoria,
        C.Nombre AS Nombre_Categoria, 
        R.Titulo,
        R.Descripcion,
        R.Fecha_Publicacion,
        R.PlatoReciente,
        R.PlatoDestacada,
        R.Ingrediente,
        R.Imagen
    FROM Receta R
    INNER JOIN Categoria C ON R.Id_Categoria = C.Id_Categoria
    ORDER BY R.Fecha_Publicacion DESC
END

------------------------ Registrar Errores ----------------------
CREATE PROCEDURE RegistrarErrores
    @Id_Usuario BIGINT,
	@Descripcion VARCHAR(MAX),
	@Origen VARCHAR(250)
AS
BEGIN
   INSERT INTO [dbo].[Errores] (Id_Usuario, Descripcion, Origen, Fecha_Publicacion)
   VALUES (@Id_Usuario, @Descripcion, @Origen, GETDATE());
END;

------------------------ Obtener Usuario Por Correo Y Contrasenia ----------------------

CREATE PROCEDURE ObtenerUsuarioPorCorreoYContrasenia
    @Email NVARCHAR(255),
    @Contrasenia NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        U.Id_Usuario,
        U.Id_Estado,
        U.Id_Rol,
        U.Nombre,
        U.Email
    FROM 
        Usuario U
    WHERE 
        U.Email = @Email AND U.Contrasenia = @Contrasenia;
END;
GO

------------------------ Obtener Usuario Por Id ----------------------

CREATE PROCEDURE ObtenerUsuarioPorId
    @Id_Usuario BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        U.Id_Usuario,
        U.Id_Estado,
        U.Id_Rol,
        U.Nombre,
        U.Email
    FROM 
        Usuario U
    WHERE 
        U.Id_Usuario = @Id_Usuario;
END;
GO

------------------------ Obtener Rol Por Id ----------------------

CREATE PROCEDURE ObtenerRolePorId
    @Id_Role BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        U.Id_Rol,
        U.Rol
    FROM 
        Roles U
    WHERE 
        U.Id_Rol = @Id_Role;
END;
GO

------evaluaciones----
CREATE PROCEDURE [dbo].[RegistrarValoracion]
    @Id_Usuario BIGINT,
    @Id_Receta BIGINT,
    @Puntuacion INT
AS
BEGIN
    
    IF EXISTS (SELECT 1 FROM Valoracion WHERE Id_Usuario = @Id_Usuario AND Id_Receta = @Id_Receta)
    BEGIN
        RAISERROR('El usuario ya ha valorado esta receta.', 16, 1);
        RETURN;
    END

    
    INSERT INTO dbo.Valoracion (Id_Usuario, Id_Receta, Puntuacion)
    VALUES (@Id_Usuario, @Id_Receta, @Puntuacion);
END
GO

------evaluaciones consultar----

CREATE PROCEDURE [dbo].[ConsultarValoraciones]
    @Id_Usuario BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        V.Id_Valoracion,
        V.Id_Usuario,
        V.Id_Receta,
        V.Puntuacion,
        R.Titulo as Nombre_Receta
    FROM 
        Valoracion V
        INNER JOIN Receta R ON V.Id_Receta = R.Id_Receta
    WHERE 
        V.Id_Usuario = @Id_Usuario
    ORDER BY 
        V.Id_Valoracion DESC;
END
GO


------consultar usuarios----
CREATE PROCEDURE [dbo].[ObtenerUsuariosRegistrados]
AS
BEGIN
    SELECT
        Id_Usuario,
        Nombre,
        Email
    FROM
        Usuario;
END;
GO

------------------------ Eliminar Usuario ----------------------

CREATE PROCEDURE EliminarUsuario
    @Id_Usuario BIGINT
AS
BEGIN
    DELETE FROM [dbo].[Usuario]
    WHERE Id_Usuario = @Id_Usuario;
END;
GO
------------------------ Actualizar Contrasenia Usuario ----------------------

CREATE PROCEDURE ActualizarContrasenia
	@Id_Usuario bigint,
	@Contrasenia varchar(50)
AS
BEGIN
	
	UPDATE dbo.Usuario
	   SET Contrasenia = @Contrasenia
	 WHERE Id_Usuario = @Id_Usuario

END
GO

------------------------ Validar Usuario Por Correo ----------------------

CREATE PROCEDURE [dbo].[ValidarUsuarioCorreo]
	@Email varchar(100)
AS
BEGIN

	SELECT	U.Id_Usuario,
			Id_Estado,
			U.Nombre,
			Email
	FROM	dbo.Usuario U
	WHERE	Email = @Email
	
END
GO