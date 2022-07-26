using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.Odbc;
using System.Collections.Generic;
using System.Text;
using Excel;
using System.IO;


namespace asf.cms.dal
{
    public class ExcelAuditDAL
    {
        string path;
        IExcelDataReader excelReader;
        DataTable dt = null;
        public ExcelAuditDAL(string path)
        {
            this.path = path;
            FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read);
            string extension = Path.GetExtension(path);
            if (extension == ".xls")
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            else if (extension == ".xlsx")
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            else
                excelReader = null;
            excelReader.IsFirstRowAsColumnNames = true;
            DataSet ds = excelReader.AsDataSet();

            dt = ds.Tables[0];  
        }
        string getValue(int i, int pos)
        {
            if (dt.Rows[i][pos]==null)
                return "";
            return dt.Rows[i][pos].ToString().Trim();
        }
        public List<String> getInsertScriptFromBook(List<string> campos, bool autoIndex)
        {
            List<String> script = new List<string>();
            int indexClaveAudit=campos.IndexOf("clave_audit");

            string query = "INSERT INTO preload_audit_report(";
            foreach (string c in campos)
                query += mapToMysql(c) + ",";
            if (autoIndex)
                query += "ordenar,num_r,";

            query += "parsed_code) values";

            System.Text.StringBuilder sb = new System.Text.StringBuilder(query);
            
            for (int i=0;i<dt.Rows.Count;i++)
            {
                if (dt.Rows[i][0].ToString().Trim() == "")
                    continue;
                string insert = "";
                if ( i!=0&& ((i% 100) == 0))
                {
                    insert = "";
                    script.Add(sb.ToString());
                    sb = new StringBuilder(query);
                }
                else if(i%100>0)
                    insert += ", ";

                insert += "(";
                for(int k=0;k<campos.Count;k++)
                    insert += "'" + getValue(i,k) + "',";

                if (autoIndex)
                    insert += "'" + i + "','"+i+"',";
                insert += "'" + parseAuditCode(getValue(i, indexClaveAudit)) + "')";
                sb.Append(insert);
            }
            excelReader.Close();
            script.Add(sb.ToString());
            return script;
        }
        public string getFirstSheetName()
        {
            if (dt == null)
                return null;

            return dt.TableName;
        }
        public List<string> getColumnNames(string sheet)
        {
            List<string> names = new List<string>();
            for (int i = 0; i < dt.Columns.Count; i++)
                names.Add(dt.Columns[i].ColumnName.ToLower());
            return names;
        }
        string parseAuditCode(string code)
        {
            string parsedCode = "";
            string[] parts = code.Split('-');
            for (int i = 0; i < parts.Length; i++)
            {
                if (i > 0)
                    parsedCode += "-";
                parsedCode += parts[i].TrimStart(' ', '0');
            }
            return parsedCode;
        }
        string mapToMysql(String excelColumn)
        {
            if(excelColumn=="c_cta_pub")
                return "anio";
            if(excelColumn=="abrev_tipo_audi")
                return "id_tipo_audit";
            if(excelColumn=="numero_auditoria")
                return "numero_audit";
            if(excelColumn=="titulo_auditoria")
                return "titulo_audit";
            if(excelColumn=="ente_fiscalizado")
                return "nombre_ente";
            if (excelColumn == "clave_depend")
                return "clave_dependencia";
            if (excelColumn == "clave_tipo_ent")
                return "clave_tipo_ente";
            if (excelColumn == "clave_entidad")
                return "clave_ente";
            if (excelColumn == "llave_gpo")
                return "group_code";
            if(excelColumn=="grupo_funcional")
                return "group_name";
            return excelColumn;
        }
        /*
        public ExcelAuditDAL(string path)
        {
            connString = "Driver=Microsoft Excel Driver (*.xls, *.xlsx, *.xlsm, *.xlsb);Dbq=" + path + ";";
        }
        public List<String> getInsertScriptFromBook(string sheetName, int type)
        {
            OdbcConnection dbConn = new OdbcConnection(connString);

            dbConn.Open();
            List<String> script = new List<string>();
            string query = "SELECT * FROM [" + sheetName + "]";
            OdbcCommand com = new OdbcCommand(query, dbConn);
            OdbcDataReader reader = com.ExecuteReader();
            query = "INSERT INTO preload_audit_report(anio, id_tipo_audit, tipo_audit, clave_audit, numero_audit, titulo_audit, " +
                "siglas_ente, nombre_ente, clave_dependencia, clave_tipo_ente, clave_ente,nombre_sector, num_r, ordenar";
            if (type == 2)
                query += ",group_code, group_name";
            query += ",parsed_code) values";
            System.Text.StringBuilder sb = new System.Text.StringBuilder(query);
            int i = 1;
            while (reader.Read())
            {
                if (reader.GetValue(0).ToString().Trim() == "")
                    continue;
                string insert = "";
                if (i % 100 == 0)
                {
                    insert = "";
                    script.Add(sb.ToString());
                    sb = new StringBuilder(query);
                    i = 1;
                }
                else if (i > 1)
                {
                    insert += ", ";
                }

                insert += "(";
                insert += "'" + getValue(reader, 0) + "',";
                insert += "'" + getValue(reader, 1) + "',";
                insert += "'" + getValue(reader, 2) + "',";
                insert += "'" + getValue(reader, 3) + "',";
                insert += "'" + getValue(reader, 4) + "',";
                insert += "'" + getValue(reader, 5) + "',";
                insert += "'" + getValue(reader, 6) + "',";
                insert += "'" + getValue(reader, 7) + "',";
                insert += "'" + getValue(reader, 8) + "',";
                insert += "'" + getValue(reader, 9) + "',";
                insert += "'" + getValue(reader, 10) + "',";
                insert += "'" + getValue(reader, 11) + "',";
                insert += "'" + getValue(reader, 12) + "',";
                insert += "'" + getValue(reader, 13) + "',";
                if (type == 2)
                {
                    insert += "'" + getValue(reader, 14) + "',";
                    insert += "'" + getValue(reader, 15) + "',";
                }

                insert += "'" + parseAuditCode(getValue(reader, 3)) + "')";
                i++;
                sb.Append(insert);

            }
            dbConn.Close();
            script.Add(sb.ToString());
            return script;
        }
        public string getFirstSheetName()
        {
            OdbcConnection dbConn = new OdbcConnection(connString);
            try
            {

                dbConn.Open();
                DataTable sheets = dbConn.GetSchema("Tables");
                if (sheets != null && sheets.Rows.Count > 0)
                {
                    string sheetName = sheets.Rows[0]["TABLE_NAME"].ToString();
                    if (!sheetName.EndsWith("$"))
                        sheetName += '$';
                    return sheetName;
                }
                return null;
            }
            finally
            {
                if (dbConn.State != ConnectionState.Closed)
                    dbConn.Close();
            }
            return null;
        }
        string getValue(OdbcDataReader r, int pos)
        {
            if (r.GetValue(pos) == null)
                return "";
            return r.GetValue(pos).ToString().Trim();
        }
        public List<string> getColumnNames(string sheet)
        {
            List<string> names = new List<string>();

            OdbcConnection dbConn = new OdbcConnection(connString);
            dbConn.Open();
            string query = "SELECT * FROM [" + sheet + "]";
            OdbcCommand com = new OdbcCommand(query, dbConn);
            OdbcDataReader reader = com.ExecuteReader();
            DataTable dt = reader.GetSchemaTable();
            for (int i = 0; i < dt.Rows.Count; i++)
                names.Add(dt.Rows[i][0].ToString().ToLower());
            return names;
        }
        string parseAuditCode(string code)
        {
            string parsedCode = "";
            string[] parts = code.Split('-');
            for (int i = 0; i < parts.Length; i++)
            {
                if (i > 0)
                    parsedCode += "-";
                parsedCode += parts[i].TrimStart(' ', '0');
            }
            return parsedCode;
        }
        public bool isValidSheet(List<string> columns)
        {
            if (!columns.Contains("c_cta_pub"))
                return false;
            if (!columns.Contains("abrev_tipo_audi"))
                return false;
            if (!columns.Contains("tipo_audit"))
                return false;
            if (!columns.Contains("clave_audit"))
                return false;
            if (!columns.Contains("numero_auditoria"))
                return false;
            if (!columns.Contains("titulo_auditoria"))
                return false;
            if (!columns.Contains("siglas_ente"))
                return false;
            if (!columns.Contains("ente_fiscalizado"))
                return false;
            if (!columns.Contains("clave_depend"))
                return false;
            if (!columns.Contains("clave_tipo_ent"))
                return false;
            if (!columns.Contains("clave_entidad"))
                return false;
            if (!columns.Contains("nombre_sector"))
                return false;
            if (!columns.Contains("num_r"))
                return false;
            if (!columns.Contains("ordenar"))
                return false;

            return true;
        }*/

    }
}
