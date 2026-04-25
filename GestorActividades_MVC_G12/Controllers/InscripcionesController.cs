using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        public ActionResult Index()
        {
            List<VistaInscripcion> lista = new List<VistaInscripcion>();
            using (SqlConnection con = Conexion.Conectar())
            {
                // Usamos el query EXACTO de tu proyecto anterior que sí funcionaba
                string query = @"SELECT i.carnet_estudiante, i.nombre_estudiante, e.nombre_evento, i.fecha_registro 
                                 FROM Inscripciones i 
                                 INNER JOIN Eventos e ON i.id_evento = e.id_evento";

                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new VistaInscripcion
                    {
                        // Mapeamos con los nombres reales de tu tabla (image_e11659.png)
                        Carnet = dr["carnet_estudiante"].ToString(),
                        Estudiante = dr["nombre_estudiante"].ToString(),
                        Actividad = dr["nombre_evento"].ToString(),
                        Fecha = dr["fecha_registro"] != DBNull.Value
                                ? Convert.ToDateTime(dr["fecha_registro"]).ToString("dd/MM/yyyy")
                                : "N/A"
                    });
                }
            }
            return View(lista);
        }
    }
}