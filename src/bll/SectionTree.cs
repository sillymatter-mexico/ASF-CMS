using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace asf.cms.bll
{
    public class SectionTree
    {
        public SectionTreeNode tree = new SectionTreeNode();
        public SectionTree()
        { 

        }
        public String GetLeftMenu(Section s)
        {
            return "<ul class='leftMenuOpen'>" + (getTree(s, "")).ToHtml() + "</ul>";
        }
        public String GetLeftMenu(Section s, int selectedPublication)
        {
            return "<ul class='leftMenuOpen'>" + (getTree(s, "")).ToHtml(selectedPublication) + "</ul>";
        }
        public String GetLeftMenu(Section s, List<Section> left)
        {
            string html = "";
            foreach(Section sec in left)
            {
                html += "<div class='title'><a class='title' href='" + sec.GetLink() + "'>" + sec.Title + "</a></div>";
                if (s.ParentSectionId != null)
                {
                    if (s.ParentSectionId == sec.SectionId)
                        html += "<ul class='leftMenuOpen'>" + (getTree(sec, "")).ToHtml() + "</ul>";
                    else
                        html += "<ul class='leftMenuClosed'>" + (getTree(sec, "")).ToHtml() + "</ul>";
                }
                else
                {
                    if (s.Title.Equals(sec.Title))
                        html += "<ul class='leftMenuOpen'>" + (getTree(sec, "")).ToHtml() + "</ul>";
                    else
                        html += "<ul class='leftMenuClosed'>" + (getTree(sec, "")).ToHtml() + "</ul>";
                }
            }
            return html;
        }
        public String GetLeftMenu(Section s, int selectedPublication, List<Section> left)
        {
            string html = "";
            foreach (Section sec in left)
            {
                html += "<div class='title'><a class='title' href='" + sec.GetLink() + "'>" + sec.Title + "</a></div>";

                if (s.ParentSectionId != null)
                {
                    if (s.ParentSectionId == sec.SectionId)
                        html += "<ul class='leftMenuOpen'>" + (getTree(sec, "")).ToHtml(selectedPublication) + "</ul>";
                    else
                        html += "<ul class='leftMenuClosed'>" + (getTree(sec, "")).ToHtml(selectedPublication) + "</ul>";
                }
                else
                {
                    if (s.Title.Equals(sec.Title))
                        html += "<ul class='leftMenuOpen'>" + (getTree(sec, "")).ToHtml(selectedPublication) + "</ul>";
                    else
                        html += "<ul class='leftMenuClosed'>" + (getTree(sec, "")).ToHtml(selectedPublication) + "</ul>";
                }
            }
            return html;
        }

        public SectionTreeNode getTree(Section s)
        {
            int selected = s.SectionId;
            SectionTreeNode stn= new SectionTreeNode();

            return getTree(stn.GetRootNode(s).SectionId, selected);
        }
        public SectionTreeNode getTree(Section s, string x)
        {
            SectionTreeNode stn = new SectionTreeNode();
            stn.Node = Section.GetSectionToShow(s.SectionId);
            stn.Parent = stn.GetUpperTree();
            stn.Actual = true;
            foreach (Section sub in Section.GetSubsections(s.SectionId))
                stn.AddChild(sub);
            tree = stn.GetRootTree();
            return tree;
        }
        public SectionTreeNode getTree(int rootSectionId, int selectedSectionId)
        {
            Section s = Section.GetSectionByCurrentLanguage(rootSectionId);
            SectionTreeNode stn = new SectionTreeNode();
            stn.Node = s;
            stn.Parent = null;

            foreach (Section sub in Section.GetSubsections(s.SectionId))
            {
                stn.AddChild(sub);
            }

            SectionTreeNode found = stn.Find(selectedSectionId);
            if (found != null)
            {
                found.Selected = true;
                found.Node = Section.GetSectionToShow(found.Node.SectionId);
            }
            return stn;
        }
        public SectionTreeNode getTree(Section rootSection, int selectedSectionId)
        {
            SectionTreeNode stn = new SectionTreeNode();
           
            stn.AddChild(rootSection);
            SectionTreeNode found = stn.Find(selectedSectionId);
            if (found != null)
                found.Selected = true;

            return stn;
        }
        public SectionTreeNode getTree()
        {
            SectionTreeNode stn = new SectionTreeNode();
            stn.Node = new Section();

            foreach (Section s in Section.GetRootSections())
            {
                stn.AddChild(s);
            }

            return stn;
        }
        public SectionTreeNode getTree(int selectedSectionId)
        {
            SectionTreeNode stn = getTree();
            SectionTreeNode found = stn.Find(selectedSectionId);
            if (found != null)
                found.Selected = true; 
            return stn;
        }
        public SectionTreeNode getTree(List<Section> lista)
        {
            SectionTreeNode stn = new SectionTreeNode();
            stn.Node = new Section();

            foreach (Section s in lista)
            {
                stn.AddChild(s);
            }

            return stn;
        }
        public SectionTreeNode getTree(List<Section> lista, int selectedSectionId)
        {
            SectionTreeNode stn = new SectionTreeNode();
            stn.Node = new Section();

            foreach (Section s in lista)
            {
                stn.AddChild(s);
            }

            SectionTreeNode found = stn.Find(selectedSectionId);
            if (found != null)
                found.Selected = true;
            return stn;
        }
    }
}
