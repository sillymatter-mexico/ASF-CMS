using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.exception;
using asf.cms.model;

namespace asf.cms.dal
{
    public class GroupHasUserDAL : DAL<GroupHasUserVO>
    {
        public IList<GroupHasUserVO> listForGroup(int id)
        {
            IList<GroupHasUserVO> Lista = new List<GroupHasUserVO>();

            String query = "select u.username as user_username, ifnull(gh.group_id,0) as group_id " +
            "from  user u " +
            "left join group_has_user gh on u.username=gh.user_username " +
            "and gh.group_id='" + id + "' where u.type!='ADMIN' order by group_id desc, u.username";

            try
            {
                Lista =  this.list(query, typeof(GroupHasUserVO));
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }

            return Lista;
        }
    }
}
