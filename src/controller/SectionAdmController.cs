using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.bll;
using asf.cms.model;
using asf.cms.dal;
using asf.cms.util;
using asf.cms.view.admin;

namespace asf.cms.controller
{
    public class SectionAdmController : Controller
    {
        public SectionAdmController(HttpContext context):base(context)
        {
            isAdmin = 1;
        }

        public void New()
        {
/*            SectionTree st = new SectionTree();
            SectionTreeNode stn = new SectionTreeNode();
            stn.Childs = new List<SectionTreeNode>();
            stn.Childs.Add(new SectionTreeNode());
            
            if (Request["parentSectionId"] == null)
            {
                stn.Childs[0] = st.getTree();
                stn.Childs[0].Selected = true;                
            }
            else
                stn.Childs[0] = st.getTree(int.Parse(Request["parentSectionId"]));
            stn.Childs[0].Node.Title = "----";
            stn.Childs[0].Node.SectionId = 0;
Michelle Macías Couret
            this.Items.Add("sectionTreeOptions", stn.ToOptions(1));*/
            this.Items.Add("action", "Insert");
//            this.Items.Add("selectedTab", "Section");
            Section s=new Section();
            if (Request["parentSectionId"] != "0" && Request["parentSectionId"] != null && Request["parentSectionId"] != "")
                s.ParentSectionId = int.Parse(Request["parentSectionId"]);

            this.Items.Add("section", s);
            this.ShowPage("section/PopUp.aspx");
        }

        public void Insert()
        {
            Section s = new Section();
            s.IsMain = false;
            s.Created = DateTime.Today;
            s.Updated = DateTime.Today;
            if (Request["parentSectionId"] != "0" && Request["parentSectionId"] != null && Request["parentSectionId"] != "")
                s.ParentSectionId = int.Parse(Request["parentSectionId"]);
            s.SpanishTitle = Request["title"].Trim();
            s.Permalink = Request["permalink"].Trim();
            s.Active = true;
            s.RedirectOptions = "";
            if(s.Save())
            {
                SendJSON("{\"error\":false,\"msj\":\"Los datos han sido guardados\",\"id\":" + s.SectionId + "}");
                ModificationLog.AddSectionRegistry(ModificationType.CREATE, s, GetCurrentUser());
            }
            else
            {
                SendJSON("{\"error\": true,\"msj\":\"Los datos han NO sido guardados\"}");
            }
        }

        public void List()
        {
            this.Items.Add("userType", GetCurrentUserType());
            this.Items.Add("selectedTab", "Section");
            this.ShowPage("section/List.aspx");
        }

