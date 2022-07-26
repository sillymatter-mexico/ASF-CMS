﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Browser;
using System.Net;
using System.IO;
/**
 * $Id: Page.xaml.cs 490 2008-10-21 16:04:35Z spocke $
 *
 * @package MCManagerCore
 * @author Moxiecode
 * @copyright Copyright © 2007, Moxiecode Systems AB, All rights reserved.
 */

using System.Collections.Generic;
using System.Threading;
using Moxiecode.Silverlight.Upload;

namespace Upload {
	/// <summary>
	///  Partial page class for the Silverlight page.
	/// </summary>
	public partial class Page : UserControl {
		#region private fields
		private Dictionary<string, FileReference> files;
		private int idCount = 0;
		private FileReference currentFile;
		#endregion

		/// <summary>
		///  Main constructor.
		/// </summary>
		/// <param name="init_params">Silverlight init params.</param>
		public Page(IDictionary<string, string> init_params) {
			InitializeComponent();
	
			HtmlPage.RegisterScriptableObject("Upload", this);

			MultiUpload.Invoke("setup", null);

			this.files = new Dictionary<string, FileReference>();
		}

		/// <summary>
		///  Reference to page level $.multiUpload script object.
		/// </summary>
		public ScriptObject MultiUpload {
			get { return ((ScriptObject) HtmlPage.Window.Eval("$.multiUpload")); }
		}

		/// <summary>
		///  Fires a specific event to the page level multi upload script.
		/// </summary>
		/// <param name="name">Event name to fire.</param>
		public void FireEvent(string name) {
			this.MultiUpload.Invoke("_fireEvent", new string[] {name});
		}

		/// <summary>
		///  Fires a specific event to the page level multi upload script.
		/// </summary>
		/// <param name="name">Event name to fire.</param>
		/// <param name="paramlist">Numerous parameters to send.</param>
		public void FireEvent(string name, params object[] paramlist) {
			List<object> args = new List<object>(paramlist);

			args.Insert(0, name);

			this.MultiUpload.Invoke("_fireEvent", args.ToArray());
		}

		[ScriptableMember]
		/// <summary>
		///  Opens the file dialog and lets the user select files.
		/// </summary>
		/// <param name="filter">Filter pattern to use.</param>
		public void SelectFiles(string filter) {
			OpenFileDialog dlg = new OpenFileDialog();

			try {
				dlg.Multiselect = true;
				dlg.Filter = filter;

				if ((bool) dlg.ShowDialog()) {
					foreach (FileInfo file in dlg.Files) {
						FileReference uploadFile = new FileReference("u" + this.idCount++, file);

						uploadFile.UploadChunkComplete += delegate(object sender, UploadEventArgs args) {
							FileReference evtFile = (FileReference) sender;

							this.FireEvent("slUploadChunkSuccessful", evtFile.Id, args.Chunk, args.Chunks, args.Response);
						};

						uploadFile.UploadComplete += delegate(object sender, UploadEventArgs args) {
							FileReference evtFile = (FileReference) sender;

							this.FireEvent("slUploadSuccessful", evtFile.Id, args.Response);
						};

						uploadFile.Error += delegate(object sender, ErrorEventArgs args) {
							FileReference evtFile = (FileReference) sender;

							this.FireEvent("slUploadChunkError", evtFile.Id, args.Chunk, args.Chunks, args.Message);
						};

						uploadFile.Progress += delegate(object sender, ProgressEventArgs args) {
							FileReference evtFile = (FileReference) sender;

							this.FireEvent("slUploadFileProgress", evtFile.Id, args.Loaded, args.Total);
						};

						this.FireEvent("slSelectFile", uploadFile.Id, uploadFile.Name, uploadFile.Size);
						this.files[uploadFile.Id] = uploadFile;
					}

					this.FireEvent("slSelectSuccessful");
				} else
					this.FireEvent("slSelectCancelled");
			} catch (Exception ex) {
				this.FireEvent("slSelectError", ex.Message);
			}
		}

		[ScriptableMember]
		/// <summary>
		///  Uploads a specific file by id to the specific url and using a chunks.
		/// </summary>
		/// <param name="id">File id to upload.</param>
		/// <param name="upload_url">Url to upload to.</param>
		/// <param name="chunk_size">Chunk size to use.</param>
		public void UploadFile(string id, string upload_url, int chunk_size) {
			if (this.files.ContainsKey(id)) {
				FileReference file = this.files[id];

				this.currentFile = file;
				file.Upload(upload_url, chunk_size);
			}
		}

		[ScriptableMember]
		/// <summary>
		///  Removes the specified file by id.
		/// </summary>
		/// <param name="id">File id to remove.</param>
		public void RemoveFile(string id) {
			if (this.files.ContainsKey(id))
				this.files[id] = null;
		}

		[ScriptableMember]
		/// <summary>
		///  Clears all files.
		/// </summary>
		public void ClearFiles() {
			this.files = new Dictionary<string, FileReference>();
		}

		[ScriptableMember]
		/// <summary>
		///  Cancels the current upload.
		/// </summary>
		public void CancelUpload() {
			if (this.currentFile != null)
				this.currentFile.CancelUpload();
		}

		/// <summary>
		///  Send debug message to firebug console.
		/// </summary>
		/// <param name="msg">Message to write.</param>
		private void Debug(string msg) {
			((ScriptObject) HtmlPage.Window.Eval("console")).Invoke("log", new string[] {msg});
		}
	}
}