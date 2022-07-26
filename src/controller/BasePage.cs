using System;
using System.Web;
using System.Collections.Generic;
using asf.cms.model;
using asf.cms.bll;

namespace asf.cms.controller
{
    public class BasePage: System.Web.UI.Page
    {
        private static int contentLimit = 200;

        protected String GetMessage(String var)
        {
            if (HttpContext.Current.Items[var] == null)
                return "";
            string prueba = HttpContext.Current.Items[var].ToString();
            return prueba;
        }
        protected Object GetRequestVar(String var)
        {
            if (HttpContext.Current.Items[var] == null)
                return null;
            return HttpContext.Current.Items[var];
        }
        protected ContentElement GetPublicationContent(int id)
        {
            return GetPublicationContent(id, true);
        }
        protected ContentElement GetPublicationContent(int id, bool complete)
        {
            SiteContent con = new SiteContent();
            PublicationVO pub = Publication.GetById(id);
            ContentElement result = new ContentElement(pub);
            if (!complete)
                result.Content = con.GetResumeWithFormat(result.Content);
            return result;
        }
        protected List<ContentElement> ContentElementFromPublications(List<PublicationVO> publications)
        {
            return ContentElementFromPublications(publications, int.MaxValue);
        }
        protected List<ContentElement> ContentElementFromPublications(List<PublicationVO> publications, int limit = int.MaxValue)
        {
            return ContentElementFromPublications(publications, true, limit);
        }
        protected List<ContentElement> ContentElementFromPublications(List<PublicationVO> publications, bool complete = true, int limit = int.MaxValue)
        {
            List<ContentElement> publicationsCE = new List<ContentElement>();
            SiteContent sc = new SiteContent();
            if (publications != null)
            {
                for(int idx = 0; idx < publications.Count && idx < limit; idx++)
                {
                    PublicationVO p = publications[idx];
                    if (!p.IsMain)
                    {
                        ContentElement pCE = new ContentElement(p);
                        if (!complete)
                            pCE.Content = sc.GetResumeWithFormat(pCE.Content);
                        publicationsCE.Add(pCE);
                    }

                }
            }
            return publicationsCE;
        }
        protected List<ContentElement> ContentElementFromMenu(List<Section> menu)
        {
            return ContentElementFromMenu(menu, true, -1);
        }
        protected List<ContentElement> ContentElementFromMenu(List<Section> menu, bool complete)
        {
            return ContentElementFromMenu(menu, complete, -1);
        }
        protected List<ContentElement> ContentElementFromMenu(List<Section> menu, bool complete, int? selectedId = null)
        {
            List<ContentElement> menuCE = new List<ContentElement>();
            SiteContent con = new SiteContent();
            foreach(Section section in menu)
            {
                ContentElement elem = new ContentElement(section);
                if (!complete)
                    elem.Content = con.GetResumeWithFormat(elem.Content);
                elem.Selected = selectedId == section.SectionId ? true : false;
                menuCE.Add(elem);
            }
            return menuCE;
        }
        protected List<ContentElement> ContentElementFromMenu(Menu menu)
        {
            return ContentElementFromMenu(menu, true, null);
        }
        protected List<ContentElement> ContentElementFromMenu(Menu menu, bool complete = true)
        {
            return ContentElementFromMenu(menu, complete, null);
        }
        protected List<ContentElement> ContentElementFromMenu(Menu menu, bool complete, int? selectedId = null)
        {
            List<ContentElement> menuCE = new List<ContentElement>();
            SiteContent con = new SiteContent();
            foreach (MenuItem mi in menu.Items)
            {
                ContentElement elem = new ContentElement(mi);
                if (!complete)
                    elem.Content = con.GetResumeWithFormat(elem.Content);
                elem.Selected = selectedId == mi.Section.SectionId ? true : false;
                menuCE.Add(elem);
            }
            return menuCE;
        }
        protected List<ContentElement> ContentElementSiblingMenu(bool appendPublications)
        {
            int? sectionId = GetRequestVar("section_id") as int?;
            if (sectionId == null || sectionId <= 1)
                return new List<ContentElement>();

            List <Section> siblings = GetRequestVar("menu_siblings") as List<Section>;
            if (siblings == null)
                siblings = new List<Section>();

            List<ContentElement> result = ContentElementFromMenu(siblings, true, sectionId);
            if(appendPublications)
            {
                List<PublicationVO> publications = GetRequestVar("menu_siblings_publications") as List<PublicationVO>;
                if (publications == null)
                    publications = new List<PublicationVO>();
                result.AddRange(ContentElementFromPublications(publications));
            }

            return result;
        }
        protected List<ContentElement> GetCurrentSectionContent(bool withPublications)
        {
            List<ContentElement> result = new List<ContentElement>();
            int? sectionId = GetRequestVar("section_id") as int?;
            if (sectionId == null || sectionId <= 1)
                return result;

            List<Section> subsections = GetRequestVar("subsections") as List<Section>;
            if (subsections != null)
            {
                foreach (Section s in subsections)
                {
                    if (s.MainPublication != null)
                    {
                        ContentElement e = new ContentElement(s);
                        //e.Content = BuildContent(e.Content, s.SectionId, s.MainPublication.Id);
                        result.Add(e);
                    }
                }
            }

            if(withPublications)
            {
                List<PublicationVO> publications = GetRequestVar("publications") as List<PublicationVO>;
                if (publications != null)
                {
                    foreach (PublicationVO p in publications)
                    {
                        ContentElement e = new ContentElement(p);
                        //e.Content = BuildContent(p.Content, current.SectionId, p.Id);
                        result.Add(e);
                    }
                }
            }

            return result;
        }
    }
}
