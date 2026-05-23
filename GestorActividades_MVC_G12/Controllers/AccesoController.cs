using System;
using System.Data.SqlClient;
using System.Web.Mvc;
using GestorActividades_MVC_G12.Models;

namespace GestorActividades_MVC_G12.Controllers
{
    public class AccesoController : Controller
    {
        public ActionResult Login() { return View(); }

        [HttpPost]
        public ActionResult Login(string usuario, string clave)
        {
            if (usuario == "admin" && clave == "123")
            {
                Session["Usuario"] = usuario;
                return RedirectToAction("DashBoard", "Acceso"); // Te manda al Dashboard
            }
            ViewBag.Error = "Credenciales incorrectas";
            return View();
        }

        // Pantalla de bienvenida
        public ActionResult DashBoard()
        {
            if (Session["Usuario"] == null) return RedirectToAction("Login");

            using (SqlConnection con = Conexion.Conectar())
            {
                SqlCommand cmdEv = new SqlCommand("SELECT COUNT(*) FROM Eventos", con);
                ViewBag.TotalEventos = cmdEv.ExecuteScalar();

                SqlCommand cmdIns = new SqlCommand("SELECT COUNT(*) FROM Inscripciones", con);
                ViewBag.TotalInscritos = cmdIns.ExecuteScalar();
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}