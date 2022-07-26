using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.dal;
using asf.cms.model;
using asf.cms.bll;
using asf.cms.widgets.ui;
namespace asf.cms.controller
{
    public class EspecialesController:Controller
    {
         public EspecialesController(HttpContext context):base(context)
        { 
        }
         public void Show()
         {
            List<Section> superior = Section.GetSectionsByMenuKey("SUPERIOR");
            List<Section> inferior = Section.GetSectionsByMenuKey("INFERIOR");
            List<Section> principal = Section.GetSectionsByMenuKey("PRINCIPAL");
            this.Items.Add("menuPrincipal", principal);
            this.Items.Add("menuSuperior", superior);
            this.Items.Add("menuInferior", inferior);
            String permalink = Request["permalink"];
            /*.publication = Especiales.GetByPermalink(permalink);
            p.publication.Visitas += 1;
            p.UpdateViews();

            Section selected = Section.GetSectionToShow(p.publication.SectionId);
            if (p.publication.HasWidgets)
                p.publication.Content = BuildContent(p.publication.Content, selected.SectionId, p.publication.Id);
            if (!String.IsNullOrEmpty(p.publication.Meta))
                p.metaList= MetaItem.ListFromJson(p.publication.Meta);

            SectionTree st = new SectionTree();
            this.Items.Add("leftMenu", st.GetLeftMenu(selected,p.publication.Id));
            this.Items.Add("subSectionTitle", st.tree.Node.Title);
            this.Items.Add("mensaje", p.publication.Content);
            this.Items.Add("meta", p.metaList);
            this.Items.Add("views", p.publication.Visitas.ToString().PadLeft(7, '0'));
            ShowPage("Default.aspx");*/
         }
    }
}
