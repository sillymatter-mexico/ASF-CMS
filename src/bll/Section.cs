using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.dal;
using asf.cms.model;
using System.Text;
namespace asf.cms.bll
{
    public class Section
    {
        #region constructores
        public Section(){}
        private Section(SectionVO svo)
        {
            this.IsMain = svo.IsMain;
            this.SectionId = svo.Id;
            this.RedirectTo = svo.RedirectTo;
            this.Permalink = svo.Permalink;
            this.Created = svo.Created;
            this.ParentSectionId = svo.ParentSectionId;
            this.Updated = svo.Updated;
            this.Visitas = svo.Visitas;
            this.NewsInclude = svo.NewsInclude;
            this.Position=svo.Position;
            this.RedirectOptions = svo.RedirectOptions;
            this.SitemapExclude = svo.SitemapExclude;
            this.Active = svo.Active;
            this.Type = svo.Type;
            this.CssClass = svo.CssClass;

            this.MainPublication = Publication.GetPrincipalBySection(this.SectionId);
        }

        private Section(SectionVO svo, SectionLabelVO label):this(svo)
        {
            this.SectionId = label.SectionLabelId.SectionId;
            this.Title = label.Content;
            this.NewsInclude = svo.NewsInclude;
        }

        private Section(SectionVO svo, SectionLabelVO spanishLabel, SectionLabelVO englishLabel)
            : this(svo)
        {
            this.EnglishTitle = englishLabel==null?"":englishLabel.Content;
            this.SpanishTitle = spanishLabel == null ? "" : spanishLabel.Content;
            this.NewsInclude = svo.NewsInclude;
            this.Title = Language.GetCurrentLanguageId() == 1 ? this.SpanishTitle : this.EnglishTitle;
        }
        private Section(SectionLabelVO label)
        {
            this.SectionId = label.SectionLabelId.SectionId;
            this.Title = label.Content;
        }
        #endregion

        #region propiedades
        public PublicationVO MainPublication
        {
            set;
            get;
        }
        public int SectionId
        {
            set;
            get;
        }
        public string Title
        {
            set;
            get;
        }
        public string EnglishTitle
        {
            set;
            get;
        }
        public string SpanishTitle
        {
            set;
            get;
        }
        public int? ParentSectionId
        {
            set;
            get;
        }
        public int Visitas
        {
            set;
            get;
        }
        public int Position
        {
            set;
            get;
        }
        public string RedirectOptions
        {
            set;
            get;
        }

        List<Section> _subsections;

        public List<Section> SubSections
        {
            set
            {
                _subsections = value;
            }
            get
            {
                if (_subsections == null)
                {
                    _subsections = GetSubsections(SectionId);
                }
                return _subsections;
            }
        }

        public DateTime Created
        {
            set;
            get;
        }

        public DateTime Updated
        {
            set;
            get;
        }

        public bool IsMain
        {
            set;
            get;
        }
        public string RedirectTo
        {
            set;
            get;
        }
        public string Permalink
        {
            set;
            get;
        }
        public bool Active
        {
            set;
            get;
        }

        public bool NewsInclude { get; set; }

        List<PublicationVO> _publications;
        public List<PublicationVO> Publications
        {
            set
            {
                _publications=value;
            }
            get 
            { 
                if (_publications == null)
                {
                    int lid = Language.GetCurrentLanguageId();
                    PublicationDAL pDAL = new PublicationDAL();

                    _publications = (List<PublicationVO>)pDAL.GetListBySection(SectionId, lid);
                }
                 
                return _publications;
            }
        }

        public bool SitemapExclude { get; set; }

        public string Type { get; set; }

        public string CssClass { get; set; }

        public bool IsRedirection { get { return !string.IsNullOrEmpty(RedirectTo); } }
        #endregion

        #region comparadores

        public static int CompareByLanguageESP(Section a, Section b)
        {
            return string.Compare(a.SpanishTitle, b.SpanishTitle);
        }

        public static int CompareByLanguageENG(Section a, Section b)
        {
            return string.Compare(a.EnglishTitle, b.EnglishTitle);
        }

        public static int CompareByLink(Section a, Section b)
        {
            return string.Compare(a.GetLink(), b.GetLink());
        }

        #endregion

