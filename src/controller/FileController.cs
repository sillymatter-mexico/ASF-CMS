using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.bll;
using asf.cms.model;
using asf.cms.dal;
using asf.cms.util;

namespace asf.cms.controller
{
    public class FileController : Controller
    {
        public FileController(HttpContext context)
            : base(context)
        {
            isAdmin = 1;
        }
        public void ListByPublication()
        {
            try
            {
                int publicationId = int.Parse(Request["publicationId"]);
                GridInput gi = new GridInput();
                gi.Rows = new List<RowGrid>();

                List<FileVO> lista = Publication.GetFiles(publicationId);
                foreach (FileVO f in lista)
                {
                    string cmd = "<a href='javascript:void(0)' onclick='toggleMain(" + f.Id + ");'><img src='../view/img/" + (f.IsMain == 1 ? "main.png" : "status3.png") + "' alt='Principal' title='Principal'/></a>";

                    RowGrid row = new RowGrid();
                    row.Id = f.Id.ToString();
                    row.Cell = new List<string>();
                    row.Cell.Add(f.IsMain == 1 ? "true" : "false");
                    row.Cell.Add(f.Name);
                    row.Cell.Add(f.Mime);
                    row.Cell.Add(f.Title);
                    gi.Rows.Add(row);
                }
                gi.calculaInternos();
                SendJSON(gi.toJSON(false));
            }
            catch (Exception ex)
            {
                SendJSON("{ \"error\": true, \"msg\": \"" + ex.Message + "\", \"details\":  \"" + (ex.InnerException != null ? ex.InnerException.Message : "" ) + "\" }");
            }
        }

        public void UpdateTitle()
        {
            try
            {
                int fileId = int.Parse(Request["fileId"]);
                string title = Request["title"];
                if(PublicationFile.UpdateTitle(fileId, title) > 0)
                {
                    SendJSON("{ \"error\": false, \"msg\": \"Se ha actualizado el título del archivo.\" }");
                }
                else
                {
                    SendJSON("{ \"error\": true, \"msg\": \"Error al actualizar el titulo del archivo.\" }");
                }
                SendMessage("");
            }
            catch (Exception ex)
            {
                SendJSON("{ \"error\": true, \"msg\": \"" + ex.Message + "\", \"details\":  \"" + (ex.InnerException != null ? ex.InnerException.Message : "") + "\" }");
            }
        }

        /*public void UpdateTitles()
        {
            try
            {

            }
            catch(Exception ex)
            {
                SendJSONException(ex);
            }
        }*/

        public void ToggleMain()
        {
            try
            {
                int publicationId = int.Parse(Request["publicationId"]),
                    fileId = int.Parse(Request["fileId"]);
                if(PublicationFile.SetMain(publicationId, fileId))
                    SendJSON("{\"error\": false, \"msg\":\"Se ha cambiado el archivo principal.\"}");
                else
                    SendJSON("{\"error\": true, \"msg\":\"Error al cambiar el archivo principal.\"}");
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger("FileController").Error("Error al cambiar el archivo principal de la publicación.", ex);
                SendJSON("{\"error\": true, \"msg\":\"" + ex.Message + "\", \"details\": \"" + (ex.InnerException != null ? ex.InnerException.Message : "") + "\" }");
            }
        }
    }
}
