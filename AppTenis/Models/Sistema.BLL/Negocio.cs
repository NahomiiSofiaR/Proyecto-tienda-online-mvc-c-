using AppTenis.Models.Entities;
using AppTenis.Models.Sistema.DLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using static AppTenis.Models.Entities.Compra;

namespace AppTenis.Models.Sistema.BLL
{
    public class Negocio
    {
        Datos objDatos = new Datos();
        Usuarios persona = new Usuarios();
        //Logica de los controllers

        public List<Producto> Obtenerproductos()
        {
            return objDatos.ConsultaProducto("Select * from Producto ", null, 1);
        }

        public Producto Obtenerproducto(int id)
        {
            SqlParameter[] Arr = { new SqlParameter("@id", id) };
            List<Producto> R = objDatos.ConsultaProducto("Select * from Producto where id_Prod = @id", Arr, 1);
            if (R.Count == 0)
                return null;
            else
                return R[0];
        }
        public List<Usuarios> GetU()
        {
            return objDatos.ConsultaUsuario("Select * from usuario ", null, 1);
        }

        public Usuarios GetUs(int id)
        {
            SqlParameter[] Arr = { new SqlParameter("@id", id) };
            List<Usuarios> R = objDatos.ConsultaUsuario("Select * from usuario where id_Usu = @id", Arr, 1);
            if (R.Count == 0)
                return null;
            else
                return R[0];
        }

        public string CreateProducto(string nm, string descrip, float price, string foto)
        {
            SqlParameter[] Arr = {

            new SqlParameter("@nm", nm),
            new SqlParameter("@descrip", descrip),
            new SqlParameter("@price", price),
            new SqlParameter("@foto", foto)};

            int Nc = objDatos.EjecutaSql("insert into Producto values(@nm,@descrip,@price,@foto)", Arr, 1);
            if (Nc == 0)
                return "Nose pudo dar de alta";
            else
                return "Los datos se agregaron de forma existosa";
        }
        public string AlterProducto(int id, string nm, string descrip, float price, string foto)
        {
            SqlParameter[] Arr = {
            new SqlParameter("@id", id),
            new SqlParameter("@nm", nm),
            new SqlParameter("@descrip", descrip),
            new SqlParameter("@price", price),
            new SqlParameter("@foto", foto)
            };

            int Nc = objDatos.EjecutaSql("update Producto set nombre=@nm,descripcion=@descrip,precio=@price,foto=@foto where id_Prod=@id", Arr, 1);
            if (Nc == 0)
                return "Nose pudo modificar el prodcuto";
            else
                return "Los datos se modificaron de forma existosa";
        }
        public void DeleteProducto(int Id)
        {
            SqlParameter[] Arr = {
                new SqlParameter("@id", Id),
            };
            objDatos.EjecutaSql("delete Producto where id_Prod = @id", Arr, 1);

        }

        //------------------------------------------------------------Apartado de logica-------------------------------------------------------------//
        public Usuarios Validar(string Nom, string Psw)
        {
            string conSql = "select * from usuario where correo ='" + Nom + "' and pswd = '" + Psw + "'";
            Usuarios[] Resp = objDatos.ObtenerUsuarios2(conSql, Nom, Psw);
            if (Resp.Length > 0)
                return Resp[0];
            else
                return null;
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------//
        public void AgregarACarrito(int idProducto, int cantidad, int idUsuario)
        {
            // Insertar el producto en el carrito del usuario en la base de datos
            string consultaInsertar = "INSERT INTO CarritoCompra(idUsuario, idProducto, cantidad) VALUES(@idUsuario, @idProducto, @cantidad)";
            SqlParameter[] parametrosInsertar = {
        new SqlParameter("@idUsuario", idUsuario),
        new SqlParameter("@idProducto", idProducto),
        new SqlParameter("@cantidad", cantidad)
    };
            objDatos.EjecutaInstr(consultaInsertar, parametrosInsertar);
        }

        public List<Carrito> ObtenerProductosCarrito(int idUsuario)
        {
            // Construir la consulta SQL para obtener los productos del carrito del usuario
            string consulta = @"SELECT c.idCompra, c.idUsuario, c.idProducto, c.cantidad, p.id_prod, p.nombre, p.descripcion, p.precio, p.foto
                         FROM CarritoCompra c
                         INNER JOIN Producto p ON c.idProducto = p.id_prod
                         WHERE c.idUsuario = @idUsuario";

            // Parámetro para el ID del usuario en la consulta SQL
            SqlParameter parametroIdUsuario = new SqlParameter("@idUsuario", idUsuario);

            // Llamar al método ConsultaCarrito con la consulta SQL y el parámetro definido
            List<Carrito> productosCarrito = objDatos.ConsultaCarrito(consulta, new SqlParameter[] { parametroIdUsuario });

            // Retornar los productos en el carrito del usuario
            return productosCarrito;
        }


        public string GuardarCompra(Compra compra)
        {
            try
            {
                SqlParameter[] parametros =
                {
            new SqlParameter("@idUsuario", compra.idUsuario),
            new SqlParameter("@idProducto", compra.idProducto),
            new SqlParameter("@cantidad", compra.cantidad),
            new SqlParameter("@fechaCompra", compra.fechadeCompra)
        };

                // Llama al método EjecutaInstr de tu capa de datos para insertar en la tabla Compras
                int filasAfectadas = objDatos.EjecutaInstr("INSERT INTO Compras (idProducto, idUsuario, cantidad, fechadeCompra) VALUES (@idProducto, @idUsuario, @cantidad, @fechaCompra)", parametros);

                // Verifica si se insertó correctamente
                if (filasAfectadas > 0)
                {
                    return "Compra realizada con éxito.";
                }
                else
                {
                    return "No se pudo realizar la compra correctamente.";
                }
            }
            catch (Exception ex)
            {
                return "Error al realizar la compra: " + ex.Message;
            }
        }

        public string CancelarCompra(int idUsuario)
        {
            try
            {
                // Crear una instancia de Datos dentro del método
                Datos objDatos = new Datos();

                // Llama al método EjecutaInstr de tu capa de datos para eliminar los registros de la tabla Compras
                SqlParameter parametroIdUsuario = new SqlParameter("@idUsuario", idUsuario);
                int filasAfectadas = objDatos.EjecutaInstr("DELETE FROM Compras WHERE idUsuario = @idUsuario", new SqlParameter[] { parametroIdUsuario });

                // Verifica si se eliminaron los registros correctamente
                if (filasAfectadas > 0)
                {
                    return "La compra ha sido cancelada con éxito.";
                }
                else
                {
                    return "No se pudo cancelar la compra.";
                }
            }
            catch (Exception ex)
            {
                return "Error al cancelar la compra: " + ex.Message;
            }
        }
        public List<CompraCliente> ObtenerComprasClientesConNombres()
        {
            // Llama al método correspondiente en tu capa de datos para obtener las compras con nombres
            List<CompraCliente> compras = objDatos.ConsultarComprasClientesConNombres();
            return compras;
        }

        public List<CompraCliente> ObtenerClientesQueCompraronProducto(int idProducto)
        {
            return objDatos.ConsultaClientesQueCompraronProducto(idProducto);
        }
        public void LimpiarCarritoPorUsuario(int idUsuario)
        {
             objDatos.LimpiarCarritoPorUsuario(idUsuario);
        }


    }
}
