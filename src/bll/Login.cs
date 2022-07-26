using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.model;
using asf.cms.dal;
using log4net;
namespace asf.cms.bll
{
    public class Login
    {
        public  UserVO ValidateUser(string username, string password)
        {
            try
            {
                UserDAL udal = new UserDAL();
                UserVO u = udal.GetByLoginPassword(username, password);
                return u;
/*                if (u != null)
                    return true;//autenticado, guardar usuario en sesion, loguear resultaod*/
            }
            catch (Exception ex)
            {
                ILog log = LogManager.GetLogger("Controller");
                log.Error(ex.Message + ex.StackTrace + "\\n");
            }
            return null;
        }
    }
}
