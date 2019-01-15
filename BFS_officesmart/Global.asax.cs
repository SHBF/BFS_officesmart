using BFS_officesmart.App_Start;
using BFS_officesmart.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BFS_officesmart
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
           
            //if (username != null)
            //{
            //    Session[Common.WebConstants.UserRoleMenu] = UserLoginController.GetMenuByUserID(username.ToString());
            //}
        }
        private static bool isExpire = false; //Session是否超时 
        protected void Session_End(object sender, EventArgs e)
        {
            isExpire = true;
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (isExpire)
            {
                isExpire = false;
                string loginUrl = System.Web.Security.FormsAuthentication.LoginUrl;
                Response.Write(string.Format("<script languge='javascript'>alert('用户信息已过期，请重新登录'); window.parent.location='{0}';</script>", loginUrl));
            } }

    }
}
