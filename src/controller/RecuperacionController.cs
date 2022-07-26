using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.bll;
using asf.cms.model;
using asf.cms.util;
using asf.cms.dal;

namespace asf.cms.controller
{
    public class RecuperacionController:Controller
    {
        public RecuperacionController(HttpContext context): base(context)
        {
        }
        public void List()
        {
            this.Items.Add("selectedTab", "Recuperacion");
            ShowPage("recuperacion/List.aspx");
        }

        public void Edit()
        {
            int id=0;
            RecuperacionDAL rdal = new RecuperacionDAL();
            RecuperacionVO r = new RecuperacionVO();

            if (int.TryParse(Request["id"], out id))
                r = rdal.GetById(id);

            if (id == 0 || r == null || r.Id == 0)
            {
                r = new RecuperacionVO();
                this.Items.Add("error", "No se encontró el informe de recuperación, capture uno nuevo aquí");
            }
            this.Items.Add("selectedTab", "Recuperacion");
            this.Items.Add("Recuperacion", r);
            ShowPage("recuperacion/Edit.aspx");
        }
        public void New()
        {
            this.Items.Add("Recuperacion", new RecuperacionVO());
            this.Items.Add("selectedTab", "Recuperacion");
            ShowPage("recuperacion/Edit.aspx");
        }
        public void getRecuperacionData()
        {
            String directoryPath = Request["directoryPath"];
            directoryPath=this.Context.Server.MapPath(directoryPath);
            RecuperacionTree r = new RecuperacionTree();
            SendJSON("["+r.getTree(directoryPath)+"]");
        }
        public void Save()
        {
            RecuperacionVO rvo = new RecuperacionVO();
            ResponseMessage rm = new ResponseMessage();

            string id = Request["id"];
            rvo.Active = bool.Parse(Request["active"]);
            rvo.DirectoryPath = Request["directoryPath"];
            rvo.Title = Request["title"];
            rvo.Files = Request["files"];

            RecuperacionDAL rdal = new RecuperacionDAL();
            if (String.IsNullOrEmpty(id)||id=="0")
            {
                rvo.CreationDate = DateTime.Now;
                rvo.Active = true;
                rvo=rdal.Insert(rvo);
            }
            else 
            {
                rvo.Id = int.Parse(Request["id"]);
                rvo.CreationDate = DateTime.Parse(Request["creationDate"]);
                rdal.Update(rvo);

            }
            rm.isError = false;
            rm.Data = rvo.Id.ToString();
            rm.Message = "Se han guardado los cambios";
            SendJSON(rm.ToJson());
            
        }
        public void Delete()
        {
            int id=0;
            if (!int.TryParse(Request["id"], out id))
            {
                SendMessage("No se encuentra el registro que está intentando eliminar");
                return;
            }

            RecuperacionDAL rdal = new RecuperacionDAL();
            rdal.InactivaById(id);
            SendMessage("Se ha eliminado el registro");
        }
        public void GetAllActive()
        {
            GridInput gi = new GridInput();
            gi.Rows = new List<RowGrid>();

            UserVO uvo = (UserVO)Context.Session["user"];
            if (uvo.Type != "ADMIN" && uvo.Type != "RECUPERACIONES")
                return;
            Recuperacion r = new Recuperacion();
            List<RecuperacionVO> lista= r.GetList();

            foreach (RecuperacionVO rec in lista)
            {
                string imgStatus = "status3.png";
                string strStatus = "Inactivo";
                if (rec.Active)
                {
                    strStatus = "Activo";
                    imgStatus = "status1.png";
                }
                imgStatus = "<img src='../view/img/" + imgStatus + "' alt='" + strStatus + "' title='" + strStatus + "'/>";
                RowGrid row = new RowGrid();
                row.Id = rec.Id.ToString();
                row.Cell = new List<string>();
                row.Cell.Add(imgStatus);
                row.Cell.Add(rec.Id.ToString());
                row.Cell.Add(rec.Title);
                row.Cell.Add(rec.CreationDate.ToString());
                row.Cell.Add("<a href='javascript:void(0)' onClick='eliminaRecuperacion(" + rec.Id + ")'><img src='../view/img/delete.png' alt='Eliminar' title='Eliminar'/></a>");
                gi.Rows.Add(row);
            }
            gi.calculaInternos();
            string json = gi.toJSON();
            ShowPage("js/default.js");
            Response.Write(json);
        }
        public void FriendlyShow()
        {
            System.IO.StringWriter sw = new System.IO.StringWriter();
            int id = int.Parse(Request["id"]);
            RecuperacionDAL rdal = new RecuperacionDAL();
            RecuperacionVO r = rdal.GetById(id);
            HttpContext.Current.Items.Add("Recuperacion", r);
            ShowPage("recuperacion/FriendlyShow.aspx");
        }
    }
}
