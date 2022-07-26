using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.util;
using asf.cms.model;
using asf.cms.bll;
using System.Text.RegularExpressions;
using log4net.Repository.Hierarchy;

namespace asf.cms.controller
{
    public class NewsAdmController : Controller
    {

        public NewsAdmController(HttpContext context)
            : base(context)
        {
            isAdmin = 1;
        }

        public void List()
        {
            this.Items.Add("selectedTab", "News");
            this.ShowPage("news/List.aspx");
        }

        public void Edit()
        {
            int id = int.Parse(Request["id"]);
            object issec = Request["isSection"];
            bool isSection = (issec!=null)?bool.Parse(issec+""):false;
            NewsVO vo = null;

            //Grab a publication, only if is not a section
            PublicationVO pvo = null;
            if (!isSection)
            {
                vo = News.GetByIdFromPublication(id);
                pvo = Publication.GetById(vo.Id);
                vo.IncludeInPublication = pvo.NewsInclude;
                vo.IncludeInSection = Section.GetById(pvo.SectionId).NewsInclude;
            }
            else 
            {
                vo = new NewsVO();
                Section sect = Section.GetById(id);
                vo.Title = sect.Title;
                vo.Id = id;
                vo.IsSection = isSection;
                vo.IncludeInSection = sect.NewsInclude;    
            }
            
            this.Items.Add("selectedTab", "News");
            this.Items.Add("new", vo);
            this.Items.Add("permalink", vo.Permalink);
            this.Items.Add("action", "Update");

            this.ShowPage("news/New.aspx");
        }

        private NewsVO GetNewsFromPublication(int id)
        {
            NewsVO nVO = News.GetByIdFromPublication(id);
            PublicationVO pubVO = Publication.GetById(nVO.Id);
            Section section = pubVO.SectionId > 0 ? Section.GetById(pubVO.SectionId) : null;
            
            nVO.IncludeInPublication = pubVO.NewsInclude;
            nVO.IncludeInSection = section != null ? section.NewsInclude : false;
            nVO.SectionTitle = section != null ? section.SpanishTitle : "-- Sin seccion --";
            nVO.IncludeIn = nVO.IncludeInPublication;
            return nVO;
        }

        private NewsVO GetNewsFromSection(int id)
        {
            NewsVO nVO = null;

            nVO = new NewsVO();
            Section sect = Section.GetById(id);
            nVO.Title = "";
            nVO.SectionTitle = sect.Title;
            nVO.Id = id;
            nVO.IsSection = true;
            nVO.IsMain = true;
            nVO.IncludeInSection = sect.NewsInclude;
            nVO.IncludeIn = nVO.IncludeInSection;
            return nVO;
        }

