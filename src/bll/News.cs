using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.model;
using asf.cms.dal;

namespace asf.cms.bll
{
    public class News
    {

        public NewsVO NewsInstance {get;set;}

        /// <summary>
        /// Used for default
        /// </summary>
        /// <returns></returns>
        public List<NewsVO> GetAllIncludedNews()
        {
            NewsDAL ndal = new NewsDAL();
            List<NewsVO> news = new List<NewsVO>(ndal.GetAll(true));
            return news;
        }

        /// <summary>
        /// Used for "all"
        /// </summary>
        /// <returns></returns>
        public List<NewsVO> GetAllNews()
        {
            NewsDAL ndal = new NewsDAL();
            List<NewsVO> news = new List<NewsVO>(ndal.GetAll(Language.GetCurrentLanguageId()));
            return news;
        }

        public List<NewsVO> GetAllNewsFromView()
        { 
            NewsDAL ndal = new NewsDAL();
            List<NewsVO> news = new List<NewsVO>(ndal.GetAllFromView(Language.GetCurrentLanguageId()));
            return news;
        }

        /// <summary>
        /// Used for specific section Id
        /// </summary>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        public List<NewsVO> GetNewsBySectionId(int sectionId)
        {
            NewsDAL ndal = new NewsDAL();
            List<NewsVO> news = new List<NewsVO>(ndal.GetNewsBySectionId(sectionId));
            return news;
        }

        /// <summary>
        /// Used for specific publication Id
        /// </summary>
        /// <param name="publicationId"></param>
        /// <returns></returns>
        public List<NewsVO> GetNewsByPublicationId(int publicationId)
        {
            NewsDAL ndal = new NewsDAL();
            List<NewsVO> news = new List<NewsVO>(ndal.GetNewsByPublicationId(publicationId));
            return news;
        }


        public static List<NewsVO> ListNewsAdmin(string username)
        {
            List<NewsVO> lista = new List<NewsVO>();
            NewsDAL ndal = new NewsDAL();

            User user = User.GetUser(username);
            if (user.user == null)
                return lista;
            if (user.user.Type == "ADMIN")
            {
                lista = new List<NewsVO>(ndal.ListNewsAdmin());
                return lista;
            }
            return new List<NewsVO>(ndal.ListNewsAdmin(username));
        }

        public static NewsVO GetById(int newsId)
        {
            NewsDAL ndal = new NewsDAL();
            NewsVO nvo = ndal.GetById(newsId);
            return nvo;
        }

        public static NewsVO GetByIdFromPublication(int newsId)
        {
            NewsDAL ndal = new NewsDAL();
            NewsVO nvo = ndal.GetByIdFromPublication(newsId);
            return nvo;
        }

        public bool Save()
        {
            //A news save is actually a Publication and Section save
            NewsDAL ndal = new NewsDAL();
            int sectionId = 0;
            //Save in publication only if its not a section
            if (!this.NewsInstance.IsSection)
            {
                PublicationDAL pDAL = new PublicationDAL();
                PublicationVO p = pDAL.GetById(this.NewsInstance.Id);

                p.NewsContent = util.Encoder.IsEncodingLatin1(NewsInstance.Content) ? NewsInstance.Content : util.Encoder.ForceEncodingLatin1(NewsInstance.Content);
                p.NewsInclude = this.NewsInstance.IncludeInPublication;
                p.NewsTTL = this.NewsInstance.NewsTTL;
                p.NewsPin = this.NewsInstance.NewsPin;
                pDAL.Update(p);
                sectionId = p.SectionId;
            }
            else 
            {
                sectionId = this.NewsInstance.Id;
            }

            SectionDAL sDAL = new SectionDAL();
            SectionVO section = sDAL.GetById(sectionId);
            section.NewsInclude = this.NewsInstance.IncludeInSection;
            sDAL.Update(section);

            return true;
        }

        public static int CompareByTitle(NewsVO a, NewsVO b)
        {
            return string.Compare(a.Title, b.Title);
        }

        public static int CompareByContent(NewsVO a, NewsVO b)
        {
            return string.Compare(a.Content, b.Content);
        }

        public static int CompareByIncludeIn(NewsVO a, NewsVO b)
        {
            return string.Compare(a.IncludeIn.ToString(), b.IncludeIn.ToString());
        }

        public static int CompareByNewsTTL(NewsVO a, NewsVO b)
        {
            return string.Compare(a.NewsTTL.ToString(), b.NewsTTL.ToString());
        }

        public static int CompareByNewsPin(NewsVO a, NewsVO b)
        {
            return string.Compare(a.NewsPin.ToString(), b.NewsPin.ToString());
        }
    }
}