        public static Section GetById(int sectionId)
        {
            SectionDAL sdal = new SectionDAL();
            SectionVO svo = sdal.GetById(sectionId);
            SectionLabelDAL sldal = new SectionLabelDAL();
            return new Section(svo,sldal.GetById(svo.Id, 1),sldal.GetById(svo.Id,2));
        }
        public static List<Section> GetRootSections(bool onlyVisible = false)
        { 
            SectionDAL sdal= new SectionDAL();
            SectionLabelDAL sldal = new SectionLabelDAL();
            List<Section> sections = new List<Section>();
            IList<SectionVO> sectionsVOS = onlyVisible ? sdal.GetRootSectionsVisible() : sdal.GetRootSections();
            foreach (SectionVO svo in sectionsVOS)
            {
                Section s = new Section(svo, sldal.GetByFitLanguage(svo.Id, Language.GetCurrentLanguageId()));
                sections.Add(s);
            }
            return sections;
        }
        public static List<Section> GetRootSectionsByUser(string username)
        {
            List<Section> lista = new List<Section>();
            User user = User.GetUser(username);
            if (user.user == null)
                return lista;
            if (user.user.Type == "ADMIN")
            {
                return Section.GetRootSections();
            }
            else 
            { 
            
            }
            SectionDAL sdal = new SectionDAL();
            SectionLabelDAL sldal = new SectionLabelDAL();
            foreach (SectionVO svo in sdal.GetByUser(username))
            {
                Section sec = new Section(svo);
                sec.SpanishTitle = sldal.GetById(sec.SectionId, 1).Content;
                lista.Add(sec);
            }
            
            return lista;
        }
        public static Section GetMainSection()
        {
            SectionDAL sdal = new SectionDAL();
            SectionVO svo = sdal.GetMain();

            return GetSectionToShow(svo);
        }
        public static Section GetSectionToShow(SectionVO svo)
        {
            PublicationDAL pdal = new PublicationDAL();
            SectionDAL sdal = new SectionDAL();
            SectionLabelDAL sldal = new SectionLabelDAL();
            SectionLabelVO sl = sldal.GetByFitLanguage(svo.Id, Language.GetCurrentLanguageId());
            Section s = new Section(svo, sl);
            s.Publications = new List<PublicationVO>(pdal.GetOrderedListBySection(s.SectionId, Language.GetCurrentLanguageId()));
            s.MainPublication = pdal.GetPrincipalBySection(svo.Id, Language.GetCurrentLanguageId());
            if (s.MainPublication == null&&s.Publications.Count==0)
            {
                s.MainPublication = new PublicationVO();
                s.MainPublication.Content = Publication.GenerateList(s.Publications,0);
            }
            s.NewsInclude = svo.NewsInclude;
            return s;
        }
        public static Section GetSectionToShow(int sectionId)
        {
            SectionDAL sdal = new SectionDAL();
            SectionVO svo = sdal.GetById(sectionId);
            return GetSectionToShow(svo);
        }
        public static Section GetSectionToShow(string permalink)
        {
            SectionDAL sdal = new SectionDAL();
            SectionVO svo = sdal.GetByPermalink(permalink);
            return GetSectionToShow(svo);
        }

        public static Section GetSectionByCurrentLanguage(int sectionId)
        {
            int currLang = Language.GetCurrentLanguageId();
            SectionLabelDAL sldal = new SectionLabelDAL();
            SectionLabelVO slvo = sldal.GetById(sectionId, currLang);
            Section s = new Section(slvo);
            return s;         
        }
        public static List<Section> GetSectionsByMenuKey(string menuKey)
        {

            SectionDAL sdal = new SectionDAL();
            SectionLabelDAL sldal = new SectionLabelDAL();
            List<Section> sections = new List<Section>();

            foreach (SectionVO svo in sdal.GetByMenu(menuKey))
            {
                Section s = new Section(svo, sldal.GetByFitLanguage(svo.Id, Language.GetCurrentLanguageId()));                
                sections.Add(s);
            }
            return sections; 
        }

