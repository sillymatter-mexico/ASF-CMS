using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.model;
using asf.cms.dal;
using System.Text;
using System.IO;
using asf.cms.util;
using asf.cms.widgets;



namespace asf.cms.bll
{
    public class Especiales
    {
        static DateTime LastUpdate = DateTime.MinValue;
        public Especiales() { }
        private Especiales(EspecialesVO evo)
        {
            this.Title = evo.Title;
            this.Content = evo.Content;
            this.IsMain = evo.IsMain;
            this.SectionId = evo.Id;
            this.Permalink = evo.Permalink;
            this.Created = evo.Created;
            this.Updated = evo.Updated;
            this.Visitas = evo.Visitas;
            this.NewsInclude = evo.NewsInclude;
            this.Position = evo.Position;
            this.SitemapExclude = evo.SitemapExclude;
            this.Active = evo.Active;
            this.Meta = evo.Meta;
        }

        #region propiedades
        public string Title { set; get; }
        public string Content { set; get; }
        public string Meta { set; get; }
        public bool IsMain { set; get; }
        public int SectionId { set; get; }
        public string Permalink { set; get; }
        public DateTime Created { set; get; }
        public DateTime Updated { set; get; }
        public int Visitas { set; get; }
        public bool NewsInclude { set; get; }
        public int Position { set; get; }
        public bool SitemapExclude { set; get; }
        public bool Active { set; get; }
        #endregion

        EspecialesVO pvo = new EspecialesVO();
        public EspecialesVO publication
        {
            set { pvo = value; }
            get { return pvo; }
        }
        public String FilesPath
        {
            set; get;
        }
        public List<MetaItem> metaList
        {
            set; get;
        }
        public List<MetaItem> buildAutomaticMeta()
        {
            this.metaList = new List<MetaItem>();
            MetaItem mi = new MetaItem();
            if (!String.IsNullOrEmpty(publication.Title))
            {
                mi.Type = "title";
                mi.Value = publication.Title;
                mi.Preview = mi.getPreview();
                metaList.Add(mi);
            }
            mi = new MetaItem();
            mi.Type = "language";
            mi.Value = Language.GetLanguageCode(publication.LanguageId);
            mi.Preview = mi.getPreview();
            metaList.Add(mi);
            if (!String.IsNullOrEmpty(publication.Content))
            {
                mi = new MetaItem();
                mi.Type = "description";
                mi.Value = asf.cms.util.Encoder.RemoveHTML(publication.Content);
                if (mi.Value.Length > 100)
                    mi.Value = mi.Value.Substring(0, 100);
                mi.Preview = mi.getPreview();
                metaList.Add(mi);
            }
            return metaList;
        }
        public MetaItem buildDescription()
        {
            MetaItem mi = new MetaItem();
            if (String.IsNullOrEmpty(publication.Content))
                return mi;
            mi = new MetaItem();
            mi.Type = "description";
            mi.Value = asf.cms.util.Encoder.RemoveHTML(publication.Content);
            if (mi.Value.Length > 100)
                mi.Value = mi.Value.Substring(0, 100);
            mi.Preview = mi.getPreview();
            return mi;

        }
        public string buildJsonAutomaticMeta()
        {
            return MetaItem.ListToJson(buildAutomaticMeta());
        }

