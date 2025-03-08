USE ProyectoWebAvanzada;

-- Insert Estado
INSERT INTO Estado (Descripcion) VALUES
('Activo'), ('Inactivo');

INSERT INTO Roles (Rol) VALUES
('Usuario'), ('Administrador');

-- Insert Usuario
INSERT INTO Usuario (Nombre, Email, Contrasenia, Id_Rol, Id_Estado) VALUES
('Nicole Hidalgo', 'Nicole@Hidalgo.com', '123456', 2, 1),
('Maricurz Lopez', 'Maricurz@Lopez.com', 'abcdef', 2, 1),
('Emmanuel Araya', 'Emmanuel@Araya.com', 'pass123', 2, 1),
('Anderson Ulate', 'Anderson@Ulate.com', 'torresana', 1, 1),
('Andres Chavarria', 'Andres@Chavarria.com', 'mendezluis', 1, 1),
('Carmel del cielo', 'Carmel@cielo.com', 'ruizelena', 1, 2);

-- Insert Receta
INSERT INTO Receta (Id_Usuario, Titulo, Descripcion) VALUES
(1, 'Tacos al Pastor', 'Tacos con carne de cerdo marinada'),
(2, 'Ensalada César', 'Ensalada con aderezo César y pollo'),
(3, 'Lasagna Boloñesa', 'Lasagna con salsa boloñesa y queso'),
(4, 'Sushi Roll', 'Roll de sushi con salmón y aguacate'),
(5, 'Pizza Margarita', 'Pizza con tomate, mozzarella y albahaca');

-- Insert Ingrediente
INSERT INTO Ingrediente (Nombre, Unidad_Medida) VALUES
('Carne de cerdo', 'gramos'),
('Lechuga', 'hojas'),
('Pasta de lasagna', 'unidad'),
('Salmón', 'gramos'),
('Masa de pizza', 'gramos'),
('Chocolate', 'gramos'),
('Pescado', 'gramos'),
('Arroz', 'gramos');

-- InsertPaso
INSERT INTO Paso (Id_Receta, Descripcion, Numero_Paso) VALUES
(1, 'Marinar la carne por 12 horas', 1),
(2, 'Lavar y cortar la lechuga', 1),
(3, 'Cocinar la pasta de lasagna', 1),
(4, 'Cortar el salmón en tiras', 1),
(5, 'Extender la masa de pizza', 1);

-- Insertando en Receta_Ingrediente
INSERT INTO Receta_Ingrediente (Id_Receta, Id_Ingrediente, Cantidad) VALUES
(1, 1, 500),
(2, 2, 5),
(3, 3, 4),
(4, 4, 200),
(5, 5, 250),
(2, 6, 300),
(3, 7, 200);

-- Insertando en Comentario
INSERT INTO Comentario (Contenido, Id_Usuario, Id_Receta) VALUES
('Excelente receta, me encantó', 1, 2),
('Muy fácil de hacer', 2, 3),
('Delicioso', 3, 3),
('Me salió muy bien', 4, 4),
('A mi familia le encantó', 5, 5),
('Lo hice y quedó perfecto', 5, 5);

-- Insertando en Valoracion
INSERT INTO Valoracion (Puntuacion, Id_Usuario, Id_Receta) VALUES
(5, 1, 2),
(4, 2, 2),
(5, 3, 3),
(3, 4, 4),
(4, 5, 5),
(5, 6, 5);

-- Insertando en Categoria
INSERT INTO Categoria (Nombre) VALUES
('Mexicana'),
('Ensaladas'),
('Pastas'),
('Japonesa'),
('Pizzas'),
('Postres'),
('Mariscos'),
('Española');

-- Insertando en Receta_Categoria
INSERT INTO Receta_Categoria (Id_Receta, Id_Categoria) VALUES
(2, 2),
(3, 3),
(4, 4),
(5, 5),
(5, 6);
