using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppTenis.Models.Entities
{
    public class Carrito
    {
        public int idCompra { get; set; }
        public int idUsuario { get; set; }
        public int idProducto { get; set; }
        public Producto Producto { get; set; }
        public int cantidad { get; set; }
    }

}