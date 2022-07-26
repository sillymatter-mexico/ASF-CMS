/*
 * Created by SharpDevelop.
 * User: spocke
 * Date: 2007-03-13
 * Time: 12:45
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using Moxiecode.Manager;
using Moxiecode.Manager.FileSystems;
using Moxiecode.Manager.Utils;
using asf.cms.bll;
//using asf.cms.bll;
namespace Moxiecode.Manager.Plugins.Old {
	/// <summary>
	///  This is a template plugin to be used to create new plugins. Rename all Template references below to your plugins name
	///  and implement the methods you need.
	/// </summary>
	public class AsfSupportPluginOld : Plugin {
        public AsfSupportPluginOld()
        {
		}

		/// <summary>
		///  Short name for the plugin, used in the authenticator config option for example
		///  so that you don't need to write the long name for it namespace.classname.
		/// </summary>
		public override string ShortName {
			get {
                return "AsfSupport";
			}
		}

		/// <summary>
		///  Gets called on a authenication request. This method should check sessions or simmilar to verify that the user has access to the backend.
		///  This method should return true if the current request is authenicated or false if it's not.
		/// </summary>
		/// <param name="man">ManagerEngine reference that the plugin is assigned to.</param>
		/// <returns>true/false if the user is authenticated</returns>
		public override bool OnAuthenticate(ManagerEngine man) {
			return false;
		}

		/// <summary>
		///  Gets called after any authenication is performed and verified.
		///  This method should return false if the execution chain is to be broken.
		/// </summary>
		/// <param name="man">ManagerEngine reference that the plugin is assigned to.</param>
		/// <returns>true/false if the execution of the event chain should continue execution.</returns>
		public override bool OnInit(ManagerEngine man) {
			ManagerConfig config = man.Config;
            HttpContext context = HttpContext.Current;
            if (context.Session["permalink"] != null )
            {
                if(!Directory.Exists(context.Server.MapPath("~/uploads/"+context.Session["permalink"])))
                    Directory.CreateDirectory(context.Server.MapPath("~/uploads/"+context.Session["permalink"]));
                //config["filesystem.rootpath"] = config["filesystem.rootpath"]+"/"+context.Session["permalink"];
                config["filesystem.path"] = config["filesystem.path"] + "/" + context.Session["permalink"].ToString();
                config["preview.wwwroot"] = context.Server.MapPath("~");
				//config["preview.urlprefix"] = "";
                
            }
            else
            {
				man.Logger.Error("No se ha creado el directorio para esta aplicacion");
            }
            return true;
		}

		/// <summary>
		///  Gets called before a file action occurs for example before a rename or copy.
		/// </summary>
		/// <param name="man">ManagerEngine reference that the plugin is assigned to.</param>
		/// <param name="action">File action type.</param>
		/// <param name="file1">File object 1 for example from in a copy operation.</param>
		/// <param name="file2">File object 2 for example to in a copy operation. Might be null in for example a delete.</param>
		/// <returns>true/false if the execution of the event chain should continue execution.</returns>
		public override bool OnBeforeFileAction(ManagerEngine man, FileAction action, IFile file1, IFile file2) {
			return true;
		}

		/// <summary>
		///  Gets called after a file action was perforem for example after a rename or copy.
		/// </summary>
		/// <param name="man">ManagerEngine reference that the plugin is assigned to.</param>
		/// <param name="action">File action type.</param>
		/// <param name="file1">File object 1 for example from in a copy operation.</param>
		/// <param name="file2">File object 2 for example to in a copy operation. Might be null in for example a delete.</param>
		/// <returns>true/false if the execution of the event chain should continue execution.</returns>
		public override bool OnFileAction(ManagerEngine man, FileAction action, IFile file1, IFile file2) {
            HttpContext context = HttpContext.Current;
            if (context.Session["permalink"] == null)
                throw new Exception("No se ha creado el directorio para esta aplicacion");           
            if (action == FileAction.Add)
            {
                string path = man.ToVisualPath(file1.AbsolutePath,man.Config["filesystem.roothpath"]);
                PublicationFile pf = new PublicationFile(context.Session["permalink"].ToString(), file1.Name, path);
                return pf.Insert();
            }
            if (action == FileAction.Copy)
            {
                string pathSrc = man.ToVisualPath(file1.AbsolutePath, man.Config["filesystem.roothpath"]);
                string pathTo = man.ToVisualPath(file2.AbsolutePath, man.Config["filesystem.roothpath"]);
                PublicationFile src = new PublicationFile(context.Session["permalink"].ToString(), file1.Name, pathSrc);
                PublicationFile to = new PublicationFile(context.Session["permalink"].ToString(), file2.Name, pathTo);
                return src.CopyTo(to);
            }
            if (action == FileAction.Delete)
            {
                string pathSrc = man.ToVisualPath(file1.AbsolutePath, man.Config["filesystem.roothpath"]);
                PublicationFile src = new PublicationFile(context.Session["permalink"].ToString(), file1.Name, pathSrc);
                return src.Delete();
            }
            if (action == FileAction.Rename)
            {

                JSONRpcCall call;
                if (context.Request["json_data"] != null)
                    call = JSON.ParseRPC(new System.IO.StringReader(context.Request["json_data"]));
                else
                    call = JSON.ParseRPC(new System.IO.StreamReader(context.Request.InputStream));
                string fromPath = (string)call.Args["frompath0"];
                int lastSlash = fromPath.LastIndexOf("/");
                int firstSlash = fromPath.IndexOf("/");
                
                lastSlash = lastSlash == -1 ? 0 : lastSlash+1;
                string fromFileName=fromPath.Substring(lastSlash,fromPath.Length-lastSlash);
                string path=fromPath.Substring(firstSlash,lastSlash-firstSlash);

                PublicationFile src = new PublicationFile(context.Session["permalink"].ToString(), fromFileName, path+fromFileName);
                PublicationFile to = new PublicationFile(context.Session["permalink"].ToString(), file2.Name, path+file2.Name);
                
                return src.RenameTo(to);
            }
            if (action == FileAction.RmDir)
            {
                //asf.cms.bll.PublicationFile src = new asf.cms.bll.PublicationFile(context.Session["permalink"].ToString(), file1.Name, file1.AbsolutePath);
                throw new Exception(action.ToString());
                //return src.DeleteDir();
            }
            if (action == FileAction.Update)
            {
                throw new Exception(action.ToString());
                //asf.cms.bll.PublicationFile src = new asf.cms.bll.PublicationFile(context.Session["permalink"].ToString(), file1.Name, file1.AbsolutePath);
                //return src.Update();
            }

            return true;
		}
        private string GetPath(string pathFM)
        {
            string path = "";
            int lastSlash = pathFM.LastIndexOf("/");
            int firstSlash = pathFM.IndexOf("/");

            path = pathFM.Substring(firstSlash+1, pathFM.Length - lastSlash+1);

            return path;
        }
		/// <summary>
		///  Gets called before a RPC command is handled.
		/// </summary>
		/// <param name="man">ManagerEngine reference that the plugin is assigned to.</param>
		/// <param name="cmd">RPC Command to be executed.</param>
		/// <param name="input">RPC input object data.</param>
		/// <returns>true/false if the execution of the event chain should continue execution.</returns>
		public override bool OnBeforeRPC(ManagerEngine man, string cmd, Hashtable input) {
			return true;
		}

		/// <summary>
		///  Gets executed when a RPC command is to be executed.
		/// </summary>
		/// <param name="man">ManagerEngine reference that the plugin is assigned to.</param>
		/// <param name="cmd">RPC Command to be executed.</param>
		/// <param name="input">RPC input object data.</param>
		/// <returns>true/false if the execution of the event chain should continue execution.</returns>
		public override object OnRPC(ManagerEngine man, string cmd, Hashtable input) {
			return null;
		}

		/// <summary>
		///  Gets called before data is streamed to client.
		/// </summary>
		/// <param name="man">ManagerEngine reference that the plugin is assigned to.</param>
		/// <param name="cmd">Stream command that is to be performed.</param>
		/// <param name="input">Array of input arguments.</param>
		/// <returns>true/false if the execution of the event chain should continue execution.</returns>
		public override bool OnBeforeStream(ManagerEngine man, string cmd, NameValueCollection input) {
			return true;
		}

		/// <summary>
		///  Gets called when data is streamed to client. This method should setup HTTP headers, content type
		///  etc and simply send out the binary data to the client and the return false ones that is done.
		/// </summary>
		/// <param name="man">ManagerEngine reference that the plugin is assigned to.</param>
		/// <param name="cmd">Stream command that is to be performed.</param>
		/// <param name="input">Array of input arguments.</param>
		/// <returns>true/false if the execution of the event chain should continue execution.</returns>
		public override bool OnStream(ManagerEngine man, string cmd, NameValueCollection input) {
			return true;
		}

		/// <summary>
		///  Gets called after data was streamed to client.
		/// </summary>
		/// <param name="man">ManagerEngine reference that the plugin is assigned to.</param>
		/// <param name="cmd">Stream command that is to was performed.</param>
		/// <param name="input">Array of input arguments.</param>
		/// <returns>true/false if the execution of the event chain should continue execution.</returns>
		public override bool OnAfterStream(ManagerEngine man, string cmd, NameValueCollection input) {
			return true;
		}

		/// <summary>
		///  Gets called before data is streamed/uploaded from client.
		/// </summary>
		/// <param name="man">ManagerEngine reference that the plugin is assigned to.</param>
		/// <param name="cmd">Upload command that is to be performed.</param>
		/// <param name="input">Array of input arguments.</param>
		/// <returns>true/false if the execution of the event chain should continue execution.</returns>
		public override bool OnBeforeUpload(ManagerEngine man, string cmd, NameValueCollection input) {
            return true;
		}

		/// <summary>
		///  Gets called when data is streamed/uploaded from client. This method should take care of
		///  any uploaded files and move them to the correct location.
		/// </summary>
		/// <param name="man">ManagerEngine reference that the plugin is assigned to.</param>
		/// <param name="cmd">Upload command that is to be performed.</param>
		/// <param name="input">Array of input arguments.</param>
		/// <returns>true/false if the execution of the event chain should continue execution.</returns>
		public override object OnUpload(ManagerEngine man, string cmd, NameValueCollection input) {
			return null;
		}

		/// <summary>
		///  Gets called before data is streamed/uploaded from client.
		/// </summary>
		/// <param name="man">ManagerEngine reference that the plugin is assigned to.</param>
		/// <param name="cmd">Upload command that is to was performed.</param>
		/// <param name="input">Array of input arguments.</param>
		/// <returns>true/false if the execution of the event chain should continue execution.</returns>
		public override bool OnAfterUpload(ManagerEngine man, string cmd, NameValueCollection input) {
			return true;
		}

		/// <summary>
		///  Gets called when custom data is to be added for a file custom data can for example be
		///  plugin specific name value items that should get added into a file listning.
		/// </summary>
		/// <param name="man">ManagerEngine reference that the plugin is assigned to.</param>
		/// <param name="file">File reference to add custom info/data to.</param>
		/// <param name="type">Where is the info needed for example list or info.</param>
		/// <param name="custom">Name/Value array to add custom items to.</param>
		/// <returns>true/false if the execution of the event chain should continue execution.</returns>
		public override bool OnCustomInfo(ManagerEngine man, IFile file, string type, Hashtable custom) {
			return true;
		}

		/// <summary>
		///  Gets called when the user selects a file and inserts it into TinyMCE or a form or similar.
		/// </summary>
		/// <param name="man">ManagerEngine reference that the plugin is assigned to.</param>
		/// <param name="file">Implementation of the BaseFile class that was inserted/returned to external system.</param>
		/// <returns>true/false if the execution of the event chain should continue execution.</returns>
		public override bool OnInsertFile(ManagerEngine man, IFile file) {
			return true;
		}
	}
}
