using AppTenis.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AppTenis.Models.Sistema.DLL
{
    public class Datos
    {
        private string cadCon;
        public string msgError;

        public Datos()
        {

            msgError = "";
            cadCon = System.Configuration.ConfigurationManager.ConnectionStrings["ccTienda"].ConnectionString;
        }

        public int EjecutaInstr(string insSql, SqlParameter[] pars)
        {
            int numCambios = 0;
            using (SqlConnection Conexion = new SqlConnection(cadCon))
            {
                using (SqlCommand Comando = new SqlCommand(insSql, Conexion))
                {
                    foreach (SqlParameter elemento in pars)
                        Comando.Parameters.Add(elemento);

                    try
                    {
                        Conexion.Open();
                        numCambios = Comando.ExecuteNonQuery();
                    }
                    catch (Exception error)
                    {
                        //Si algo sale mal, se activa este bloque de código
                        msgError = error.Message;
                    }
                }
            }
            return numCambios;
        }

        public int EjecutaSql(string insSql, SqlParameter[] Arr, int Tipo)
        {
            int NumC = 0;// Número de cambios
            using (SqlConnection Conexion = new SqlConnection(cadCon))
            {
                using (SqlCommand Comando = new SqlCommand(insSql, Conexion))
                {
                    if (Arr != null) // Hay parámetros por agregar al Comando
                        foreach (SqlParameter P in Arr)
                            Comando.Parameters.Add(P);

                    if (Tipo == 1) // Consulta de texto plano
                        Comando.CommandType = System.Data.CommandType.Text;
                    else
                        Comando.CommandType = System.Data.CommandType.StoredProcedure;

                    Conexion.Open();
                    NumC = Comando.ExecuteNonQuery();
                }
            }
            return NumC;
        }

        public object DevuelveDato(string insSql, SqlParameter[] pars)
        {
            //DataTable resp = new DataTable();
            object resp = null;
            SqlConnection Conexion = new SqlConnection(cadCon);
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexion;
            Comando.CommandText = insSql;

            foreach (SqlParameter elemento in pars)
                Comando.Parameters.Add(elemento);


            try //Intenta ejecutar este codigo
            {
                Conexion.Open();
                resp = Comando.ExecuteScalar();
                Conexion.Close();
            }
            catch (Exception error)
            {
                //Si algo sale mal, se activa este bloque de codigo
                msgError = error.Message;
            }
            finally
            { //Exista o no un error, siempre se pasa al bloque finally
                Conexion.Close();
            }
            return resp;
        }


        public object ConsultaDato(string insSql, SqlParameter[] Arr, int Tipo)
        {
            object Resp = null;
            using (SqlConnection Conexion = new SqlConnection(cadCon))
            {
                using (SqlCommand Comando = new SqlCommand(insSql, Conexion))
                {
                    if (Arr != null) // Hay parámetros por agregar al Comando
                        foreach (SqlParameter P in Arr)
                            Comando.Parameters.Add(P);

                    if (Tipo == 1) // Consulta de texto plano
                        Comando.CommandType = System.Data.CommandType.Text;
                    else
                        Comando.CommandType = System.Data.CommandType.StoredProcedure;

                    Conexion.Open();
                    Resp = Comando.ExecuteScalar();
                }
            }
            return Resp;
        }

        public List<string> ConsultaColumna(string insSql, SqlParameter[] Arr, int Tipo)
        {
            List<string> Resp = new List<string>();
            using (SqlConnection Conexion = new SqlConnection(cadCon))
            {
                using (SqlCommand Comando = new SqlCommand(insSql, Conexion))
                {
                    if (Arr != null) // Hay parámetros por agregar al Comando
                        foreach (SqlParameter P in Arr)
                            Comando.Parameters.Add(P);

                    if (Tipo == 1) // Consulta de texto plano
                        Comando.CommandType = System.Data.CommandType.Text;
                    else
                        Comando.CommandType = System.Data.CommandType.StoredProcedure;

                    Conexion.Open();
                    SqlDataReader Lector = Comando.ExecuteReader();
                    while (Lector.Read())
                        Resp.Add(Lector[0].ToString());
                }
            }
            return Resp;
        }
        public List<Carrito> ObtenerProductosCarrito(int idUsuario)
        {
            // Construir la consulta SQL para obtener los productos del carrito del usuario
            string consulta = "SELECT c.idCompra, c.idUsuario, c.idProducto, c.cantidad, p.id_prod, p.nombre, p.descripcion, p.precio, p.foto " +
                              "FROM CarritoCompra c INNER JOIN Producto p ON c.idProducto = p.id_prod WHERE c.idUsuario = " + idUsuario;

            // Llamar al método ConsultaProducto con la consulta SQL construida
            List<Carrito> productosCarrito = new List<Carrito>();

            using (SqlConnection conexion = new SqlConnection(cadCon))
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    conexion.Open();
                    SqlDataReader lector = comando.ExecuteReader();
                    while (lector.Read())
                    {
                        Carrito carrito = new Carrito
                        {
                            idCompra = Convert.ToInt32(lector["idCompra"]),
                            idUsuario = Convert.ToInt32(lector["idUsuario"]),
                            idProducto = Convert.ToInt32(lector["idProducto"]),
                            cantidad = Convert.ToInt32(lector["cantidad"]),
                            Producto = new Producto
                            {
                                id = Convert.ToInt32(lector["id_prod"]),
                                nombre = lector["nombre"].ToString(),
                                descripcion = lector["descripcion"].ToString(),
                                precio = Convert.ToSingle(lector["precio"]),
                                foto = lector["foto"].ToString()
                            }
                        };
                        productosCarrito.Add(carrito);
                    }
                }
            }

            return productosCarrito;
        }

        public List<Producto> ConsultaProducto(string insSql, SqlParameter[] Arr, int Tipo)
        {
            List<Producto> Resp = new List<Producto>();
            using (SqlConnection Conexion = new SqlConnection(cadCon))
            {
                using (SqlCommand Comando = new SqlCommand(insSql, Conexion))
                {
                    if (Arr != null) // Hay parámetros por agregar al Comando
                        foreach (SqlParameter P in Arr)
                            Comando.Parameters.Add(P);

                    if (Tipo == 1) // Consulta de texto plano
                        Comando.CommandType = System.Data.CommandType.Text;
                    else
                        Comando.CommandType = System.Data.CommandType.StoredProcedure;

                    Conexion.Open();
                    SqlDataReader Lector = Comando.ExecuteReader();
                    while (Lector.Read())
                        Resp.Add(new Producto()
                        {
                            id = (int)Lector["id_Prod"],
                            nombre = (string)Lector["nombre"],
                            descripcion = (string)Lector["descripcion"],
                            precio = float.Parse(Lector["precio"].ToString()), // Conversión del precio a float
                            foto = (string)Lector["foto"],
                        });
                }
            }
            return Resp;
        }

        public List<Usuarios> ConsultaUsuario(string insSql, SqlParameter[] Arr, int Tipo)
        {
            List<Usuarios> Resp = new List<Usuarios>();
            using (SqlConnection Conexion = new SqlConnection(cadCon))
            {
                using (SqlCommand Comando = new SqlCommand(insSql, Conexion))
                {
                    if (Arr != null) // Hay parámetros por agregar al Comando
                        foreach (SqlParameter P in Arr)
                            Comando.Parameters.Add(P);

                    if (Tipo == 1) // Consulta de texto plano
                        Comando.CommandType = System.Data.CommandType.Text;
                    else
                        Comando.CommandType = System.Data.CommandType.StoredProcedure;

                    Conexion.Open();
                    SqlDataReader Lector = Comando.ExecuteReader();
                    while (Lector.Read())
                        Resp.Add(new Usuarios()
                        {
                            id = (int)Lector["id_Usu"],
                            nombre = (string)Lector["nombre"],
                            apellidos = (string)Lector["apellidos"],
                            correo = (string)Lector["correo"],
                            pswd = (string)Lector["pswd"],
                            rol = Lector["id_Rol"].ToString(),
                        });
                }
            }
            return Resp;
        }

        public Usuarios[] ObtenerUsuarios2(string conSql, string usur, string pass)
        {
            Usuarios[] Resp = new Usuarios[1000]; //------
            int NumDatos = 0;//------
            using (SqlConnection Conexion = new SqlConnection(cadCon))
            {
                using (SqlCommand Command = new SqlCommand())
                {
                    SqlDataReader Lector;
                    Command.Connection = Conexion;

                    SqlParameter Par1 = new SqlParameter("@Nom", usur);
                    SqlParameter Par2 = new SqlParameter("@pws", pass);

                    Command.Parameters.Add(Par1);
                    Command.Parameters.Add(Par2);
                    Command.CommandText = "Select * from usuario where correo=@Nom and pswd = @pws";

                    Command.CommandText = conSql; ///<------- ******

                    Conexion.Open();
                    Lector = Command.ExecuteReader();//------
                    if (Lector.HasRows)
                        while (Lector.Read())
                        {
                            Usuarios newUser = new Usuarios()
                            {
                                //Datos aun no asignados
                                id = Convert.ToInt32(Lector["id_Usu"]),
                                nombre = Lector["nombre"].ToString(),
                                apellidos = Lector["apellidos"].ToString(),
                                correo = Lector["correo"].ToString(),
                                pswd = Lector["pswd"].ToString(),
                                rol = Lector["id_Rol"].ToString(),
                            };
                            Resp[NumDatos] = newUser;
                            NumDatos++;
                        }
                }
            }
            Array.Resize(ref Resp, NumDatos);//Recortar el arreglo

            return Resp;//-------                    
        }

        public List<Producto> ConsultaProducto(string insSql, SqlParameter[] pars)
        {
            List<Producto> resp = new List<Producto>();
            SqlConnection Conexion = new SqlConnection(cadCon); // Verifica que cadCon esté correctamente configurada
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexion;
            Comando.CommandText = insSql;
            foreach (SqlParameter elemento in pars)
                Comando.Parameters.Add(elemento);
            try
            {
                Conexion.Open();
                SqlDataReader renglon = Comando.ExecuteReader();
                while (renglon.Read())
                {
                    // Asegúrate de que los nombres de columna en el lector coincidan exactamente con los nombres de las propiedades de Producto
                    resp.Add(new Producto()
                    {
                        id = (int)renglon["id"],
                        nombre = (string)renglon["nombre"],
                        descripcion = (string)renglon["descripcion"],
                        precio = (int)renglon["precio"],
                        foto = (string)renglon["foto"]
                    });
                }
            }
            catch (Exception error)
            {
                // Si ocurre un error, lanza una excepción o registra el mensaje de error
                throw new Exception("Error al ejecutar la consulta SQL: " + error.Message);
            }
            finally
            {
                if (Conexion.State != ConnectionState.Closed)
                    Conexion.Close();
            }
            return resp;
        }
        public List<Carrito> ConsultaCarrito(string insSql, SqlParameter[] pars)
        {
            List<Carrito> resp = new List<Carrito>();
            SqlConnection Conexion = new SqlConnection(cadCon);

            using (SqlCommand Comando = new SqlCommand(insSql, Conexion))
            {
                foreach (SqlParameter parametro in pars)
                {
                    Comando.Parameters.Add(parametro);
                }

                try
                {
                    Conexion.Open();
                    SqlDataReader renglon = Comando.ExecuteReader();
                    while (renglon.Read())
                    {
                        Producto producto = new Producto
                        {
                            id = renglon["id_prod"] != DBNull.Value ? (int)renglon["id_prod"] : 0,
                            nombre = renglon["nombre"] != DBNull.Value ? renglon["nombre"].ToString() : string.Empty,
                            descripcion = renglon["descripcion"] != DBNull.Value ? renglon["descripcion"].ToString() : string.Empty,
                            precio = renglon["precio"] != DBNull.Value ? Convert.ToSingle(renglon["precio"]) : 0,
                            foto = renglon["foto"] != DBNull.Value ? renglon["foto"].ToString() : string.Empty
                        };

                        Carrito carrito = new Carrito()
                        {
                            idCompra = renglon["idCompra"] != DBNull.Value ? (int)renglon["idCompra"] : 0,
                            idUsuario = renglon["idUsuario"] != DBNull.Value ? (int)renglon["idUsuario"] : 0,
                            idProducto = renglon["idProducto"] != DBNull.Value ? (int)renglon["idProducto"] : 0,
                            cantidad = renglon["cantidad"] != DBNull.Value ? (int)renglon["cantidad"] : 0,
                            Producto = producto
                        };

                        resp.Add(carrito);
                    }
                }
                catch (Exception error)
                {
                    throw new Exception("Error al ejecutar la consulta SQL: " + error.Message);
                }
                finally
                {
                    if (Conexion.State != ConnectionState.Closed)
                        Conexion.Close();
                }
            }

            return resp;
        }

        public void LimpiarCarritoPorUsuario(int idUsuario)
        {
            string consulta = "DELETE FROM CarritoCompra WHERE idUsuario = @idUsuario";

            using (SqlConnection conexion = new SqlConnection(cadCon))
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@idUsuario", idUsuario);
                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }


        public DataTable DevuelveTabla(string insSql, SqlParameter[] pars)
        {
            DataTable resp = new DataTable();
            SqlConnection Conexion = new SqlConnection(cadCon);
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexion;
            Comando.CommandText = insSql;

            foreach (SqlParameter elemento in pars)
                Comando.Parameters.Add(elemento);


            try //Intenta ejecutar este codigo
            {
                SqlDataAdapter DA = new SqlDataAdapter(Comando);
                DA.Fill(resp);
            }
            catch (Exception error)
            {
                //Si algo sale mal, se activa este bloque de codigo
                msgError = error.Message;
            }
            return resp;
        }

        public List<Compra> ObtenerComprasPorCliente(int idCliente)
        {
            List<Compra> compras = new List<Compra>();

            using (SqlConnection conexion = new SqlConnection(cadCon))
            {
                string consulta = "SELECT idProducto, idUsuario, cantidad, fechadeCompra FROM Compras WHERE idUsuario = @idUsuario";
                SqlCommand comando = new SqlCommand(consulta, conexion);
                comando.Parameters.AddWithValue("@idUsuario", idCliente);

                try
                {
                    conexion.Open();
                    SqlDataReader lector = comando.ExecuteReader();

                    while (lector.Read())
                    {
                        Compra compra = new Compra
                        {
                            idProducto = Convert.ToInt32(lector["idProducto"]),
                            idUsuario = Convert.ToInt32(lector["idUsuario"]),
                            cantidad = Convert.ToInt32(lector["cantidad"]),
                            fechadeCompra = Convert.ToDateTime(lector["fechadeCompra"])
                        };

                        compras.Add(compra);
                    }

                    lector.Close();
                }
                catch (Exception ex)
                {
                    // Manejar la excepción adecuadamente
                    throw new Exception("Error al obtener las compras del cliente: " + ex.Message);
                }
            }

            return compras;
        }

        public List<Compra> ConsultaComprasPorCliente(int idCliente)
        {
            // Consulta SQL para obtener las compras por cliente
            string consulta = "SELECT idProducto, idUsuario, cantidad, fechadeCompra FROM Compras WHERE idUsuario = @idCliente";

            // Parámetros de la consulta
            SqlParameter[] parametros = { new SqlParameter("@idCliente", idCliente) };

            // Ejecutar la consulta y mapear los resultados a objetos de tipo Compra
            List<Compra> compras = new List<Compra>();

            using (SqlConnection conexion = new SqlConnection(cadCon))
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddRange(parametros);
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            Compra compra = new Compra
                            {
                                idProducto = (int)lector["idProducto"],
                                idUsuario = (int)lector["idUsuario"],
                                cantidad = (int)lector["cantidad"],
                                fechadeCompra = (DateTime)lector["fechadeCompra"]
                            };
                            compras.Add(compra);
                        }
                    }
                }
            }

            return compras;
        }

        // Método para consultar las compras de los clientes con los nombres de los artículos
        public List<CompraCliente> ConsultarComprasClientesConNombres()
        {
            List<CompraCliente> compras = new List<CompraCliente>();

            using (SqlConnection conexion = new SqlConnection(cadCon))
            {
                string consulta = @"
                SELECT C.idProducto AS IdArticulo, P.nombre AS NombreArticulo, U.nombre AS NombreCliente, C.cantidad, C.fechadeCompra
                FROM Compras C
                INNER JOIN Producto P ON C.idProducto = P.id_Prod
                INNER JOIN Usuario U ON C.idUsuario = U.id_Usu
            ";

                SqlCommand comando = new SqlCommand(consulta, conexion);
                conexion.Open();

                SqlDataReader lector = comando.ExecuteReader();
                while (lector.Read())
                {
                    CompraCliente compra = new CompraCliente
                    {
                        IdArticulo = Convert.ToInt32(lector["IdArticulo"]),
                        NombreArticulo = lector["NombreArticulo"].ToString(),
                        NombreCliente = lector["NombreCliente"].ToString(),
                        Cantidad = Convert.ToInt32(lector["cantidad"]),
                        FechaCompra = Convert.ToDateTime(lector["fechadeCompra"])
                    };
                    compras.Add(compra);
                }
                lector.Close();
            }

            return compras;
        }
        public List<CompraCliente> ConsultaClientesQueCompraronProducto(int idProducto)
        {
            string consulta = @"
        SELECT U.Nombre AS NombreCliente, C.Cantidad, C.fechadeCompra
        FROM Usuario U
        JOIN Compras C ON U.id_Usu = C.idUsuario
        WHERE C.idProducto = @idProducto
    ";

            List<CompraCliente> clientesQueCompraron = new List<CompraCliente>();

            using (SqlConnection conexion = new SqlConnection(cadCon))
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@idProducto", idProducto);
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            CompraCliente cliente = new CompraCliente
                            {
                                NombreCliente = lector["NombreCliente"].ToString(),
                                Cantidad = Convert.ToInt32(lector["Cantidad"]),
                                FechaCompra = Convert.ToDateTime(lector["fechadeCompra"])
                            };
                            clientesQueCompraron.Add(cliente);
                        }
                    }
                }
            }

            return clientesQueCompraron;
        }


        //public List<CompraCliente> ConsultaClientesQueCompraronArticulo(int idArticulo)
        //    {
        //        string consulta = @"
        //    SELECT
        //        A.idArticulo,
        //        A.Nombre AS NombreArticulo,
        //        C.Nombre AS NombreCliente,
        //        CO.Cantidad,
        //        CO.FechaCompra
        //    FROM
        //        Compras CO
        //    INNER JOIN
        //        Clientes C ON CO.idCliente = C.idCliente
        //    INNER JOIN
        //        Articulos A ON CO.idArticulo = A.idArticulo
        //    WHERE
        //        CO.idArticulo = @idArticulo
        //";

        //        List<CompraCliente> clientesQueCompraron = new List<CompraCliente>();

        //        using (SqlConnection conexion = new SqlConnection(cadCon))
        //        {
        //            using (SqlCommand comando = new SqlCommand(consulta, conexion))
        //            {
        //                comando.Parameters.AddWithValue("@idArticulo", idArticulo);
        //                conexion.Open();

        //                using (SqlDataReader lector = comando.ExecuteReader())
        //                {
        //                    while (lector.Read())
        //                    {
        //                        CompraCliente compraCliente = new CompraCliente
        //                        {
        //                            IdArticulo = Convert.ToInt32(lector["idArticulo"]),
        //                            NombreArticulo = lector["NombreArticulo"].ToString(),
        //                            NombreCliente = lector["NombreCliente"].ToString(),
        //                            Cantidad = Convert.ToInt32(lector["Cantidad"]),
        //                            FechaCompra = Convert.ToDateTime(lector["FechaCompra"])
        //                        };
        //                        clientesQueCompraron.Add(compraCliente);
        //                    }
        //                }
        //            }
        //        }

        //        return clientesQueCompraron;
        //    }


        //    public List<(string NombreCliente, int Cantidad, DateTime FechaCompra)> ConsultaClientesPorArticulo(int idArticulo)
        //    {
        //        string consulta = "SELECT C.nombre AS NombreCliente, CO.cantidad AS Cantidad, CO.fechadeCompra AS FechaCompra " +
        //                          "FROM Compras CO " +
        //                          "INNER JOIN Cliente C ON CO.idCliente = C.id " +
        //                          "WHERE CO.idProducto = @idArticulo";

        //        List<(string, int, DateTime)> clientesCompradores = new List<(string, int, DateTime)>();

        //        using (SqlConnection conexion = new SqlConnection(cadCon))
        //        {
        //            using (SqlCommand comando = new SqlCommand(consulta, conexion))
        //            {
        //                comando.Parameters.AddWithValue("@idArticulo", idArticulo);
        //                conexion.Open();

        //                using (SqlDataReader lector = comando.ExecuteReader())
        //                {
        //                    while (lector.Read())
        //                    {
        //                        string nombreCliente = (string)lector["NombreCliente"];
        //                        int cantidad = (int)lector["Cantidad"];
        //                        DateTime fechaCompra = (DateTime)lector["FechaCompra"];

        //                        clientesCompradores.Add((nombreCliente, cantidad, fechaCompra));
        //                    }
        //                }
        //            }
        //        }

        //        return clientesCompradores;
        //    }

        //    public List<ClienteCompra> ObtenerClientesQueHanCompradoProducto(int idProducto)
        //    {
        //        List<ClienteCompra> clientesCompradores = new List<ClienteCompra>();

        //        // Consulta SQL para obtener los clientes que han comprado un producto específico
        //        string consulta = "SELECT u.Nombre AS NombreCliente, c.Cantidad, c.FechaCompra " +
        //                          "FROM Compras c " +
        //                          "INNER JOIN Usuarios u ON c.idUsuario = u.id " +
        //                          "WHERE c.idProducto = @idProducto";

        //        // Parámetros de la consulta
        //        SqlParameter[] parametros = { new SqlParameter("@idProducto", idProducto) };

        //        using (SqlConnection conexion = new SqlConnection(cadCon)) // cadCon es la cadena de conexión
        //        {
        //            using (SqlCommand comando = new SqlCommand(consulta, conexion))
        //            {
        //                comando.Parameters.AddRange(parametros);
        //                conexion.Open();

        //                using (SqlDataReader lector = comando.ExecuteReader())
        //                {
        //                    while (lector.Read())
        //                    {
        //                        ClienteCompra clienteCompra = new ClienteCompra
        //                        {
        //                            NombreCliente = (string)lector["NombreCliente"],
        //                            Cantidad = (int)lector["Cantidad"],
        //                            FechaCompra = (DateTime)lector["FechaCompra"]
        //                        };
        //                        clientesCompradores.Add(clienteCompra);
        //                    }
        //                }
        //            }
        //        }

        //        return clientesCompradores;
        //    }



        //        public List<ClienteCompra> ConsultaClientesPorArticulo(int idArticulo)
        //{
        //    string consulta = "SELECT C.nombre AS NombreCliente, CO.cantidad AS Cantidad, CO.fechadeCompra AS FechaCompra " +
        //                      "FROM Compras CO " +
        //                      "INNER JOIN Cliente C ON CO.idCliente = C.id " +
        //                      "WHERE CO.idProducto = @idArticulo";

        //    List<ClienteCompra> clientesCompradores = new List<ClienteCompra>();

        //    using (SqlConnection conexion = new SqlConnection(cadCon))
        //    {
        //        using (SqlCommand comando = new SqlCommand(consulta, conexion))
        //        {
        //            comando.Parameters.AddWithValue("@idArticulo", idArticulo);
        //            conexion.Open();

        //            using (SqlDataReader lector = comando.ExecuteReader())
        //            {
        //                while (lector.Read())
        //                {
        //                    ClienteCompra clienteCompra = new ClienteCompra
        //                    {
        //                        NombreCliente = (string)lector["NombreCliente"],
        //                        Cantidad = (int)lector["Cantidad"],
        //                        FechaCompra = (DateTime)lector["FechaCompra"]
        //                    };
        //                    clientesCompradores.Add(clienteCompra);
        //                }
        //            }
        //        }
        //    }

        //    return clientesCompradores;
        //}


    }

}