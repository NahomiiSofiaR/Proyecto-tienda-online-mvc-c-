using AppTenis.Models.Entities;
using AppTenis.Models.Sistema.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static AppTenis.Models.Entities.Compra;

namespace AppTenis.Controllers
{
    public class ProductosController : Controller
    {

        // GET: Productos

        Negocio Operaciones = new Negocio();
        public ActionResult Administrador()
        {
            List<Producto> Lista = Operaciones.Obtenerproductos();
            return View(Lista);
        }
        public ActionResult Catalogo()
        {

            if (Session["User"] == null || ((Usuarios)Session["User"]).rol != "2")
            {
                return RedirectToAction("Index.aspx", "Index.aspx"); // o alguna otra acción para redirigir al usuario a la página de inicio de sesión
            }

            List<Producto> Lista = Operaciones.Obtenerproductos();
            return View(Lista);
        }
        public ActionResult Detalles()
        {
            if (Session["User"] == null || ((Usuarios)Session["User"]).rol != "1")
            {
                return RedirectToAction("Index.aspx", "Index.aspx"); // O redirige a la página de inicio de sesión
            }

            List<Producto> Lista = Operaciones.Obtenerproductos();
            return View(Lista);
        }
        public ActionResult Detalles_Comprador(int id)
        {

            if (Session["User"] == null || ((Usuarios)Session["User"]).rol != "2")
            {
                return RedirectToAction("Index.aspx", "Index.aspx"); // O redirige a la página de inicio de sesión
            }

            Producto producto = Operaciones.Obtenerproducto(id);

            if (producto == null)
            {
                return HttpNotFound(); // Otra acción si el producto no se encuentra
            }

            return View(producto);
        }

        [HttpGet]
        public ActionResult Alta()
        {
            if (Session["User"] == null || ((Usuarios)Session["User"]).rol != "1")
            {
                return RedirectToAction("Index.aspx", "Index.aspx"); // O redirige a la página de inicio de sesión
            }

            return View(new Producto());
        }

        [HttpPost]
        public ActionResult Alta(Producto NewProduc)
        {
            if (Session["User"] == null || ((Usuarios)Session["User"]).rol != "1")
            {
                return RedirectToAction("Index.aspx", "Index.aspx"); // O redirige a la página de inicio de sesión
            }

            if (ModelState.IsValid)
            {
                Operaciones.CreateProducto(NewProduc.nombre, NewProduc.descripcion, NewProduc.precio, NewProduc.foto);
                return RedirectToAction("Administrador");
            }
            else
            {
                return View();
            }
        }
        public ActionResult Edit(int id)
        {
            if (Session["User"] == null || ((Usuarios)Session["User"]).rol != "1")
            {
                return RedirectToAction("Login"); // O redirige a la página de inicio de sesión
            }

            return View(Operaciones.Obtenerproducto(id));
        }

        [HttpPost]
        public ActionResult Edit(Producto EditProduc)
        {
            if (Session["User"] == null || ((Usuarios)Session["User"]).rol != "1")
            {
                return RedirectToAction("Index.aspx", "Index.aspx"); // O redirige a la página de inicio de sesión
            }

            if (ModelState.IsValid)
            {
                Operaciones.AlterProducto(EditProduc.id, EditProduc.nombre, EditProduc.descripcion, EditProduc.precio, EditProduc.foto);
                return RedirectToAction("Administrador");
            }
            else
            {
                return View(EditProduc);
            }
        }



        public ActionResult Delete(int id)
        {
            if (Session["User"] == null || ((Usuarios)Session["User"]).rol != "1")
            {
                return RedirectToAction("Index.aspx", "Index.aspx"); // O redirige a la página de inicio de sesión
            }

            Operaciones.DeleteProducto(id);
            return RedirectToAction("Administrador");
        }

        //--------------------------Carrito--------------------------------------------------------
        [HttpPost]
        public ActionResult AgregarAlCarrito(int id, int cantidad)
        {
            // Verificar si el usuario está autenticado
            if (Session["User"] != null)
            {
                // Obtener el ID del usuario actual desde la sesión
                Usuarios usuario = Session["User"] as Usuarios;
                if (usuario != null && usuario.rol == "2")
                {
                    int idUsuario = usuario.id;

                    // Llamada al método para agregar al carrito
                    Operaciones.AgregarACarrito(id, cantidad, idUsuario);

                    // Redirigir a la acción Carrito en el controlador Productos
                    return RedirectToAction("Carrito", "Productos");
                }
            }

            // Si no está autenticado o no es un cliente, redirigir a la página de inicio de sesión
            return RedirectToAction("Index.aspx", "Inicio");
        }


