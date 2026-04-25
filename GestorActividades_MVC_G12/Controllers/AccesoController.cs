using System.Web.Mvc;

namespace GestorActividades_MVC_G12.Controllers
{
    public class AccesoController : Controller
    {
        public ActionResult Login() { return View(); }

        [HttpPost]
        public ActionResult Login(string usuario, string clave)
        {
            // Validamos con la que acordamos: admin / 123
            if (usuario == "admin" && clave == "123")
            {
                Session["Usuario"] = usuario;
                return RedirectToAction("Index", "Eventos");
            }
            ViewBag.Error = "Credenciales incorrectas";
            return View();
        }

        // Método para cerrar la sesión
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}