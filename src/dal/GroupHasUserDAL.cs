using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.model;

namespace asf.cms.dal
{
    public class GroupHasUserDAL : DAL<GroupHasUserVO>
    {
        public IList<GroupHasUserVO> listForGroup(int id)
        {
            String query="select u.username as user_username, ifnull(gh.group_id,0) as group_id "+
            "from  user u "+
            "left join group_has_user gh on u.username=gh.user_username "+
            "and gh.group_id='"+id+"' where u.type!='ADMIN' order by group_id desc, u.username";
            return this.list(query, typeof(GroupHasUserVO));

        }
    }
}
