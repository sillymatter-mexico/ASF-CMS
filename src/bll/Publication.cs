using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.model;
using asf.cms.dal;
using System.Text;
using System.IO;
using asf.cms.widgets;
using System.Configuration;
using System.Text.RegularExpressions;

namespace asf.cms.bll
{
    public class Publication
    {
        static DateTime LastUpdate = DateTime.MinValue;

        PublicationVO pvo = new PublicationVO(),
                      pvoHistoric = null;

        public PublicationVO publication
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
        public static List<PublicationAdminVO> ListPublicationAdmin(String username, string orden)
        {
            List<PublicationAdminVO> lista = new List<PublicationAdminVO>();
            PublicationDAL pdal = new PublicationDAL();

            User user = User.GetUser(username);
            if (user.user == null)
                return lista;
            if (user.user.Type == "ADMIN")
                return new List<PublicationAdminVO>(pdal.ListPublicationAdmin(orden));
            List<Section> sections = Section.GetSectionsByUser(username);
            foreach (Section s in sections)
                lista.AddRange(ListPublicationAdmin(s.SectionId));
            return lista;
        }
        public static List<PublicationAdminVO> ListPublicationAdmin(int sectionId)
        {
            PublicationDAL pdal = new PublicationDAL();
            return new List<PublicationAdminVO>(pdal.ListPublicationAdmin(sectionId));
        }
        public static List<PublicationVO> GetAll()
        {
            PublicationDAL pdal = new PublicationDAL();
            return new List<PublicationVO>(pdal.GetAll());
        }
        public static string GenerateList(List<PublicationVO> publications, int selected)
        {
            if (publications.Count == 0)
                return "";
            StringBuilder sb = new StringBuilder();
            foreach (PublicationVO p in publications)
            {
                if (p.IsMain) continue;
                sb.Append("<li " + ((p.Id == selected) ? "class='selected'" : "") + ">");
                sb.Append("<a href='../Publication/" + p.Permalink + "'>" + p.Title + "</a>");
                sb.Append("</li>");
            }
            return sb.ToString();
        }
        public static PublicationVO GetById(int publicationId)
        {
            PublicationDAL pdal = new PublicationDAL();
            PublicationVO pvo = pdal.GetById(publicationId);
            return pvo;

        }
        public static PublicationVO GetByPermalink(string permalink)
        {
            PublicationDAL pdal = new PublicationDAL();
            PublicationVO pvo = pdal.GetByPermalink(permalink);
            return pvo;

        }
        public bool UpdateViews()
        {
            PublicationDAL pdal = new PublicationDAL();
            pdal.UpdateViews(this.publication.Id, this.publication.Visitas);
            return true;

        }
        public bool UpdateMeta()
        {
            PublicationDAL pdal = new PublicationDAL();
            pdal.UpdateMeta(this.publication.Id, this.publication.Meta);
            return true;

        }
        public bool Save()
        {
            UserVO uvo = (UserVO)HttpContext.Current.Session["user"];
            PublicationDAL pdal = new PublicationDAL();
            publication.Permalink = util.Encoder.RemoveSigns(publication.Permalink.Trim());

            //Perform content strings encoding check
            publication.Title = GetLatinString(publication.Title);
            publication.Content = GetLatinString(CleanupMceTags(publication.Content));
            publication.NewsContent = GetLatinString(publication.NewsContent);
            publication.CssClass = GetLatinString(publication.CssClass);
            publication.Meta = GetLatinString(publication.Meta);

            //Retrieve the original publication and set the data of the new, so it doesn´t get erased
            //This is because is not the responsability of this bll to handle that data
            PublicationVO originalPublication = GetById(publication.Id);

            if (originalPublication != null)
            {
                publication.NewsContent = GetLatinString(originalPublication.NewsContent);
                publication.NewsInclude = originalPublication.NewsInclude;
                publication.NewsIncludeInSection = originalPublication.NewsIncludeInSection;
                publication.NewsPin = originalPublication.NewsPin;
                //    publication.NewsTTL = originalPublication.NewsTTL;
                pvoHistoric = Clone(originalPublication);
                if (HasContentChanged())
                    SaveHistoric();
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
        public List<PublicationVO> GetBySectionId(int sectionId)
        {
            PublicationDAL pDAL = new PublicationDAL();
            List<PublicationVO> publications = new List<PublicationVO>(pDAL.GetListBySection(sectionId, Language.GetCurrentLanguageId()));
            return publications;
        }
        public List<PublicationVO> GetBySectionId(int sectionId, string orderByFields)
        {
            PublicationDAL pDAL = new PublicationDAL();
            List<PublicationVO> publications = new List<PublicationVO>(pDAL.GetListBySection(sectionId, Language.GetCurrentLanguageId(), orderByFields));
            return publications;
        }
        public static string GetLink(PublicationVO pub)
        {
            return "../Publication/" + pub.Permalink;
        }
        public bool SaveNews()
        {
            PublicationDAL pDAL = new PublicationDAL();
            int res = pDAL.UpdatePublicationNews(publication.Id, publication.NewsContent, publication.NewsTTL, publication.NewsInclude, publication.NewsPin);

            if (res > 0)
            {
                SectionDAL sDAL = new SectionDAL();
                SectionVO section = sDAL.GetById(publication.SectionId);
                section.NewsInclude = publication.NewsIncludeInSection;

                sDAL.Update(section);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AssignWidget(bool has_widgets)
        {
            PublicationDAL pdal = new PublicationDAL();
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

        public List<PublicationVO> GetHistoric()
        {
            int maxPublications = 100;
            int.TryParse(ConfigurationManager.AppSettings.Get("HistoricPublicationLimit"), out maxPublications);

            PublicationDAL pDAL = new PublicationDAL();
            return new List<PublicationVO>(pDAL.GetHistoric(publication.Id, maxPublications));
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
            PublicationDAL pdal = new PublicationDAL();
            if (pdal.Inactive(publicationId) != 1)
                return false;
            return true;
        }
        public static List<PublicationVO> GetWithoutContentBySectionId(int sectionId)
        {
            PublicationDAL pdal = new PublicationDAL();
            return new List<PublicationVO>(pdal.GetWithoutContentBySectionId(sectionId));
        }
        public static DateTime GetLastUpdateDate()
        {
            PublicationDAL pdal = new PublicationDAL();
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
            PublicationDAL pdal = new PublicationDAL();
            if (LastUpdate == DateTime.MinValue)
                LastUpdate = pdal.getFechaUltimaModificacionPorSeccion(sectionId);
            return LastUpdate;
        }
        public static PublicationVO GetPrincipalBySection(int sectionId)
        {
            PublicationDAL pDAL = new PublicationDAL();
            return pDAL.GetPrincipalBySection(sectionId, Language.GetCurrentLanguageId());
        }
        public static List<FileVO> GetFiles(int publicationId)
        {
            FileDAL fDAL = new FileDAL();
            return new List<FileVO>(fDAL.ListByPublicationId(publicationId));
        }
        public static FileVO GetMainFile(int publicationId)
        {
            FileDAL fDAL = new FileDAL();
            return fDAL.GetMainFile(publicationId);
        }
        public PublicationVO Clone(PublicationVO publication)
        {
            PublicationVO clonePub = new PublicationVO();

            clonePub.Active = publication.Active;
            clonePub.AutogeneratedType = publication.AutogeneratedType;
            clonePub.Content = publication.Content;
            clonePub.Created = publication.Created;
            clonePub.CssClass = publication.CssClass;
            clonePub.HasWidgets = publication.HasWidgets;
            clonePub.Id = publication.Id;
            clonePub.IsMain = publication.IsMain;
            clonePub.LanguageId = publication.LanguageId;
            clonePub.Meta = publication.Meta;
            clonePub.NewsContent = publication.NewsContent;
            clonePub.NewsInclude = publication.NewsInclude;
            clonePub.NewsIncludeInSection = publication.NewsIncludeInSection;
            clonePub.NewsPin = publication.NewsPin;
            clonePub.NewsTTL = publication.NewsTTL;
            clonePub.Permalink = publication.Permalink;
            clonePub.Position = publication.Position;
            clonePub.Published = publication.Published;
            clonePub.SectionId = publication.SectionId;
            clonePub.SitemapExclude = publication.SitemapExclude;
            clonePub.Status = publication.Status;
            clonePub.Title = publication.Title;
            clonePub.Unpublished = publication.Unpublished;
            clonePub.Updated = publication.Updated;
            clonePub.Visitas = publication.Visitas;

            return clonePub;
        }
        private bool SaveHistoric()
        {
            if (pvoHistoric != null)
            {
                int historicLimit = 10;
                int.TryParse(ConfigurationManager.AppSettings.Get("HistoricPublicationLimit"), out historicLimit);

                pvoHistoric.Active = false;
                pvoHistoric.Id = 0;
                pvoHistoric.Status = 4;
                pvoHistoric.Unpublished = DateTime.Now;
                pvoHistoric.Updated = DateTime.Now;

                PublicationDAL pDAL = new PublicationDAL();
                pvoHistoric = pDAL.Insert(pvoHistoric);

                pDAL.PruneHistoric(pvo.Id, historicLimit);

                return true;
            }
            return false;
        }
        private bool HasContentChanged()
        {
            return pvoHistoric == null ? false :
                   pvo.Content != pvoHistoric.Content ||
                   pvo.NewsContent != pvoHistoric.NewsContent ||
                   pvo.Title != pvoHistoric.Title;
        }

        private string GetLatinString(string testString)
        {
            return testString == null ? null : util.Encoder.IsEncodingLatin1(testString) ? testString : util.Encoder.ForceEncodingLatin1(testString);
        }

        private string CleanupMceTags(string contentString)
        {
            return contentString == null ? null : contentString.Replace("mce:script", "script").Replace("_mce_src", "src");
        }
    }
}
