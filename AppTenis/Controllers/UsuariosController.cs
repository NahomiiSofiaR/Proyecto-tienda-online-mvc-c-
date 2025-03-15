using AppTenis.Models.Entities;
using AppTenis.Models.Sistema.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppTenis.Controllers
{
    public class UsuariosController : Controller
    {
        Negocio Operaciones = new Negocio();
        // GET: Usuarios
        public ActionResult VistaUsuario()
        {
            List<Usuarios> Lista = Operaciones.GetU();
            return View(Lista);
        }

        public ActionResult VerComprasClientes()
        {
            // Obtener las compras de los clientes con los nombres de los artículos desde la capa de negocio
            var comprasClientes = Operaciones.ObtenerComprasClientesConNombres();

            // Pasar las compras a la vista para mostrarlas
            return View(comprasClientes);
        }









    }
}