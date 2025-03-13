CREATE DATABASE ProyectoWebAvanzada

------------------------  TABLAS  ----------------------
CREATE TABLE Estado (
    Id_Estado bigint IDENTITY(1,1) PRIMARY KEY Not null,
    Descripcion NVARCHAR(50) NOT NULL,
);

CREATE TABLE Errores (
    Id_Errores bigint IDENTITY(1,1) PRIMARY KEY Not null,
    Descripcion NVARCHAR(50) NOT NULL,
);

CREATE TABLE Roles (
    Id_Rol bigint IDENTITY(1,1) PRIMARY KEY Not null,
    Rol NVARCHAR(50) NOT NULL,
);

CREATE TABLE Categoria (
    Id_Categoria bigint IDENTITY(1,1) PRIMARY KEY Not null,
    Nombre NVARCHAR(100) UNIQUE NOT NULL
);

CREATE TABLE Usuario (
    Id_Usuario bigint IDENTITY(1,1) PRIMARY KEY Not null,
	Id_Estado bigint NOT NULL, 
    Id_Rol bigint NOT NULL, 
    Nombre NVARCHAR(50) NOT NULL,
    Email NVARCHAR(255) UNIQUE NOT NULL,
    Contrasenia NVARCHAR(50) NOT NULL,
	FOREIGN KEY (Id_Estado) REFERENCES Estado(Id_Estado),
    FOREIGN KEY (Id_Rol) REFERENCES Roles(Id_Rol)
);

CREATE TABLE Receta (
    Id_Receta bigint IDENTITY(1,1) PRIMARY KEY Not null,
    Id_Categoria bigint NOT NULL, 
    Titulo NVARCHAR(255) NOT NULL,
    Ingrediente VARCHAR(255) NOT NULL,
    Descripcion VARCHAR(255) NOT NULL,
    Fecha_Publicacion DATETIME DEFAULT GETDATE(),
    PlatoReciente BIT NOT NULL,
    PlatoDestacada BIT NOT NULL,
    Imagen NVARCHAR(255) NULL
    FOREIGN KEY (Id_Categoria) REFERENCES Categoria(Id_Categoria)
);

CREATE TABLE Paso (
    Id_Paso bigint IDENTITY(1,1) PRIMARY KEY  Not null,
	Id_Receta bigint NOT NULL,
    Descripcion NVARCHAR(255) NOT NULL,
    Numero_Paso INT NOT NULL,
    FOREIGN KEY (Id_Receta) REFERENCES Receta(Id_Receta) 
);

CREATE TABLE Comentario (
    Id_Comentario bigint IDENTITY(1,1) PRIMARY KEY Not null,
	Id_Usuario bigint NULL,
    Id_Receta bigint NULL,
    Contenido NVARCHAR(255) NOT NULL,
    Fecha_Comentario DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (Id_Usuario) REFERENCES Usuario(Id_Usuario) ,
    FOREIGN KEY (Id_Receta) REFERENCES Receta(Id_Receta) 
);

CREATE TABLE Valoracion (
    Id_Valoracion bigint IDENTITY(1,1) PRIMARY KEY Not null,
    Id_Usuario bigint NOT NULL,
    Id_Receta bigint NOT NULL,
	Puntuacion INT,
    FOREIGN KEY (Id_Usuario) REFERENCES Usuario(Id_Usuario),
    FOREIGN KEY (Id_Receta) REFERENCES Receta(Id_Receta)
);

------------------------  RESTRICCIONES  ----------------------
ALTER TABLE [dbo].[Usuario] ADD  CONSTRAINT [Uk_Email] UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Usuario] ADD  CONSTRAINT [Uk_Id_Usuario] UNIQUE NONCLUSTERED 
(
	[Id_Usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
