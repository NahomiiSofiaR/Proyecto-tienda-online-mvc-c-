using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppTenis.Models.Entities
{
    public class CompraConCliente
    {
        public Compra Compra { get; set; }

        public Usuarios Cliente { get; set; }
    }
}