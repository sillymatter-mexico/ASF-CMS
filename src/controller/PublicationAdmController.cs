﻿using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.bll;
using asf.cms.model;
using asf.cms.dal;
using asf.cms.util;
using System.Text.RegularExpressions;
using asf.cms.widgets;
using System.Text;

namespace asf.cms.controller
{
    public class PublicationAdmController:Controller
    {
        public PublicationAdmController(HttpContext context)
            : base(context)
        {
            isAdmin = 1;
        }
       
        public void Insert()
        {
            try
            {
                int parentSectionId = 0;
                int.TryParse(Request["parentSectionId"], out parentSectionId);

                Publication p = new Publication();
                p.publication.LanguageId = Language.GetCurrentLanguageId();
                p.publication.Created = DateTime.Today;
                p.publication.Updated = DateTime.Today;
                p.publication.Published = DateTime.Now;
                p.publication.Unpublished = DateTime.Now.AddYears(20);
                p.publication.IsMain = false;
                p.publication.SectionId = parentSectionId;
                p.publication.Title = Request["title"].Trim();
                p.publication.Permalink = Request["permalink"];
                p.publication.Position = 0;
                p.publication.Content = "";
                p.publication.Status = 1;
                p.publication.Active = true;
                p.publication.NewsTTL = 30;
                p.publication.Visitas = 0;
                p.publication.Meta = p.buildJsonAutomaticMeta();
                Publication.SetLastUpdateDate(DateTime.Today);
                p.FilesPath = uploadUrl;

                if (p.Save())
                {
                    p.CreateDirectory();
                    this.Items.Add("publication", p.publication);
                    SendJSON("{\"error\":false,\"msj\":\"Los datos han sido guardados\",\"id\":" + p.publication.Id + "}");
                    ModificationLog.AddPublicationRegistry(ModificationType.CREATE, p.publication, GetCurrentUser());
                }
                else
                {
                    SendJSON("{\"error\":true,\"msj\":\"Error al guardar los datos.\"}");
                }
            }
            catch(Exception ex)
            {
                SendJSON("{\"error\":true,\"msj\":\"Error en el proceso de creación de publicación.\", \"details\": " + ex.Message + "}");
            }
        }

        public void List()
        {
            this.Items.Add("userType", GetCurrentUserType());
            this.Items.Add("selectedTab", "Publication");
            this.ShowPage("publication/List.aspx");
        }

        public void GetAllPublications()
        {
            try
            {
                GridInput gi = new GridInput();
                gi.Rows = new List<RowGrid>();
                if (Request["page"] != null)
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
                List<PublicationAdminVO> lista = Publication.ListPublicationAdmin(username, "");
                /*gi.Records = lista.Count;
                gi.calculaInternos();

                // Parche para ordenar por seccion
                if (gi.Sidx == "Section")
                {
                    lista.Sort(PublicationAdminVO.CompareBySection);
                    if(gi.Sord == "desc")
                    {
                        lista.Reverse();
                    }
                } else
                {
                    lista = Publication.ListPublicationAdmin(username, gi.Orden); //NOTA: Quitar esto. Hace doble consulta.
                }*/
                // Termina parche
                foreach (PublicationAdminVO pvo in lista)
                {
                    if (pvo.AutogeneratedType == null && pvo.LanguageId == 1)
                    {
                        //string imgIsMain = pvo.IsMain ? "<img src='../view/img/main.png' alt='Principal' title='Principal'/>" : "";
                        //string imgStatus = "<img src='../view/img/status" + pvo.Status + ".png' alt='" + pvo.Status + "' title='" + pvo.Status + "'/>";
                        string cmd = "<a href='javascript:void(0)' onclick='deletePublication(" + pvo.Id + ");'><img src='../view/img/delete.png' alt='Eliminar' title='Eliminar'/></a>";
                        string cmd2_img = pvo.SitemapExclude ? "status4.png" : "status1.png", cmd2_txt = pvo.SitemapExclude ? "Incluir en el mapa" : "Excluir del mapa";
                        string cmd2 = "<a href='jacascript:void(0)' onclick='switchMapEnable(" + pvo.Id + ");'><img src='../view/img/" + cmd2_img + "' alt='" + cmd2_txt + "' title='" + cmd2_txt + "' /></a>";

                        RowGrid row = new RowGrid();
                        row.Id = pvo.Id.ToString();
                        row.Cell = new List<string>();
                        row.Cell.Add(pvo.IsMain.ToString().ToLower());
                        row.Cell.Add(pvo.Status.ToString());
                        row.Cell.Add(pvo.Title);
                        row.Cell.Add(pvo.Section);
                        row.Cell.Add(pvo.LanguageId == 1 ? "es-MX" : "en-US");
                        row.Cell.Add(pvo.SitemapExclude.ToString().ToLower());
                        row.Cell.Add(pvo.Position.ToString());
                        //row.Cell.Add(cmd);
                        gi.Rows.Add(row);
                    }
                }
                gi.calculaInternos();

                SendJSON(gi.toJSON(false));
            }
            catch(Exception ex)
            {
                SendJSONException(ex, "Error al listar las publicaciones.");
            }
        }

