using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.util;
using asf.cms.bll;
using asf.cms.model;
using asf.cms.view.widgets.ui;

namespace asf.cms.controller
{
    public class GroupController:Controller
    {
        public GroupController(HttpContext context):base(context)
        {
            isAdmin = 1;
        }

        public void List()
        {
            List<GroupHasUserVO> users = Group.ListForGroup(0);

            this.Items.Add("selectedTab", "Group");
            this.Items.Add("users", users);
            this.ShowPage("group/List.aspx");
        }

        public void GetSectionsInGroup()
        {
            try
            {
                int id = int.Parse(Request["id"]);
                List<Section> sections = Section.GetSectionsByGroupId(id);

                GridInput gi = new GridInput();
                gi.Rows = new List<RowGrid>();
                
                foreach (Section s in sections)
                {
                    RowGrid row = new RowGrid();
                    row.Id = s.SectionId.ToString();
                    row.Cell = new List<string>();
                    row.Cell.Add(s.Title);
                    gi.Rows.Add(row);
                }
                gi.calculaInternos();
                string json = gi.toJSON(false);
                SendJSON(json);
            }
            catch(Exception ex)
            {
                SendJSONException(ex, "Error al obtener las secciones de grupo.");
            }
        }

        public void GetUsersInGroup()
        {
            int id = int.Parse(Request["id"]);
            GridInput gi = new GridInput();
            gi.Rows = new List<RowGrid>();
            List<GroupHasUserVO> lista = Group.ListForGroup(id);
            foreach (GroupHasUserVO g in lista)
            {
                /*string comando="";
                if (g.GroupHasUserId.GroupId == 0)
                    comando += "<a href='javascript:void(0)' onclick='addUserToGroup(\\\"" + g.GroupHasUserId.Username + "\\\"," + id + ");'><img  title='Agregar al grupo' src='../view/img/user_add.png'/></a>";
                else
                    comando += "Integrante <a href='Javascript:void(0)' onclick='deleteUserFromGroup(\\\"" + g.GroupHasUserId.Username + "\\\","+id+");' > <img  title='Eliminar' src='../view/img/user_delete.png'/></a>";*/
                if(g.GroupHasUserId.GroupId == id)
                {
                    RowGrid row = new RowGrid();
                    row.Id = g.GroupHasUserId.Username;
                    row.Cell = new List<string>();
                    row.Cell.Add(g.GroupHasUserId.Username);
                    row.Cell.Add(id.ToString());
                    gi.Rows.Add(row);
                }
            }
            gi.calculaInternos();
            string json = gi.toJSON(false);
            SendJSON(json);
        }
        public void GetAllGroups()
        {
            GridInput gi = new GridInput();
            gi.Rows = new List<RowGrid>();
            List<GroupVO> lista = Group.ListAll();
            foreach (GroupVO g in lista)
            {
                List<Section> sectionsInGroup = Section.GetSectionsByGroupId(g.Id);
                List<GroupHasUserVO> usersInGroup = Group.ListForGroup(g.Id);
                int userNumber = 0;
                foreach (GroupHasUserVO u in usersInGroup)
                    if (u.GroupHasUserId.GroupId == g.Id)
                        userNumber++;
                /*string comando = "<a href='javascript:void(0)' onclick='editUsers(" + g.Id + ")'><img  title='Editar usuarios' src='../view/img/usuarios.png'/></a>&nbsp;&nbsp;&nbsp; ";
                comando += "<a href='javascript:void(0)' onclick='editSections(" + g.Id + ")'><img  title='Editar secciones' src='../view/img/secciones.png'/></a>&nbsp;&nbsp;&nbsp;";
                comando += "<a href='javascript:void(0)' onclick='deleteGroup(" + g.Id + ")'><img  title='Eliminar Grupo' src='../view/img/delete.png'/></a>";*/
                RowGrid row = new RowGrid();
                row.Id = g.Id.ToString();
                row.Cell = new List<string>();
                row.Cell.Add(g.Name);
                row.Cell.Add(sectionsInGroup.Count.ToString());
                row.Cell.Add(userNumber.ToString());
                gi.Rows.Add(row);
            }
            gi.calculaInternos();
            string json = gi.toJSON();
            ShowPage("js/default.js");
            Response.Write(json);
        }
        public void AddUser()
        {
            int id = int.Parse(Request["id"]);
            string username = Request["username"];
            Group.AddUser(id, username);
            SendMessage(username + " se ha agregado al grupo");
        }
        public void DeleteUser()
        {
            int id = int.Parse(Request["id"]);
            string username = Request["username"];
            Group.DeleteUser(id, username);
            SendMessage(username + " se ha eliminado del grupo");
        }
        public void SetUsers()
        {
            try
            {
                int groupId = int.Parse(Request["group_id"]);
                List<string> curIds = new List<string>(),
                newIds = new List<string>();
                short idx = -1;

                string currentUser;

                if(!string.IsNullOrEmpty(Request["groupUsers[]"])) {
                    string[] tmp = Request["groupUsers[]"].Split(',');
                    foreach(string t in tmp)
                        newIds.Add(t.Trim());
                } else {
                    do
                    {
                        idx++;
                        currentUser = "groupUsers[" + idx.ToString() + "]";
                        if (!string.IsNullOrEmpty(Request[currentUser]))
                        {
                            newIds.Add(Request[currentUser]);
                        }
                    } while (!string.IsNullOrEmpty(Request[currentUser]));
                }

                List<GroupHasUserVO> usrGp = Group.ListForGroup(groupId);
                usrGp.ForEach(delegate (GroupHasUserVO ug) { if (ug.GroupHasUserId.GroupId == groupId) curIds.Add(ug.GroupHasUserId.Username); });

                curIds.Sort();
                newIds.Sort();

                List<string> toAdd = Except(newIds, curIds),
                    toDel = Except(curIds, newIds);

                foreach (string add in toAdd)
                    Group.AddUser(groupId, add);
                foreach (string del in toDel)
                    Group.DeleteUser(groupId, del);

                SendJSON("{ \"error\": false, \"msg\": \"Las secciones de grupo se han actualizado.\" }");
            }
            catch (Exception ex)
            {
                SendJSONException(ex, "Error al ingresar secciones de grupo.");
            }
        }
        public void Sections()
        {
            int groupId = int.Parse(Request["id"]);
            SectionTree st = new SectionTree();
            SectionTreeNode stn = st.getTree();
            List<Section> sectionsInGroup = Section.GetSectionsByGroupId(groupId);
            this.Items.Add("sectionTreeOptions", stn.ToOptions(1));
            this.Items.Add("sections", sectionsInGroup);
            ShowPage("group/Sections.aspx");
        }
        public void AddSection()
        {
            int groupId = int.Parse(Request["group_id"]);
            int sectionId = int.Parse(Request["section_id"]);
            Group.AddSection(groupId, sectionId);
            SendMessage("La seccion se ha agregado al grupo");
        }
        public void DeleteSection()
        {
            int groupId = int.Parse(Request["group_id"]);
            int sectionId = int.Parse(Request["section_id"]);
            Group.DeleteSection(groupId, sectionId);
            SendMessage("La seccion se ha eliminado del grupo");
        }
        public void SetSections()
        {
            try
            {
                int groupId = int.Parse(Request["group_id"]);
                List<int> curIds = new List<int>(),
                newIds = new List<int>();

                string[] tmp = new string[0];
                if (!string.IsNullOrEmpty(Request["sections"]))
                    tmp = Request["sections"].Trim().Split(',');
                for (int i = 0; i < tmp.Length; i++)
                    newIds.Add(int.Parse(tmp[i]));
                
                Section.GetSectionsByGroupId(groupId).ForEach(delegate(Section s) {
                    curIds.Add(s.SectionId);
                });

                curIds.Sort();
                newIds.Sort();

                List<int> toAdd = Except(newIds, curIds),
                    toDel = Except(curIds, newIds);

                foreach (int add in toAdd)
                    Group.AddSection(groupId, add);
                foreach (int del in toDel)
                    Group.DeleteSection(groupId, del);

                SendJSON("{ \"error\": false, \"msg\": \"Las secciones de grupo se han actualizado.\" }");
            }
            catch(Exception ex)
            {
                SendJSONException(ex, "Error al ingresar secciones de grupo.");
            }
        }
        public void Insert()
        {
            try
            {
                GroupVO gvo = new GroupVO();
                gvo.Name = Request["name"];
                if (Group.Insert(gvo))
                    SendJSON("{ \"error\": false, \"msg\": \"Se ha creado el grupo.\" }");
                else
                    SendJSON("{ \"error\": true, \"msg\": \"Error al crear el grupo.\" }");
            }
            catch (Exception ex)
            {
                SendJSONException(ex, "Error al crear el grupo.");
            }
        }
        public void Update()
        {
            GroupVO gvo = new GroupVO();
            gvo.Id = int.Parse(Request["id"]);
            gvo.Name = Request["name"];
            Group.Update(gvo);
            SendMessage("Los datos han sido guardados");
        }
        public void Delete()
        {
            try
            {
                int groupId = int.Parse(Request["groupId"]);
                if (Group.Delete(groupId))
                    SendMessage("{ \"error\": false, \"msg\": \"Se ha eliminado el grupo.\" }");
                else
                    SendJSON("{ \"error\": true, \"msg\": \"Hubo un problema al eliminar el grupo.\" }");
            }
            catch (Exception ex)
            {
                SendJSONException(ex, "Error al eliminar grupo.");
            }
        }

        protected List<T> Except<T>(List<T> left, List<T> right)
        {
            List<T> diff = new List<T>();
            foreach(T c in left)
            {
                if (!right.Contains(c))
                    diff.Add(c);
            }
            return diff;
        }
    }
}
