using AppTenis.Models.Entities;
using AppTenis.Models.Sistema.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppTenis
{
    public partial class Index : System.Web.UI.Page
    {
        Negocio Valida = new  Negocio();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string name = txbCorreo_Electronico.Text;
            string contra = txbPassword.Text;

            Usuarios R = Valida.Validar(name, contra);

            if (R == null)
            {
                Response.Write("<script>alert('Usuario inválido!!!')</script>");
            }
            else
            {
                // Usuario válido
                Response.Write("<script>alert('Usuario Correcto')</script>");
                Session["User"] = R;

                // Asignar roles y redirigir según el rol
                switch (R.rol)
                {
                    case "1": // Admin
                        Response.Redirect("~/Productos/Administrador");
                        break;

                    case "2": // Cliente
                        Response.Redirect("~/Productos/Catalogo");
                        break;

                    default:
                        // Otros roles o manejo de errores
                        break;
                }
            }
        }

    }
}
