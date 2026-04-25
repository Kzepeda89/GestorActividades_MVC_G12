using System;
using System.Collections.Generic;
using System.Data;
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

        // 2. CREAR
        public ActionResult Create() { return View(); }

        [HttpPost]
        public ActionResult Create(Evento e)
        {
            using (SqlConnection con = Conexion.Conectar())
            {
                string sql = "INSERT INTO Eventos (nombre_evento, id_categoria, cupos_disponibles) VALUES (@nom, 1, @cup)";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@nom", e.NombreEvento);
                cmd.Parameters.AddWithValue("@cup", e.CuposDisponibles);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        // 3. EDITAR
        public ActionResult Edit(int id)
        {
            Evento evento = new Evento();
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
                string sql = "UPDATE Eventos SET nombre_evento=@nom, cupos_disponibles=@cup WHERE id_evento=@id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@nom", e.NombreEvento);
                cmd.Parameters.AddWithValue("@cup", e.CuposDisponibles);
                cmd.Parameters.AddWithValue("@id", e.IdEvento);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        // 4. ELIMINAR (GET para mostrar confirmación)
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

        // 5. CONFIRMAR ELIMINACIÓN (POST)
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
    }
}