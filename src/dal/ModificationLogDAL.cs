using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.exception;
using asf.cms.helper;
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
            try
            {
                return new List<ModificationLogVO>(list("SELECT mod FROM ModificationLogVO mod"));
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }
        }

        public List<ModificationLogVO> List(DateTime start, DateTime end)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                string dq = DateRangeQuery(start, end, ref param);
                return new List<ModificationLogVO>(list("SELECT mod FROM ModificationLogVO mod" + (String.IsNullOrEmpty(dq) ? "" : (" WHERE " + dq)), param));
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public List<ModificationLogVO> GetByTypeAndTarget(string type, string target)
        {
            string query = "SELECT mod FROM ModificationLogVO mod WHERE mod.Type=:type AND mod.TargetType=:target";

            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("type", ExpresionStringHelper.replaceEscapeCharacter(type));
                param.Add("target", ExpresionStringHelper.replaceEscapeCharacter(target));

                return new List<ModificationLogVO>(list(query, param));
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }
        }

        public List<ModificationLogVO> GetByTypeAndTarget(string type, string target, DateTime start, DateTime end)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                string dq = DateRangeQuery(start, end, ref param);
                string query = "SELECT mod FROM ModificationLogVO mod WHERE mod.Type=:type AND mod.TargetType=:target" + (String.IsNullOrEmpty(dq) ? "" : (" AND " + dq));

                param.Add("type", ExpresionStringHelper.replaceEscapeCharacter(type));
                param.Add("target", ExpresionStringHelper.replaceEscapeCharacter(target));

                return new List<ModificationLogVO>(list(query, param));
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public List<ModificationLogVO> GetByType(string type, DateTime start, DateTime end)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                string dq = DateRangeQuery(start, end, ref param);
                string query = "SELECT mod FROM ModificationLogVO mod WHERE mod.Type=:type" + (String.IsNullOrEmpty(dq) ? "" : (" AND " + dq));

                param.Add("type", ExpresionStringHelper.replaceEscapeCharacter(type));

                return new List<ModificationLogVO>(list(query, param));
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public List<ModificationLogVO> GetByTarget(string target, DateTime start, DateTime end)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                string dq = DateRangeQuery(start, end, ref param);

                string query = "SELECT mod FROM ModificationLogVO mod WHERE mod.TargetType=:target" + (String.IsNullOrEmpty(dq) ? "" : (" AND " + dq));

                param.Add("target", ExpresionStringHelper.replaceEscapeCharacter(target));

                return new List<ModificationLogVO>(list(query, param));
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        private static string DateRangeQuery(DateTime start, DateTime end, ref Dictionary<string, object> param)
        {
            try
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
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }
        }
    }
}