using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BFS_officesmart.Models
{
    public class MenuModels
    {
        //MenuID, MenuName, MenuURL, MenuPartNo
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public string MenuURL { get; set; }
        public string MenuIcon { get; set; }
        public int MenuPartNo { get; set; }
        public object _menus;
        public children _attributes = new children();
        public List<children> attributes;
        public object menus
        {
            get { return _menus; }
            set { _menus = value; }
        }
        public class children
        {
            public string MenuName { get; set; }
            public string MenuURL { get; set; }
        }

        public class User
        {
            public int UserID { get; set; }
            public string UserName { get; set; }
            public string PassWord { get; set; }

        }
    }
}