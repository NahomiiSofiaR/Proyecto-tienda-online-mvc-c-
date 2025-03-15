create database Tiendasneakers
go
Use Tiendasneakers
go
-- Tabla Rol---------------------------------------------------------
CREATE TABLE Rol (
    id_Rol INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(50)
);

INSERT INTO Rol VALUES ('Administrador');
INSERT INTO Rol VALUES ('Comprador');
--------------Tabla Usuario ------------------------------------------
go
CREATE TABLE Usuario (
    id_Usu INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(50),
    apellidos VARCHAR(50),
    correo VARCHAR(50),
    pswd VARCHAR(50),
    id_Rol INT,
    CONSTRAINT fk_Rol FOREIGN KEY (id_Rol) REFERENCES Rol(id_Rol)
);

INSERT INTO Usuario (nombre, apellidos, correo, pswd, id_Rol) VALUES ('Nahomi ', 'N', 'nahomin@gmail.com', '111', 1);
INSERT INTO Usuario (nombre, apellidos, correo, pswd, id_Rol) VALUES ('Amayrani', 'A', 'Amayrani@gmail.com', '222', 2);
INSERT INTO Usuario (nombre, apellidos, correo, pswd, id_Rol) VALUES ('Liz', 'L', 'liz@gmail.com', '333', 1);
INSERT INTO Usuario (nombre, apellidos, correo, pswd, id_Rol) VALUES ('Maria', 'M', 'Maria@gmail.com', '444', 2);
INSERT INTO Usuario (nombre, apellidos, correo, pswd, id_Rol) VALUES ('Juan', 'J', 'juan@gmail.com', '555', 1);
INSERT INTO Usuario (nombre, apellidos, correo, pswd, id_Rol) VALUES ('Pedro', 'P', 'pedro@gmail.com', '666', 2);
INSERT INTO Usuario (nombre, apellidos, correo, pswd, id_Rol) VALUES ('Ana', 'A', 'ana@gmail.com', '777', 1);
INSERT INTO Usuario (nombre, apellidos, correo, pswd, id_Rol) VALUES ('Sofia', 'S', 'sofia@gmail.com', '888', 2);
--------------------------------------------Tabla Productos ----------------
	CREATE TABLE Producto (
    id_Prod INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(50),
    descripcion VARCHAR(255),
    precio FLOAT,
    foto VARCHAR(255),
);
GO
-----------------------------------------------------------------------------------------------
INSERT INTO Producto (nombre, descripcion, precio, foto) VALUES ('Nike Air Zoom Vapor X', 'Zapatillas de tenis profesionales con tecnología Zoom Air', 1399.50, 'nikeairzoom.jpg');
INSERT INTO Producto (nombre, descripcion, precio, foto) VALUES ('Nike Court Lite 2', 'Zapatillas de tenis duraderas con suela GDR', 2000.99, 'nikecourt.jpg');
INSERT INTO Producto (nombre, descripcion, precio, foto) VALUES ('Nike Air Zoom Zero', 'Zapatillas de tenis ultraligeras con amortiguación Zoom Air', 1499.99, 'nikezommzero.jpg');
INSERT INTO Producto (nombre, descripcion, precio, foto) VALUES ('Adidas Barricade 2018', 'Zapatillas de tenis duraderas con suela Adiwear', 2099.99, 'adidasbarricade.jpg');
INSERT INTO Producto (nombre, descripcion, precio, foto) VALUES ('Adidas SoleCourt Boost', 'Zapatillas de tenis con tecnología Boost para retorno de energía', 1039.99, 'adidassolecourt.jpg');
INSERT INTO Producto (nombre, descripcion, precio, foto) VALUES ('Adidas Adizero Ubersonic 3', 'Zapatillas de tenis ligeras y ágiles', 3000, 'adidas_adizero_ubersonic_3.jpg');


CREATE TABLE CarritoCompra (
    idCompra INT PRIMARY KEY IDENTITY(1, 1),
    idUsuario INT,
    idProducto INT,
	cantidad int,
    FOREIGN KEY (idUsuario) REFERENCES Usuario(id_usu),
    FOREIGN KEY (idProducto) REFERENCES Producto(id_prod)
);
GO
ALTER TABLE CarritoCompra
ADD cantidad INT;
---------------------------------------------------------------------------------------------------------
CREATE TABLE Compras (
    idProducto INT,
    idUsuario INT,
    cantidad INT,
	fechadeCompra Date,
    FOREIGN KEY (idUsuario) REFERENCES Usuario(id_usu),
    FOREIGN KEY (idProducto ) REFERENCES Producto
	(id_prod)
);
GO

SELECT p.* FROM CarritoCompra c INNER JOIN Producto p ON c.productoId = p.id_prod WHERE c.idUsuario = @idUsuario



select * from Usuario;
select * from Producto;
select * from CarritoCompra;
select * from Compras;
DELETE FROM CarritoCompra;

DELETE FROM Producto
WHERE id_Prod = 105;
select * from Usuario;

insert into Compras (idProducto, idUsuario, cantidad, fechadeCompra) values (1, 4, 2, "2024-04-15T10:29:10");

DELETE FROM Compras;
