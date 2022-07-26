using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.dal;
using asf.cms.model;
namespace asf.cms.bll
{
    public class AuditFinder
    {
        public Dictionary<String, String> Years;
        public Dictionary<String, String> AuditTypes;
        public Dictionary<string, string> AuditEntitys;
        public Dictionary<string, string> Sectors;
        public Dictionary<string, string> Titles;
        public Dictionary<string, string> FunctionalGroups;
        public List<AuditResultVO> results;
        public void PerformSearch(string anio, string tipo, string ente, string sector, string titulo,string grupoFuncionalId,string numero)
        {
            string filtro = "";
            if (anio != "")
                filtro += " and year='" + anio + "'";
            if (tipo != "")
                filtro += " and audit_type_id='" + tipo + "'";
            if (ente != "")
                filtro += " and audit_entity.name like '%" + ente + "%'";
            if (sector != "")
                filtro += " and audit_sector.id='" + sector + "'";
            if (titulo != "")
                filtro += " and audit.title like '" + titulo + "'";
            if (numero != "")
                filtro += " and audit.number like '" + numero + "'";
            if (grupoFuncionalId != "")
                filtro += " and audit.grupo_funcional_id= '" + grupoFuncionalId + "'";
            AuditDAL adal = new AuditDAL();
            results= new List<AuditResultVO>( adal.SearchAudit(filtro));
            Years = new Dictionary<string, string>();
            AuditTypes = new Dictionary<string, string>();
            AuditEntitys = new Dictionary<string, string>();
            Sectors = new Dictionary<string, string>();
            Titles = new Dictionary<string, string>();
            FunctionalGroups = new Dictionary<string, string>();
            foreach (AuditResultVO avo in results)
            {
                addItemToDictionary(Years, avo.Year.ToString(), avo.Year.ToString());
                addItemToDictionary(AuditTypes, avo.TypeId.ToString(), avo.TypeName);
                addItemToDictionary(AuditEntitys, avo.EntityId.ToString(), avo.EntityName);
                addItemToDictionary(Sectors, avo.SectorId.ToString(), avo.SectorName);
                addItemToDictionary(FunctionalGroups, avo.GrupoFuncionalId.ToString(), avo.GrupoFuncionalName);
                addItemToDictionary(Titles, avo.Title, "");
 
            }

        }
        public void addItemToDictionary(Dictionary<string, string> dictionary, string key, string value)
        {
            if (!dictionary.ContainsKey(key))
                dictionary.Add(key, value);
        }
        
    }
}
