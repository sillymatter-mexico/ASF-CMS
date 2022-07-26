using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.bll;
using asf.cms.dal;
using asf.cms.model;
using asf.cms.widgets;
using asf.cms.widgets.ui;
using log4net;

namespace asf.cms.controller
{
    public class SectionController:Controller
    {
        private ILog _log = LogManager.GetLogger("SectionController");

        public SectionController(HttpContext context):base(context)
        { 
        }
        public void Show()
        {
            _log.Debug("Show Start");
            Section selected = new Section();
            String permalink = Request["permalink"];
            selected = Section.GetSectionToShow(permalink);
            selected.Visitas += 1;
            selected.UpdateViews();

            PublicationVO current = selected.MainPublication != null ? selected.MainPublication : selected.Publications.Count > 0 ? selected.Publications[0] : null;

            if (current != null && current.Id > 0)
            {
                current.Visitas += 1;
                Publication p = new Publication();
                p.publication = current;
                p.UpdateViews();

                /*if (current.HasWidgets)
                    current.Content = BuildContent(current.Content, selected.SectionId, current.Id);*/
            }

            _log.Debug(String.Format("Basic content identified, start building complex content for section {0}[{1}] with main publication {2}", selected.Title, selected.SectionId, current != null ? current.Title : "--NO--"));


            BuildMeta(current);
            BuildSpecialPublications();
            BuildSiblingsMenu(selected);
            BuildMenus(selected);
            BuildSectionContent(selected);
            BuildSectionContentTree(selected);

            _log.Debug("Complex content built.");

            this.Items.Add("views", selected.Visitas.ToString().PadLeft(7,'0'));
            this.Items.Add("section_id", selected.SectionId);
            this.Items.Add("is_root_section", selected.ParentSectionId == null || selected.ParentSectionId <= 0);

            BuildMenuOptions();

            _log.Debug("Show End");
            ShowPage("SectionView.aspx");
        }

        protected void BuildSectionContent(Section current)
        {
            // Subsections
            if(current.SubSections != null)
            {
                foreach (Section s in current.SubSections)
                {
                    if(s.MainPublication != null)
                        s.MainPublication.Content = BuildContent(s.MainPublication.Content, s.SectionId, s.MainPublication.Id);
                }
            }
            Items.Add("subsections", current.SubSections);

            // Publications
            if(current.Publications != null)
            {
                foreach(PublicationVO p in current.Publications)
                {
                    p.Content = BuildContent(p.Content, current.SectionId, p.Id);
                }
            }
            Items.Add("publications", current.Publications);
            
        }

        protected void BuildSectionContentTree(Section currentSection)
        {
            List<ContentElement> content = new List<ContentElement>();
            bool isSpecial = currentSection.Type != null;

            int maxDepth = 0;
            int.TryParse(System.Configuration.ConfigurationManager.AppSettings.Get("SectionContentDepth"), out maxDepth);
            if (maxDepth == -1)
            {
                Items.Add("sectionContent", content);
                return;
            }
            if (maxDepth < -1)
                maxDepth = int.MaxValue;

            SectionTree st = new SectionTree();
            SectionTreeNode root = st.getTree(currentSection.SectionId, currentSection.SectionId);

            List<SectionTreeNode> stack = new List<SectionTreeNode>();
            stack.Add(root);

            while(stack.Count > 0)
            {
                SectionTreeNode current = stack[0];
                stack.RemoveAt(0);

                if (current.Childs != null && current.Childs.Count > 0 && current.RelativeDepth(root) < maxDepth)
                {
                    stack.InsertRange(0, current.Childs);
                }

                foreach(PublicationVO p in current.Node.Publications)
                {
                    p.Content = BuildContent(p.Content, p.SectionId, p.Id);
                    ContentElement ce = new ContentElement(p);
                    if(isSpecial)
                    {
                        string tmp = ce.ContentNews;
                        ce.ContentNews = ce.Content;
                        ce.Content = tmp;
                    }
                    content.Add(ce);
                }
            }

            Items.Add("sectionContent", content);
            Items.Add("sectionIsSpecial", isSpecial);
        }
    }
}
