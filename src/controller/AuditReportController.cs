using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.bll;
using asf.cms.model;
using asf.cms.util;
using asf.cms.dal;

namespace asf.cms.controller
{
    public class AuditReportController:Controller
    {
        public AuditReportController(HttpContext context)
            : base(context)
        {
            isAdmin = 1;
        }
        public void  List()
        {
            this.Items.Add("selectedTab", "Audit");
            ShowPage("auditReport/List.aspx");
        }
        public void GetAllActiveReports()
        {
            GridInput gi = new GridInput();
            gi.Rows = new List<RowGrid>();

            UserVO uvo = (UserVO)Context.Session["user"];
            string username = "";
            AuditReport ar = new AuditReport();
            List<AuditReportInfo> lista= ar.GetList();

            foreach (AuditReportInfo ari in lista)
            {
                String imgStatus = "<img src='../view/img/status" + (ari.IsPublished ? 1 : 2) + ".png' alt='" + (ari.IsPublished ? "Publicado" : "No Publicado") + "' title='" + (ari.IsPublished ? "Publicado" : "No Publicado") + "'/>";

                RowGrid row = new RowGrid();
                row.Id = ari.ReportId.ToString();
                row.Cell = new List<string>();
                row.Cell.Add(imgStatus);
                row.Cell.Add(ari.Year + "");
                row.Cell.Add(ari.Title);
                row.Cell.Add(ari.PublishDate.ToShortDateString());
                row.Cell.Add(ari.LoadedAudits+"");
                gi.Rows.Add(row);
            }
            gi.calculaInternos();
            string json = gi.toJSON();
            ShowPage("js/default.js");
            Response.Write(json);
        }
        public void New()
        {
            ShowPage("auditReport/New.aspx");
        }
        public void Insert()
        {
            try
            {
                string year = Request["year"];
                if (year == null)
                    return;
                Publication p = new Publication();
                p.publication.LanguageId = Language.GetCurrentLanguageId();
                p.publication.Created = DateTime.Today;
                p.publication.Updated = DateTime.Today;
                p.publication.Published = DateTime.Today.AddYears(31);
                p.publication.Unpublished = DateTime.Today.AddYears(31);
                if (!String.IsNullOrEmpty(Request["published"]))
                    p.publication.Published = DateTime.Parse(Request["published"]);
                if (!String.IsNullOrEmpty(Request["unpublished"]))
                    p.publication.Unpublished = DateTime.Parse(Request["unpublished"]);
                p.publication.IsMain = false;
                p.publication.Title = Request["title"].Trim();
                p.publication.Permalink = Request["permalink"];
                p.publication.Position = 0;
                p.publication.Content = "";
                p.publication.Status = 2;
                p.publication.Active = true;
                p.publication.NewsTTL = 0;
                p.publication.Visitas = 0;
                Publication.SetLastUpdateDate(DateTime.Today);
                p.FilesPath = uploadUrl;
                p.Save();
                p.CreateDirectory();
                AuditReportVO arvo = new AuditReportVO();
                arvo.Year = int.Parse(Request["year"]);
                arvo.MainFilePath = Request["mainFilePath"];
                arvo.PresentationPath = Request["presentationPath"];
                arvo.PublicationPermalink = p.publication.Permalink;
                arvo.Published = false;
                arvo.Type = 1;
                arvo.DirectoryPath = Request["directoryPath"];
                arvo.ExecutiveReportPath = Request["executiveReportPath"];
                AuditReportDAL adal = new AuditReportDAL();
                adal.Insert(arvo);
                SendMessage("El informe ha sido creado");
            }
            catch (Exception ex)
            {
                SendMessage(ex.Message + "<br/>" + ex.StackTrace);
            }
        }

        public void Edit()
        {
            int id = int.Parse(Request["auditReportId"]);
            AuditReportDAL adal = new AuditReportDAL();
            AuditReportVO arvo = adal.GetById(id);
            PublicationVO pvo = Publication.GetByPermalink(arvo.PublicationPermalink);
            Context.Session["permalink"] = "";
            this.Items.Add("selectedTab", "Audit");
            this.Items.Add("auditReport", arvo);
            this.Items.Add("publication", pvo);
            ShowPage("auditReport/Edit.aspx");
        }
        public void Update()
        {
            try
            {
                string permalink = Request["permalink"];
                int auditReportId= int.Parse(Request["auditReportId"]);

                Publication p = new Publication();
                p.publication = Publication.GetByPermalink(permalink);
                p.publication.Updated = DateTime.Today;
                p.publication.Published = DateTime.Parse(Request["published"]);
                p.publication.Unpublished = DateTime.Parse(Request["unpublished"]);
                p.publication.Title = Request["title"].Trim();
                p.publication.Status = p.GetStatus();
                Publication.SetLastUpdateDate(DateTime.Today);
                p.Save();

                AuditReportDAL adal = new AuditReportDAL();
                
                AuditReportVO arvo = adal.GetById(auditReportId);
                arvo.Year = int.Parse(Request["year"]);
                arvo.MainFilePath = Request["mainFilePath"];
                arvo.PresentationPath = Request["presentationPath"];
                arvo.Published = false;
                arvo.Type = 1;
                arvo.DirectoryPath = Request["directoryPath"];
                arvo.ExecutiveReportPath = Request["executiveReportPath"];
                arvo.IndexPermalink = Request["indexPermalink"];
                arvo.Published = (p.publication.Status == 1);
                adal.Update(arvo);
                SendMessage("Los datos han sido guardados");
            }
            catch (Exception ex)
            {
                SendMessage(ex.Message + "<br/>" + ex.StackTrace);
            }
        }
        public void GetIndex()
        {
            AuditDAL adal = new AuditDAL();
            List<AuditVO> lista = new List<AuditVO>(adal.GetByYear(int.Parse(Request["year"])));
            lista.Sort();
            this.Items.Add("Indice", lista);
            this.ShowPage("auditReport/Indice.aspx");
        }
        public void SaveAutomaticIndex()
        {

            Publication p = new Publication();
            AuditReportVO ar = new AuditReportVO();
            AuditReportDAL ardal = new AuditReportDAL();
            ar = ardal.GetById(int.Parse(Request["auditReportId"]));
            if (String.IsNullOrEmpty(ar.IndexPermalink))
            {
                p.publication.LanguageId = Language.GetCurrentLanguageId();
                p.publication.Created = DateTime.Today;
                p.publication.Updated = DateTime.Today;
                p.publication.Published = DateTime.Parse(Request["published"]);
                p.publication.Unpublished = DateTime.Parse(Request["unpublished"]);
                p.publication.IsMain = false;
                p.publication.Title = Request["title"].Trim();
                p.publication.Permalink = Request["permalink"] + "_Index";
                p.publication.Position = 666;
                p.publication.Content = Request["content"];
                p.publication.Status = 1;
                p.publication.Active = true;
                p.publication.NewsTTL = 0;
                p.publication.Visitas = 0;
                p.FilesPath = Context.Server.MapPath("~/uploads/");
                p.Save();
                p.CreateDirectory();
                ar.IndexPermalink = p.publication.Permalink;
                ardal.Update(ar);
            }
            else {
                PublicationDAL pdal = new PublicationDAL();
                p.publication=pdal.GetByPermalink(ar.IndexPermalink);
                p.publication.Content = Request["content"];
                p.Save();
            }
            SendMessage(p.publication.Id+"");
            
        }

    }
}
