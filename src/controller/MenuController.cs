using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.bll;
using asf.cms.model;

namespace asf.cms.controller
{
    public class MenuController:Controller
    {
        public MenuController(HttpContext context):base(context)
        {
            isAdmin = 1;
        }
        public void List()
        {
            List<MenuVO> menuVOS = Menu.ListMenus();
            List<Menu> menus = new List<Menu>();
            string classesJSON = "{";
            int idx = 0;

            foreach (MenuVO menuVO in menuVOS)
            {
                Menu m = new Menu(menuVO.MenuKey, menuVO.CSSClass);
                classesJSON += menuVO.MenuKey + ":" + PrepareItemCSSClassJSON(m.Items) + (idx < menuVOS.Count - 1 ? "," : "");
                idx++;
                menus.Add(m);
            }

            classesJSON += "}";

            this.Items.Add("menus", menus);
            this.Items.Add("classesJSON", classesJSON);

            SectionTree st = new SectionTree();
            SectionTreeNode stn=st.getTree();
            this.Items.Add("selectedTab", "Menu");

            this.Items.Add("sectionTreeOptions", stn.ToOptions(1));
            this.ShowPage("menu/Edit.aspx");
        }

        public void Save()
        {
            try
            {
                int i = 0;
                string key = Request["key"];
                string currentElementKey = "sections[" + i.ToString() + "]";

                Menu m = new Menu(key);
                m.Items = new List<MenuItem>();
                while (Request[currentElementKey + "[sectionId]"] != null)
                {
                    MenuItem mi = new MenuItem();
                    mi.Item.Position = i;
                    mi.Item.SectionHasMenuId.MenuKey = key;
                    mi.Item.SectionHasMenuId.SectionId = int.Parse(Request[currentElementKey + "[sectionId]"]);

                    string className = currentElementKey + "[cssClass]";
                    if (Request[className] != null)
                    {
                        mi.Item.CSSClass = Request[className];
                    }

                    m.Items.Add(mi);
                    currentElementKey = "sections[" + (++i).ToString() + "]";
                }
                m.UpdateItems();

                if (Request["cssClass"].Length == 0)
                    m.UpdateCssClass(null);
                else
                    m.UpdateCssClass(Request["cssClass"]);

                SendJSON("{\"error\":false,\"msg\":\"Los datos han sido guardados\",\"id\":\"" + key + "\"}");
            }
            catch (Exception ex)
            {
                SendJSONException(ex, "Error al guardar menu.");
            }
        }

        protected string PrepareItemCSSClassJSON(List<MenuItem> menu)
        {
            string res = "{";
            bool first = true;
            foreach(MenuItem mItem in menu)
            {
                if(!String.IsNullOrEmpty(mItem.Item.CSSClass))
                {
                    if (!first)
                    {
                        res += ",";
                    } else
                    {
                        first = false;
                    }
                        
                    res += "'" + mItem.Item.SectionHasMenuId.SectionId.ToString() + "'" + ":'" + mItem.Item.CSSClass + "'";
                }
            }
            return res + "}";
        }
    }
}
