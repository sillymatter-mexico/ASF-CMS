using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.model;
using asf.cms.dal;
namespace asf.cms.bll
{
    public class User
    {
        public UserVO user = new UserVO();
        public int groupId;
        public User()
        { }
        private User(UserVO u)
        {
            user = u;
        }
        public static List<UserVO>  ListAll()
        {
            UserDAL udal = new UserDAL();
            return new List<UserVO>(udal.GetAll());
        }
        public static User GetUser(string username)
        {
            UserDAL udal = new UserDAL();
            User u = new User();
            u.user = udal.GetById(username);
            return u;
        }
       /* public static List<User> GetByGroup(int groupId)
        {
            UserDAL udal = new UserDAL();
            List<UserVO> inGroup = new List<UserVO>(udal.GetInGroupId(groupId));
            List<UserVO> notInGroup = new List<UserVO>(udal.GetNotInGroupId(groupId));
            inGroup.ForEach(delegate 

        }*/

        public bool Update()
        {
            UserDAL udal = new UserDAL();
            udal.Update(this.user);
            return true;
        }

        public bool Insert()
        {
            UserDAL udal = new UserDAL();
            UserVO u=udal.GetById(user.Username);
            if (u != null)
                throw new Exception("Ya existe otro usuario con este login");
            udal.Insert(this.user);
            return true;
        }

        public bool Delete()
        {
            UserDAL udal = new UserDAL();
            UserVO u = udal.GetById(user.Username);
            if (u != null)
            {
                udal.Delete(u);
                return true;
            }
            return false;
        }
    }
}