        public static List<EspecialesAdminVO> ListPublicationAdmin(String username, string orden)
        {
            List<EspecialesAdminVO> lista = new List<EspecialesAdminVO>();
            EspecialesDAL pdal = new EspecialesDAL();

            User user = User.GetUser(username);

            if (user.user == null)
                return lista;

            if (user.user.Type == "ADMIN")
                return new List<EspecialesAdminVO>(pdal.ListPublicationAdmin(orden));

            List<Section> sections = Section.GetSectionsByUser(username);

            foreach (Section s in sections)
                lista.AddRange(ListPublicationAdmin(s.SectionId));

            return lista;
        }
        public static List<EspecialesAdminVO> ListPublicationAdmin(int sectionId)
        {
            EspecialesDAL pdal = new EspecialesDAL();
            return new List<EspecialesAdminVO>(pdal.ListPublicationAdmin(sectionId));
        }
        public static List<EspecialesVO> GetAll()
        {
            EspecialesDAL pdal = new EspecialesDAL();
            return new List<EspecialesVO>(pdal.GetAll());
        }
        public static string GenerateList(List<EspecialesVO> publications, int selected)
        {
            if (publications.Count == 0)
                return "";
            StringBuilder sb = new StringBuilder();
            foreach (EspecialesVO p in publications)
            {
                if (p.IsMain) continue;
                sb.Append("<li " + ((p.Id == selected) ? "class='selected'" : "") + ">");
                sb.Append("<a href='../Especiales/" + p.Permalink + "'>" + p.Title + "</a>");
                sb.Append("</li>");
            }
            return sb.ToString();
        }
        public static EspecialesVO GetById(int publicationId)
        {
            EspecialesDAL pdal = new EspecialesDAL();
            EspecialesVO pvo = pdal.GetById(publicationId);
            return pvo;

        }
        public static EspecialesVO GetByPermalink(string permalink)
        {
            EspecialesDAL pdal = new EspecialesDAL();
            EspecialesVO pvo = pdal.GetByPermalink(permalink);
            return pvo;

        }
        public static List<Especiales> GetByAutogeneratedType(string especialesId)
        {
            EspecialesDAL edal = new EspecialesDAL();
            List<Especiales> especiales = new List<Especiales>();

            foreach (EspecialesVO evo in edal.GetByType(especialesId))
            {
                Especiales e = new Especiales(evo);
                especiales.Add(e);
            }
            return especiales;
        }
        public bool UpdateViews()
        {
            EspecialesDAL pdal = new EspecialesDAL();
            pdal.UpdateViews(this.publication.Id, this.publication.Visitas);
            return true;

        }
        public bool UpdateMeta()
        {
            EspecialesDAL pdal = new EspecialesDAL();
            pdal.UpdateMeta(this.publication.Id, this.publication.Meta);
            return true;

        }

        public bool Save()
        {
            UserVO uvo = (UserVO)HttpContext.Current.Session["user"];
            EspecialesDAL pdal = new EspecialesDAL();
            publication.Permalink = util.Encoder.RemoveSigns(publication.Permalink.Trim());

            //Retrieve the original publication and set the data of the new, so it doesn´t get erased
            //This is because is not the responsability of this bll to handle that data
            EspecialesVO originalPublication = GetById(publication.Id);
            if (originalPublication != null)
            {
                publication.NewsContent = originalPublication.NewsContent;
                publication.NewsInclude = originalPublication.NewsInclude;
                publication.NewsIncludeInSection = originalPublication.NewsIncludeInSection;
                publication.NewsPin = originalPublication.NewsPin;
                //    publication.NewsTTL = originalPublication.NewsTTL;
            }

            publication = pdal.Update(publication);
            if (publication.IsMain)
                pdal.SetUniqueMainBySection(publication.Id, publication.SectionId, publication.LanguageId);
            if (!publication.Permalink.StartsWith(publication.Id.ToString() + "_"))
                publication.Permalink = publication.Id + "_" + publication.Permalink;

            //If the publication contains a widget expression, then set the flag as true and save it
            WidgetParseResult wpr = WidgetParser.ContainsWidgetExpression(publication.Content, uvo.Username);
            AssignWidget(wpr.ContainsWidgetExpression);
            if (string.IsNullOrEmpty(publication.Meta))
                publication.Meta = buildJsonAutomaticMeta();
            publication = pdal.Update(publication);
            return true;
        }
        public int GetStatus()
        {
            if (this.publication.Published.CompareTo(DateTime.Today) > 0)
                return 2;//aun no se publica
            if (this.publication.Unpublished.CompareTo(DateTime.Today) > 0)
                return 1; //publicada
            return 3; //retirada

        }
        public List<EspecialesVO> GetBySectionId(int sectionId)
        {
            EspecialesDAL pDAL = new EspecialesDAL();
            List<EspecialesVO> publications = new List<EspecialesVO>(pDAL.GetListBySection(sectionId, Language.GetCurrentLanguageId()));
            return publications;
        }
        public List<EspecialesVO> GetBySectionId(int sectionId, string orderByFields)
        {
            EspecialesDAL pDAL = new EspecialesDAL();
            List<EspecialesVO> publications = new List<EspecialesVO>(pDAL.GetListBySection(sectionId, Language.GetCurrentLanguageId(), orderByFields));
            return publications;
        }
        public static string GetLink(EspecialesVO pub)
        {
            return "../Especiales/" + pub.Permalink;
        }
        public bool SaveNews()
        {
            EspecialesDAL pDAL = new EspecialesDAL();
            pDAL.UpdatePublicationNews(publication.Id, publication.NewsContent, publication.NewsTTL, publication.NewsInclude, publication.NewsPin);

            SectionDAL sDAL = new SectionDAL();
            SectionVO section = sDAL.GetById(publication.SectionId);
            section.NewsInclude = publication.NewsIncludeInSection;

            sDAL.Update(section);

            return true;
        }
        public bool AssignWidget(bool has_widgets)
        {
            EspecialesDAL pdal = new EspecialesDAL();
            publication.HasWidgets = true;
            int updatedRows = pdal.SetHasWidgetsBySection(publication.Id, publication.SectionId, has_widgets);
            return (updatedRows > 0);
        }
        public bool CreateDirectory()
        {
            FilesPath += publication.Permalink;
            if (!File.Exists(FilesPath))
                Directory.CreateDirectory(FilesPath);
            return true;
        }

