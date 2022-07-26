using System;
using System.Data;
using System.Collections.Generic;

/// <summary>
/// Descripción breve de GridInput
/// </summary>
/// 
namespace asf.cms.util
{
    public class GridInput
    {
        int total=0;
        int page=1;
        int records=0;
        List<RowGrid> rows;
        string sidx="1";
        string sord="";
        int limit=20;
        String orden;
        public String Orden
        {
            get { return orden; }
            set { orden = value; }
        }
        public int Total
        {
            get { return total; }
            set { total = value; }
        }
        public int Page
        {
            get { return page; }
            set { page = value; }
        }
        public int Records
        {
            get { return records; }
            set { records = value; }
        }
        public List<RowGrid> Rows
        {
            get { return rows; }
            set { rows = value; }
        }
        public string Sord
        {
            get { return sord; }
            set { sord = value; }
        }
        public string Sidx
        {
            get { return sidx; }
            set { sidx = value; }
        }
        public int Limit
        {
            get { return limit; }
            set { limit = value; }
        }

        public string toJSON(bool boolAsString = true)
        {
            //{"page":"1","total":1,"records":2,"rows":[{"id":"12:00","cell":["12:00","334"]},{"id":"13:00","cell":["13:00","205"]}]}

            string rowsJson = "[";
            foreach (RowGrid rg in rows)
                rowsJson += rg.toJSON(boolAsString) + ",";
            if (rowsJson.EndsWith(","))
                rowsJson=rowsJson.Remove(rowsJson.Length - 1);
            rowsJson += "]";

            String json = "{" + Encoder.getJSONNameValue("page", page) + ","
                + Encoder.getJSONNameValue("total", total) + ","
                + Encoder.getJSONNameValue("records", records) + ","
                + "\"rows\":"+rowsJson+"}";

            return json;
  
        }
       

        public void calculaInternos()
        {
            int start = 0;
            orden = "";
            start = limit * page - limit;
            total = (int)Math.Ceiling((double)records / limit);
            if (page > total)
                page = total;
            if (start < 0)
                start = 0;
            if(records>0)
                orden = " ORDER BY " + sidx + " " + sord + " LIMIT " + start + " , " + limit;
        }
    }
}