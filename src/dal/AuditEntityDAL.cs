using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.model;
using asf.cms.helper;
using NHibernate.Criterion;
using NHibernate.Transform;
using NHibernate;
using NHibernate.Cfg;

namespace asf.cms.dal
{
    public class AuditEntityDAL:DAL<AuditEntityVO>
    {

        public IList<AuditEntityVO> GetInsertableEntitiesFromPreload(int year)
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                String query = "select distinct clave_ente as EntityKey, clave_dependencia as DepKey,clave_tipo_ente as TypeKey," +
                "siglas_ente as ShortName, nombre_ente as Name " +
                "from preload_audit_report where entity_id is null and anio='" + year + "';";

                ISQLQuery iquery = session.CreateSQLQuery(query);
                iquery.SetResultTransformer(Transformers.AliasToBean(typeof(AuditEntityVO)));
                IList<AuditEntityVO> Lista = iquery.List<AuditEntityVO>();
                return Lista;
            }
        }

    }
}
