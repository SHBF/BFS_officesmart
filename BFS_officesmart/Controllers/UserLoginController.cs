using BFS_officesmart.Common;
using BFS_officesmart.Models;
using BFS_officesmart.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BFS_officesmart.Controllers
{
    public class UserLoginController : Controller
    {
        // GET: UserLogin
        [AllowAnonymous]
        public ActionResult Login()
        {
           
            return View();
        }
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            System.Web.HttpContext.Current.Session["username"] = null;
            Session[WebConstants.UserSession] = null;
            Session[WebConstants.UserRoleMenu] = null;
            Session["tUsers"] = null;
            return RedirectToAction("Login");
        }
        [AllowAnonymous]    
        public ActionResult GetLogin(tAdminDTO t)
        {
            if (t.Code != Session["ValidateCode"].ToString())
            {
                ModelState.AddModelError("Code", "验证码不正确！");
                return View("Login");
            }
            if (ModelState.IsValid)
            {
                //string User_Name = this.Request.Form["UserName"];
                //string User_Pw = this.Request.Form["PassWord"];
                if (!string.IsNullOrEmpty(t.Name) && !string.IsNullOrEmpty(t.Password))
                {
                    db_BFSEntities ent = new db_BFSEntities();
                    var login = (from p in ent.tAdmin where p.Name == t.Name && p.Password == t.Password select p).Distinct().ToList();
                    if (login.Count > 0)
                    {
                        tAdmin tadmin = login.FirstOrDefault();
                        Session[WebConstants.UserSession] = tadmin;
                        Session[WebConstants.UserRoleMenu] = GetMenuByUserID(login.First().Name);
                        System.Web.HttpContext.Current.Session["username"] = login.First().Name.ToString();
                        Session["tUsers"] = login.ToList();
                        string fromurl = Request.UrlReferrer.Query;
                        if (fromurl.IndexOf("?fromurl=") > -1)
                        {
                            fromurl = fromurl.Substring(9);

                            return Redirect(fromurl);
                        }
                        else
                        {
                            return this.RedirectToAction("Index", "Account");
                        }

                    }
                    else {
                        ModelState.AddModelError("Name", "登陆失败！");
                    }
                }

            }
            return View("Login");
        }
        [AllowAnonymous]
        public ActionResult GetValidateCode()
        {
            ValidateCode vCode = new ValidateCode();
            string code = vCode.CreateValidateCode(4);
            Session["ValidateCode"] = code;
            byte[] bytes = vCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }
        public static List<tSysMenu> GetMenuByUserID(string Name)
        {
               db_BFSEntities ent = new db_BFSEntities();
            //var TreeList = ent.SysMenu.ToList();
            var TreeList = (from p in ent.tAdmin
                            where p.Name == Name
                            join urole in ent.tSmartRole on p.AdminID equals urole.AdminID
                            join r in ent.tRole on urole.RoleID equals r.RoleID
                            join rm in ent.tRoleMenu on r.RoleID equals rm.RoleID
                            join m in ent.tSysMenu on rm.MenuID equals m.MenuID
                            select m).ToList();
            return TreeList.Distinct().ToList();
        }
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">加密对象</param>
        /// <returns></returns>
        public static string MD5(string str)
        {
            System.Text.ASCIIEncoding ae = new System.Text.ASCIIEncoding();
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] AfterMD5 = md5.ComputeHash(md5.ComputeHash(ae.GetBytes(str)));
            for (int i = 0; i < AfterMD5.Length; i++)
                AfterMD5[i] = (byte)(AfterMD5[i] % 95 + 32);
            return ae.GetString(AfterMD5);
        }
        [AllowAnonymous]
        public ActionResult NoPremission()
        {
            return View();
        }
    }
}