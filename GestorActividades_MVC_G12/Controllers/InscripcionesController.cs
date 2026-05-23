using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq; // Permite el uso de las consultas declarativas de LINQ
using System.Web.Mvc;
using GestorActividades_MVC_G12.Models;

namespace GestorActividades_MVC_G12.Controllers
{
    public class VistaInscripcion
    {
        public string Carnet { get; set; }
        public string Estudiante { get; set; }
        public string Actividad { get; set; }
        public string Fecha { get; set; }
    }

    public class InscripcionesController : Controller
    {
        // GET: Inscripciones
        public ActionResult Index(string buscar)
        {
            // Seguridad: Si no hay sesión iniciada, redirige al Login
            if (Session["Usuario"] == null) return RedirectToAction("Login", "Acceso");

            List<VistaInscripcion> lista = new List<VistaInscripcion>();
            using (SqlConnection con = Conexion.Conectar())
            {
                // 1. LECTURA BASE CON TU QUERY DE CONFIANZA (ADO.NET)
                string query = @"SELECT i.carnet_estudiante, i.nombre_estudiante, e.nombre_evento, i.fecha_registro 
                                 FROM Inscripciones i 
                                 INNER JOIN Eventos e ON i.id_evento = e.id_evento";

                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new VistaInscripcion
                    {
                        Carnet = dr["carnet_estudiante"].ToString(),
                        Estudiante = dr["nombre_estudiante"].ToString(),
                        Actividad = dr["nombre_evento"].ToString(),
                        Fecha = Convert.ToDateTime(dr["fecha_registro"]).ToString("dd/MM/yyyy")
                    });
                }
                dr.Close();

                // 2. CARGA DEL COMBO PARA EL FORMULARIO DE NUEVA INSCRIPCIÓN
                List<SelectListItem> eventos = new List<SelectListItem>();
                SqlCommand cmdE = new SqlCommand("SELECT id_evento, nombre_evento FROM Eventos", con);
                SqlDataReader drE = cmdE.ExecuteReader();
                while (drE.Read())
                {
                    eventos.Add(new SelectListItem
                    {
                        Text = drE["nombre_evento"].ToString(),
                        Value = drE["id_evento"].ToString()
                    });
                }
                ViewBag.Eventos = eventos;
            }

            // =========================================================================
            // FILTRADO DE REPORTES CON SINTAXIS LINQ TO OBJECTS
            // =========================================================================
            if (!string.IsNullOrEmpty(buscar))
            {
                // Consulta declarativa LINQ pura sobre la lista cargada en memoria
                var resultadoFiltrado = (from i in lista
                                         where i.Carnet.Contains(buscar) ||
                                               i.Estudiante.ToLower().Contains(buscar.ToLower())
                                         select i).ToList();

                ViewBag.Busqueda = buscar; // Retorna el término buscado a la barra
                return View(resultadoFiltrado); // Retorna a la vista el set procesado por LINQ
            }

            return View(lista); // Si no hay filtro, retorna la colección base completa
        }

        // POST: Registrar Inscripción
        [HttpPost]
        public ActionResult Inscribir(string carnet, string nombre, int idEvento)
        {
            using (SqlConnection con = Conexion.Conectar())
            {
                string sql = "INSERT INTO Inscripciones (carnet_estudiante, nombre_estudiante, id_evento, fecha_registro) VALUES (@c, @n, @e, GETDATE())";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@c", carnet);
                cmd.Parameters.AddWithValue("@n", nombre);
                cmd.Parameters.AddWithValue("@e", idEvento);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}