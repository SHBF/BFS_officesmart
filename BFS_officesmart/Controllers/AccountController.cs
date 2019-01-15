using BFS_officesmart.Common;
using BFS_officesmart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BFS_officesmart.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        [AllowAnonymous]
        public ActionResult Index()
        {
            try
            {
                LoadTree();
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "UserLogin");
            }
           
            return View();
        }
        public ActionResult UserManagement()
        {
            TestController.Calc(2, 0);
            return View();
        }
        public ActionResult LoadTree()
        {
            List<MenuModels> list = InitTree();
            ViewBag.MenuTree = list;
            return View("Index");
        }
        /// <summary>
        /// 初始化树 默认找出顶级菜单
        /// </summary>
        /// <returns></returns>
        public List<MenuModels> InitTree()
        {
            db_BFSEntities ent = new db_BFSEntities();
            //var TreeList = ent.SysMenu.ToList();

            string username = System.Web.HttpContext.Current.Session["username"].ToString();
            var TreeList = (from p in ent.tAdmin
                            where p.Name == username
                            join urole in ent.tSmartRole on p.AdminID equals urole.AdminID
                            join r in ent.tRole on urole.RoleID equals r.RoleID
                            join rm in ent.tRoleMenu on r.RoleID equals rm.RoleID
                            join m in ent.tSysMenu on rm.MenuID equals m.MenuID  orderby m.Menusort
                            select m).ToList();
            List<MenuModels> rootNode = new List<MenuModels>();
            foreach (var plist in TreeList.Where(t => t.MenuURL == "#"))
            {
                MenuModels jt = new MenuModels();
                jt.MenuID = plist.MenuID;
                jt.MenuName = plist.MenuName;
                jt.MenuPartNo = Convert.ToInt32(plist.MenuPartNo);
                jt.MenuURL = plist.MenuURL;
                jt.MenuIcon = plist.MenuIcon.Trim();
                jt.attributes = CreateUrl(TreeList, jt);
                jt.menus = CreateChildTree(TreeList, jt);
                rootNode.Add(jt);
            }
            return rootNode;
        }


        /// <summary>
        /// 递归生成子树
        /// </summary>
        /// <param name="TreeList"></param>
        /// <param name="jt"></param>
        /// <returns></returns>
        private List<MenuModels> CreateChildTree(List<tSysMenu> TreeList, MenuModels jt)
        {
            int keyid = jt.MenuPartNo;//根节点ID
            List<MenuModels> nodeList = new List<MenuModels>();
            var children = TreeList.Where(t => t.MenuID == keyid);
            foreach (var chl in children)
            {
                MenuModels node = new MenuModels();
                node.MenuID = chl.MenuID;
                node.MenuName = chl.MenuName;
                node.MenuPartNo = Convert.ToInt32(chl.MenuPartNo);
                node.MenuURL = chl.MenuURL;
                jt.MenuIcon = chl.MenuIcon;
                node.attributes = CreateUrl(TreeList, node);
                node.menus = CreateChildTree(TreeList, node);
                nodeList.Add(node);
            }
            return nodeList;
        }
        /// <summary>
        /// 把Url属性添加到attribute中，如果需要别的属性，也可以在这里添加
        /// </summary>
        /// <param name="TreeList"></param>
        /// <param name="jt"></param>
        /// <returns></returns>
        private List<MenuModels.children> CreateUrl(List<tSysMenu> TreeList, MenuModels jt)
        {
            List<MenuModels.children> dic = new List<MenuModels.children>();
            int keyid = jt.MenuID;

            //var urlList = TreeList.Where(t => t.MenuPartNo == keyid).ToList();
            var urlList = (from p in TreeList where p.MenuPartNo != 0 && p.MenuPartNo == keyid select p).ToList();
            foreach (var item in urlList)
            {
                dic.Add(new MenuModels.children { MenuURL = item.MenuURL, MenuName = item.MenuName });
            }

            return dic;
        }
    }
    public class TestController
    {
        public static int Calc(int a, int b)
        {
            return a / b;
        }
    }
}