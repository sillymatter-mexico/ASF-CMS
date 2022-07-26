using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.util;
using asf.cms.bll;
using asf.cms.model;
namespace asf.cms.controller
{
    public class UserController:Controller
    {
        public UserController(HttpContext context):base(context)
        {
            isAdmin = 1;
        }

        public void List()
        {
            this.Items.Add("selectedTab", "User");
            this.ShowPage("user/List.aspx");
        }

        public void Update()
        {
            try
            {
                string username = Request["username"].Trim();
                string oldUsername = Request["oldUsername"].Trim();

                User u = User.GetUser(oldUsername);

                if(oldUsername != username)
                {
                    if (!u.Delete())
                    {
                        SendJSON("{\"error\":true, \"msg\": \"Error al actualizar usuario. No se puede modificar el nombre.\", \"usr\": \"" + u.user.Username + "\" }");
                    } 
                
                
                }

                u.user.Username = username;

                if (!String.IsNullOrEmpty(Request["password"].Trim()))
                    u.user.Password = Encoder.ToMD5(Request["password"].Trim());
                u.user.Type = Request["type"];
                u.user.Active = !String.IsNullOrEmpty(Request["active"]);
                if (u.Update())
                    SendJSON("{ \"error\":false, \"msg\": \"El usuario ha sido actualizado\", \"usr\": \"" + u.user.Username + "\" }");
                else
                    SendJSON("{\"error\":true, \"msg\": \"Error al actualizar usuario.\", \"usr\": \"" + u.user.Username + "\" }");
            }
            catch (Exception ex)
            {
                SendJSONException(ex, "Error al actualizar usuario.");
            }
        }

        public void Insert()
        {
            try
            {
                User u = new User();
                u.user.Username = Request["username"].Trim();
                u.user.Password = Encoder.ToMD5(Request["password"].Trim());
                u.user.Type = Request["type"];
                u.user.Active = !String.IsNullOrEmpty(Request["active"]);

                if(u.Insert())
                {
                    SendJSON("{ \"error\": false, \"msg\": \"El usuario ha sido creado.\", \"usr\": \"" + u.user.Username + "\" }");
                }
                else
                {
                    SendJSON("{ \"error\": true, \"msg\": \"Error al crear usuario.\", \"usr\": \"" + u.user.Username + "\" }");
                }
            }
            catch (Exception ex)
            {
                SendJSONException(ex, "Error al crear nuevo usuario.");
            }
        }

        public void GetAllUsers()
        {
            try
            {
                GridInput gi = new GridInput();
                gi.Rows = new List<RowGrid>();
                List<UserVO> lista = User.ListAll();
                foreach (UserVO u in lista)
                {
                    //String imgStatus = "../view/img/status1.png";
                    RowGrid row = new RowGrid();
                    row.Id = u.Username;
                    row.Cell = new List<string>();
                    row.Cell.Add(u.Username);
                    row.Cell.Add(u.Type);
                    row.Cell.Add(u.Active.ToString().ToLower());
                    /*if (!u.Active)
                        imgStatus = "../view/img/status3.png";
                    row.Cell.Add("<img src='" + imgStatus + "' title='" + u.Active + "'/>");*/
                    gi.Rows.Add(row);
                }
                gi.calculaInternos();
                string json = gi.toJSON(false);
                SendJSON(json);
            }
            catch (Exception ex)
            {
                SendJSONException(ex, "Error al listar usuarios");
            }
        }
    }
}
