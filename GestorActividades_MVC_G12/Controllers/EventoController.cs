using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using GestorActividades_MVC_G12.Models;

namespace GestorActividades_MVC_G12.Controllers
{
    public class EventosController : Controller
    {
        // 1. LISTADO PRINCIPAL
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

        // 2. CREAR (GET)
        public ActionResult Create()
        {
            CargarCategorias();
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

        // 3. EDITAR (GET) - Muestra qué estás editando
        public ActionResult Edit(int id)
        {
            Evento evento = new Evento();
            CargarCategorias();
            using (SqlConnection con = Conexion.Conectar())
            {
                string sql = "SELECT * FROM Eventos WHERE id_evento = @id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    evento.IdEvento = (int)dr["id_evento"];
                    evento.NombreEvento = dr["nombre_evento"].ToString();
                    evento.IdCategoria = (int)dr["id_categoria"];
                    evento.CuposDisponibles = (int)dr["cupos_disponibles"];
                }
            }
            return View(evento);
        }

        [HttpPost]
        public ActionResult Edit(Evento e)
        {
            using (SqlConnection con = Conexion.Conectar())
            {
                string sql = "UPDATE Eventos SET nombre_evento=@nom, id_categoria=@cat, cupos_disponibles=@cup WHERE id_evento=@id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@nom", e.NombreEvento);
                cmd.Parameters.AddWithValue("@cat", e.IdCategoria);
                cmd.Parameters.AddWithValue("@cup", e.CuposDisponibles);
                cmd.Parameters.AddWithValue("@id", e.IdEvento);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        // 4. ELIMINAR (GET) - Pantalla de confirmación
        public ActionResult Delete(int id)
        {
            Evento evento = new Evento();
            using (SqlConnection con = Conexion.Conectar())
            {
                string sql = "SELECT e.id_evento, e.nombre_evento, c.nombre_categoria, e.cupos_disponibles FROM Eventos e JOIN Categorias c ON e.id_categoria = c.id_categoria WHERE e.id_evento = @id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    evento.IdEvento = (int)dr["id_evento"];
                    evento.NombreEvento = dr["nombre_evento"].ToString();
                    evento.NombreCategoria = dr["nombre_categoria"].ToString();
                    evento.CuposDisponibles = (int)dr["cupos_disponibles"];
                }
            }
            return View(evento);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection con = Conexion.Conectar())
            {
                string sql = "DELETE FROM Eventos WHERE id_evento = @id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        // Helper para cargar el combo de categorías
        private void CargarCategorias()
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
        }
    }
}