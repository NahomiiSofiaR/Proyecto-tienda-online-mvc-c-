using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppTenis.Models.Entities
{
    public class Compra
    {
        public int idProducto { get; set; }
        public int idUsuario { get; set; }
        public int cantidad { get; set; }
        public DateTime fechadeCompra { get; set; }

        // Propiedades adicionales para detalles del producto comprado
        public string NombreProducto { get; set; }
        public string DescripcionProducto { get; set; }
        public decimal PrecioProducto { get; set; }
    }
}