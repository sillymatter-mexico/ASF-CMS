using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using asf.cms.widgets.ui;
using asf.cms.model;
using asf.cms.bll;
using NHibernate.Criterion;

namespace asf.cms.controller
{
    public class Controller
    {
        protected HttpRequest Request;
        protected HttpResponse Response;
        protected HttpContext Context;
        protected IDictionary Items;
        protected string uploadUrl = "";
        public int isAdmin = 0;
        protected Controller(HttpContext Context)
        {
            this.Context = Context;
            Request = Context.Request;
            Response = Context.Response;
            Items = Context.Items;
            uploadUrl = Context.Server.MapPath("~/uploads/");

        }
        public bool isValidReq()
        {
            if (Context.Session["user"] == null && isAdmin==1)
            {
                ShowPage("Login.aspx");
                return false;
            }
            return true;
        }
        protected void ShowPage(String page)
        {
           // Context.Response.WriteFile("/" + System.Configuration.ConfigurationSettings.AppSettings["RootDir"] + "/view/" + page);
           // Context.RewritePath("/" + System.Configuration.ConfigurationSettings.AppSettings["RootDir"] + "/view/" + page);
            //Context.Server.Execute("/" + System.Configuration.ConfigurationSettings.AppSettings["RootDir"] + "/view/" + page); //Correcta
            string path = "/" + System.Configuration.ConfigurationManager.AppSettings.Get("RootDir") + "/view/" + page;
            Context.Server.Execute(path);
            //Context.Server.Execute("/view/" + page);
        }
        protected void SendMessage(String msg)
        {
            ShowPage("util.aspx");
            Response.Output.Write(msg);
        }
        protected void SendJSON(String json)
        {
            ShowPage("js/default.js");
            Response.Write(json);
        }
        protected UserVO GetCurrentUser()
        {
            return ((UserVO)Context.Session["user"]);
        }
        protected string GetCurrentUserType()
        {
            UserVO u = GetCurrentUser();
            if (u == null)
                return "";
            return u.Type;
        }
        /// <summary>
        /// Builds then content with embedded widgets
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected string BuildContent(string content, int globalPublicationId, int currentPublication)
        {
            return BaseWidget.GetWidgetFormattedContent(content, globalPublicationId, currentPublication, Request);
        }

        protected void BuildMenus(Section selected)
        {
            List<MenuVO> menuKeys = Menu.ListMenus();
            foreach (MenuVO menuVO in menuKeys)
            {
                this.Items.Add("menu_" + menuVO.MenuKey.ToLower(), new Menu(menuVO.MenuKey, menuVO.CSSClass));
                //List<Section> menu = Section.GetSectionsByMenuKey(menuVO.MenuKey);
            }
        }

        protected void BuildMenusClassic(Section selected)
        {
            List<Section> superior = Section.GetSectionsByMenuKey("SUPERIOR");
            List<Section> inferior = Section.GetSectionsByMenuKey("INFERIOR");
            List<Section> principal = Section.GetSectionsByMenuKey("PRINCIPAL");

            this.Items.Add("menuPrincipal", principal);
            this.Items.Add("menuSuperior", superior);
            this.Items.Add("menuInferior", inferior);

            List<Section> leftMenuList = new List<Section>();
            leftMenuList.AddRange(principal);
            leftMenuList.AddRange(inferior);

            //If the publication has widgets, then add them
            SectionTree st = new SectionTree();
            string leftMenu = "";
            string leftMenuClassic = "";
            if (selected.MainPublication == null && selected.Publications.Count > 0)
            {
                selected.MainPublication = selected.Publications[0];
                leftMenu = st.GetLeftMenu(selected, selected.MainPublication.Id, leftMenuList);
                leftMenuClassic = st.GetLeftMenu(selected, selected.MainPublication.Id);
            }
            else
            {
                leftMenu = st.GetLeftMenu(selected, leftMenuList);
                leftMenuClassic = st.GetLeftMenu(selected);
            }

            if (leftMenuClassic.Trim() == "<ul></ul>")
                leftMenuClassic = BuildContent("#${announce}", selected.SectionId, selected.MainPublication.Id);
            this.Items.Add("leftMenuClassic", leftMenuClassic);
            this.Items.Add("leftMenu", leftMenu);


            if (st.tree.Node.Title.ToUpper() == "PRINCIPAL")
                this.Items.Add("subSectionTitle", "Temas de Interés");
            else
                this.Items.Add("subSectionTitle", st.tree.Node.Title);
        }

