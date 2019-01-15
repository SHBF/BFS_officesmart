using BFS_officesmart.App_Start;
using System.Web;
using System.Web.Mvc;

namespace BFS_officesmart
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new UserAuthAttribute());//注册
            filters.Add(new CustomErrorAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