        public void GetAllNews()
        {
            try
            {
                UserVO uvo = (UserVO)Context.Session["user"];
                GridInput gi = new GridInput();
                gi.Rows = new List<RowGrid>();
                if (Request["page"] != null)
                {
                    gi.Page = int.Parse(Request["page"]);
                    gi.Sidx = Request["sidx"];
                    gi.Sord = Request["sord"];
                    gi.Limit = int.Parse(Request["rows"]);
                }

                List<NewsVO> newsList = new List<NewsVO>();
                List<Section> userSections = Section.GetRootSectionsByUser(uvo.Username);

                if (!string.IsNullOrEmpty(gi.Sidx))
                {
                    List<PublicationAdminVO> publicacionLista = Publication.ListPublicationAdmin(uvo.Username, "");
                    //List<Section> seccionLista = Section.GetSectionsByUser(uvo.Username);

                    foreach (PublicationAdminVO p in publicacionLista)
                        newsList.Add(GetNewsFromPublication(p.Id));

                    /*foreach (Section s in seccionLista)
                        newsList.Add(GetNewsFromSection(s.SectionId));*/

                    switch (gi.Sidx)
                    {
                        case "Title":
                            newsList.Sort(News.CompareByTitle);
                            break;
                        case "Content":
                            newsList.Sort(News.CompareByContent);
                            break;
                        case "IncludeIn":
                            newsList.Sort(News.CompareByIncludeIn);
                            break;
                        case "NewsTTL":
                            newsList.Sort(News.CompareByNewsTTL);
                            break;
                        case "NewsPin":
                            newsList.Sort(News.CompareByNewsPin);
                            break;
                    }

                    if (gi.Sord == "desc")
                    {
                        newsList.Reverse();
                    }
                }
                else
                {
                    List<PublicationAdminVO> publicacionLista = Publication.ListPublicationAdmin(uvo.Username, "");
                    //List<Section> seccionLista = Section.GetSectionsByUser(uvo.Username);

                    foreach (PublicationAdminVO p in publicacionLista)
                        newsList.Add(GetNewsFromPublication(p.Id));

                    /*foreach (Section s in seccionLista)
                        newsList.Add(GetNewsFromSection(s.SectionId));*/
                }

                foreach (NewsVO nvo in newsList)
                {
                    RowGrid row = new RowGrid();
                    row.Id = nvo.Id.ToString();
                    row.Cell = new List<string>();

                    /*string title = Regex.Replace(nvo.Title, "\\n", "</br>");
                    if (nvo.IsMain)
                    {
                        string indent = string.Empty;
                        for (int i = 0; i < nvo.Level; i++)
                        {
                            indent += "&nbsp;&nbsp;";
                        }
                        row.Cell.Add(string.Format("{0}<STRONG>{1}</STRONG>", indent, title));
                        nvo.IncludeIn = nvo.IncludeInSection;
                        row.Cell.Add((nvo.IncludeIn) ? "si" : "no");
                    }
                    else
                    {
                        string indent = string.Empty;
                        for (int i = 0; i < nvo.Level; i++)
                        {
                            indent += "&nbsp;&nbsp;&nbsp;&nbsp;";
                        }
                        row.Cell.Add(string.Format("{0}{1}", indent, title));
                        nvo.IncludeIn = nvo.IncludeInPublication;
                        row.Cell.Add((nvo.IncludeIn) ? "si" : "no");
                    }*/

                    row.Cell.Add(nvo.Title);
                    row.Cell.Add(nvo.SectionTitle);
                    row.Cell.Add(nvo.IncludeInPublication.ToString().ToLower());
                    row.Cell.Add(nvo.IncludeInSection.ToString().ToLower());
                    row.Cell.Add(nvo.NewsTTL.ToString());
                    row.Cell.Add(nvo.NewsPin.ToString().ToLower());
                    row.Cell.Add((string.IsNullOrEmpty(nvo.Content)) ? "false" : "true");
                    row.Cell.Add(nvo.IsSection.ToString().ToLower());

                    gi.Rows.Add(row);
                }
                gi.calculaInternos();
                SendJSON(gi.toJSON(false));
            }
            catch (Exception ex)
            {
                SendJSON("{ \"error\": true, \"msg\": \"" + ex.Message + "\", \"details\": \"" + ex.InnerException != null ? ex.InnerException.Message : "" + "\"}");
            }
        }

