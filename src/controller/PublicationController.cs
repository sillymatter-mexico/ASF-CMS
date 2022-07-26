﻿using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.dal;
using asf.cms.model;
using asf.cms.bll;
using asf.cms.widgets.ui;
namespace asf.cms.controller
{
    public class PublicationController:Controller
    {
        public PublicationController(HttpContext context):base(context)
        { 
        }
        public void Show()
        {
            Publication p = new Publication();
            String permalink = Request["permalink"];
            p.publication = Publication.GetByPermalink(permalink);
            p.publication.Visitas += 1;
            p.UpdateViews();

            Section selected = Section.GetSectionToShow(p.publication.SectionId);

            if (String.IsNullOrEmpty(p.publication.AutogeneratedType))
            {
                Response.Redirect("/Section/" + selected.Permalink + "#" + permalink, false);
            }
            else
            {
                BuildMeta(p.publication);
                BuildSpecialPublications();
                BuildSiblingsMenu(selected);
                BuildMenus(selected);

                //Build Publication Content
                if (p.publication.HasWidgets)
                    p.publication.Content = BuildContent(p.publication.Content, selected.SectionId, p.publication.Id);
                this.Items.Add("publicationContent", new ContentElement(p.publication));
                this.Items.Add("publicationIsSpecial", String.IsNullOrEmpty(p.publication.AutogeneratedType));
                this.Items.Add("sectionIsSpecial", selected.Type != null);
                this.Items.Add("section_id", selected.SectionId);

                this.Items.Add("views", p.publication.Visitas.ToString().PadLeft(7, '0'));
                this.Items.Add("is_root_section", selected.ParentSectionId == null || selected.ParentSectionId <= 0);

                BuildMenuOptions();

                ShowPage("PublicationView.aspx");
            }
        }
    }
}