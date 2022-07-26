using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using asf.cms.dal;
using asf.cms.model;
using asf.cms.util;

namespace asf.cms.bll
{
    public class SectionTreeNode
    {
        public Section Node { set; get; }
        public SectionTreeNode Parent { set; get; }
        public List<SectionTreeNode> Childs { set; get; }
        public List<int> Control { get; set; }
        public bool Selected { set; get; }
        public bool Actual { set; get; }

        public SectionTreeNode()
        {
            Childs = new List<SectionTreeNode>();
            Actual = false;
        }

        public void AddChild(Section s)
        {
            SectionTreeNode stn = new SectionTreeNode();
            stn.Parent = this;
            stn.Node = s;
            stn.Selected = false;
            stn.Actual = false;

            foreach (Section subsection in Section.GetSubsections(s.SectionId))
                stn.AddChild(subsection);

            this.Childs.Add(stn);
        }
        public List<int> AddChild(Section s, List<int> control)
        {
            SectionTreeNode stn = new SectionTreeNode();
            stn.Parent = this;
            stn.Node = s;
            stn.Selected = false;
            stn.Actual = false;
            control.Add(s.SectionId);

            foreach (Section subsection in Section.GetSubsections(s.SectionId))
            {
                if (!control.Contains(subsection.SectionId))
                    control = stn.AddChild(subsection, control);
            }

            this.Childs.Add(stn);
            return control;
        }
        public void AddFullChild(Section s)
        {
            SectionTreeNode stn = new SectionTreeNode();
            stn.Parent = this;
            stn.Node = s;
            stn.Node.Publications = Publication.GetWithoutContentBySectionId(stn.Node.SectionId);
            stn.Selected = false;
            stn.Actual = false;
            foreach (Section subsection in Section.GetSubsections(s.SectionId))
                stn.AddFullChild(subsection);
            this.Childs.Add(stn);
        }
        public Section GetRootNode(Section s)
        {
          
            if (!s.ParentSectionId.HasValue)
            {
                return s;
            }
            return GetRootNode(Section.GetById(s.ParentSectionId.Value));
        }
        public SectionTreeNode GetRootTree()
        {
            if (this.Parent != null)
                return this.Parent.GetRootTree();
            return this;
        }
        public SectionTreeNode Find(int sectionId)
        {
            if (this.Node.SectionId == sectionId)
                return this;
            if (this.Childs.Count==0)
                return null;
            foreach (SectionTreeNode child in Childs)
            {
                SectionTreeNode found = child.Find(sectionId);
                if (found != null)
                    return found;
            }
            return null;            
        }
        public string ToHtml(int selectedPublication)
        {
            List<asf.cms.util.TreeItem> items = new List<asf.cms.util.TreeItem>();
            StringBuilder sb = new StringBuilder();
            if (this.Selected)
            {
                items.AddRange(asf.cms.util.TreeItem.GetList(this.Node.Publications,selectedPublication));
            }

            foreach (SectionTreeNode child in Childs)
            {
                TreeItem item=TreeItem.Get(child.Node);
                if(child.Actual&&selectedPublication==0)
                    item.Actual=true;
                if (child.Selected)
                {
                    item.Expandible = true;
                    item.Node = child;
                }
                items.Add(item);
            }
            items.Sort();
            foreach (TreeItem ti in items)
            {
                string html = ti.ToHtml();
                if (html == "")
                    continue;
                sb.Append(html);
                if (ti.Expandible)
                {
                    sb.Append("<ul>\n");
                    sb.Append(ti.Node.ToHtml(selectedPublication));
                    sb.Append("</ul>\n");
                }
                if (this.Parent == null)
                    sb.Append("<hr/>\n");
            }

            return sb.ToString();
        }

