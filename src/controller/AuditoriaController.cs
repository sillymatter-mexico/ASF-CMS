using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.bll;
using asf.cms.model;
using asf.cms.util;
using System.IO;
using asf.cms.dal;
namespace asf.cms.controller
{
    public class AuditoriaController:Controller
    {
        public AuditoriaController(HttpContext context)
            : base(context)
        {
        }
        public void Search()
        {
            ShowPage("audit/Search.aspx");
        }
        public void PerformSearch()
        {
            System.Threading.Thread.Sleep(2000);
            string anio = String.IsNullOrEmpty(Request["anio"]) ? "" : Request["anio"];
            string tipo = String.IsNullOrEmpty(Request["tipo"]) ? "" : Request["tipo"];
            string ente = String.IsNullOrEmpty(Request["nombreEnte"]) ? "" : Request["nombreEnte"];
            string sector = String.IsNullOrEmpty(Request["sector"]) ? "" : Request["sector"];
            string titulo = String.IsNullOrEmpty(Request["titulo"]) ? "" : Request["titulo"];
            string numero = String.IsNullOrEmpty(Request["numero"]) ? "" : Request["numero"];
            string grupoFuncionalId = String.IsNullOrEmpty(Request["grupoFuncionalId"]) ? "" : Request["grupoFuncionalId"];
            AuditFinder af = new AuditFinder();
            af.PerformSearch(anio.Trim(), tipo.Trim(), ente.Trim(), sector.Trim(), titulo.Trim(),grupoFuncionalId.Trim(),numero.Trim());
            List<IJsonable> results = new List<IJsonable>();
            af.results.ForEach(delegate(AuditResultVO r)
            {
                results.Add(r);
            });
            string json = "{";
            json+="\"results\":"+ Encoder.ListToJson(results);
            json += ", " + "\"anios\":" + Encoder.SortDictionaryToJson(af.Years);
            json += ", " + "\"tipos\":" + Encoder.SortDictionaryToJson(af.AuditTypes);
            json += ", " +"\"entes\":"+Encoder.DictionaryToJson(af.AuditEntitys);
            json += ", " + "\"sectores\":" + Encoder.SortDictionaryToJson(af.Sectors);
            json += ", " + "\"titulos\":" + Encoder.DictionaryToJson(af.Titles);
            json += ", " + "\"grupos\":" + Encoder.DictionaryToJson(af.FunctionalGroups);
            json += "}";
            SendJSON(json);
        }
   
        public void Insert()
        {
            AuditLoader al = new AuditLoader();
            al.LoadAudit(int.Parse(Request["year"]));
            SendMessage("La operacion se realizo con exito");
        }
        public void Cancel()
        {
            AuditDAL adal = new AuditDAL();
            adal.DeletePreload();
        }
        public void Load()
        {
            int Year = int.Parse(Request["year"]);
            AuditLoader af = new AuditLoader();
            string filePath = Request["auditFile"];
            string directoryPath = Request["auditDirectory"];
            if (String.IsNullOrEmpty(directoryPath) || directoryPath.Trim() == "")
            {
                SendMessage("Especifique el directorio de las auditorias");
                return;
            }
            if (String.IsNullOrEmpty(filePath) || filePath.Trim() == "")
            {
                SendMessage("Especifique un archivo de auditorias");
                return;
            }
            filePath=Context.Server.MapPath(filePath);
            if (!File.Exists(filePath))
            {
                SendMessage("No se encontro el archivo de auditorias");
                return;
            }
            directoryPath = Context.Server.MapPath(directoryPath);
            if (!Directory.Exists(directoryPath))
            {
                SendMessage("No se encontro el directorio de las auditorias");
                return;
            }
            if (!af.LoadPreloadAudit(filePath, Year))
            {
                SendMessage(af.message);
                return;
            }
            if(!af.LoadPreloadAuditFiles(directoryPath,Year))
            {
                SendMessage(af.message);
                return;
            }
        }
        public void Delete()
        {

            int auditReportId = int.Parse(Request["auditReportId"]);
            AuditLoader al = new AuditLoader();
            if (al.DeleteCurrentAudits(auditReportId))
            {
                SendMessage("Se han eliminado todas las auditorias de este informe");
                return;
            }

            SendMessage("Ocurrio un error al realizar la operacion");

        }
        public void PreloadReport()
        {
            int Year = int.Parse(Request["year"]);
            this.Items.Add("selectedTab", "Audit");
            this.Items.Add("year", Year);
            ShowPage("audit/PreloadReport.aspx");
        }
        public void ShowReportList()
        {
            AuditReport ar = new AuditReport();
            this.Items.Add("list", ar.GetDisplayableList());
            ShowPage("audit/listReports.aspx");
        }

