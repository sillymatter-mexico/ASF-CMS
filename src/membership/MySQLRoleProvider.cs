using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration.Provider;
using System.Web.Security;
using asf.cms.dal;
using asf.cms.model;

namespace asf.cms.membership
{
    public class MySQLRoleProvider:RoleProvider
    {
        public MySQLRoleProvider()
        {
        }

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(name, config);
        }

        public override string ApplicationName
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string[] GetRolesForUser(string username)
        {
            UserDAL udal = new UserDAL();
            UserVO u=udal.GetById(username);
            return new string[] { u.Type };
        }

        public override void CreateRole(string roleName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool RoleExists(string roleName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string[] GetAllRoles()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new Exception("The method or operation is not implemented.");
        }


       
    }
}
