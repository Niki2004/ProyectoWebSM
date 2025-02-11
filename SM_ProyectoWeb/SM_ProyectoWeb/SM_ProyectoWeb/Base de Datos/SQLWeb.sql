CREATE TABLE Usuario (
    Id_Usuario INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL,
    Email NVARCHAR(255) UNIQUE NOT NULL,
    Contrasenia NVARCHAR(50) NOT NULL,
    Fecha_Registro DATETIME DEFAULT GETDATE(),
    Rol NVARCHAR(50)
);

CREATE TABLE Receta (
    Id_Receta INT IDENTITY(1,1) PRIMARY KEY,
	Id_Usuario INT NOT NULL,
    Titulo NVARCHAR(255) NOT NULL,
    Descripcion VARCHAR(255) NOT NULL,
    Fecha_Publicacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (Id_Usuario) REFERENCES Usuario(Id_Usuario) ON DELETE CASCADE
);

CREATE TABLE Ingrediente (
    Id_Ingrediente INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) UNIQUE NOT NULL,
    Unidad_Medida NVARCHAR(50) NOT NULL
);

CREATE TABLE Paso (
    Id_Paso INT IDENTITY(1,1) PRIMARY KEY,
	Id_Receta INT NOT NULL,
    Descripcion NVARCHAR(255) NOT NULL,
    Numero_Paso INT NOT NULL,
    FOREIGN KEY (Id_Receta) REFERENCES Receta(Id_Receta) ON DELETE CASCADE
);

CREATE TABLE Receta_Ingrediente (
    Id_Receta INT NOT NULL,
    Id_Ingrediente INT NOT NULL,
    Cantidad DECIMAL(10,2) NOT NULL,
    PRIMARY KEY (Id_Receta, Id_Ingrediente),
    FOREIGN KEY (Id_Receta) REFERENCES Receta(Id_Receta) ON DELETE CASCADE,
    FOREIGN KEY (Id_Ingrediente) REFERENCES Ingrediente(Id_Ingrediente) ON DELETE CASCADE
);

CREATE TABLE Comentario (
    Id_Comentario INT IDENTITY(1,1) PRIMARY KEY,
    Contenido NVARCHAR(255) NOT NULL,
    Fecha_Comentario DATETIME DEFAULT GETDATE(),
    Id_Usuario INT NULL,
    Id_Receta INT NULL,
    FOREIGN KEY (Id_Usuario) REFERENCES Usuario(Id_Usuario) ,
    FOREIGN KEY (Id_Receta) REFERENCES Receta(Id_Receta) 
);


CREATE TABLE Valoracion (
    Id_Valoracion INT IDENTITY(1,1) PRIMARY KEY,
    Puntuacion INT,
    Id_Usuario INT NOT NULL,
    Id_Receta INT NOT NULL,
    FOREIGN KEY (Id_Usuario) REFERENCES Usuario(Id_Usuario),
    FOREIGN KEY (Id_Receta) REFERENCES Receta(Id_Receta)
);

CREATE TABLE Categoria (
    Id_Categoria INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) UNIQUE NOT NULL
);

CREATE TABLE Receta_Categoria (
    Id_Receta INT NOT NULL,
    Id_Categoria INT NOT NULL,
    PRIMARY KEY (Id_Receta, Id_Categoria),
    FOREIGN KEY (Id_Receta) REFERENCES Receta(Id_Receta),
    FOREIGN KEY (Id_Categoria) REFERENCES Categoria(Id_Categoria)
);