        public void GetAllVisibleNews()
        {
            try
            {
                UserVO uvo = (UserVO)Context.Session["user"];
                GridInput gi = new GridInput();
                gi.Rows = new List<RowGrid>();
                if (Request["page"] != null)
                {
                    gi.Page = int.Parse(Request["page"]);
                    gi.Sidx = Request["sidx"];
                    gi.Sord = Request["sord"];
                    gi.Limit = int.Parse(Request["rows"]);
                }

                List<NewsVO> newsList = new List<NewsVO>();
                List<Section> userSections = Section.GetRootSectionsByUser(uvo.Username);

                if (!string.IsNullOrEmpty(gi.Sidx))
                {
                    List<PublicationAdminVO> publicacionLista = Publication.ListPublicationAdmin(uvo.Username, "");
                    //List<Section> seccionLista = Section.GetSectionsByUser(uvo.Username);

                    foreach (PublicationAdminVO p in publicacionLista)
                        newsList.Add(GetNewsFromPublication(p.Id));

                    /*foreach (Section s in seccionLista)
                        newsList.Add(GetNewsFromSection(s.SectionId));*/

                    switch (gi.Sidx)
                    {
                        case "Title":
                            newsList.Sort(News.CompareByTitle);
                            break;
                        case "Content":
                            newsList.Sort(News.CompareByContent);
                            break;
                        case "IncludeIn":
                            newsList.Sort(News.CompareByIncludeIn);
                            break;
                        case "NewsTTL":
                            newsList.Sort(News.CompareByNewsTTL);
                            break;
                        case "NewsPin":
                            newsList.Sort(News.CompareByNewsPin);
                            break;
                    }

                    if (gi.Sord == "desc")
                    {
                        newsList.Reverse();
                    }
                }
                else
                {
                    List<PublicationAdminVO> publicacionLista = Publication.ListPublicationAdmin(uvo.Username, "");
                    //List<Section> seccionLista = Section.GetSectionsByUser(uvo.Username);

                    foreach (PublicationAdminVO p in publicacionLista)
                        newsList.Add(GetNewsFromPublication(p.Id));

                    /*foreach (Section s in seccionLista)
                        newsList.Add(GetNewsFromSection(s.SectionId));*/
                    
                }

                foreach (NewsVO nvo in newsList)
                {
                    if (nvo.IncludeIn)
                    {
                        RowGrid row = new RowGrid();
                        row.Id = nvo.Id.ToString();
                        row.Cell = new List<string>();

                        /*string title = Regex.Replace(nvo.Title, "\\n", "</br>");
                        if (nvo.IsMain)
                        {
                            string indent = string.Empty;
                            for (int i = 0; i < nvo.Level; i++)
                            {
                                indent += "&nbsp;&nbsp;";
                            }
                            row.Cell.Add(string.Format("{0}<STRONG>{1}</STRONG>", indent, title));
                            nvo.IncludeIn = nvo.IncludeInSection;
                            row.Cell.Add((nvo.IncludeIn) ? "si" : "no");
                        }
                        else
                        {
                            string indent = string.Empty;
                            for (int i = 0; i < nvo.Level; i++)
                            {
                                indent += "&nbsp;&nbsp;&nbsp;&nbsp;";
                            }
                            row.Cell.Add(string.Format("{0}{1}", indent, title));
                            nvo.IncludeIn = nvo.IncludeInPublication;
                            row.Cell.Add((nvo.IncludeIn) ? "si" : "no");
                        }*/

                        row.Cell.Add(nvo.Title);
                        row.Cell.Add(nvo.SectionTitle);
                        row.Cell.Add(nvo.IncludeIn.ToString().ToLower());
                        row.Cell.Add(nvo.IncludeInSection.ToString().ToLower());
                        row.Cell.Add(nvo.NewsTTL.ToString());
                        row.Cell.Add(nvo.NewsPin.ToString().ToLower());
                        row.Cell.Add((string.IsNullOrEmpty(nvo.Content)) ? "false" : "true");
                        row.Cell.Add(nvo.IsSection.ToString().ToLower());

                        gi.Rows.Add(row);
                    }
                }

                gi.calculaInternos();
                SendJSON(gi.toJSON(false));
            }
            catch (Exception ex)
            {
                SendJSON("{ \"error\": true, \"msg\": \"" + ex.Message + "\", \"details\": \"" + ex.InnerException != null ? ex.InnerException.Message : "" + "\"}");
            }
        }

        public void Update()
        {
            News n = new News();

            n.NewsInstance = new NewsVO();

            n.NewsInstance.Id = int.Parse(Request["Id"]);
            n.NewsInstance.IsSection = bool.Parse(Request["IsSection"]);

            string newsContent = Request["newsContent"];

            if (!string.IsNullOrEmpty(newsContent))
            {
                n.NewsInstance.Content = newsContent;
            }

            string newsttl = Request["txtNewsTTL"];
            if (!string.IsNullOrEmpty(newsttl))
            {
                n.NewsInstance.NewsTTL = int.Parse(newsttl);
            }

            string newsSectionInclude = Request["cbNewsSectionInclude"];
            n.NewsInstance.IncludeInSection = !string.IsNullOrEmpty(newsSectionInclude);

            string newsPublicationInclude = Request["cbNewsPublicationInclude"];
            n.NewsInstance.IncludeInPublication = !string.IsNullOrEmpty(newsPublicationInclude);

            string cbNewsPin = Request["cbNewsPin"];
            n.NewsInstance.NewsPin = !string.IsNullOrEmpty(cbNewsPin);

            bool saved = n.Save();

            string exitMessage = string.Empty;
            exitMessage = (saved) ? "{\"error\":0, \"msj\":\"Los datos han sido guardados\",\"id\":" + n.NewsInstance.Id + "}" : "{\"error\":1,\"msj\":\"Los datos NO han sido guardados\",\"id\":" + n.NewsInstance.Id + "}";
            SendMessage(exitMessage);
        }
    }
} 