        public static List<Section> GetSectionsByUser(string username)
        {
            List<Section> lista = new List<Section>();
            User user = User.GetUser(username);
            if (user.user == null)
                return lista;
            if (user.user.Type == "ADMIN")
                return Section.ListAll();
            SectionDAL sdal= new SectionDAL();
            SectionLabelDAL sldal= new SectionLabelDAL();
            foreach (SectionVO svo in sdal.GetByUser(username))
            {
                Section s = new Section(svo);
                lista.Add(s);
            }
            SectionTree st = new SectionTree();
            SectionTreeNode stn=st.getTree(lista);
            lista = stn.ToSectionList();
            foreach (Section sec in stn.ToSectionList())
            {
                sec.SpanishTitle=sldal.GetById(sec.SectionId, 1).Content;
            }
            return lista;
        }
        public static List<Section> GetSectionsByLogin(string login)
        {
            List<Section> lista = new List<Section>();
            SectionDAL sdal = new SectionDAL();
            SectionLabelDAL sldal = new SectionLabelDAL();
            foreach (SectionVO svo in sdal.GetByUser(login))
            {
                Section s = new Section(svo, sldal.GetById(svo.Id, 1), sldal.GetById(svo.Id, 2));
                lista.Add(s);
            }
            return lista;
        }
        public static string GetSectionTreeNodeByUser(string username, Section s)
        {
            List<Section> lista = new List<Section>();
            SectionTree st = new SectionTree();
            SectionTreeNode stn = new SectionTreeNode();
            User user = User.GetUser(username);
            if (user.user == null)
                return "";
            if (user.user.Type == "ADMIN")
            {
                stn.Childs = new List<SectionTreeNode>();
                stn.Childs.Add(new SectionTreeNode());

                if (!s.ParentSectionId.HasValue)
                {
                    stn.Childs[0] = st.getTree();
                    stn.Childs[0].Selected = true;
                }
                else
                    stn.Childs[0] = st.getTree(s.ParentSectionId.Value);
                stn.Childs[0].Node.Title = "Top";
                stn.Childs[0].Node.SectionId = 0;
                return stn.ToOptions(0);
            }
            /*SectionDAL sdal = new SectionDAL();
            SectionLabelDAL sldal = new SectionLabelDAL();
            foreach (SectionVO svo in sdal.GetByUser(username))
            {
                Section sec = new Section(svo,sldal.GetByFitLanguage(svo.Id, Language.GetCurrentLanguageId()));
                lista.Add(sec);
            }
            stn = st.getTree(lista,s.SectionId);*/
            Section parent = new Section();
            if (s.ParentSectionId.HasValue)
                parent = Section.GetById(s.ParentSectionId.Value);
            else
            {
                parent.SectionId = 0;
                parent.SpanishTitle = "Top";
            } 
            return "<option value='" + parent.SectionId + "' selected >" + parent.SpanishTitle + "</option>";
        }
        public static List<Section> GetSectionsByGroupId(int groupId)
        {

            SectionDAL sdal = new SectionDAL();
            SectionLabelDAL sldal = new SectionLabelDAL();
            List<Section> sections = new List<Section>();

            foreach (SectionVO svo in sdal.GetByGroup(groupId))
            {
                Section s = new Section(svo, sldal.GetByFitLanguage(svo.Id, Language.GetCurrentLanguageId()));
                sections.Add(s);
            }
            return sections;
        }
        public static List<Section> ListAll()
        {

            SectionDAL sdal = new SectionDAL();
            SectionLabelDAL sldal = new SectionLabelDAL();
            List<Section> sections = new List<Section>();

            foreach (SectionVO svo in sdal.GetAll())
            {
                Section s = new Section(svo, sldal.GetById(svo.Id, 1), sldal.GetById(svo.Id, 2));
                sections.Add(s);
            }
            return sections;
        }
        /*solo las secciones*/
        public static List<Section> GetSubsections(int parenSectionId)
        {
            SectionDAL sdal = new SectionDAL();
            IList<SectionVO> list = sdal.GetByParentSectionId(parenSectionId);
            if (list.Count == 0)
                return new List<Section>();
            int currLang = Language.GetCurrentLanguageId();
            List<Section> sections = new List<Section>();

            foreach (SectionVO svo in list)
            {
                SectionLabelDAL sldal = new SectionLabelDAL();
                Section s = new Section(svo, sldal.GetByFitLanguage(svo.Id, currLang));
                sections.Add(s);
            }
            return sections;
        }

        public List<Section> GetSiblings()
        {
            SectionDAL sdal = new SectionDAL();
            SectionLabelDAL sldal = new SectionLabelDAL();
            int currLang = Language.GetCurrentLanguageId();
            IList<SectionVO> sectionVOS;
            List<Section> sections = new List<Section>();
            if (ParentSectionId != null)
            {
                sectionVOS = sdal.GetByParentSectionId((int)ParentSectionId);
            } else
            {
                sectionVOS = sdal.GetRootSectionsVisible();
            }

            foreach(SectionVO section in sectionVOS)
            {
                sections.Add(new Section(section, sldal.GetByFitLanguage(section.Id, currLang)));
            }

            return sections;
        }

        public bool UpdateViews()
        {
            SectionDAL sdal = new SectionDAL();
            sdal.UpdateViews(this.SectionId, this.Visitas);
            return true;
        }

