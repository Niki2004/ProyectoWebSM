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
(Id_Categoria, Titulo, Ingrediente, Descripcion, Fecha_Publicacion, PlatoReciente, PlatoDestacada, Imagen)
VALUES
(1, 'Desayuno Saludable', 'Avena, Frutas', 'Un desayuno energético con avena, frutas frescas y miel.', '2025-03-08', 1, 1, 'https://images.pexels.com/photos/1099680/pexels-photo-1099680.jpeg?auto=compress&cs=tinysrgb&w=600'),
(1, 'Tostadas con Aguacate', 'Pan integral, Aguacate, Huevo', 'Tostadas con aguacate y huevo revuelto, acompañadas de un jugo natural.', '2025-03-08', 1, 0, 'https://images.pexels.com/photos/236813/pexels-photo-236813.jpeg?auto=compress&cs=tinysrgb&w=600'),
(1, 'Batido de Frutas', 'Frutillas, Plátano, Yogur', 'Un batido refrescante de frutas frescas, ideal para el desayuno.', '2025-03-08', 0, 1, 'https://images.pexels.com/photos/7936980/pexels-photo-7936980.jpeg?auto=compress&cs=tinysrgb&w=600');

INSERT INTO [ProyectoWebAvanzada].[dbo].[Receta] 
(Id_Categoria, Titulo, Ingrediente, Descripcion, Fecha_Publicacion, PlatoReciente, PlatoDestacada, Imagen)
VALUES
(2, 'Ensalada César', 'Lechuga, Pollo, Queso', 'Ensalada fresca con pollo a la parrilla, queso parmesano y aderezo César.', '2025-03-08', 1, 0,'https://img.freepik.com/fotos-premium/ensalada-cesar-fresca-croutons-pollo-parrilla-queso-parmesano-salpicado-aderezo-cesar_1354780-3801.jpg' ),
(2, 'Bruschetta', 'Pan, Tomate, Albahaca', 'Pan tostado con tomate fresco, albahaca y un toque de aceite de oliva.', '2025-03-08', 0, 1, 'https://i0.wp.com/www.simplerojo.com/wp-content/uploads/2024/08/20180916_154520.jpg?fit=2048%2C1536&ssl=1'),
(2, 'Sopa de Tomate', 'Tomate, Ajo, Cebolla', 'Sopa cremosa de tomate, con un toque de albahaca y crema fresca.', '2025-03-08', 1, 0, 'https://www.recetasnestle.com.ar/sites/default/files/srh_recipes/41d0d43e53b03fc14ebcfde36833ad93.jpg' );

INSERT INTO [ProyectoWebAvanzada].[dbo].[Receta] 
(Id_Categoria, Titulo, Ingrediente, Descripcion, Fecha_Publicacion, PlatoReciente, PlatoDestacada, Imagen)
VALUES
(3, 'Pollo al Horno', 'Pollo, Papas, Especias', 'Pollo asado al horno con papas y un toque de especias.', '2025-03-08', 1, 1,'https://cocinemosjuntos.com.co/media/mageplaza/blog/post/t/i/tips-para-preparar-pollo-al-horno-jugoso-y-perfecto_1_.jpg' ),
(3, 'Tacos de Carne', 'Carne, Tortillas, Cilantro', 'Tacos con carne de res, cebolla, cilantro y salsa picante.', '2025-03-08', 0, 0,'https://www.goya.com/wp-content/uploads/2023/10/carne-asada-tacos1.jpg' ),
(3, 'Paella de Mariscos', 'Arroz, Mariscos, Azafrán', 'Paella de mariscos con arroz, azafrán y verduras.', '2025-03-08', 1, 0,'https://content-cocina.lecturas.com/medio/2022/09/15/paella-de-marisco_00000000_240903144336_1200x1200.jpg');

INSERT INTO [ProyectoWebAvanzada].[dbo].[Receta] 
(Id_Categoria, Titulo, Ingrediente, Descripcion, Fecha_Publicacion, PlatoReciente, PlatoDestacada,imagen)
VALUES
(4, 'Tarta de Manzana', 'Manzana, Masa, Azúcar', 'Tarta de manzana casera con una capa crujiente de azúcar.', '2025-03-08', 0, 1, 'https://www.recetassinlactosa.com/wp-content/uploads/2023/11/Tarta-de-manzana-americana.jpg'),
(4, 'Brownie de Chocolate', 'Chocolate, Harina, Nueces', 'Brownie de chocolate con nueces, suave por dentro y crujiente por fuera.', '2025-03-08', 1, 0, 'https://images.aws.nestle.recipes/original/2024_10_23T08_38_28_badun_images.badun.es_ac5fa47c04dd_brownie_de_chocolate_negro.jpg'),
(4, 'Helado de Vainilla', 'Leche, Azúcar, Vainilla', 'Helado casero de vainilla, cremoso y delicioso.', '2025-03-08', 0, 0, 'https://www.recetasnestle.com.do/sites/default/files/srh_recipes/62099096785a3c939a1a1eefb06bf358.jpg');

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

--En caso de hacer corrido la tabla de recetas sin imagen corran este comando 
ALTER TABLE Receta
ADD Imagen NVARCHAR(255) NULL;
