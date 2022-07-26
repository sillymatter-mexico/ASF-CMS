using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.model;
using asf.cms.dal;
namespace asf.cms.bll
{
    public class Group
    {
        public static List<GroupVO> ListAll()
        {
            GroupDAL gdal = new GroupDAL();
            return new List<GroupVO>(gdal.GetAll());
        }
        public static List<GroupHasUserVO> ListForGroup(int groupId)
        {
            GroupHasUserDAL ghdal = new GroupHasUserDAL();
            return new List<GroupHasUserVO>(ghdal.listForGroup(groupId));

        }
        public static void AddUser(int groupId, String username)
        {
            GroupHasUserVO ghvo = new GroupHasUserVO();
            ghvo.GroupHasUserId = new GroupHasUserIdVO();
            ghvo.GroupHasUserId.GroupId = groupId;
            ghvo.GroupHasUserId.Username = username;
            GroupHasUserDAL ghdal = new GroupHasUserDAL();
            ghdal.Insert(ghvo);
        }

        public static void DeleteUser(int groupId, String username)
        {
            GroupHasUserVO ghvo = new GroupHasUserVO();
            ghvo.GroupHasUserId = new GroupHasUserIdVO();
            ghvo.GroupHasUserId.GroupId = groupId;
            ghvo.GroupHasUserId.Username = username;
            GroupHasUserDAL ghdal = new GroupHasUserDAL();
            ghdal.Delete(ghvo);
        }
        public static void AddSection(int groupId, int sectionId)
        {
            GroupHasSectionVO ghvo = new GroupHasSectionVO();
            ghvo.GroupHasSectionId = new GroupHasSectionIdVO();
            ghvo.GroupHasSectionId.GroupId = groupId;
            ghvo.GroupHasSectionId.SectionId = sectionId;
            GroupHasSectionDAL ghdal = new GroupHasSectionDAL();
            ghdal.Update(ghvo);
        }

        public static void DeleteSection(int groupId, int sectionId)
        {
            GroupHasSectionVO ghvo = new GroupHasSectionVO();
            ghvo.GroupHasSectionId = new GroupHasSectionIdVO();
            ghvo.GroupHasSectionId.GroupId = groupId;
            ghvo.GroupHasSectionId.SectionId = sectionId;
            GroupHasSectionDAL ghdal = new GroupHasSectionDAL();
            ghdal.Delete(ghvo);
        }
        public static bool Insert(GroupVO gvo)
        {
            GroupDAL gdal = new GroupDAL();
            gdal.Insert(gvo);
            return true;
        }
        public static bool Update(GroupVO gvo)
        {
            GroupDAL gdal = new GroupDAL();
            gdal.Update(gvo);
            return true;
        }
        public static bool Delete(int groupId)
        {
            GroupDAL gdal = new GroupDAL();
            gdal.Delete(groupId);
            return true;
 
        }
    }
}
