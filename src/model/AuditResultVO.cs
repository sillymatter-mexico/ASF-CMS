using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.util;
namespace asf.cms.model
{
    public class AuditResultVO:IJsonable
    {
        public virtual int Year
        {
            set;
            get;
        }
        public virtual int TypeId
        {
            set;
            get;
        }
        public virtual String TypeName
        {
            set;
            get;
        }
        public virtual int EntityId
        {
            set;
            get;
        }
        public virtual String EntityName
        {
            set;
            get;
        }
        public virtual uint SectorId
        {
            set;
            get;
        }
        public virtual String SectorName
        {
            set;
            get;
        }
        public virtual String Title
        {
            set;
            get;
        }
        public virtual UInt64 Number
        {
            set;
            get;
        }
        public virtual String File
        {
            set;
            get;
        }
        public virtual String Page
        {
            set;
            get;
        }
        public virtual int GrupoFuncionalId
        {
            set;
            get;
        }
        public virtual String GrupoFuncionalName
        {
            set;
            get;
        }
        public string ToJson()
        {
            string json = "{" +
                Encoder.getJSONNameValue("Year", Year) + "," +
                Encoder.getJSONNameValue("TypeId", TypeId) + "," +
                Encoder.getJSONNameValue("TypeName", TypeName) + "," +
                Encoder.getJSONNameValue("EntityId", EntityId) + "," +
                Encoder.getJSONNameValue("EntityName", EntityName) + "," +
                Encoder.getJSONNameValue("Sector", SectorName) + "," +
                Encoder.getJSONNameValue("Title", Title) + "," +
                Encoder.getJSONNameValue("Number", Number) + "," +
                Encoder.getJSONNameValue("File", File) + "," +
                Encoder.getJSONNameValue("Page", Page) +","+
                Encoder.getJSONNameValue("GrupoFuncionalId", GrupoFuncionalId) + "," +
                Encoder.getJSONNameValue("GrupoFuncionalId", GrupoFuncionalName) +
                "}";
            return json;
        }

    }
}
