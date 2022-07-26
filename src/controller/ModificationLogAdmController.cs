using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.bll;
using asf.cms.model;

namespace asf.cms.controller
{
    public class ModificationLogAdmController : Controller
    {
        public ModificationLogAdmController(HttpContext Context) : base(Context)
        {
            isAdmin = 1;
        }

        public void List()
        {
            Items.Add("userType", GetCurrentUserType());
            Items.Add("selectedTab", "ModificationLog");
            ShowPage("log/ModificationLog.aspx");
        }

        public void Entries()
        {
            string msgTemplate = "El usuario {0} ha {1} la {2} {3} con id {4}",
                   recTemplate = " con versión histórica con id {0}.",
                   chaTemplate = "El campo {0} ha cambiado de {1} a {2}.",
                   chaTemplate1 = "El campo {0} se ha modificado.";

            string searchType = Request["searchType"],
                   searchTarget = Request["searchTarget"];
            DateTime searchStart = String.IsNullOrEmpty(Request["searchStart"]) ? DateTime.Now : DateTime.Parse(Request["searchStart"]),
                     searchEnd = String.IsNullOrEmpty(Request["searchEnd"]) ? DateTime.MinValue : DateTime.Parse(Request["searchEnd"]);

            List<ModificationLog> entries = ModificationLog.Filter(searchStart, searchEnd, searchType, searchTarget);
            List<object> results = new List<object>();

            foreach(ModificationLog entry in entries)
            {
                string msg = String.Format(msgTemplate, entry.UserName, entry.TypeReadable(), entry.TargetTypeReadable(), entry.Permalink, entry.TargetId),
                       desc = "";
                if (entry.Type == ModificationType.RESTORE)
                {
                    msg += String.Format(recTemplate, entry.HistoricId);
                }
                msg += ". ";
                foreach(ModificationLogField change in entry.FieldChanges)
                {
                    if (change.Field == "Content" || change.Field == "NewsContent")
                        desc += String.Format(chaTemplate1, change.Field);
                    else
                        desc += String.Format(chaTemplate, change.Field, change.From, change.To);
                }
                results.Add(new { Created = entry.Created, Message = msg, Details = desc, Type = entry.Type.ToString(), TargetType = entry.TargetType.ToString() });
            }

            SendJSON(Newtonsoft.Json.JsonConvert.SerializeObject(results));
        }
    }
}