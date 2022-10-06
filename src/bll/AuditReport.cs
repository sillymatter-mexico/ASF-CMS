using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.dal;
using asf.cms.model;

namespace asf.cms.bll
{
    public class AuditReport
    {
        public List<AuditReportItem> GetDisplayableList()
        {
            AuditReportDAL ardal = new AuditReportDAL();
            List<AuditReportVO> list= new List<AuditReportVO>(ardal.GetPublished());
            List<AuditReportItem> itemList=new List<AuditReportItem>();
            PublicationDAL pdal=new PublicationDAL();
            foreach(AuditReportVO arvo in list)
            {
                AuditReportItem item = new AuditReportItem();
                PublicationVO pvo=pdal.GetByPermalink(arvo.PublicationPermalink);
                item.GlobalTitle = "Cuenta P&uacute;blica " + arvo.Year;
                item.ReportTitle = pvo.Title;
                item.ReportLink = arvo.MainFilePath;
                if(!String.IsNullOrEmpty(arvo.PresentationPath.Trim()))
                {   
                    item.PresentationTitle = "Presentaci&oacute;n";
                    item.PresentationLink = arvo.PresentationPath;
                }
                if (!String.IsNullOrEmpty(arvo.ExecutiveReportPath.Trim()))
                {
                    item.ExecutiveResumeTitle = "Resumen Ejecutivo";
                    item.ExecutiveResumeLink = arvo.ExecutiveReportPath;
                }
                itemList.Add(item);
            }

            return itemList;
        }
        public List<AuditReportInfo> GetList()
        {
            AuditReportDAL adal = new AuditReportDAL();
            return new List<AuditReportInfo>(adal.GetAllActive());
        }
    }
}
