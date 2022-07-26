using asf.cms.helper;
using asf.cms.model;
using NHibernate;
using System.Collections.Generic;

namespace asf.cms.dal
{
    public class PublicationLinkAccessDAL : DAL<PublicationLinkAccessVO>
    {
        private const string GET_PUBLICATION_LINK_ACCESS_RESULTS = @"SELECT `access_url`, SUM(`hit_count`) AS `total`, SUM(IF(YEAR(CURDATE()) = YEAR(`access_date`), `hit_count`, 0)) AS `year`, SUM(IF(MONTH(CURDATE()) = MONTH(`access_date`), `hit_count`, 0)) AS `month`, SUM(IF(DAY(CURDATE()) = DAY(`access_date`), `hit_count`, 0)) AS `day` FROM `publication_link_access` group BY `access_url`";

        public IList<PublicationLinkAccessResultVO> GetPublicationLinkAccessResults()
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                ISQLQuery iquery = session.CreateSQLQuery(GET_PUBLICATION_LINK_ACCESS_RESULTS);
                iquery.AddEntity(typeof(PublicationLinkAccessResultVO));
                return iquery.List<PublicationLinkAccessResultVO>();
            }
        }
    }
}