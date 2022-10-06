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
    public class AuditSectorDAL : DAL<AuditSectorVO>
    {

        public IList<AuditSectorVO> GetInsertableSectorsFromPreload(int year)
        {
            IList<AuditSectorVO> Lista = new List<AuditSectorVO>();

            String query = "select distinct nombre_sector as Name, '' as AsfKey " +
            " from preload_audit_report par where sector_id is null and anio=:year";

            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                try
                {
                    ISQLQuery iquery = session.CreateSQLQuery(query);
                    iquery.SetParameter("year", year);
                    iquery.SetResultTransformer(Transformers.AliasToBean(typeof(AuditSectorVO)));
                    Lista = iquery.List<AuditSectorVO>();
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
    }
}
