using System.Web;
using System.Web.Mvc;

namespace GestorActividades_MVC_G12
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
