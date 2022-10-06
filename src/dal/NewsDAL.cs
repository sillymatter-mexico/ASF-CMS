using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.model;
using asf.cms.bll;
using NHibernate;
using asf.cms.helper;
using NHibernate.Transform;
using static System.Collections.Specialized.BitVector32;
using asf.cms.exception;

namespace asf.cms.dal
{
    /// <summary>
    /// Data Access Layer class for the News view/Publications table
    /// </summary>
    public class NewsDAL : DAL<NewsVO>
    {
        /// <summary>
        /// Returns all the news (with include true), is used for the default command
        /// </summary>
        /// <returns></returns>
        public IList<NewsVO> GetAllFromView()
        {
            try
            {
                return this.list("select n from NewsVO as n");
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public IList<NewsVO> GetAllFromView(int languageId)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("language_id", languageId);
                List<NewsVO> nvsToRemove = new List<NewsVO>();
                IList<NewsVO> anchoredNews = this.list("select n from NewsVO as n where n.LanguageId=:language_id order by n.NewsPin DESC, n.Updated DESC", param);
                removeDueNews(anchoredNews);
                return anchoredNews;
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        /// <summary>
        /// Returns all the news that has the provided value in the news_include field
        /// </summary>
        /// <param name="news_include"></param>
        /// <returns></returns>
        public IList<NewsVO> GetAll(bool news_include)
        {
            try
            {
                PublicationDAL pDAL = new PublicationDAL();
                IList<PublicationVO> publications = pDAL.ListPublicationsByNewsInclude(news_include);
                IList<NewsVO> news = new List<NewsVO>();


                foreach (PublicationVO publication in publications)
                {
                    news.Add(new NewsVO()
                    {
                        Content = publication.NewsContent,
                        Id = publication.Id,
                        NewsPin = publication.NewsPin,
                        NewsTTL = publication.NewsTTL,
                        Permalink = publication.Permalink,
                        Title = publication.Title,
                        Updated = publication.Updated
                    });
                }

                removeDueNews(news);

                return news;
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        /// <summary>
        /// Returns all the news, included or not
        /// </summary>
        /// <param name="news_include"></param>
        /// <returns></returns>
        public IList<NewsVO> GetAll()
        {
            try
            {
                PublicationDAL pDAL = new PublicationDAL();
                IList<PublicationVO> publications = pDAL.GetAll();
                IList<NewsVO> news = new List<NewsVO>();

                foreach (PublicationVO publication in publications)
                {
                    news.Add(new NewsVO()
                    {
                        Content = publication.NewsContent,
                        Id = publication.Id,
                        NewsPin = publication.NewsPin,
                        NewsTTL = publication.NewsTTL,
                        Permalink = publication.Permalink,
                        Title = publication.Title,
                        Updated = publication.Updated
                    });
                }

                return news;
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        /// <summary>
        /// Returns all the news, included or not and that correspond to the provided language id
        /// </summary>
        /// <param name="languageId"></param>
        /// <returns></returns>
        public IList<NewsVO> GetAll(int languageId)
        {
            try
            {
                PublicationDAL pDAL = new PublicationDAL();
                IList<PublicationVO> publications = pDAL.GetAll(languageId);
                IList<NewsVO> news = new List<NewsVO>();


                foreach (PublicationVO publication in publications)
                {
                    news.Add(new NewsVO()
                    {
                        Content = publication.NewsContent,
                        Id = publication.Id,
                        NewsPin = publication.NewsPin,
                        NewsTTL = publication.NewsTTL,
                        Permalink = publication.Permalink,
                        Title = publication.Title,
                        Updated = publication.Updated
                    });
                }

                return news;
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        /// <summary>
        /// Get the news that belong to the provided section id
        /// </summary>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        public IList<NewsVO> GetNewsBySectionId(int sectionId)
        {
            try
            {
                PublicationDAL pDAL = new PublicationDAL();
                IList<PublicationVO> publications = pDAL.GetListBySection(sectionId, Language.GetCurrentLanguageId());
                IList<NewsVO> news = new List<NewsVO>();


                foreach (PublicationVO publication in publications)
                {
                    news.Add(new NewsVO()
                    {
                        Content = publication.NewsContent,
                        Id = publication.Id,
                        NewsPin = publication.NewsPin,
                        NewsTTL = publication.NewsTTL,
                        Permalink = publication.Permalink,
                        Title = publication.Title,
                        Updated = publication.Updated
                    });
                }
                removeDueNews(news);
                return news;
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        /// <summary>
        /// Get the news that belong to the provided section and to the provided publication ids
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="publicationIds"></param>
        /// <returns></returns>
        public IList<NewsVO> GetNewsBySectionIdAndPublicationIds(int sectionId, IList<int> publicationIds)
        {
            try
            {
                PublicationDAL pDAL = new PublicationDAL();
                IList<PublicationVO> publications = pDAL.GetListBySection(sectionId, Language.GetCurrentLanguageId());
                IList<NewsVO> news = new List<NewsVO>();

                foreach (PublicationVO publication in publications)
                {
                    if (publicationIds.Contains(publication.Id))
                    {
                        news.Add(new NewsVO()
                        {
                            Content = publication.NewsContent,
                            Id = publication.Id,
                            NewsPin = publication.NewsPin,
                            NewsTTL = publication.NewsTTL,
                            Permalink = publication.Permalink,
                            Title = publication.Title,
                            Updated = publication.Updated
                        });
                    }
                }

                return news;
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        /// <summary>
        /// Get the news that belong to a specific publication
        /// </summary>
        /// <param name="publicationId"></param>
        /// <returns></returns>
        public IList<NewsVO> GetNewsByPublicationId(int publicationId)
        {
            try
            {
                PublicationDAL pDAL = new PublicationDAL();
                PublicationVO publication = pDAL.GetById(publicationId);
                IList<NewsVO> news = new List<NewsVO>();
                news.Add(new NewsVO()
                {
                    Content = publication.NewsContent,
                    Id = publication.Id,
                    NewsPin = publication.NewsPin,
                    NewsTTL = publication.NewsTTL,
                    Permalink = publication.Permalink,
                    Title = publication.Title,
                    Updated = publication.Updated
                });
                removeDueNews(news);

                return news;
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        /// <summary>
        /// List all the news, in admin mode
        /// </summary>
        /// <returns></returns>
        public IList<NewsVO> ListNewsAdmin()
        {
            IList<NewsVO> Lista = new List<NewsVO>();

            string query = "select publication.id  as Id,title as Title, publication.news_content as Content, section.news_include as IncludeInSection, " +
                "publication.news_include as IncludeInPublication, publication.news_ttl as NewsTTL, publication.news_pin as NewsPin, publication.is_main as IsMain " +
                "from publication, section_label, section " +
                "where section_label.language_id=publication.language_id " +
                "and section_label.section_id=publication.section_id " +
                "and section.active=1 and publication.active=1 " +
                "and section.id=publication.section_id ORDER BY publication.section_id ASC, publication.is_main DESC, section.position ASC, publication.position ASC;";
 //           return this.list(query, typeof(NewsVO));

            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                try
                {
                    ISQLQuery iquery = session.CreateSQLQuery(query);
                    iquery.SetResultTransformer(Transformers.AliasToBean(typeof(NewsVO)));
                    Lista = iquery.List<NewsVO>();
                }
                catch (Exception ex)
                {
                    throw new ProcessException("Error en el proceso.");
                }
                finally
                {
                    session.Close();
                }
            }

            return Lista;
        }

        /// <summary>
        /// List all the news, in admin mode, that belong to the provided username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public IList<NewsVO> ListNewsAdmin(string username)
        {
            IList<NewsVO> Lista = new List<NewsVO>();

            string query = "select publication.id  as Id,title as Title, publication.news_content as Content, section.news_include as IncludeInSection, " +
                "publication.news_include as IncludeInPublication, publication.news_ttl as NewsTTL, publication.news_pin as NewsPin " +
                "from publication, section_label , group_has_section sg, group_has_user gu, section " +
                "where gu.user_username=:username " +
                "and sg.group_id=gu.group_id " +
                "and publication.section_id=sg.section_id " +
                "and section_label.language_id=publication.language_id " +
                "and section_label.section_id=publication.section_id " +
                "and section.active=1 and publication.active=1 " +
                "and section.id=publication.section_id order by Title;";
         //   return this.list(query, typeof(NewsVO));

            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                try
                {
                    ISQLQuery iquery = session.CreateSQLQuery(query);
                    iquery.SetParameter("username", ExpresionStringHelper.replaceEscapeCharacter(username));
                    iquery.SetResultTransformer(Transformers.AliasToBean(typeof(NewsVO)));
                    Lista = iquery.List<NewsVO>();
                }
                catch (Exception ex)
                {
                    throw new ProcessException("Error en el proceso.");
                }
                finally
                {
                    session.Close();
                }
            }

            return Lista;
        }

        public NewsVO GetByIdFromPublication(int newsId)
        {
            try
            {
                PublicationDAL pDAL = new PublicationDAL();
                PublicationVO publication = pDAL.GetById(newsId);
                NewsVO newVO = new NewsVO();
                bool isSection = (publication == null);
                if (isSection)
                {
                    newVO = new NewsVO()
                    {
                        IsSection = isSection
                    };
                }
                else
                {
                    newVO = new NewsVO()
                    {
                        Content = publication.NewsContent,
                        Id = publication.Id,
                        NewsPin = publication.NewsPin,
                        NewsTTL = publication.NewsTTL,
                        Permalink = publication.Permalink,
                        Title = publication.Title,
                        Updated = publication.Updated,
                        IsSection = isSection
                    };
                }
                return newVO;
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        /// <summary>
        /// Removes those news that are beyond its time to live (which is its updated time + ttl)
        /// </summary>
        /// <param name="original"></param>
        private void removeDueNews(IList<NewsVO> original) 
        {
            List<NewsVO> nvsToRemove = new List<NewsVO>();

            try
            {
                //Remove the ones that should not be displayed
                foreach (NewsVO nv in original)
                {
                    if (!nv.NewsPin)
                    {
                        if (nv.NewsTTL != null && nv.NewsTTL != 0)
                        {

                            DateTime dateOfDead = nv.Updated.AddDays(nv.NewsTTL);
                            int comparison = dateOfDead.CompareTo(DateTime.Now);
                            if (comparison < 0)
                            {
                                //Remove it
                                nvsToRemove.Add(nv);
                            }
                        }
                    }
                }

                foreach (NewsVO nv in nvsToRemove)
                {
                    original.Remove(nv);
                }
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }
    }
}