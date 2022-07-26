using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
/// <summary>
/// Descripción breve de RowGrid
/// </summary>
 namespace asf.cms.util
{
     public class RowGrid
     {
         string id;
         List<string> cell;
         public string Id
         {
             get { return id; }
             set { id = value; }
         }
         public List<string> Cell
         {
             get { return cell; }
             set { cell = value; }
         }
         public string toJSON(bool boolAsString = true)
         {
             string jsonCell = "[";
            foreach (string s in cell)
            {
                if ((s == "true" || s == "false") && !boolAsString)
                    jsonCell += s + ",";
                else
                    jsonCell += "\"" + s + "\",";
            }
                 
             if (jsonCell.EndsWith(","))
                 jsonCell=jsonCell.Remove(jsonCell.Length - 1);
             jsonCell += "]";

             string json = "{" + getJSONNameValue("id", id) + "," + "\"cell\":"+ jsonCell + "}";
             return json;
         }
         public string getJSONNameValue(string name, object value)
         {
            return "\"" + name + "\":\"" + value + "\"";
         }
     }
}
