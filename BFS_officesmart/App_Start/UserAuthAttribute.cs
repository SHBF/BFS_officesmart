using BFS_officesmart.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BFS_officesmart.Models;
namespace BFS_officesmart.App_Start
{
    public class UserAuthAttribute: AuthorizeAttribute
    {
        public bool IsLogin = false;
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool Pass = false;
            try
            {
                var websession = httpContext.Session[WebConstants.UserSession];
                if (websession == null)
                {
                    httpContext.Response.StatusCode = 401;//无权限状态码
                    Pass = false;
                    IsLogin = false;
                }
                else
                {
                   tAdmin user = httpContext.Session[WebConstants.UserSession] as tAdmin;
                    if (user == null)
                    {
                        httpContext.Response.StatusCode = 401;//无权限状态码
                        Pass = false;
                        IsLogin = false;
                    }
                    else
                    if (!IsMenuRole(httpContext))
                    {
                        httpContext.Response.StatusCode = 401;//无权限状态码
                        Pass = false;
                        IsLogin = true;
                    }
                    else
                    {
                        Pass = true;
                    }
                }
            }
            catch (Exception)
            {
                return Pass;
            }
            return Pass;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            else
            {
                if (!IsLogin)
                {
                    string fromUrl = filterContext.HttpContext.Request.Url.PathAndQuery;
                    // string strUrl = new UrlHelper(filterContext.RequestContext).Action("Login", "Account","") + "?fromurl={0}";
                    string strUrl = "~/UserLogin/Login/?fromurl={0}";
                    filterContext.HttpContext.Response.Redirect(string.Format(strUrl, fromUrl), true);
                }
                else
                {
                    filterContext.Result = new RedirectResult("~/UserLogin/NoPremission");
                }

            }
        }
        public bool IsMenuRole(HttpContextBase httpContext)
        {
            string rawurll = httpContext.Request.RawUrl.ToLower();
            List<tSysMenu> MenuList = httpContext.Session[WebConstants.UserRoleMenu] as List<tSysMenu>;

            Guid guid;
            //这里是过滤掉 url后的GUID参数
            int rindex = rawurll.LastIndexOf("/");
            if (rindex > 0 && Guid.TryParse(rawurll.Substring(rindex + 1), out guid))
            {
                rawurll = rawurll.Substring(0, rindex);
            }
            if (MenuList.Where(e => e.MenuURL.ToLower().Contains(rawurll)).Count() == 0)
            {
                return false;
            }

            return true;
        }
    }
}