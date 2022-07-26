using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.bll;
using asf.cms.model;
namespace asf.cms.controller
{
    public class LoginController:Controller
    {
        public LoginController(HttpContext context):base(context)
        { 
        }
        public void Default()
        {
            ShowPage("Login.aspx");
        }
        public void Authenticate()
        {
            string login = Request["login"];
            string password = Request["password"];
            Login l = new Login();
            UserVO u=l.ValidateUser(login,password);
            if (u != null)
            {
                if (HttpContext.Current.Session["User"] == null)
                    HttpContext.Current.Session.Add("User", u);
                else
                    HttpContext.Current.Session["User"] = u;
                //Response.Redirect("~/SectionAdm/List");
                
                this.Items.Add("User", u);
                SectionAdmController sac = new SectionAdmController(HttpContext.Current);
                sac.List();
                
                //ShowPage("section/List.aspx");
            }
            else
            {
                this.Items.Add("isLogin", "1");
                this.Items.Add("msg", "Los datos no coinciden");
                ShowPage("Login.aspx");
            }
            //Response.Redirect("~/Login/Default");
        }
        public void Logout()
        {
            if (HttpContext.Current.Session["User"] != null)
                HttpContext.Current.Session["User"] = null;
            ShowPage("Login.aspx");
            //Response.Redirect("~/Login/Default");
        }
    }
}