        public ActionResult Carrito()
        {
            // Verificar si el usuario está autenticado
            if (Session["User"] != null)
            {
                // Obtener el ID del usuario actual desde la sesión
                Usuarios usuario = Session["User"] as Usuarios;
                if (usuario != null)
                {
                    int idUsuario = usuario.id;

                    // Llamar al método para obtener los productos del carrito del usuario
                    List<Carrito> productosCarrito = Operaciones.ObtenerProductosCarrito(idUsuario);

                    // Pasar los productos a la vista
                    return View(productosCarrito);
                }
            }

            // Si no está autenticado, redirigir a la página de inicio de sesión
            return RedirectToAction("Index.aspx", "Index.aspx");
        }

        public ActionResult LimpiarCarrito()
        {
            // Verificar si el usuario está autenticado y obtener el ID de usuario desde la sesión
            if (Session["User"] != null)
            {
                Usuarios usuario = Session["User"] as Usuarios;
                if (usuario != null)
                {
                    int idUsuario = usuario.id;

                    // Llamar al método para limpiar el carrito del usuario
                    Operaciones.LimpiarCarritoPorUsuario(idUsuario);

                    // Redirigir a la página del carrito o a donde sea apropiado
                    return RedirectToAction("Carrito", "Productos");
                }
            }

            // Si no está autenticado, redirigir a la página de inicio de sesión
            return RedirectToAction("Index.aspx", "Index.aspx");
        }



        [HttpPost]
        public ActionResult RealizarCompra()
        {
            // Verificar si el usuario está autenticado
            if (Session["User"] != null)
            {
                // Obtener el ID del usuario actual desde la sesión
                Usuarios usuario = Session["User"] as Usuarios;
                if (usuario != null)
                {
                    int idUsuario = usuario.id;

                    // Llamar al método para obtener los productos del carrito del usuario
                    List<Carrito> productosCarrito = Operaciones.ObtenerProductosCarrito(idUsuario);

                    // Iterar sobre los productos del carrito y guardarlos en la tabla de compras
                    foreach (var item in productosCarrito)
                    {
                        Compra compra = new Compra
                        {
                            idProducto = item.idProducto,
                            idUsuario = idUsuario,
                            cantidad = item.cantidad,
                            fechadeCompra = DateTime.Now // Puedes ajustar la fecha según tus necesidades
                        };

                        // Guardar la compra en la base de datos
                        string resultado = Operaciones.GuardarCompra(compra);
                        // Verificar el resultado de la operación si es necesario
                    }

                    // Redirigir a una página de confirmación de compra
                    return RedirectToAction("CompraExitosa", "Productos");
                }
            }

            // Si no está autenticado, redirigir a la página de inicio de sesión
            return RedirectToAction("Index.aspx", "Index.aspx");
        }

        public ActionResult CompraExitosa()
        {
            return View();
        }


        public ActionResult CancelarCompra()
        {
            // Verificar si el usuario está autenticado
            if (Session["User"] != null)
            {
                // Obtener el ID del usuario actual desde la sesión
                Usuarios usuario = Session["User"] as Usuarios;
                if (usuario != null)
                {
                    int idUsuario = usuario.id;

                    // Llamar al método para cancelar la compra del usuario
                    string mensaje = Operaciones.CancelarCompra(idUsuario);

                    // Redirigir a la acción Cancelada del controlador Productos
                    return RedirectToAction("Cancelada", "Productos", new { mensaje = mensaje });
                }
            }

            // Si no está autenticado, redirigir a la página de inicio de sesión
            return RedirectToAction("Index.aspx", "Index.aspx");
        }
        public ActionResult Cancelada(string mensaje)
        {
            ViewBag.Title = "Compra Cancelada";
            ViewBag.Mensaje = mensaje;
            return View();
        }

        // GET: /Producto/ClientesQueCompraron/5
        public ActionResult ClientesQueCompraron(int id)
        {
            if (id <= 0)
            {
                // Manejar el caso cuando no se proporciona un ID válido
                return RedirectToAction("Index", "Home"); // Redirigir a la página de inicio u otra página adecuada
            }

            // Llamar al método en la capa de negocio para obtener los clientes que compraron el producto
            var clientes = Operaciones.ObtenerClientesQueCompraronProducto(id);

            // Pasar los resultados a la vista
            return View(clientes);
        }





    }





}