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

CREATE TABLE Usuario (
    Id_Usuario bigint IDENTITY(1,1) PRIMARY KEY Not null,
	Id_Estado bigint NOT NULL, 
    Nombre NVARCHAR(50) NOT NULL,
    Email NVARCHAR(255) UNIQUE NOT NULL,
    Contrasenia NVARCHAR(50) NOT NULL,
    Rol NVARCHAR(50), 
	FOREIGN KEY (Id_Estado) REFERENCES Estado(Id_Estado)
);

CREATE TABLE Receta (
    Id_Receta bigint IDENTITY(1,1) PRIMARY KEY Not null,
	Id_Usuario bigint NOT NULL,
    Titulo NVARCHAR(255) NOT NULL,
    Descripcion VARCHAR(255) NOT NULL,
    Fecha_Publicacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (Id_Usuario) REFERENCES Usuario(Id_Usuario)
);

CREATE TABLE Ingrediente (
    Id_Ingrediente bigint IDENTITY(1,1) PRIMARY KEY Not null,
    Nombre NVARCHAR(100) UNIQUE NOT NULL,
    Unidad_Medida NVARCHAR(50) NOT NULL
);

CREATE TABLE Paso (
    Id_Paso bigint IDENTITY(1,1) PRIMARY KEY  Not null,
	Id_Receta bigint NOT NULL,
    Descripcion NVARCHAR(255) NOT NULL,
    Numero_Paso INT NOT NULL,
    FOREIGN KEY (Id_Receta) REFERENCES Receta(Id_Receta) 
);

CREATE TABLE Receta_Ingrediente (
    Id_Receta_Ingrediente bigint IDENTITY(1,1) PRIMARY KEY NOT NULL,
    Id_Receta bigint NOT NULL,
    Id_Ingrediente bigint NOT NULL,
    Cantidad DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (Id_Receta) REFERENCES Receta(Id_Receta) ON DELETE CASCADE,
    FOREIGN KEY (Id_Ingrediente) REFERENCES Ingrediente(Id_Ingrediente) ON DELETE CASCADE
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

CREATE TABLE Categoria (
    Id_Categoria bigint IDENTITY(1,1) PRIMARY KEY Not null,
    Nombre NVARCHAR(100) UNIQUE NOT NULL
);

CREATE TABLE Receta_Categoria (
    Id_Receta_Categoria bigint IDENTITY(1,1) PRIMARY KEY Not null,
    Id_Receta bigint NOT NULL,
    Id_Categoria bigint NOT NULL,
    FOREIGN KEY (Id_Receta) REFERENCES Receta(Id_Receta),
    FOREIGN KEY (Id_Categoria) REFERENCES Categoria(Id_Categoria)
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

