using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.model;

namespace asf.cms.dal
{
    public class ModificationLogDAL : DAL<ModificationLogVO>
    {
        public ModificationLogDAL()
        {
         
        }

        public List<ModificationLogVO> ListAll()
        {
            return new List<ModificationLogVO>(list("SELECT mod FROM ModificationLogVO mod"));
        }

        public List<ModificationLogVO> List(DateTime start, DateTime end)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            string dq = DateRangeQuery(start, end, ref param);
            return new List<ModificationLogVO>(list("SELECT mod FROM ModificationLogVO mod" + (String.IsNullOrEmpty(dq) ? "" : (" WHERE " + dq)), param));
        }

        public List<ModificationLogVO> GetByTypeAndTarget(string type, string target)
        {
            string query = "SELECT mod FROM ModificationLogVO mod WHERE mod.Type=:type AND mod.TargetType=:target";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("type", type);
            param.Add("target", target);

            return new List<ModificationLogVO>(list(query, param));
        }

        public List<ModificationLogVO> GetByTypeAndTarget(string type, string target, DateTime start, DateTime end)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            string dq = DateRangeQuery(start, end, ref param);
            string query = "SELECT mod FROM ModificationLogVO mod WHERE mod.Type=:type AND mod.TargetType=:target" + (String.IsNullOrEmpty(dq) ? "" : (" AND " + dq));

            param.Add("type", type);
            param.Add("target", target);

            return new List<ModificationLogVO>(list(query, param));
        }

        public List<ModificationLogVO> GetByType(string type, DateTime start, DateTime end)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            string dq = DateRangeQuery(start, end, ref param);
            string query = "SELECT mod FROM ModificationLogVO mod WHERE mod.Type=:type" + (String.IsNullOrEmpty(dq) ? "" : (" AND " + dq));
            
            param.Add("type", type);

            return new List<ModificationLogVO>(list(query, param));
        }

        public List<ModificationLogVO> GetByTarget(string target, DateTime start, DateTime end)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            string dq = DateRangeQuery(start, end, ref param);

            string query = "SELECT mod FROM ModificationLogVO mod WHERE mod.TargetType=:target" + (String.IsNullOrEmpty(dq) ? "" : (" AND " + dq));

            param.Add("target", target);

            return new List<ModificationLogVO>(list(query, param));
        }

        private static string DateRangeQuery(DateTime start, DateTime end, ref Dictionary<string, object> param)
        {
            string res = "";
            if (start != null)
            {
                res += "mod.Created<=:start";
                param.Add("start", start);
            }
            if (end != null)
            {
                if (res != "")
                    res += " AND ";
                res += "mod.Created>=:end";
                param.Add("end", end);
            }
            return res;
        }
    }
}