using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppTenis.Models.Entities
{
    public class CompraCliente
    {
            public int IdArticulo { get; set; }
            public string NombreArticulo { get; set; }
            public string NombreCliente { get; set; }
            public int Cantidad { get; set; }
            public DateTime FechaCompra { get; set; }
       
    }
}