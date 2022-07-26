using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.model;
namespace asf.cms.dal
{
    public class GroupDAL : DAL<GroupVO>
    {
        public IList<GroupVO> GetAll()
        {
            return this.list("select g from GroupVO as  g");
        }
        public void Delete( int groupId)
        {
            string query = "delete from group_has_section where group_id='" + groupId + "'";
            this.Delete(query);
            query = "delete from group_has_user where group_id='" + groupId + "'";
            this.Delete(query);
            query = "delete from groups where id='" + groupId + "'";
            this.Delete(query);
        }
    }
}
