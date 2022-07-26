using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.model;
using asf.cms.bll;
using System.Text;
namespace asf.cms.util
{
    public class TreeItem:IComparable
    {
        public virtual int Id { get; set; }
        public virtual string Link { get; set; }
        public virtual bool IsMain { get; set; }
        public virtual string Title { get; set; }
        public virtual int Position { get; set; }
        public virtual bool Actual { get; set; }
        public virtual SectionTreeNode Node { get; set; }
        public virtual bool Expandible { get; set; }

        public static List<TreeItem> GetList(List<PublicationVO> publications, int selectedPublication)
        {
            List<TreeItem> list = new List<TreeItem>();
            foreach (PublicationVO pvo in publications)
            {
                if (pvo.Position == 666)
                    continue;
                TreeItem ti = new TreeItem();
                ti.Id = pvo.Id;
                ti.IsMain = pvo.IsMain;
                ti.Link = "../Publication/" + pvo.Permalink ;
                ti.Position = pvo.Position;
                ti.Title = pvo.Title;
                ti.Actual = (selectedPublication == pvo.Id);
                ti.Expandible = false;
                ti.Node = null;
                list.Add(ti);
 
            }
            return list;
        }
        public static TreeItem Get(Section s)
        {
            TreeItem ti = new TreeItem();
            ti.Id = s.SectionId;
            ti.IsMain = false;
            ti.Link = s.GetLink();
            ti.Position = s.Position;
            ti.Title = s.Title;
            ti.Actual = false;
            ti.Expandible = false;
            ti.Node = null;
            return ti;

        }
        public String ToHtml()
        {
            if (IsMain)
                return "";
            StringBuilder sb= new StringBuilder(); 
            sb.Append("<li "+(Actual? "class='selected'" : "style='list-style-type: none;'") +">");
            sb.Append("<a href='"+Link+ "'>" + Title + "</a></li>\n");
            return sb.ToString();
        }
        #region IComparable Members

        int IComparable.CompareTo(object obj)
        {
            TreeItem segundo=((TreeItem)obj);
            int res= this.Position.CompareTo(segundo.Position);
            if (res == 0)
                res= this.Id.CompareTo(segundo.Id);
            return res;
        }

        #endregion
    }
}