        public void GetPreloadInsertableEntities()
        {
            int year = int.Parse(Request["year"]);
            AuditEntityDAL aedal = new AuditEntityDAL();
            List<AuditEntityVO> lista = new List<AuditEntityVO>(aedal.GetInsertableEntitiesFromPreload(year));
            int i = 0;
            GridInput gi = new GridInput();
            gi.Rows = new List<RowGrid>();

            foreach (AuditEntityVO ae in lista)
            {
                RowGrid row = new RowGrid();
                row.Id = i.ToString();
                row.Cell = new List<string>();
                row.Cell.Add(ae.EntityKey);
                row.Cell.Add(ae.DepKey);
                row.Cell.Add(ae.TypeKey);
                row.Cell.Add(ae.ShortName);
                row.Cell.Add(ae.Name.Replace("\"", "\\\""));
                gi.Rows.Add(row);
            }
            gi.calculaInternos();
            string json = gi.toJSON();
            ShowPage("js/default.js");
            Response.Write(json);
        }
        public void GetPreloadInsertableSectors()
        {
            int year = int.Parse(Request["year"]);
            AuditSectorDAL asdal = new AuditSectorDAL();
            List<AuditSectorVO> lista = new List<AuditSectorVO>(asdal.GetInsertableSectorsFromPreload(year));
            int i = 0;
            GridInput gi = new GridInput();
            gi.Rows = new List<RowGrid>();

            foreach (AuditSectorVO asvo in lista)
            {
                RowGrid row = new RowGrid();
                row.Id = i.ToString();
                row.Cell = new List<string>();
                row.Cell.Add(asvo.AsfKey);
                row.Cell.Add(asvo.Name);
                gi.Rows.Add(row);
            }
            gi.calculaInternos();
            string json = gi.toJSON();
            ShowPage("js/default.js");
            Response.Write(json);
        }
        public void GetPreloadInsertableGroups()
        {
            int year = int.Parse(Request["year"]);
            GrupoFuncionalDAL gfdal = new GrupoFuncionalDAL();
            List<GrupoFuncionalVO> lista = new List<GrupoFuncionalVO>(gfdal.GetInsertableGroupsFromPreload(year));
            int i = 0;
            GridInput gi = new GridInput();
            gi.Rows = new List<RowGrid>();

            foreach (GrupoFuncionalVO gfvo in lista)
            {
                RowGrid row = new RowGrid();
                row.Id = i.ToString();
                row.Cell = new List<string>();
                row.Cell.Add(gfvo.Code);
                row.Cell.Add(gfvo.Name);
                gi.Rows.Add(row);
            }
            gi.calculaInternos();
            string json = gi.toJSON();
            ShowPage("js/default.js");
            Response.Write(json);
        }
        public void GetPreloadCompleteAuditList()
        {
            int year = int.Parse(Request["year"]);
            AuditDAL adal = new AuditDAL();
            List<AuditResultVO> lista = new List<AuditResultVO>(adal.GetAuditWithFileFromPreload(year));
            int i = 0;
            GridInput gi = new GridInput();
            gi.Rows = new List<RowGrid>();

            foreach (AuditResultVO ar in lista)
            {
                RowGrid row = new RowGrid();
                row.Id = i.ToString();
                row.Cell = new List<string>();
                row.Cell.Add(ar.TypeName.Replace("\"","\\\""));
                row.Cell.Add(ar.SectorName);
                row.Cell.Add(ar.Number.ToString());
                row.Cell.Add(ar.Title.Replace("\"", "\\\""));
                row.Cell.Add(ar.EntityName.Replace("\"", "\\\""));
                row.Cell.Add(ar.File);
                gi.Rows.Add(row);
            }
            gi.calculaInternos();
            string json = gi.toJSON();
            ShowPage("js/default.js");
            Response.Write(json);
        }
        public void GetPreloadOrphanAuditList()
        {
            int year = int.Parse(Request["year"]);
            AuditDAL adal = new AuditDAL();
            List<AuditResultVO> lista = new List<AuditResultVO>(adal.GetAuditWithoutFileFromPreload(year));
            int i = 0;
            GridInput gi = new GridInput();
            gi.Rows = new List<RowGrid>();

            foreach (AuditResultVO ar in lista)
            {
                RowGrid row = new RowGrid();
                row.Id = i.ToString();
                row.Cell = new List<string>();
                row.Cell.Add(ar.TypeName.Replace("\"", "\\\""));
                row.Cell.Add(ar.SectorName);
                row.Cell.Add(ar.Number.ToString());
                row.Cell.Add(ar.Title.Replace("\"", "\\\""));
                row.Cell.Add(ar.EntityName.Replace("\"", "\\\""));
                row.Cell.Add(ar.File);
                gi.Rows.Add(row);
            }
            gi.calculaInternos();
            string json = gi.toJSON();
            ShowPage("js/default.js");
            Response.Write(json);
        }

        public void GetPreladOrphanFileList()
        {
            int year = int.Parse(Request["year"]);
            AuditDAL adal = new AuditDAL();
            List<AuditResultVO> lista = new List<AuditResultVO>(adal.GetOrphanFilesFromPreload(year));
            int i = 0;
            GridInput gi = new GridInput();
            gi.Rows = new List<RowGrid>();

            foreach (AuditResultVO ar in lista)
            {
                RowGrid row = new RowGrid();
                row.Id = i.ToString();
                row.Cell = new List<string>();
                row.Cell.Add(ar.SectorName);
                row.Cell.Add(ar.File);
                row.Cell.Add(ar.Title.Replace("\"", "'").Replace("\n", " ").Replace("\r","").Replace(">","&gt").Replace("<","&lt"));   
                gi.Rows.Add(row);
            }
            gi.calculaInternos();
            string json = gi.toJSON();
            ShowPage("js/default.js");
            Response.Write(json);
        }

    }
}
