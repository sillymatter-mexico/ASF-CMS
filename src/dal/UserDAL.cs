using System;
using System.Collections.Generic;
using System.Web;
using NHibernate;
using NHibernate.Cfg;
using asf.cms.model;
using asf.cms.helper;
using asf.cms.exception;

namespace asf.cms.dal
{
    public class UserDAL:DAL<UserVO>
    {
        public UserVO GetByLoginPassword(String login, String password)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("login", login);
                param.Add("password", password);
                return getObject("select u from UserVO as u where u.Username=:login and u.Password=md5(:password) and u.Active=1", param);
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public IList<UserVO> GetListByRole(String role)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                return list("select u from UserVO as u where u.Type LIKE '%" + ExpresionStringHelper.replaceEscapeCharacter(role) + "%'");
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
               
            }            
        }

        public IList<UserVO> GetAll()
        {
            try
            {
                return this.list("select u from UserVO as  u");
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