        public void GetAllSections()
        {
            GridInput gi = new GridInput();
            gi.Rows = new List<RowGrid>();
            if(Request["page"] != null)
            {
                gi.Page = int.Parse(Request["page"]);
                gi.Sidx = Request["sidx"];
                gi.Sord = Request["sord"];
                gi.Limit = int.Parse(Request["rows"]);
            }

            UserVO uvo = (UserVO)Context.Session["user"];
            string username = "";
            if (uvo != null)
                username = uvo.Username;
            List<Section> lista = Section.GetSectionsByUser(username);
            // Ordenamiento
            switch(gi.Sidx)
            {
                case "SpanishTitle":
                    lista.Sort(Section.CompareByLanguageESP);
                    break;
                case "EnglishTitle":
                    lista.Sort(Section.CompareByLanguageENG);
                    break;
                case "RedirectTo":
                    lista.Sort(Section.CompareByLink);
                    break;
            }
            if(gi.Sord == "desc")
            {
                lista.Reverse();
            }
            // Termina ordenamiento
            foreach (Section s in lista)
            {
                string cmd = "", cmd2 = "", cmd3 = "";
                cmd3 = "<a><i class='small edit icon'></i></a>";
                if (uvo.Type != "COLABORATOR" && uvo.Type != "RECUPERACIONES")
                {
                    
                    cmd2 = "<a><i class='small " + (s.SitemapExclude ? "yellow " : "green ") + "circle icon'></i></a>";
                    cmd = "<a><i class='small red trash icon'></i></a>";
                    /*string cmd2_img = s.SitemapExclude ? "status2.png" : "status1.png", cmd2_txt = s.SitemapExclude ? "Incluir en el mapa" : "Excluir del mapa";
                    cmd = "<a href='javascript:void(0)' onclick='deleteSection(" + s.SectionId + ");'><img src='../view/img/delete.png' alt='Eliminar' title='Eliminar'/></a>";
                    cmd2 = "<a href='jacascript:void(0)' onclick='switchMapEnable("+ s.SectionId +");'><img src='../view/img/" + cmd2_img + "' alt='" + cmd2_txt + "' title='" + cmd2_txt + "' /></a>";*/
                }
                RowGrid row = new RowGrid();
                row.Id = s.SectionId.ToString();
                row.Cell = new List<string>();
                row.Cell.Add(s.SpanishTitle);
                row.Cell.Add(s.EnglishTitle);
                row.Cell.Add(s.RedirectTo);
                row.Cell.Add(cmd3);
                row.Cell.Add(s.SitemapExclude.ToString().ToLower()); //cmd2);
                row.Cell.Add(cmd);
                row.Cell.Add(s.ParentSectionId.HasValue.ToString().ToLower());
                row.Cell.Add(s.IsMain.ToString().ToLower());
                gi.Rows.Add(row);
            }
            gi.calculaInternos();
            string json = gi.toJSON(false);
            ShowPage("js/default.js");
            Response.Write(json);
        }
        public void Edit()
        {
            int id = int.Parse(Request["sectionId"]);
            Section s = Section.GetById(id);
            UserVO uvo = (UserVO)Context.Session["user"];
            string username = "";
            if (uvo != null)
                username = uvo.Username;
            string  op = Section.GetSectionTreeNodeByUser(username, s);
          
            if (uvo.Type == "COLABORATOR" || uvo.Type == "RECUPERACIONES")
                this.Items.Add("readonly", "readonly");
            else
                this.Items.Add("readonly", "");
            this.Items.Add("sectionTreeOptions", op);
            this.Items.Add("selectedTab", "Section");

            this.Items.Add("section", s);
            this.Items.Add("action", "Update");
            this.ShowPage("section/New.aspx");
        }
        public void Update()
        {
            try
            {
                int id, parent;
                DateTime created = DateTime.Now;

                if (!int.TryParse(Request["sectionId"], out id))
                {
                    SendJSON("{ \"error\": true, \"msg\": \"No se pudo actualizar la información Id de sección inválido.\" }");
                    return;
                }

                Section prev = Section.GetById(id);

                Section s = new Section();
                s.SectionId = id;
                if (int.TryParse(Request["parentSectionId"], out parent))
                {
                    if (parent != 0)
                    {
                        if (HasCycle(parent, id))
                        {
                            SendJSON("{ \"error\": true, \"msg\": \"No se pudo actualizar la información. Se forma un ciclo.\" }");
                            return;
                        }
                    }
                }

                s.Created = created;
                //TODO: Validar encoding de titulos
                s.SpanishTitle = Request["spanishTitle"];
                s.EnglishTitle = Request["englishTitle"];
                s.IsMain = Request["isMain"] != null;
                s.Permalink = Request["Permalink"];
                s.Position = int.Parse(Request["position"]);
                s.RedirectTo = Request["redirectTo"];
                s.RedirectOptions = Request["redirectOptions"];
                
                s.Updated = DateTime.Now;
                s.Visitas = int.Parse(Request["visitas"]);
                s.Active = true;
                s.ParentSectionId = parent;
                s.CssClass = Request["CssClass"];

                if (s.Save())
                {
                    SendJSON("{ \"error\": false, \"msg\": \"Se ha modificado la sección correctamente.\"}");
                    ModificationLog.AddSectionRegistry(ModificationType.MODIFY, s, GetCurrentUser(), prev);
                }
                else
                {
                    SendJSON("{ \"error\": true, \"msg\": \"Error al guardar los cambios de la seccion.\"}");
                }
            }
            catch(Exception ex)
            {
                SendJSON("{ \"error\": true, \"msg\": \"Se ha producido un error en el proceso\", \"details\": \"" + ex.Message + "\"}");
            }
        }

