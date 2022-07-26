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


namespace asf.cms.dal
{
    public class AuditSectorDAL : DAL<AuditSectorVO>
    {

        public IList<AuditSectorVO> GetInsertableSectorsFromPreload(int year)
        {
            String query = "select distinct nombre_sector as Name, '' as AsfKey " +
            " from preload_audit_report par where sector_id is null and anio='" + year + "'";
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                ISQLQuery iquery = session.CreateSQLQuery(query);
                iquery.SetResultTransformer(Transformers.AliasToBean(typeof(AuditSectorVO)));
                IList<AuditSectorVO> Lista = iquery.List<AuditSectorVO>();
                return Lista;
            }

        }
     
    }
}
