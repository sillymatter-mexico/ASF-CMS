using System;
using System.Collections.Generic;
using NHibernate;
using asf.cms.helper;

namespace asf.cms.dal
{
    public class DAL<T>
    {
        public T Insert(T o)
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                using (ITransaction tx = session.BeginTransaction())
                {
                    session.Save(o);
                    tx.Commit();
                }
            }
            return o;
        }

        public void Delete(T o)
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                using (ITransaction tx = session.BeginTransaction())
                {
                    session.Delete(o);
                    tx.Commit();
                }
            }
        }

        protected void Delete(string  query)
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                using (ITransaction tx = session.BeginTransaction())
                {
                    session.CreateSQLQuery(query).ExecuteUpdate();
                    tx.Commit();
                }
            }
        }

        public T Update(T o)
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                using (ITransaction tx = session.BeginTransaction())
                {
                    session.SaveOrUpdate(o);
                    tx.Commit();
                    return o;
                }
            }
        }

        public List<T> UpdateList(List<T> list, string deleteQuery)
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                using (ITransaction tx = session.BeginTransaction())
                {
                    session.CreateQuery(deleteQuery).ExecuteUpdate();
                    foreach (T o in list)
                        session.SaveOrUpdate(o);
                    tx.Commit();
                }
            }
            return list;
        }
        
        public T GetById(object id)
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                    T o = session.Get<T>(id);
                    return o;
            }
        }

        protected IList<T> list(String query)
        {
            return list(query, new Dictionary<string, object>());
        }

        protected IList<T> list(String query,Dictionary<string,object> parameters)
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                    IQuery iquery =  session.CreateQuery(query);
                    foreach (KeyValuePair<string, object> param in parameters)
                        iquery.SetParameter(param.Key, param.Value);
                    IList<T> Lista = iquery.List<T>();
                    return Lista;
            }
        }

        protected IList<T> list(String query, Type resultType)
        {
                return list(query, new Dictionary<string, object>(),resultType);
        }

        protected IList<T> list(String query, Dictionary<string, object> parameters, Type resultType)
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                ISQLQuery iquery = session.CreateSQLQuery(query);
                iquery.AddEntity(resultType);
                string[] paramNames = iquery.NamedParameters;
                foreach (KeyValuePair<string, object> param in parameters)
                    iquery.SetParameter(param.Key, param.Value);
                IList<T> Lista = iquery.List<T>();
                return Lista;
            }
        }

        protected IList<T> list(String query, Dictionary<string, object> parameters, int maxResults, int offset)
        {
            offset = offset < 0 ? 0 : offset;
            maxResults = maxResults < 1 ? 1 : maxResults;
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                IQuery iquery = session.CreateQuery(query);
                iquery.SetMaxResults(maxResults);
                iquery.SetFirstResult(offset);
                foreach (KeyValuePair<string, object> param in parameters)
                    iquery.SetParameter(param.Key, param.Value);
                return iquery.List<T>();
            }
        }

        protected T getObject(String query)
        {
            return getObject(query, new Dictionary<string, object>());
        }

        protected T getObject(String query, Dictionary<string, object> parameters)
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                IQuery iquery = session.CreateQuery(query);
                foreach (KeyValuePair<string, object> param in parameters)
                    iquery.SetParameter(param.Key, param.Value);
                T obj = iquery.UniqueResult<T>();
                return obj;
            }
        }

        protected T getObject(String query, Type resultType)
        {
            return getObject(query, new Dictionary<string, object>(),resultType);
        }

        protected T getObject(String query, Dictionary<string, object> parameters, Type resultType)
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                ISQLQuery iquery = session.CreateSQLQuery(query);
                iquery.AddEntity(resultType);
                foreach (KeyValuePair<string, object> param in parameters)
                    iquery.SetParameter(param.Key, param.Value);
                T obj = iquery.UniqueResult<T>();
                return obj;
            }
        }

        protected int Update(string query,Dictionary<string, object> parameters )
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                ISQLQuery iquery = session.CreateSQLQuery(query);
                foreach (KeyValuePair<string, object> param in parameters)
                    iquery.SetParameter(param.Key, param.Value);
                return iquery.ExecuteUpdate();
            }
        }

        protected string getString(string query)
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                ISQLQuery iquery = session.CreateSQLQuery(query).AddScalar("result", NHibernateUtil.String);
                return (string)iquery.UniqueResult();
            }
        }
    }
}
