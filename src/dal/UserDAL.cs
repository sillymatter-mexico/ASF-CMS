using System;
using System.Collections.Generic;
using System.Web;
using NHibernate;
using NHibernate.Cfg;
using asf.cms.model;
using asf.cms.helper;

namespace asf.cms.dal
{
    public class UserDAL:DAL<UserVO>
    {
        public UserVO GetByLoginPassword(String login, String password)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("login", login);
            param.Add("password", password);
            return getObject("select u from UserVO as u where u.Username=:login and u.Password=md5(:password) and u.Active=1",param); 
        }
        public IList<UserVO> GetListByRole(String role)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            return list("select u from UserVO as u where u.Type LIKE '%"+role+"%'"); 
        }
        public IList<UserVO> GetAll()
        {
            return this.list("select u from UserVO as  u");
        }
    }
}
