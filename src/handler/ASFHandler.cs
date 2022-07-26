using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using System.Reflection;
using asf.cms.controller;
using System.Text.RegularExpressions;
using log4net;

namespace asf.cms.handler
{
    public class ASFHandler: IHttpHandler
	{
		public ASFHandler()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		#region Implementation of IHttpHandler
		public void ProcessRequest(System.Web.HttpContext Context)
		{
            Context.Items.Clear();
            string controller = "";
            string metodo = "";
            string[] partes = Context.Request.Url.Segments;
            if (partes.Length >= 2)
            {
                controller = partes[partes.Length - 2].Replace("/", "");
                metodo = partes[partes.Length - 1].Replace("/", "");
            }
            if (String.IsNullOrEmpty(controller.Trim()) || String.IsNullOrEmpty(metodo.Trim()))
            {
                Context.Response.Redirect("~/Default/Index");
                return;
            }

            Type classType = Type.GetType("asf.cms.controller." + controller + "Controller", false);

            object obj = null;
            if (classType == null)
            {
                Context.Items.Clear();
                Context.Response.Redirect("~/Default/NotFound");
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
                Context.Response.Redirect("~/Default/NotFound");
            }
        }

		public bool IsReusable
		{
			get
			{
				return true;
			}
		}
		#endregion
	}
}