        public string ToJSON(bool full, int selectedSectionId = -1, int selectedPublicationId = -1)
        {
            string json = "{";

            json += "\"id\" : " + Node.SectionId.ToString() + ",";
            json += "\"isMain\" : " + Node.IsMain.ToString().ToLower() + ",";
            json += "\"type\" : \"section\",";
            json += "\"permalink\" : \"" + Node.Permalink + "\","; 
            json += "\"title\" : \"" + Node.Title + "\",";
            json += "\"selected\" : " + (selectedSectionId == Node.SectionId ? "true" : "false") + ",";
            json += "\"children\" : [";

            int currIndex = 0;
            foreach(SectionTreeNode childSection in Childs)
            {
                json += childSection.ToJSON(full) + (currIndex == Childs.Count - 1 ? "" : ",");
                currIndex++;
            }

            if(full)
            {
                json += Childs.Count > 0 && Node.Publications.Count > 0 ? "," : "";
                currIndex = 0;
                foreach(PublicationVO childPublication in Node.Publications)
                {
                    json += "{";
                    json += "\"id\" : " + childPublication.Id.ToString() + ",";
                    json += "\"isMain\" : " + childPublication.IsMain.ToString().ToLower() + ",";
                    json += "\"type\" : \"publication\",";
                    json += "\"permalink\" : \"" + childPublication.Permalink + "\",";
                    json += "\"title\" : \"" + childPublication.Title + "\",";
                    json += "\"selected\" : " + (childPublication.Id == selectedPublicationId ? "true" : "false");
                    json += "}" + (currIndex == (Node.Publications.Count - 1) ? "" : ",");
                    currIndex++;
                }
            }

            json += "]";
            return json + "}";
        }
        public string ToPositionedOptions(int level)
        {

            /*string espacio = "";
            for (int i = 1; i < level; i++)
                espacio += "&nbsp;&nbsp;&nbsp;";

            List<asf.cms.util.TreeItem> items = new List<asf.cms.util.TreeItem>();
            StringBuilder sb = new StringBuilder();
            items.AddRange(TreeItem.GetList(this.Node.Publications,0));

            foreach (SectionTreeNode child in Childs)
            {
                TreeItem item=TreeItem.Get(child.Node);
                items.Add(item);
            }
            items.Sort();

            foreach (TreeItem ti in items)
            {
                string option = ti.ToJSON();
                sb.Append(option);
                sb.Append(ti.Node.ToHtml(selectedPublication));
                }
                if (this.Parent == null)
                    sb.Append("<hr/>\n");
            }

            return sb.ToString();

            if (Childs.Count == 0)
                return "";
            
            StringBuilder sb=new StringBuilder();
            foreach (SectionTreeNode child in Childs)
            { 
                sb.Append("<option value='"+child.Node.SectionId+"' ");
                if (child.Selected)
                    sb.Append("selected");
                sb.Append(">"+espacio + "- " + child.Node.Title + "</option>");
                sb.Append(child.ToOptions(level+1));
            }
            return sb.ToString();*/
            return "";
        }
        public string ToHtmlFullyExpandedWithPosts()
        {
            return ToHtmlFullyExpandedWithPosts(0);
        }
        public string ToHtmlFullyExpandedWithPosts(int level, bool useExclude = false)
        {
            if (useExclude && this.Node.SitemapExclude)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(this.Node.Title))
            {
                sb.Append("<li " + ((this.Selected) ? "class='selected'" : "") + ">");
                for (int i = 0; i < level; i++)
                {
                    sb.Append("&nbsp;");
                    sb.Append("&nbsp;");
                }
                sb.Append("<a href='" + this.Node.GetLink());
                sb.Append("'>" + this.Node.Title + "</a>");
                sb.Append("</li>\n");

                //If this section has publications (posts), add them!
                Publication pbs = new Publication();
                List<PublicationVO> posts=pbs.GetBySectionId(this.Node.SectionId, "Position,Updated");
                foreach (PublicationVO post in posts)
                {
                    if (post.IsMain || (useExclude && post.SitemapExclude))
                        continue;
                    sb.Append("<li>");
                    for (int i = 0; i < level+1; i++)
                    {
                        sb.Append("&nbsp;");
                        sb.Append("&nbsp;");
                    }
                    sb.Append("-");
                    sb.Append("<a href='" + Publication.GetLink(post));
                    sb.Append("'>" + post.Title + "</a>");
                    sb.Append("</li>\n");
                }
                
            }

            if (Childs.Count == 0)
                return sb.ToString();

            foreach (SectionTreeNode child in Childs)
            {
                if(useExclude && child.Node.SitemapExclude)
                {
                    continue;
                }

                sb.Append("<li " + ((child.Selected) ? "class='selected'" : "") + ">");
                if (this.Parent != null)
                {
                    for (int i = 0; i < level+1; i++)
                    {
                        sb.Append("&nbsp;");
                        sb.Append("&nbsp;");
                    }
                }
                sb.Append("<a href='" + child.Node.GetLink());
                sb.Append("'>" + child.Node.Title + "</a>");
                //If this section has publications (posts), add them!
                Publication pbs = new Publication();
                List<PublicationVO> posts = pbs.GetBySectionId(child.Node.SectionId, "Position,Updated");
                foreach (PublicationVO post in posts)
                {
                    if (post.IsMain || (useExclude && post.SitemapExclude))
                        continue;
                    sb.Append("<li>");
                    for (int i = 0; i < level+2; i++)
                    {
                        sb.Append("&nbsp;");
                        sb.Append("&nbsp;");
                    }
                    sb.Append("-");
                    sb.Append("<a href='" + Publication.GetLink(post));
                    sb.Append("'>" + post.Title + "</a>");
                    sb.Append("</li>\n");
                }

                //Recursively fill with their childs
                foreach (SectionTreeNode grandchild in child.Childs)
                {
                    sb.Append(grandchild.ToHtmlFullyExpandedWithPosts(level+3, useExclude));
                }
                sb.Append("</li>\n");
                if (this.Parent == null)
                    sb.Append("<hr/>\n");
            }
            return sb.ToString();
        }
        public string ToHtmlFullyExpanded()
        {
            return ToHtmlFullyExpanded(0);
        }
        public string ToHtmlFullyExpanded(int level, bool useExclude = false)
        {
            if (this.Node.SitemapExclude && useExclude)
            {
                return "";
            }

            StringBuilder sbIndent = new StringBuilder();
            for (int i = 0; i < level; i++)
            {
                sbIndent.Append("&nbsp;");
            }

            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(this.Node.Title))
            {
                sb.Append("<li " + ((this.Selected) ? "class='selected'" : "") + ">");
                sb.Append("&nbsp;");
                sb.Append("<a href='" + this.Node.GetLink());
                sb.Append("'>" + this.Node.Title + "</a>");
                sb.Append("</li>\n");
            }

