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
    public class RecuperacionDAL : DAL<RecuperacionVO>
    {

        public IList<RecuperacionVO> GetAllActive()
        {
            return this.list("select r from RecuperacionVO as  r where r.Active=true");

        }
        public void InactivaById(int id)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("id", id);
            this.Update("update recuperaciones rvo set active=0 where id=:id",param);
        }

    }
}
