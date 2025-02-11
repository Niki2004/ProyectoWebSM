-- Insert Usuario
INSERT INTO Usuario (Nombre, Email, Contrasenia, Rol) VALUES
('Nicole Hidalgo', 'Nicole@Hidalgo.com', '123456', 'Admin'),
('Maricurz Lopez', 'Maricurz@Lopez.com', 'abcdef', 'Admin'),
('Emmanuel Araya', 'Emmanuel@Araya.com', 'pass123', 'Admin'),
('Anderson Ulate', 'Anderson@Ulate.com', 'torresana', 'User'),
('Andres Chavarria', 'Andres@Chavarria.com', 'mendezluis', 'User'),
('Carmel del cielo', 'Carmel@cielo.com', 'ruizelena', 'User');

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
(2, 'Marinar la carne por 12 horas', 1),
(3, 'Lavar y cortar la lechuga', 1),
(4, 'Cocinar la pasta de lasagna', 1),
(5, 'Cortar el salmón en tiras', 1),
(6, 'Extender la masa de pizza', 1);

-- Insertando en Receta_Ingrediente
INSERT INTO Receta_Ingrediente (Id_Receta, Id_Ingrediente, Cantidad) VALUES
(2, 1, 500),
(3, 2, 5),
(4, 3, 4),
(5, 4, 200),
(6, 5, 250),
(6, 6, 300),
(3, 7, 200);

-- Insertando en Comentario
INSERT INTO Comentario (Contenido, Id_Usuario, Id_Receta) VALUES
('Excelente receta, me encantó!', 1, 2),
('Muy fácil de hacer', 2, 3),
('Delicioso!', 3, 3),
('Me salió muy bien', 4, 4),
('A mi familia le encantó', 5, 5),
('Lo hice y quedó perfecto', 6, 6);

-- Insertando en Valoracion
INSERT INTO Valoracion (Puntuacion, Id_Usuario, Id_Receta) VALUES
(5, 1, 2),
(4, 2, 2),
(5, 3, 3),
(3, 4, 4),
(4, 5, 5),
(5, 6, 6);

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
(6, 6);