            if (Childs.Count == 0)
                return sb.ToString();

            foreach (SectionTreeNode child in Childs)
            {
                if(useExclude && child.Node.SitemapExclude)
                {
                    continue;
                }

                sb.Append("<li " + ((child.Selected) ? "class='selected'" : "") + ">");
                if (this.Parent != null)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        sb.Append("&nbsp;");
                    }
                }
                sb.Append("<a href='" + child.Node.GetLink());
                sb.Append("'>" + child.Node.Title + "</a>");
                //Recursively fill with their childs
                foreach (SectionTreeNode grandchild in child.Childs)
                {
                    sb.Append(grandchild.ToHtmlFullyExpanded(level++, useExclude));
                }
                sb.Append("</li>\n");
                if (this.Parent == null)
                    sb.Append("<hr/>\n");
            }
            return sb.ToString();
        }
        public string ToHtml()
        {
            return ToHtml(0);
        }
        public string ToOptions(int level)
        {
            string espacio = "";
            for (int i = 1; i < level; i++)
                espacio += "&nbsp;&nbsp;&nbsp;";

            if (Childs.Count == 0)
                return "";
            
            StringBuilder sb=new StringBuilder();
            foreach (SectionTreeNode child in Childs)
            { 
                sb.Append("<option value='"+child.Node.SectionId+"' title='"+child.Node.Title+"' ");
                if (child.Selected)
                    sb.Append("selected");
                sb.Append(">"+espacio + "- " + child.Node.Title + "</option>");
                sb.Append(child.ToOptions(level+1));
            }
            return sb.ToString();
        }
        public List<Section> ToSectionList()
        {
            List<Section> lista = new List<Section>();
            foreach (SectionTreeNode child in Childs)
            {
                if (!lista.Contains(child.Node))
                {
                    lista.Add(child.Node);
                    lista.AddRange(child.ToSectionList());
                }
            }
            return lista;
        }

        public SectionTreeNode GetUpperTree()
        {
            this.Selected = true;
            SectionTreeNode stn = new SectionTreeNode();
            if (!this.Node.ParentSectionId.HasValue)
                return null;

            stn.Node = Section.GetSectionToShow(this.Node.ParentSectionId.Value);
            stn.Selected = true;
            foreach (Section subsection in Section.GetSubsections(Node.ParentSectionId.Value))
            {
                SectionTreeNode stnchild = new SectionTreeNode();

                if (subsection.SectionId == Node.SectionId)
                    stnchild = this;
                else
                    stnchild.Node = subsection;
                stnchild.Parent = stn;
                stn.Childs.Add(stnchild);
            }
            stn.Parent = stn.GetUpperTree();
            return stn;
        }

        public SectionTreeNode GetUpperTree(List<int> control)
        {
            this.Selected = true;
            SectionTreeNode stn = new SectionTreeNode();
            if (!this.Node.ParentSectionId.HasValue)
               return null;
            
            stn.Node = Section.GetSectionToShow(this.Node.ParentSectionId.Value);
            stn.Selected = true;
            control.Add(stn.Node.SectionId);

            foreach (Section subsection in Section.GetSubsections(Node.ParentSectionId.Value))
            {
                if (!control.Contains(subsection.SectionId))
                {
                    SectionTreeNode stnchild = new SectionTreeNode();

                    if (subsection.SectionId == Node.SectionId)
                        stnchild = this;
                    else
                        stnchild.Node = subsection;

                    stnchild.Parent = stn;
                    stn.Childs.Add(stnchild);
                    control.Add(stnchild.Node.SectionId);
                }
            }

            stn.Control = control;
            stn.Parent = stn.GetUpperTree(stn.Control);
            return stn;
        }
        /**/

        public int RelativeDepth(SectionTreeNode root)
        {
            int d = 0;
            SectionTreeNode current = this;

            while(current.Parent != null && root.Node.SectionId != current.Node.SectionId)
            {
                d++;
                current = current.Parent;
            }

            return d;
        }
    }
}
