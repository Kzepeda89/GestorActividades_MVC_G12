using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using GestorActividades_MVC_G12.Models;

namespace GestorActividades_MVC_G12.Controllers
{
    public class EventosController : Controller
    {
        // LISTADO PRINCIPAL
        public ActionResult Index()
        {
            List<Evento> lista = new List<Evento>();
            using (SqlConnection con = Conexion.Conectar())
            {
                string sql = "SELECT e.id_evento, e.nombre_evento, c.nombre_categoria, e.cupos_disponibles FROM Eventos e JOIN Categorias c ON e.id_categoria = c.id_categoria";
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new Evento
                    {
                        IdEvento = (int)dr["id_evento"],
                        NombreEvento = dr["nombre_evento"].ToString(),
                        NombreCategoria = dr["nombre_categoria"].ToString(),
                        CuposDisponibles = (int)dr["cupos_disponibles"]
                    });
                }
            }
            return View(lista);
        }

        // VISTA CREAR (Carga el combo de categorías)
        public ActionResult Create()
        {
            List<SelectListItem> categorias = new List<SelectListItem>();
            using (SqlConnection con = Conexion.Conectar())
            {
                string sql = "SELECT id_categoria, nombre_categoria FROM Categorias";
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    categorias.Add(new SelectListItem
                    {
                        Text = dr["nombre_categoria"].ToString(),
                        Value = dr["id_categoria"].ToString()
                    });
                }
            }
            ViewBag.ListaCategorias = categorias;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Evento e)
        {
            using (SqlConnection con = Conexion.Conectar())
            {
                string sql = "INSERT INTO Eventos (nombre_evento, id_categoria, cupos_disponibles) VALUES (@nom, @cat, @cup)";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@nom", e.NombreEvento);
                cmd.Parameters.AddWithValue("@cat", e.IdCategoria);
                cmd.Parameters.AddWithValue("@cup", e.CuposDisponibles);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id) { return View(); }
        public ActionResult Delete(int id) { return View(); }
    }
}