        protected void BuildSiblingsMenu(Section selected)
        {
            List<Section> siblingSections = selected.GetSiblings();
            Items.Add("menu_siblings", siblingSections);
            Items.Add("menu_siblings_publications", selected.Publications);
        }

        protected void BuildSpecialPublications()
        {
            List<Especiales> especiales = Especiales.GetByAutogeneratedType("especiales");
            List<MetaItem> especialesMeta = null;

            // ===== Traer los banners ===== //
            List<Especiales> especialesSuperior = new List<Especiales>();
            List<Especiales> especialesLateral = new List<Especiales>();
            List<Especiales> especialesFooter = new List<Especiales>();

            foreach (Especiales especial in especiales)
            {
                especialesMeta = MetaItem.ListFromJson(especial.Meta);
                if (especialesMeta.Count > 3)
                {
                    string value = MetaItem.GetBannerValue(especialesMeta);
                    if (value.ToLower().Equals("superior"))
                        especialesSuperior.Add(especial);
                    else if (value.ToLower().Equals("lateral"))
                        especialesLateral.Add(especial);
                    else if (value.ToLower().Equals("footer"))
                        especialesFooter.Add(especial);
                    else
                    {
                        especialesSuperior.Add(especial);
                        especialesLateral.Add(especial);
                    }
                }
                else
                {
                    especialesSuperior.Add(especial);
                    especialesLateral.Add(especial);
                }
            }

            this.Items.Add("bannerSuperior", especialesSuperior);
            this.Items.Add("bannerLateral", especialesLateral);
            this.Items.Add("footer", especialesFooter);
        }

        protected void BuildSpecialSections()
        {
            List<Section> specialSections = Section.GetSpecialSections();

            foreach(Section special in specialSections)
            {
                if(Items[special.Type] == null)
                    Items[special.Type] = new List<PublicationVO>();
                
                (Items[special.Type] as List<PublicationVO>).AddRange(special.Publications);
            }
        }

        protected void BuildMeta(PublicationVO current)
        {
            List<MetaItem> meta = null;
            if (!String.IsNullOrEmpty(current.Meta))
                meta = MetaItem.ListFromJson(current.Meta);
            this.Items.Add("meta", meta);
        }

        protected void BuildMenuOptions()
        {
            bool smart = false, superior = false, sibling = false;
            switch (System.Configuration.ConfigurationManager.AppSettings.Get("SectionMenuMode").ToUpper())
            {
                case "ALL":
                    superior = sibling = true;
                    break;
                case "SMART":
                    smart = true;
                    break;
                case "SUPERIOR":
                    superior = true;
                    break;
                case "SIBLING":
                    sibling = true;
                    break;
                case "NONE":
                default:
                    break;
            }

            Items.Add("menu_mode_smart", smart);
            Items.Add("menu_show_sibling", sibling);
            Items.Add("menu_show_superior", superior);
        }

        protected void SendJSONException(Exception ex, string message = null)
        {
            string msg;
            string det;
            if (string.IsNullOrEmpty(message))
            {
                msg = ex.Message;
                det = ex.InnerException != null ? ex.InnerException.Message : ex.StackTrace;
            }
            else
            {
                msg = message;
                det = ex.Message + (ex.InnerException != null ? " :: " + ex.InnerException.Message : ex.StackTrace); 
            }

            ShowPage("js/default.js");
            Response.Write("{ \"error\": true, \"msg\": \"" + msg + "\", \"details\": \"" + det + "\" }");
        }
    }
}
