using System;
using System.Collections.Generic;
using System.Web;
using NHibernate;
using NHibernate.Cfg;
using asf.cms.model;
using asf.cms.helper;
using System.Text;
using NHibernate.Criterion;
using NHibernate.Transform;
using asf.cms.exception;

namespace asf.cms.dal
{
    public class FileDAL:DAL<FileVO>
    {
        public FileVO getByPath(string path)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("path", path);
                return this.getObject("select fvo from FileVO fvo where fvo.Path=:path", param);
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public IList<FileVO> GetFilesByPublicationIdAndMime(string publicationId, string mimes)
        {
            try
            {
                int cont = 0;
                StringBuilder mimesCondition = new StringBuilder("(");
                string[] mimesArr = mimes.Split(',');
                foreach (string mime in mimesArr)
                {
                    mimesCondition.Append(" mime LIKE '%" + mime + "%' ");
                    if (cont != mimesArr.Length - 1)
                    {
                        mimesCondition.Append(" OR ");
                    }
                    cont++;
                }
                mimesCondition.Append(" ) ");

                string query = "select id as Id, publication_id as PublicationId, mime as Mime, path as Path, name as Name, title as Title from file where publication_id=" + publicationId + " and " + mimesCondition;
                
                using (ISession session = NHibernateHelper.GetCurrentSession())
                {
                    ISQLQuery iquery = session.CreateSQLQuery(query);
                    iquery.SetResultTransformer(Transformers.AliasToBean(typeof(FileVO)));
                    IList<FileVO> Lista = iquery.List<FileVO>();
                    return Lista;
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

        public IEnumerable<FileVO> GetFilesByMime(string mimes, int languageId)
        {
            try
            {
                int cont = 0;
                StringBuilder mimesCondition = new StringBuilder("(");
                string[] mimesArr = mimes.Split(',');
                foreach (string mime in mimesArr)
                {
                    mimesCondition.Append(" mime LIKE '%" + mime + "%' ");
                    if (cont != mimesArr.Length - 1)
                    {
                        mimesCondition.Append(" OR ");
                    }
                    cont++;
                }
                mimesCondition.Append(" ) ");

                string query = "select f.id as Id, publication_id as PublicationId, mime as Mime, path as Path, name as Name,f.title as Title from file f INNER JOIN publication p ON p.id=f.publication_id WHERE language_id= " + languageId + " AND " + mimesCondition;
                using (ISession session = NHibernateHelper.GetCurrentSession())
                {
                    ISQLQuery iquery = session.CreateSQLQuery(query);
                    iquery.SetResultTransformer(Transformers.AliasToBean(typeof(FileVO)));
                    IList<FileVO> Lista = iquery.List<FileVO>();
                    return Lista;
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

        public IList<FileVO> ListByPublicationId(int publicationId)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("publicationId", publicationId);
                return this.list("select fvo from FileVO fvo where fvo.PublicationId=:publicationId", param);
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }
        
        public int UpdateTitle(int fileId,string title)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("fileId", fileId);
                param.Add("title", title);
                string query = "update file set title=:title where id=:fileId";
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

        public int UpdateIsMain(int fileId, int isMain)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("fileId", fileId);
                param.Add("isMain", isMain);
                string query = "UPDATE file SET is_main=:isMain WHERE id=:fileId";
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

        public FileVO GetMainFile(int publicationId)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("publicationId", publicationId);
                string query = "SELECT fvo FROM FileVO fvo WHERE fvo.PublicationId=:publicationId AND fvo.IsMain = 1";

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
    }
}