        public void GetPublicationsBySection()
        {
            int sectionId = int.Parse(Request["sectionId"]);
            GridInput gi = new GridInput();
            gi.Rows = new List<RowGrid>();
            List<PublicationAdminVO> lista = Publication.ListPublicationAdmin(sectionId);
            foreach (PublicationAdminVO pvo in lista)
            {
                String imgIsMain = "";
                String imgStatus = "<img src='../view/img/status" + pvo.Status + ".png' alt='"+pvo.Status+"' title='"+pvo.Status+"'/>";
                if (pvo.IsMain)
                    imgIsMain = "<img src='../view/img/main.png' alt='Principal' title='Principal'/>";
                
                RowGrid row = new RowGrid();
                row.Id = pvo.Id.ToString();
                row.Cell = new List<string>();
                row.Cell.Add(imgIsMain);
                row.Cell.Add(imgStatus);
                row.Cell.Add(pvo.Title);
                row.Cell.Add(pvo.Section);
                row.Cell.Add(pvo.LanguageId == 1 ? "es-MX" : "en-US");
                row.Cell.Add("<a href='javascript:void(0)' onclick='deletePublication(" + pvo.Id + ");'><img src='../view/img/delete.png' alt='Eliminar' title='Eliminar'/></a>");
                gi.Rows.Add(row);
            }
            gi.calculaInternos();
            string json = gi.toJSON();
            ShowPage("js/default.js");
            Response.Write(json);
        }
        public void Delete()
        {
            int sectionId = int.Parse(Request["sectionId"]);
            try
            {
                Section s = Section.GetById(sectionId);

                Section.Delete(sectionId);
                SendJSON("{ \"error\" : false, \"msg\" : \"Se ha eliminado la sección " + s.Title + ".\", \"details\" : " + s.SectionId + " }");
                ModificationLog.AddSectionRegistry(ModificationType.DELETE, s, GetCurrentUser());
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger("SectionAdmController").Error("Error al eliminar seccion", ex);
                SendJSON("{ \"error\" : true, \"msg\" : \"Ocurrio un error al eliminar la sección.\", \"details\" : \"" + ex.Message + "\" }");
            }
        }

        public void SwitchMapExclude()
        {
            UserVO uvo = (UserVO)Context.Session["user"];
            if (uvo.Type == "ADMIN")
            {
                int sectionId = int.Parse(Request["sectionId"]);
                Section section = Section.GetById(sectionId), sectionOld = section.Clone();
                section.SitemapExclude = !section.SitemapExclude;
                if (!section.Save())
                {
                    SendJSON("{ \"error\" : true, \"msg\" : \"Hubo un problema al cambiar el estado del visibilidad en el mapa de navegación.\" }");
                    //SendMessage("Hubo un problema al cambiar el estado del visibilidad en el mapa de navegación.");
                }
                else
                {
                    SendJSON("{ \"error\" : false, \"msg\" : \"Cambio Exitoso.\", \"modifiedValue\" : " + section.SitemapExclude.ToString().ToLower() + " }");
                    //SendMessage("Cambio Exitoso.");
                    ModificationLog.AddSectionRegistry(ModificationType.MODIFY, section, uvo, sectionOld);
                }
            }
            else
            {
                SendJSON("{ \"error\" : true, \"msg\" : \"Usuario no autorizado\" }");
                //SendMessage("Usuario no autorizado.");
            }
        }

        public void GetTree()
        {
            UserVO uvo = (UserVO)Context.Session["user"];
            int sectionId = Request["sectionId"] != null ? int.Parse(Request["sectionId"]) : 0;
            bool includePublications = Request["includePublications"] != null ? (Request["includePublications"] == "true") : false;

            if(uvo != null)
            {
                SectionTree tree = new SectionTree();
                SectionTreeNode root = sectionId == 0 ? tree.getTree() : tree.getTree(sectionId).Find(sectionId);

                string json_root = root != null ? root.ToJSON(includePublications) : "null";
                
                SendJSON("{ \"error\": false, \"msg\": \"OK\", \"data\" : " + json_root + " }");
            }
            else
            {
                SendJSON("{ \"error\": true, \"msg\": \"Ususario no autorizado.\" }");
            }
        }

        protected bool HasCycle(int parentId, int childId)
        {
            List<int> visited = new List<int>();
            visited.Add(childId);

            bool cycle = false;
            Section cur;

            do {
                cur = Section.GetById(parentId);
                if (visited.Contains(cur.SectionId))
                {
                    cycle = true;
                }
                else
                {
                    visited.Add(cur.SectionId);
                    parentId = cur.ParentSectionId != null ? (int)cur.ParentSectionId : -1;
                }
            } while (cur.ParentSectionId != null && !cycle);

            return cycle;
        }
    }
}
