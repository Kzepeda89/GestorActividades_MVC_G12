// GET: Eventos/Delete/5
using GestorActividades_MVC_G12.Models;
using System.Data.SqlClient;
using System.Web.Mvc;

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

// POST: Eventos/Delete/5
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