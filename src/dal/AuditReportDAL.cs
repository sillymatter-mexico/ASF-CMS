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
    public class AuditReportDAL : DAL<AuditReportVO>
    {
        public IList<AuditReportVO> GetPublished()
        {
            String query="select ar from AuditReportVO as ar, PublicationVO p "+
                "where ar.PublicationPermalink=p.Permalink "+
                "and p.Active=true and p.Status=true and ar.Published=true";
            return this.list(query);
        }
        public IList<AuditReportInfo> GetAllActive()
        {
            string query = "select ar.id as ReportId, p.id as PublicationId,p.permalink as Permalink, ar.year as Year, " +
            "p.title as Title, ar.published*1 as Published, p.published as PublishDate, count(a.id) as LoadedAudits " +
            "from publication p, audit_report ar " +
            "left join audit a on a.audit_report_id=ar.id " +
            "where ar.publication_permalink=p.permalink and p.active=true " +
            "group by ar.id, p.id,p.permalink, ar.year, p.title, p.published, ar.published;";           
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                ISQLQuery iquery = session.CreateSQLQuery(query);
                iquery.SetResultTransformer(Transformers.AliasToBean(typeof(AuditReportInfo)));
                IList<AuditReportInfo> Lista = iquery.List<AuditReportInfo>();
                return Lista;
            }
        }
    }
}
