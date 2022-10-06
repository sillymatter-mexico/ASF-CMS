using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.model;
using asf.cms.helper;
using NHibernate.Criterion;
using NHibernate.Transform;
using NHibernate;
using NHibernate.Cfg;
using asf.cms.exception;

namespace asf.cms.dal
{
    public class AuditEntityDAL:DAL<AuditEntityVO>
    {

        public IList<AuditEntityVO> GetInsertableEntitiesFromPreload(int year)
        {
            IList<AuditEntityVO> Lista = new List<AuditEntityVO>();

            String query = "select distinct clave_ente as EntityKey, clave_dependencia as DepKey,clave_tipo_ente as TypeKey," +
                "siglas_ente as ShortName, nombre_ente as Name " +
                "from preload_audit_report where entity_id is null and anio=:year;";

            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                try
                {
                    ISQLQuery iquery = session.CreateSQLQuery(query);
                    iquery.SetParameter("year", year);
                    iquery.SetResultTransformer(Transformers.AliasToBean(typeof(AuditEntityVO)));
                    Lista = iquery.List<AuditEntityVO>();
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
