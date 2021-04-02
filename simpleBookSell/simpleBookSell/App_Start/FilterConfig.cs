using System.Web;
using System.Web.Mvc;
using simpleBookSell.Filters;

namespace simpleBookSell
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new myErrorAttribute());
            filters.Add(new authorAttribute());

        }
    }
}