        public void GetHistoricPublications()
        {
            try
            {
                int publicationId = int.Parse(Request["Id"]);

                Publication publication = new Publication();
                publication.publication = Publication.GetById(publicationId);

                List<PublicationVO> historicPublications = publication.GetHistoric();

                GridInput gi = new GridInput();
                gi.Rows = new List<RowGrid>();
                gi.Records = historicPublications.Count;
                gi.Total = historicPublications.Count;

                foreach (PublicationVO historic in historicPublications)
                {
                    RowGrid row = new RowGrid();
                    row.Id = historic.Id.ToString();
                    row.Cell = new List<string>();
                    row.Cell.Add(historic.Unpublished.ToShortDateString());
                    row.Cell.Add(historic.Title);
                    if (historic.Content != null)
                        row.Cell.Add(historic.Content.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r"));
                    else
                        row.Cell.Add("");
                    if (historic.NewsContent != null)
                        row.Cell.Add(historic.NewsContent.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r"));
                    else
                        row.Cell.Add("");
                    gi.Rows.Add(row);
                }

                gi.calculaInternos();

                SendJSON(gi.toJSON());
            }
            catch (Exception ex)
            {
                SendJSONException(ex, "Error al listar publicaicones históricas.");
            }
        }

        public void RestoreHistoricPublication()
        {
            int publicationId = int.Parse(Request["id"]),
                historicId = int.Parse(Request["HistoricId"]);
            UserVO uvo = GetCurrentUser();

            Publication publication = new Publication(),
                        historic = new Publication();
            publication.publication = Publication.GetById(publicationId);
            historic.publication = Publication.GetById(historicId);

            bool changed = false;
            if (Request["fieldContent"] != null)
            {
                publication.publication.Content = historic.publication.Content;
                changed = true;
            }
            if(Request["fieldTitle"] != null)
            {
                publication.publication.Title = historic.publication.Title;
                changed = true;
            }
            if(Request["fieldNews"] != null)
            {
                publication.publication.NewsContent = historic.publication.NewsContent;
                changed = true;
            }
            
            if(changed)
            {
                if(publication.Save())
                {
                    SendJSON("{\"error\": false, \"msg\":\"Publicación restaurada exitosamente.\" }");
                    ModificationLog.AddPublicationRegistry(ModificationType.RESTORE, publication.publication, GetCurrentUser(), historic.publication);
                }
                else
                {
                    SendJSON("{\"error\": true, \"msg\":\"Error al restaurar la publicacion.\" }");
                }
            } else
            {
                SendJSON("{\"error\": true, \"msg\":\"No se han especificado campos para restaurar.\" }");
            }
        }

        public void UpdateMeta()
        {
            try
            {
                String meta = Request["meta"];
                int id = int.Parse(Request["id"]);
                Publication p = new Publication();
                p.publication = Publication.GetById(id);
                PublicationVO h = p.Clone(p.publication);
                p.metaList = MetaItem.ListFromJson(meta);
                p.metaList.ForEach(delegate (MetaItem mi)
                {
                    mi.Preview = mi.getPreview();
                });
                p.publication.Meta = MetaItem.ListToJson(p.metaList);
                if (p.UpdateMeta())
                {
                    SendJSON("{ \"error\": false, \"msg\": \"Se han guardado los meta tags.\", \"id\": " + id + " }");
                    ModificationLog.AddPublicationRegistry(ModificationType.MODIFY, p.publication, GetCurrentUser(), h);
                }
                else
                {
                    SendJSON("{ \"error\": true, \"msg\": \"Error al guardar los meta tags.\", \"details\": \"\" }");
                }
            }
            catch (Exception ex)
            {
                SendJSON("{ \"error\": true, \"msg\": \"" + ex.Message + "\", \"details\": \"" + (ex.InnerException != null ? ex.InnerException.Message : "") + "\" }");
                return;
            }
        }

        public void Edit()
        {
            UserVO uvo=GetCurrentUser();
            Publication p = new Publication();
            if (Request["id"] == null && Request["permalink"] != null)
            {
                string perma = Request["permalink"];
                p.publication = Publication.GetByPermalink(perma);
            }
            else
            {
                int id = int.Parse(Request["id"]);
                p.publication = Publication.GetById(id);
            }
            SectionTree st = new SectionTree();
            SectionTreeNode stn;
            if (uvo.Type == "ADMIN")
               stn= st.getTree(p.publication.SectionId);
            else
                stn=st.getTree(Section.GetSectionsByLogin(uvo.Username),p.publication.SectionId);
            bool newsinclude = false;
            if (p.publication.SectionId != 0)
                newsinclude = Section.GetById(p.publication.SectionId).NewsInclude;
            if (String.IsNullOrEmpty(p.publication.Meta))
                p.publication.Meta = p.buildJsonAutomaticMeta();
            
            this.Items.Add("sectionTreeOptions", stn.ToOptions(1));
            this.Items.Add("selectedTab", "Publication");
            this.Items.Add("sectionNewsInclude", newsinclude.ToString().ToLower());
            this.Items.Add("publication", p.publication);
            this.Items.Add("action", "Update");
            this.Items.Add("readonly", uvo.Type == "ADMIN" ? "" : "readonly");

            this.ShowPage("publication/New.aspx");
        }

        public void Update()
        {
            try
            { 
                UserVO uvo = (UserVO)Context.Session["user"];
                int id, section, lang;
                DateTime published, unpublished;

                if (!int.TryParse(Request["Id"], out id) || !int.TryParse(Request["parentSectionId"], out section) || !int.TryParse(Request["languageId"], out lang))
                {
                    SendJSON("{\"error\":true,\"msg\":\"No se pudo actualizar la informacion\",\"id\":0}");
                    return;
                }
                if (!DateTime.TryParse(Request["published"], out published))
                {
                    SendJSON("{\"error\":true,\"msg\":\"La Fecha de publicacion es invalida\",\"id\":0}");
                    return;
                }
                if (!DateTime.TryParse(Request["unpublished"], out unpublished))
                {
                    SendJSON("{\"error\":true,\"msg\":\"La Fecha para retirar la publicacion es invalida\",\"id\":0}");
                    return;
                }
                if (unpublished.CompareTo(published) < 1)
                {
                    SendJSON("{\"error\":true,\"msg\":\"La Fecha para retirar la publicacion debe ser mayor a la fecha de publicacion\",\"id\":0}");
                    return;
                }

                string x = Request["meta"];
                bool hasDescription = false;
                Publication p = new Publication();
                p.publication.Id = id;
                p.publication = Publication.GetById(p.publication.Id);
                PublicationVO h = p.Clone(p.publication);
                p.metaList = MetaItem.ListFromJson(p.publication.Meta);
                p.metaList.ForEach(delegate (MetaItem mi)
                {
                    if (mi.Type == "description")
                        hasDescription = true;
                });
                p.publication.SectionId = section;
                p.publication.LanguageId = lang;
                p.publication.Created = DateTime.Parse(Request["created"]);
                p.publication.Updated = DateTime.Now;
                p.publication.Published = published;
                p.publication.Unpublished = unpublished;
                p.publication.IsMain = Request["isMain"] != null;
                p.publication.Title = Request["title"];
                p.publication.Content = Request["elm1"].Replace("\\n", "<br/>");
                p.publication.Permalink = Request["permalink"];
                p.publication.Status = p.GetStatus();
                p.publication.NewsTTL = 30;
                p.publication.Active = true;
                p.publication.Position = int.Parse(Request["Position"]);
                p.publication.Visitas = int.Parse(Request["visitas"]);
                p.publication.CssClass = Request["CssClass"];
                if (!hasDescription && !String.IsNullOrEmpty(p.publication.Content))
                {
                    MetaItem mi = p.buildDescription();
                    p.metaList.Add(mi);
                    p.publication.Meta = MetaItem.ListToJson(p.metaList);
                }

                Publication.SetLastUpdateDate(DateTime.Today);

                //If the publication contains a widget expression, then set the flag as true and save it
                WidgetParseResult wpr = WidgetParser.ContainsWidgetExpression(p.publication.Content, uvo.Username);
                if (!wpr.ContainsWidgetExpression && wpr.ErrorsInExpression != null)
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (string error in wpr.ErrorsInExpression)
                    {
                        sb.Append(string.Format("- {0}\\n", error));
                    }
                    SendJSON("{\"error\":true,\"msg\":\"" + sb.ToString() + "\",\"id\":" + p.publication.Id + "}");
                }
                else
                {
                    p.publication.HasWidgets = wpr.ContainsWidgetExpression;
                    bool saved = p.Save();
                    string exitMessage = string.Empty;
                    if (saved)
                    {
                        exitMessage = "{\"error\":false,\"msg\":\"Los datos han sido guardados\",\"id\":" + p.publication.Id + "}";
                        ModificationLog.AddPublicationRegistry(ModificationType.MODIFY, p.publication, uvo, h);
                    }
                    else
                    {
                        exitMessage = "{\"error\":true,\"msg\":\"Los datos NO han sido guardados\",\"id\":" + p.publication.Id + "}";
                    }

                    SendJSON(exitMessage);
                }
            }
            catch (Exception ex)
            {
                SendJSON("{ \"error\": true, \"msg\": \"" + ex.Message + "\", \"id\": 0, \"details\": \"" + ex.InnerException != null ? ex.InnerException.Message : "" + "\" }");
            }
        }
      
        /*
        public void EditNew()
        {
            int id = int.Parse(Request["id"]);
            PublicationVO vo = Publication.GetById(id);
            if (vo != null)
            {
                this.Items.Add("selectedTab", "Publication");
                this.Items.Add("sectionNewsInclude", Section.GetById(vo.SectionId).NewsInclude + "");
                this.Items.Add("publication", vo);
                this.Items.Add("action", "UpdateNews");
            }
            else
            {
                this.Items.Add("selectedTab", "Publication");
                this.Items.Add("sectionNewsInclude", true + "");
                this.Items.Add("publication", new PublicationVO());
                this.Items.Add("action", "UpdateNews");
            }

            this.ShowPage("publication/NewsEdition.aspx");
        }*/

        public void UpdateNews()
        {
            try
            {

                int id;
                if(!int.TryParse(Request["publicationId"], out id))
                {
                    SendJSON("{ \"error\": true, \"msg\": \"Id de publicación invalido.\" }");
                    return;
                }

                Publication p = new Publication();
                p.publication = Publication.GetById(id);

                string newsContent = Request["pubNewContent"];

                if (!string.IsNullOrEmpty(newsContent))
                {
                    p.publication.NewsContent = newsContent;
                }

                string newsttl = Request["pubNewTTL"];
                if (!string.IsNullOrEmpty(newsttl))
                {
                    p.publication.NewsTTL = int.Parse(newsttl);
                }

                string newsSectionInclude = Request["pubNewSectionInclude"];
                p.publication.NewsIncludeInSection = !string.IsNullOrEmpty(newsSectionInclude);

                string newsPublicationInclude = Request["pubNewInclude"];
                p.publication.NewsInclude = !string.IsNullOrEmpty(newsPublicationInclude);

                string cbNewsPin = Request["pubNewPermanent"];
                p.publication.NewsPin = !string.IsNullOrEmpty(cbNewsPin);

                if (p.SaveNews())
                {
                    SendJSON("{\"error\":false, \"msg\":\"Los datos han sido guardados\", \"id\":" + p.publication.Id + "}");
                }
                else
                {
                    SendJSON("{\"error\":true, \"msg\":\"Los datos NO han sido guardados\", \"id\":" + p.publication.Id + "}");
                }
            }
            catch (Exception ex)
            {
                SendJSONException(ex, "Error al actualizar noticia de publicación.");
            }
        }
        public void Delete()
        {
            int publicationId = int.Parse(Request["publicationId"]);
            UserVO uvo = (UserVO)Context.Session["user"];
            try
            {
                Publication.Delete(publicationId);
                SendJSON("{ \"error\": false, \"msg\": \"Se ha eliminado el registro\", \"modifiedValue\": " + publicationId + " }");
                ModificationLog.AddPublicationRegistry(ModificationType.DELETE, publicationId, GetCurrentUser().Username);
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger("PublicationAdmController").Error("Error al borrar la publicacion", ex);
                SendMessage("{ \"error\": true, \"msg\": \"Ocurrio un error al eliminar el registro\", \"details\": \"" + ex.Message + "\" }");
            }
        }
        public void SwitchMapExclude()
        {
            UserVO uvo = (UserVO)Context.Session["user"];
            if (uvo.Type == "ADMIN")
            {
                int publicationId = int.Parse(Request["publicationId"]);
                PublicationDAL pdal = new PublicationDAL();
                PublicationVO pvo = Publication.GetById(publicationId);
                pvo.SitemapExclude = !pvo.SitemapExclude;
                try
                {
                    pdal.Update(pvo);
                    SendJSON("{ \"error\": false, \"msg\": \"Cambio exitoso.\", \"modifiedValue\": " + pvo.SitemapExclude.ToString().ToLower() + " }");
                    ModificationLog.AddPublicationRegistry(ModificationType.MODIFY, pvo, GetCurrentUser());
                }
                catch (Exception ex)
                {
                    log4net.LogManager.GetLogger("PublicationAdmController").Error("Hubo un problema al cambiar el estado del visibilidad en el mapa de navegación.", ex);
                    SendJSON("{ \"error\": true, \"msg\": \"Hubo un problema al cambiar el estado del visibilidad en el mapa de navegación.\", \"details\": " + ex.Message + " }");
                }
            } else
            {
                SendJSON("{ \"error\": true, \"msg\": \"Usuario no autorizado.\" }");
            }
        }
    }
}