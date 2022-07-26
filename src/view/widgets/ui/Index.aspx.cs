using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Reflection;
using asf.cms.controller;
using System.Text.RegularExpressions;
using log4net;
using Moxiecode.Manager;
using Moxiecode.Manager.FileSystems;
using Moxiecode.Manager.Utils;

namespace asf.cms.view.widgets.ui
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string metodo = (String)Context.Items["metodo"];
            string controller = (String)Context.Items["controller"];

            if (metodo!=null&&metodo.EndsWith(".aspx"))
                return;

            if (String.IsNullOrEmpty(controller.Trim()) || String.IsNullOrEmpty(metodo.Trim()))
            {
                Response.Redirect("~/Default/Index");
                return;
            }

            Type classType = Type.GetType("asf.cms.controller." + controller + "Controller", false);

            object obj = null;
            if (classType == null)
            {
                Context.Items.Clear();
                Response.Redirect("~/Default/NotFound");
                return;
            }

            obj = Activator.CreateInstance(classType, new object[] { Context });
            MethodInfo mi = null;
            try
            {
                Controller c = (Controller)obj;
                if (!c.isValidReq())
                    return;
                mi = classType.GetMethod(metodo);
                mi.Invoke(obj, null);

            }
            catch (UnauthorizedAccessException uaex)
            {
                Response.Redirect("~/Login/Default");

            }
            catch (Exception ex)
            {
                ILog log = LogManager.GetLogger("Controller");
                log.Error(ex.Message + ex.StackTrace + "\\n");
                if (ex.InnerException != null) log.Error(ex.InnerException.Message + ex.InnerException.StackTrace);
                Response.Redirect("~/Default/NotFound");
            }


            /*string controller = Request["controllerClass"].ToString();
            string metodo = Request["metodoMethod"].ToString();
            if (String.IsNullOrEmpty(controller.Trim()) || String.IsNullOrEmpty(metodo.Trim()))
            {
                Response.Redirect("~/Default/Index");
                return;
            }

            Type classType = Type.GetType("asf.cms.controller." + controller + "Controller", false);

            object obj = null;
            if (classType == null)
            {
                Context.Items.Clear();
                Response.Redirect("~/Default/NotFound");
                return;
            }

            obj = Activator.CreateInstance(classType, new object[] { Context });

            MethodInfo mi = null;
            try
            {
                mi = classType.GetMethod(metodo);
                mi.Invoke(obj, null);
            }
            catch (Exception ex)
            {
                ILog log = LogManager.GetLogger("Controller");
                log.Error(ex.Message + ex.StackTrace + "\\n");
                if (ex.InnerException != null) log.Error(ex.InnerException.Message + ex.InnerException.StackTrace);
                Response.Redirect("~/Default/NotFound");
            }         */


          
        }
    }
}
