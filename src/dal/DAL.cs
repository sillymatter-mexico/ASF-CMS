using System;
using System.Collections.Generic;
using NHibernate;
using asf.cms.helper;
using asf.cms.exception;

namespace asf.cms.dal
{
    public class DAL<T>
    {
        public T Insert(T o)
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                try
                {
                    using (ITransaction tx = session.BeginTransaction())
                    {
                        session.Save(o);
                        tx.Commit();
                    }
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

            return o;
        }

        public void Delete(T o)
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                try
                {
                    using (ITransaction tx = session.BeginTransaction())
                    {
                        session.Delete(o);
                        tx.Commit();
                    }
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
        }

        protected void Delete(string  query)
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                try
                {
                    using (ITransaction tx = session.BeginTransaction())
                    {
                        session.CreateSQLQuery(query).ExecuteUpdate();
                        tx.Commit();
                    }
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
        }

        public T Update(T o)
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                try
                {
                    using (ITransaction tx = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(o);
                        tx.Commit();
                        
                    }
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

            return o;
        }

        public List<T> UpdateList(List<T> list, string deleteQuery)
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                try
                {
                    using (ITransaction tx = session.BeginTransaction())
                    {
                        session.CreateQuery(deleteQuery).ExecuteUpdate();
                        foreach (T o in list)
                            session.SaveOrUpdate(o);
                        tx.Commit();
                    }
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

            return list;
        }
        
        public T GetById(object id)
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                try
                {
                    T o = session.Get<T>(id);
                    return o;
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
        }

        protected IList<T> list(String query)
        {
            IList<T> Lista = new List<T>();

            try
            {
                Lista = list(query, new Dictionary<string, object>());
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }

            return Lista;
        }

        protected IList<T> list(String query,Dictionary<string,object> parameters)
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                try
                {
                    IQuery iquery = session.CreateQuery(query);
                    foreach (KeyValuePair<string, object> param in parameters)
                        iquery.SetParameter(param.Key, param.Value);
                    IList<T> Lista = iquery.List<T>();
                    return Lista;
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
        }

        protected IList<T> list(String query, Type resultType)
        {
            try
            {
                return list(query, new Dictionary<string, object>(), resultType);
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        protected IList<T> list(String query, Dictionary<string, object> parameters, Type resultType)
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                try
                {
                    ISQLQuery iquery = session.CreateSQLQuery(query);
                    iquery.AddEntity(resultType);
                    string[] paramNames = iquery.NamedParameters;
                    foreach (KeyValuePair<string, object> param in parameters)
                        iquery.SetParameter(param.Key, param.Value);
                    IList<T> Lista = iquery.List<T>();
                    return Lista;
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
        }

        protected IList<T> list(String query, Dictionary<string, object> parameters, int maxResults, int offset)
        {
            offset = offset < 0 ? 0 : offset;
            maxResults = maxResults < 1 ? 1 : maxResults;
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                try
                {
                    IQuery iquery = session.CreateQuery(query);
                    iquery.SetMaxResults(maxResults);
                    iquery.SetFirstResult(offset);
                    foreach (KeyValuePair<string, object> param in parameters)
                        iquery.SetParameter(param.Key, param.Value);
                    return iquery.List<T>();
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
        }

        protected T getObject(String query)
        {
            try
            {
                return getObject(query, new Dictionary<string, object>());
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        protected T getObject(String query, Dictionary<string, object> parameters)
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                try
                {
                    IQuery iquery = session.CreateQuery(query);
                    foreach (KeyValuePair<string, object> param in parameters)
                        iquery.SetParameter(param.Key, param.Value);
                    T obj = iquery.UniqueResult<T>();
                    return obj;
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
        }

        protected T getObject(String query, Type resultType)
        {
            try
            {
                return getObject(query, new Dictionary<string, object>(), resultType);
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        protected T getObject(String query, Dictionary<string, object> parameters, Type resultType)
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                try
                {
                    ISQLQuery iquery = session.CreateSQLQuery(query);
                    iquery.AddEntity(resultType);
                    foreach (KeyValuePair<string, object> param in parameters)
                        iquery.SetParameter(param.Key, param.Value);
                    T obj = iquery.UniqueResult<T>();
                    return obj;
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
        }

        protected int Update(string query,Dictionary<string, object> parameters )
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                try
                {
                    ISQLQuery iquery = session.CreateSQLQuery(query);
                    foreach (KeyValuePair<string, object> param in parameters)
                        iquery.SetParameter(param.Key, param.Value);
                    return iquery.ExecuteUpdate();
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
        }

        protected string getString(string query)
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                try
                {
                    ISQLQuery iquery = session.CreateSQLQuery(query).AddScalar("result", NHibernateUtil.String);
                    return (string)iquery.UniqueResult();
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
        }
    }
}
