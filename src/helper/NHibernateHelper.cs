using System.Collections.Generic;
using System;
using System.Web;
using NHibernate;
using NHibernate.Cfg;

namespace asf.cms.helper
{
    public sealed class NHibernateHelper
    {


        private const string CurrentSessionKey = "nhibernate.current_session";
        private static readonly ISessionFactory sessionFactory;

        static NHibernateHelper()
        {
            sessionFactory = new Configuration().SetProperty("nhibernate.show_sql", "true").Configure().BuildSessionFactory();
        }

        public static ISession GetCurrentSession()
        {
            HttpContext context = HttpContext.Current;
            ISession currentSession = context.Items[CurrentSessionKey] as ISession;
            try
            {
                if (currentSession == null || !currentSession.IsOpen)
                {
                    currentSession = sessionFactory.OpenSession();
                    context.Items[CurrentSessionKey] = currentSession;
                }
                //return currentSession;
            }
            catch (Exception ex) {
                /*ILog log = LogManager.GetLogger("Controller");
                log.Error("Nuevo LOG " + ex.Message + ex.StackTrace + "\\n");
                return null;/**/
            }
            return currentSession;
        }

        public static void CloseSession()
        {
            HttpContext context = HttpContext.Current;
            ISession currentSession = context.Items[CurrentSessionKey] as ISession;

            if (currentSession == null)
            {
                // No current session
                return;
            }

            currentSession.Close();
            context.Items.Remove(CurrentSessionKey);
        }

        public static void CloseSessionFactory()
        {
            if (sessionFactory != null)
            {
                sessionFactory.Close();
            }
        }
    }
}    