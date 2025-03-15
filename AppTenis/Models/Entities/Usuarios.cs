using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppTenis.Models.Entities
{
    public class Usuarios
    {

        public int id { set; get; }
        public string nombre { set; get; }
        public string apellidos { set; get; }
        public string correo { set; get; }
        public string pswd { set; get; }
        public string rol { set; get; }
    }
}