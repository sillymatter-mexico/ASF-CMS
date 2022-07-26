using asf.cms.bll;
using asf.cms.model;
using System;

namespace asf.cms.controller
{
    public struct ContentElement
    {
        public string Title;
        public string Content;
        public string ContentNews;
        public string Link;
        public string Class;
        public bool Selected;
        public string MainImage;
        public DateTime Published;
        public string Target;

        public ContentElement(Section section)
        {
            Title = section.Title;
            Content = section.MainPublication != null ? section.MainPublication.Content : "";
            ContentNews = section.MainPublication != null ? section.MainPublication.NewsContent != null ? section.MainPublication.NewsContent : "" : "";
            Link = section.GetLink();
            Class = section.CssClass != null ? section.CssClass : section.MainPublication != null ? section.MainPublication.CssClass : "";
            Selected = false;
            MainImage = null;
            Published = section.MainPublication != null ? section.MainPublication.Published : section.Updated;
            Target = section.IsRedirection ? section.RedirectOptions : "";
        }

        public ContentElement(PublicationVO publication)
        {
            Title = publication.Title;
            Content = publication.Content;
            ContentNews = publication.NewsContent != null ? publication.NewsContent : "";
            Link = "/Publication/" + publication.Permalink;
            Class = publication.CssClass;
            Selected = false;

            FileVO file = Publication.GetMainFile(publication.Id);

            MainImage = file != null ? file.Path : null;
            Published = publication.Published;
            Target = "";
        }

        public ContentElement(MenuItem menuItem)
        {
            Title = menuItem.Section.Title;
            Content = menuItem.Section.MainPublication != null ? menuItem.Section.MainPublication.Content : "";
            ContentNews = menuItem.Section.MainPublication != null ? menuItem.Section.MainPublication.NewsContent != null ? menuItem.Section.MainPublication.NewsContent : "" : "";
            Link = menuItem.Section.GetLink();
            Class = menuItem.Item.CSSClass;
            Selected = false;
            MainImage = null;
            Published = menuItem.Section.MainPublication != null ? menuItem.Section.MainPublication.Published : menuItem.Section.Updated;
            Target = menuItem.Section.IsRedirection ? menuItem.Section.RedirectOptions : "";
        }
    }
}