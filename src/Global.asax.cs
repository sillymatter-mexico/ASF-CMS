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

namespace asf.cms.web
{

    public class Global : System.Web.HttpApplication
    {

        private static List<string> directExtensions = new List<string>();

        protected void Application_Start(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            directExtensions.Add("swf");
        }

        protected void Session_Start(object sender, EventArgs e)
        {
           
           
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            string controller = "";
            string metodo = "";
            string[] partes = Request.Url.Segments;
            if (partes.Length >= 2)
            {
                controller = partes[partes.Length - 2].Replace("/", "");
                metodo = partes[partes.Length - 1].Replace("/", "");
            }
            if (metodo.EndsWith(".aspx"))
                return;
            Context.Items.Add("controller", controller);
            Context.Items.Add("metodo", metodo);

            Context.RewritePath("~/view/Index.aspx");
        }
       /* protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //Context.Items.Clear();
            string controller = "";
            string metodo = "";
            string[] partes = Request.Url.Segments;
            if (partes.Length >= 2)
            {
                controller = partes[partes.Length - 2].Replace("/", "");
                metodo = partes[partes.Length - 1].Replace("/", "");
            }

            if (!DirectExtension(metodo))
            {

                if (metodo.EndsWith(".aspx"))
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
                    mi = classType.GetMethod(metodo);
                    mi.Invoke(obj, null);

                }
                catch (Exception ex)
                {
                    ILog log = LogManager.GetLogger("Controller");
                    log.Error(ex.Message + ex.StackTrace + "\\n");
                    if (ex.InnerException != null) log.Error(ex.InnerException.Message + ex.InnerException.StackTrace);
                    Response.Redirect("~/Default/NotFound");
                }
            }
        }
        */
        private bool DirectExtension(string method) 
        { 
            string ext = PathUtils.GetExtension(method);
            return directExtensions.Contains(ext);
        }


        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {


        }

        protected void Application_AcquireRequest(object sender, EventArgs e)
        {
//            string controller = Context.Items["controllerClass"].ToString();
  //          string metodo = Context.Items["metodoMethod"].ToString();
 /*           string controller = Request["controllerClass"].ToString();
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
             }         
            */

        }
       
        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}