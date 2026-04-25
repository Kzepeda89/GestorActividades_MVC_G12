using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace GestorActividades_MVC_G12.Models
{
    public class Conexion
    {
        public static SqlConnection Conectar()
        {
            // Usamos "CadenaSQL" que es el nombre en el Web.config
            string cadena = ConfigurationManager.ConnectionStrings["CadenaSQL"].ConnectionString;
            SqlConnection con = new SqlConnection(cadena);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            return con;
        }
    }
}