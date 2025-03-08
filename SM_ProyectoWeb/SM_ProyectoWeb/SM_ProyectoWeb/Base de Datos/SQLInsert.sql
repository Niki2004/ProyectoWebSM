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
INSERT INTO [ProyectoWebAvanzada].[dbo].[Receta] 
(Id_Categoria, Titulo, Ingrediente, Descripcion, Fecha_Publicacion, PlatoReciente, PlatoDestacada)
VALUES
(1, 'Desayuno Saludable', 'Avena, Frutas', 'Un desayuno energético con avena, frutas frescas y miel.', '2025-03-08', 1, 1),
(1, 'Tostadas con Aguacate', 'Pan integral, Aguacate, Huevo', 'Tostadas con aguacate y huevo revuelto, acompañadas de un jugo natural.', '2025-03-08', 1, 0),
(1, 'Batido de Frutas', 'Frutillas, Plátano, Yogur', 'Un batido refrescante de frutas frescas, ideal para el desayuno.', '2025-03-08', 0, 1);

INSERT INTO [ProyectoWebAvanzada].[dbo].[Receta] 
(Id_Categoria, Titulo, Ingrediente, Descripcion, Fecha_Publicacion, PlatoReciente, PlatoDestacada)
VALUES
(2, 'Ensalada César', 'Lechuga, Pollo, Queso', 'Ensalada fresca con pollo a la parrilla, queso parmesano y aderezo César.', '2025-03-08', 1, 0),
(2, 'Bruschetta', 'Pan, Tomate, Albahaca', 'Pan tostado con tomate fresco, albahaca y un toque de aceite de oliva.', '2025-03-08', 0, 1),
(2, 'Sopa de Tomate', 'Tomate, Ajo, Cebolla', 'Sopa cremosa de tomate, con un toque de albahaca y crema fresca.', '2025-03-08', 1, 0);

INSERT INTO [ProyectoWebAvanzada].[dbo].[Receta] 
(Id_Categoria, Titulo, Ingrediente, Descripcion, Fecha_Publicacion, PlatoReciente, PlatoDestacada)
VALUES
(3, 'Pollo al Horno', 'Pollo, Papas, Especias', 'Pollo asado al horno con papas y un toque de especias.', '2025-03-08', 1, 1),
(3, 'Tacos de Carne', 'Carne, Tortillas, Cilantro', 'Tacos con carne de res, cebolla, cilantro y salsa picante.', '2025-03-08', 0, 0),
(3, 'Paella de Mariscos', 'Arroz, Mariscos, Azafrán', 'Paella de mariscos con arroz, azafrán y verduras.', '2025-03-08', 1, 0);

INSERT INTO [ProyectoWebAvanzada].[dbo].[Receta] 
(Id_Categoria, Titulo, Ingrediente, Descripcion, Fecha_Publicacion, PlatoReciente, PlatoDestacada)
VALUES
(4, 'Tarta de Manzana', 'Manzana, Masa, Azúcar', 'Tarta de manzana casera con una capa crujiente de azúcar.', '2025-03-08', 0, 1),
(4, 'Brownie de Chocolate', 'Chocolate, Harina, Nueces', 'Brownie de chocolate con nueces, suave por dentro y crujiente por fuera.', '2025-03-08', 1, 0),
(4, 'Helado de Vainilla', 'Leche, Azúcar, Vainilla', 'Helado casero de vainilla, cremoso y delicioso.', '2025-03-08', 0, 0);


-- InsertPaso
INSERT INTO Paso (Id_Receta, Descripcion, Numero_Paso) VALUES
(1, 'Marinar la carne por 12 horas', 1),
(2, 'Lavar y cortar la lechuga', 1),
(3, 'Cocinar la pasta de lasagna', 1),
(4, 'Cortar el salmón en tiras', 1),
(5, 'Extender la masa de pizza', 1);


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
-- Desayunos
INSERT INTO Categoria (Nombre)
VALUES
('Desayunos'),
('Entradas'),
('Platos Fuertes'),
('Postres');
