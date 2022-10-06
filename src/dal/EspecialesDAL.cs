﻿using System;
using System.Collections.Generic;
using System.Web;
using NHibernate;
using NHibernate.Cfg;
using asf.cms.model;
using asf.cms.helper;
using NHibernate.Transform;
using System.Text;
using asf.cms.exception;

namespace asf.cms.dal
{
    public class EspecialesDAL : DAL<EspecialesVO>
    {
        public IList<EspecialesAdminVO> ListPublicationAdmin(string orden)
        {
            IList<EspecialesAdminVO> Lista = new List<EspecialesAdminVO>();

            if (orden == "")
                orden = "order by  Section, position, Status, Title;";
            
            string query = "select publication.id as Id,title as Title, publication.language_id as LanguageId, publication.autogenerated_type as AutogeneratedType, section_label.content as Section," +
                "case when publication.sitemap_exclude=0 then false else true end as _sitemapExclude," +
                "case when is_main=0 then false else true end as isMain,status as Status " +
                "from publication, section_label " +
                "where section_label.language_id=publication.language_id " +
                "and section_label.section_id=publication.section_id " +
                "and publication.active=1 " + 
                "and publication.autogenerated_type='especiales' " +
                orden;

            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                try
                {
                    ISQLQuery iquery = session.CreateSQLQuery(query);
                    iquery.SetResultTransformer(Transformers.AliasToBean(typeof(EspecialesAdminVO)));
                    Lista = iquery.List<EspecialesAdminVO>();
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
        public IList<EspecialesAdminVO> ListPublicationAdmin(String username, string orden)
        {
            IList<EspecialesAdminVO> Lista = new List<EspecialesAdminVO>();

            string query = "select publication.id  as Id,title as Title, publication.language_id as LanguageId,section_label.content as Section," +
                "case when publication.sitemap_exclude=0 then false else true end as _sitemapExclude," +
                "case when is_main=0 then false else true end as isMain,status as Status " +
                "from publication, section_label , group_has_section sg, group_has_user gu " +
                "where gu.user_username='" + username + "' " +
                "and sg.group_id=gu.group_id " +
                "and publication.section_id=sg.section_id " +
                "publication.active=1 " +
                "and section_label.language_id=publication.language_id " +
                "and section_label.section_id=publication.section_id order by Section,position, Status, Title;" +
                "and publication.autogenerated_type='especiales'";

            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                try
                {
                    ISQLQuery iquery = session.CreateSQLQuery(query);
                    iquery.SetResultTransformer(Transformers.AliasToBean(typeof(EspecialesAdminVO)));
                    Lista = iquery.List<EspecialesAdminVO>();
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

        public IList<EspecialesAdminVO> ListPublicationAdmin(int sectionId)
        {
            IList<EspecialesAdminVO> Lista = new List<EspecialesAdminVO>();

            string query = "select publication.id  as Id,title as Title, publication.language_id as LanguageId,section_label.content as Section," +
                "case when publication.sitemap_exclude=0 then false else true end as _sitemapExclude," +
                "case when is_main=0 then false else true end as isMain,status as Status " +
                "from publication, section_label " +
                "where publication.section_id='" + sectionId + "' " +
                "and section_label.language_id=publication.language_id " +
                "and publication.active=1 " +
                "and section_label.section_id=publication.section_id order by Section,position, Status, Title;" +
                "and publication.autogenerated_type='especiales'";

            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                try
                {
                    ISQLQuery iquery = session.CreateSQLQuery(query);
                    iquery.SetResultTransformer(Transformers.AliasToBean(typeof(EspecialesAdminVO)));
                    Lista = iquery.List<EspecialesAdminVO>();
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

        public IList<EspecialesVO> ListPublicationsByNewsInclude(bool news_include)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("news_include", news_include);
                string query = "select p from EspecialesVO as p where p.NewsInclude=:news_include and p.Active=true order by position";
                
                return this.list(query, param);
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public EspecialesVO GetPrincipalBySection(int sectionId, int languageId)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("sId", sectionId);
                param.Add("lId", languageId);
                string query = "select * from publication where section_id=:sId and is_main=1 and status=1 and active=1 and language_id=:lId limit 1 and autogenerated_type='especiales';";
                
                return this.getObject(query, param, typeof(EspecialesVO));
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public IList<EspecialesVO> GetByType(string type)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                string query = "select * from publication where autogenerated_type='especiales' and active=1;"; /*
                param.Add("autogen_type", type);
                string query = "select * from publication where autogenerated_type=:autogen_type;";/**/
                
                return this.list(query, param, typeof(EspecialesVO));
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public IList<EspecialesVO> GetListBySection(int sectionId, int languageId)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("sectionId", sectionId);
                param.Add("languageId", languageId);

                string query = "select p from EspecialesVO as p where p.SectionId=:sectionId and p.LanguageId=:languageId  and p.Active=true and p.Status=1 order by p.NewsPin desc, p.Updated desc";
                
                return this.list(query, param);
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {

            }            
        }

        public IList<EspecialesVO> GetListBySection(int sectionId, int languageId, string fieldsToOrderBy)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("sectionId", sectionId);
                param.Add("languageId", languageId);

                StringBuilder sb = new StringBuilder();
                string[] fieldsArr = fieldsToOrderBy.Split(',');
                int cont = 0;
                foreach (string field in fieldsArr)
                {
                    sb.Append("p.");
                    sb.Append(field);
                    if (cont != fieldsArr.Length - 1)
                    {
                        sb.Append(",");
                    }
                    cont++;
                }

                string query = "select p from EspecialesVO as p where p.SectionId=:sectionId and p.LanguageId=:languageId  and p.Active=true and p.Status=1 order by " + sb.ToString() + " ASC";
                
                return this.list(query, param);
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public IList<EspecialesVO> GetOrderedListBySection(int sectionId, int languageId)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("sectionId", sectionId);
                param.Add("languageId", languageId);

                string query = "select p from EspecialesVO as p where p.SectionId=:sectionId and p.LanguageId=:languageId  and p.Active=true and p.Status=1 order by p.Position";
                
                return this.list(query, param);
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public IList<EspecialesVO> GetWithoutContentBySectionId(int sectionId)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("sId", sectionId);
                string query = "select p.Id,p.LanguageId,p.SectionId, p.Permalink,p.Title,p.Created,p.Updated,p.Published,p.IsMain, "
                + "p.IsAutogenerated,p.AutogeneratedType,p.unpublished,p.Status,p.Visitas,p.HasWidgets, p.NewsInclude,p.NewsPin,p.NewsTtl,p.NewsContent,p.Position,p.Active "
                + "from EspecialesVO as p where p.SectionId=:sId  and p.Status=1 and p.Active=true order by p.Position;";
                
                return this.list(query, param);
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public EspecialesVO GetByPermalink(String permalink)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("permalink", permalink);
                string query = "select p from EspecialesVO as p where p.Permalink=:permalink and p.Active=true";
                return this.getObject(query, param);
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public int SetUniqueMainBySection(int publicationId, int sectionId, int languageId)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("sectionId", sectionId);
                param.Add("publicationId", publicationId);
                param.Add("languageId", languageId);
                string query = "update publication set is_main=0 where section_id=:sectionId and id!=:publicationId and language_id=:languageId";
                return this.Update(query, param);
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }
        public int SetHasWidgetsBySection(int publicationId, int sectionId, bool has_widgets)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("sectionId", sectionId);
            param.Add("publicationId", publicationId);
            param.Add("has_widgets", has_widgets);

            string query = "update publication set has_widgets=:has_widgets where section_id=:sectionId and id=:publicationId";
            return this.Update(query, param);
        }
        public IList<EspecialesVO> GetAll()
        {
            try
            {
                return this.list("select p from EspecialesVO as p where p.AutogeneratedType!=null and p.Active=true order by p.NewsPin desc, p.Updated desc");

            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public int UpdateViews(int publicationId, int views)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("publicationId", publicationId);
                param.Add("views", views);
                string query = "update publication set visitas=:views where id=:publicationId";
                
                return this.Update(query, param);
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }
        public int UpdateMeta(int publicationId, string meta)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("publicationId", publicationId);
                param.Add("meta", meta);
                string query = "update publication set meta=:meta where id=:publicationId";
                return this.Update(query, param);
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public IList<EspecialesVO> GetAll(int languageId)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("language_id", languageId);
                return this.list("select p from EspecialesVO as p where p.Active=true and p.LanguageId=:language_id order by p.NewsPin, p.Updated desc", param);
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public int UpdatePublicationNews(int publicationId, string newsContent, int newsTTL, bool newsInclude, bool newsPin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("publicationId", publicationId);
                param.Add("newsContent", newsContent);
                param.Add("newsTTL", newsTTL);
                param.Add("newsInclude", newsInclude);
                param.Add("newsPin", newsPin);

                string query = "update publication set news_content=:newsContent,news_ttl=:newsTTL,news_include=:newsInclude,news_pin=:newsPin where id=:publicationId";
                return this.Update(query, param);
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public int Inactive(string sectionJoined)
        {
            try
            {
                string query = "update publication set active=0 where section_id in (" + sectionJoined + ")";
                return this.Update(query, new Dictionary<string, object>());
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public int Inactive(int publicationId)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("publicationId", publicationId);
                string query = "update publication set active=0 where id=:publicationId";
                return this.Update(query, param);
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public DateTime getFechaUltimaModificacion()
        {
            try
            {
                string query = "select p from EspecialesVO as p where p.Active=1 order by p.Updated desc Limit 0,1";
                IList<EspecialesVO> p = this.list(query);
                if (p.Count > 0)
                    return p[0].Updated;
                return DateTime.Today;
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public DateTime getFechaUltimaModificacionPorSeccion(int sectionId)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("sectionId", sectionId);

                string query = "select p from EspecialesVO as p where p.Active=1 and p.SectionId=:sectionId order by p.Updated desc Limit 0,1";
                IList<EspecialesVO> p = this.list(query);
                if (p.Count > 0)
                    return p[0].Updated;
                return DateTime.Today;
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