        public bool Save()
        {
            SectionDAL sdal = new SectionDAL();
            if (this.SectionId > 0)
            {
                SectionVO oldSection = sdal.GetMain();
                if (oldSection != null && oldSection.Id == this.SectionId && !this.IsMain)
                    this.IsMain = true;

                if(this.IsMain)
                {
                    PublicationDAL pdal = new PublicationDAL();
                    if(pdal.GetPrincipalBySection(this.SectionId, 1) == null)
                        this.IsMain = false;
                }
            }

            SectionVO svo = new SectionVO();
            svo.Created = this.Created;
            svo.IsMain = this.IsMain;
            if(this.SectionId>0)
                svo.Id = this.SectionId;
            svo.ParentSectionId = this.ParentSectionId == 0 ? null : this.ParentSectionId;
            svo.RedirectTo = this.RedirectTo;
            svo.RedirectOptions = this.RedirectOptions;
            svo.Updated = this.Updated;
            svo.Permalink = util.Encoder.RemoveSigns(this.Permalink.Trim());
            svo.Position = this.Position;
            svo.Visitas = this.Visitas;
            svo.Active = this.Active;
            svo.SitemapExclude = this.SitemapExclude;
            svo.CssClass = this.CssClass;

            svo = sdal.Update(svo);
            if (!svo.Permalink.StartsWith(svo.Id.ToString() + "_"))
            {
                svo.Permalink = svo.Id + "_" + svo.Permalink;
                sdal.Update(svo);
            }
            SectionLabelDAL sldal = new SectionLabelDAL();
            if (!String.IsNullOrEmpty(this.SpanishTitle))
            {
                SectionLabelVO slvo = new SectionLabelVO();
                slvo.SectionLabelId = new SectionLabelIdVO();
                slvo.SectionLabelId.SectionId = svo.Id;
                slvo.SectionLabelId.LanguageId = 1;
                slvo.Content = this.SpanishTitle;
                sldal.Update(slvo);
            }
            if (!String.IsNullOrEmpty(this.EnglishTitle))
            {
                SectionLabelVO slvo = new SectionLabelVO();
                slvo.SectionLabelId = new SectionLabelIdVO();
                slvo.SectionLabelId.SectionId = svo.Id;
                slvo.SectionLabelId.LanguageId = 2;
                slvo.Content = this.EnglishTitle;
                sldal.Update(slvo);
            }
            if (svo.IsMain)
                sdal.SetUniqueMainSection(svo.Id);
            this.SectionId = svo.Id;
            this.Permalink = svo.Permalink;
            
            return true;
        }
        public static int GetGlobalViews()
        {
            SectionDAL sdal = new SectionDAL();
            SectionVO svo= sdal.GetMain();
            if (svo != null)
                return svo.Visitas;
            return 0;
        }

        public string GetLink()
        {
            try
            {
                if (!string.IsNullOrEmpty(RedirectTo))
                    return new UriBuilder(RedirectTo.Trim()).Uri.AbsoluteUri;
                else
                    return "../Section/" + Permalink;
            }
            catch (UriFormatException ex)
            {
                return RedirectTo;
            }
        }

        public static bool Delete(int sectionId)
        {
            SectionDAL sdal= new SectionDAL();
            if (sdal.Inactive(sectionId) != 1)
                return false;
            string inactives = ""+sectionId;
            int changed = inactives.Length;
            int ninactives = 0;
            while (changed > ninactives)
            {
                sdal.InactiveChilds(inactives);
                inactives = sdal.GetJoinedInactive();
                ninactives = changed;
                changed = inactives.Length;
            }
            PublicationDAL pdal = new PublicationDAL();
            pdal.Inactive(inactives);
            return true;
        }

        public Section Clone()
        {
            Section c = new Section();
            c.Active = Active;
            c.Created = Created;
            c.CssClass = CssClass;
            c.EnglishTitle = EnglishTitle;
            c.IsMain = IsMain;
            c.ParentSectionId = ParentSectionId;
            c.Permalink = Permalink;
            c.Position = Position;
            c.RedirectOptions = RedirectOptions;
            c.RedirectTo = RedirectTo;
            c.SitemapExclude = SitemapExclude;
            c.SpanishTitle = SpanishTitle;
            c.Title = Title;
            c.Updated = Updated;
            return c;
        }

        #region Special Sections
        internal static Section GetSpecialByType(string type)
        {
            SectionDAL sDAL = new SectionDAL();
            SectionLabelDAL slDAL = new SectionLabelDAL();
            IList<SectionVO> sectionsVO = sDAL.GetByType(type);

            if (sectionsVO.Count > 0)
                return new Section(sectionsVO[0], slDAL.GetByFitLanguage(sectionsVO[0].Id, Language.GetCurrentLanguageId()));
            else
                return new Section(); //TODO: Check if ok
        }

        internal static List<Section> GetSpecialSections()
        {
            SectionDAL sDAL = new SectionDAL();
            PublicationDAL pDAL = new PublicationDAL();
            SectionLabelDAL slDAL = new SectionLabelDAL();

            List<Section> specials = new List<Section>();

            IList<SectionVO> specialsVO = sDAL.GetSpecials();

            foreach (SectionVO svo in specialsVO)
            {
                Section s = new Section(svo, slDAL.GetByFitLanguage(svo.Id, Language.GetCurrentLanguageId()));

                s.Publications = new List<PublicationVO>(pDAL.GetListBySection(s.SectionId, Language.GetCurrentLanguageId(), false));

                specials.Add(s);
            }

            return specials;
        }
        #endregion
    }
}
