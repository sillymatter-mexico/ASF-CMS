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

namespace asf.cms.view
{
    public partial class Index : System.Web.UI.Page
    {
        ILog log = LogManager.GetLogger("Controller");
        protected void Page_Load(object sender, EventArgs e)
        {

            string metodo = (String)Context.Items["metodo"];
            string controller = (String)Context.Items["controller"];
            log.Info("URL PARSEADA, dirigiendose a clase: " + controller + "Controller, metodo=" + metodo);

            if (metodo != null && metodo.EndsWith(".aspx"))
            {
                log.Warn("aspx detectado, mostrandolo tal cual");
                return;
            }
            if (String.IsNullOrEmpty(controller.Trim()) || String.IsNullOrEmpty(metodo.Trim()))
            {
                log.Warn("no se especifico controller, redireccionando a Default/Index");
                Response.Redirect("~/Default/Index");
                return;
            }

            Type classType = Type.GetType("asf.cms.controller." + controller + "Controller", false);

            object obj = null;
            if (classType == null)
            {
                log.Warn("no se encontro clase asf.cms.controller." + controller + "Controller, dirigiendose a default/notFound");
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
                log.Debug("Invocando controlador: " + (c != null ? obj.GetType().ToString() : "-- Sin Controller --") + "." + (mi != null ? mi.Name : "-- Sin MethodInfo --"));
                mi.Invoke(c, null);

            }
            catch (UnauthorizedAccessException uaex)
            {
                log.Warn("No hay sesion activa, dirigiendose a login/default");

                Response.Redirect("~/Login/Default");

            }
            catch (Exception ex)
            {
                log.Error(ex.Message + ex.StackTrace.ToString() + "\\n");
                if (ex.InnerException != null) log.Error("--- INNER:" + ex.InnerException.Message + ex.InnerException.StackTrace.ToString());
                log.Error("dirigiendose a default/notfound");
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
 