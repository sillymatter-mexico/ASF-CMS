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
    public class GrupoFuncionalDAL: DAL<GrupoFuncionalVO>
    {
        public IList<GrupoFuncionalVO> GetInsertableGroupsFromPreload(int year)
        {
            string query = "select distinct group_code as Code, group_name as Name" +
                   " from preload_audit_report where group_id is null " +
                   " and (group_code is not null or group_name is not null)" +
                   " and (group_code!='' or group_name!='') and anio='" + year + "';";
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                ISQLQuery iquery = session.CreateSQLQuery(query);
                iquery.SetResultTransformer(Transformers.AliasToBean(typeof(GrupoFuncionalVO)));
                IList<GrupoFuncionalVO> Lista = iquery.List<GrupoFuncionalVO>();
                return Lista;
            }
        }

    }
}