        /// <summary>
        /// Returns a list of all files of type f4v and flv
        /// </summary>
        /// <returns></returns>
        public List<FileVO> GetAllVideos()
        {
            FileDAL fdal = new FileDAL();
            return new List<FileVO>(fdal.GetFilesByMime("f4v,flv", Language.GetCurrentLanguageId()));
        }

        /// <summary>
        /// Returns a list of files of type f4v and flv for the provided publication id
        /// </summary>
        /// <param name="publicationId"></param>
        /// <returns></returns>
        public List<FileVO> GetVideosByPublicationId(string publicationId)
        {
            FileDAL fdal = new FileDAL();
            return new List<FileVO>(fdal.GetFilesByPublicationIdAndMime(publicationId, "f4v,flv"));
        }

        public static bool Delete(int publicationId)
        {
            EspecialesDAL pdal = new EspecialesDAL();
            if (pdal.Inactive(publicationId) != 1)
                return false;
            return true;
        }
        public static List<EspecialesVO> GetWithoutContentBySectionId(int sectionId)
        {
            EspecialesDAL pdal = new EspecialesDAL();
            return new List<EspecialesVO>(pdal.GetWithoutContentBySectionId(sectionId));
        }

        public static DateTime GetLastUpdateDate()
        {
            EspecialesDAL pdal = new EspecialesDAL();
            if (LastUpdate == DateTime.MinValue)
                LastUpdate = pdal.getFechaUltimaModificacion();
            return LastUpdate;
        }
        public static void SetLastUpdateDate(DateTime date)
        {
            LastUpdate = date;
        }

        public static DateTime GetLastUpdateDateBySectionId(int sectionId)
        {
            EspecialesDAL pdal = new EspecialesDAL();
            if (LastUpdate == DateTime.MinValue)
                LastUpdate = pdal.getFechaUltimaModificacionPorSeccion(sectionId);
            return LastUpdate;
        }

        public static EspecialesVO GetPrincipalBySection(int sectionId)
        {
            EspecialesDAL pDAL = new EspecialesDAL();
            return pDAL.GetPrincipalBySection(sectionId, Language.GetCurrentLanguageId());
        }
        public static List<FileVO> GetFiles(int publicationId)
        {
            FileDAL fDAL = new FileDAL();
            return new List<FileVO>(fDAL.ListByPublicationId(publicationId));
        }

        public EspecialesVO Clone()
        {
            EspecialesVO clone = new EspecialesVO();

            clone.Active = this.publication.Active;
            clone.AutogeneratedType = this.publication.AutogeneratedType;
            clone.Content = this.publication.Content;
            clone.Created = this.publication.Created;
            clone.HasWidgets = this.publication.HasWidgets;
            //Id
            clone.IsAutogenerated = this.publication.IsAutogenerated;
            clone.IsMain = this.publication.IsMain;
            clone.LanguageId = this.publication.LanguageId;
            clone.Meta = this.publication.Meta;
            clone.NewsContent = this.publication.NewsContent;
            clone.NewsInclude = this.publication.NewsInclude;
            clone.NewsIncludeInSection = this.publication.NewsIncludeInSection;
            clone.NewsPin = this.publication.NewsPin;
            clone.NewsTTL = this.publication.NewsTTL;
            clone.Permalink = this.publication.Permalink;
            clone.Position = this.publication.Position;
            clone.Published = this.publication.Published;
            clone.SectionId = this.publication.SectionId;
            clone.SitemapExclude = this.publication.SitemapExclude;
            clone.Status = this.publication.Status;
            clone.Title = this.publication.Title;
            clone.Unpublished = this.publication.Unpublished;
            clone.Updated = this.publication.Updated;
            clone.Visitas = this.publication.Visitas;

            return clone;
        }
    }
}
