using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.dal;
using asf.cms.model;

namespace asf.cms.bll
{
    public class Menu
    {
        public List<MenuItem> Items ;
        public string Key;
        public string CSSClass;

        /*public Menu(string Key):this(Key, null)
        {
            
        }*/

        public Menu(string Key, string Class = null)
        {
            this.Key = Key;
            CSSClass = Class;
            Items = new List<MenuItem>();

            SectionHasMenuDAL shmDAL = new SectionHasMenuDAL();
            IList<SectionHasMenuVO> shmVOS = shmDAL.GetByMenu(Key);
            foreach(SectionHasMenuVO shmVO in shmVOS)
            {
                Section s = Section.GetById(shmVO.SectionHasMenuId.SectionId);
                Items.Add(new MenuItem(shmVO, s));
            }
        }
      
        public static List<string> ListMenuKeys()
        {
            MenuDAL mDAL = new MenuDAL();
            IList<MenuVO> menus = mDAL.GetMenus();
            List<string> menuKeys = new List<string>();
            foreach(MenuVO menu in menus)
            {
                menuKeys.Add(menu.MenuKey);
            }
            return menuKeys;
        }

        public static List<MenuVO> ListMenus()
        {
            MenuDAL mDAL = new MenuDAL();
            return (List<MenuVO>)mDAL.GetMenus();
        }

        public bool UpdateItems()
        {
            SectionHasMenuDAL smdal = new SectionHasMenuDAL();
            List<SectionHasMenuVO> list = new List<SectionHasMenuVO>();
            Items.ForEach(delegate(MenuItem item)
            {
                list.Add(item.Item);
            });
            smdal.UpdateMenu(Key,list);
            return true;
        }

        public bool UpdateCssClass(string cssClass)
        {
            MenuVO uMenu = new MenuVO();
            uMenu.MenuKey = this.Key;
            uMenu.CSSClass = cssClass;

            MenuDAL mDAL = new MenuDAL();
            mDAL.Update(uMenu);
            return true;
        }
    }
}
