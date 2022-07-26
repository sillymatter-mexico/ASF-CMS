using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using asf.cms.dal;

namespace asf.cms.bll
{
    public class AuditLoader
    {
        string _message = "";
        public string message { get { return _message; } }
        public void LoadAudit(int year)
        {
             AuditDAL adal = new AuditDAL();
            adal.AuditLoad(year);
            adal.DeletePreload();
        }
        public bool DeleteCurrentAudits(int auditReportId)
        {
            AuditDAL adal = new AuditDAL();
            adal.DeleteByReport(auditReportId);
            return true;
        }
  
        public bool LoadPreloadAuditFiles(String path, int year)
        {
            AuditDAL adal = new AuditDAL();
            string query = "insert into preload_audit_file values";
            StringBuilder sb = new StringBuilder(query);
            string[] htms = Directory.GetFiles(path, "*audit*.htm");
            List<string> files = new List<string>(htms);
            files.AddRange(Directory.GetFiles(path, "*audit*.html"));
            if (files.Count == 0)
            {
                _message = "No se encontraron archivos en la carpeta de auditorias";
                return false;
            }
            int i = 1;
            foreach (string f in files)
            {
                String file = File.ReadAllText(f,util.Encoder.GetFileEncoding(f));
                string pattern = @"<a[^>]+href=""([^#&^""]+)(#[^""]+)*""[^>]*>\s*([^\s]+)[\s]*[^<]*</a>";
                
                Regex r = new Regex(pattern, RegexOptions.IgnoreCase);

                Match m = r.Match(file);
                if (!m.Success)
                {
                    _message = "No se encontraron auditorias en la carpeta de auditorias";
                    return false;
                }
                while (m.Success)
                {
                    string relative="";
                    string npath=Path.Combine(path, m.Groups[1].Value);
                    int existe = File.Exists(npath) ? 1 : 0;
                    relative=".."+npath.Replace(HttpContext.Current.Server.MapPath(".."),"").Replace("\\","/");
                    string code = m.Groups[3].Value;
                    string parsedCode = parseAuditCode(code);
                    if ((i % 100) == 0)
                    {
                        string q = sb.ToString();
                        adal.ExecuteUpdate(sb.ToString());
                        sb = new StringBuilder(query);
                        i = 1;
                    }
                    else if(i>1)
                        sb.Append(",");
                    sb.Append("('" + code.Trim() + "','" + relative.Trim() + "','" + m.Groups[2].Value.Trim() + "','" + m.Value + "','" + existe + "','" + parsedCode.Trim() + "','"+year+"')");
                    i++;
                    m = m.NextMatch();
                }

            }
            adal.ExecuteUpdate(sb.ToString());
            adal.DetectFiles(year);
            return true;
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
        public bool LoadPreloadAudit(String path, int year)
        {
            AuditDAL adal = new AuditDAL();
            adal.DeletePreload();
            try
            {
                ExcelAuditDAL edal = new ExcelAuditDAL(path);
                string sheet = edal.getFirstSheetName();
                if (String.IsNullOrEmpty(sheet))
                {
                    _message = "El archivo de auditorias no tiene datos";
                    return false;
                } 
                List<string> names = edal.getColumnNames(sheet);
                if (!this.isValidSheet(names))
                {
                    _message = "El archivo de auditorias no tiene el formato correcto";
                    return false;
                }
                List<String> script = edal.getInsertScriptFromBook(names,this.isAutoIndex(names));
                foreach (string query in script)
                    adal.ExecuteUpdate(query);
                adal.DetectEntities(year);
                adal.DetectSectors(year);
                if (this.hasGrupos(names))
                    adal.DetectGroups(year);
            }
            catch (Exception ex)
            {
                _message = "Error al abrir el archivo de auditorias. Verifique que la ruta sea correcta y que el archivo no este en uso";
                return false;

            }

            return true;

        }
        public bool isValidSheet(List<string> columns)
        {
            List<string> camposRequeridos = new List<string>();
            camposRequeridos.Add("c_cta_pub");
            camposRequeridos.Add("abrev_tipo_audi");
            camposRequeridos.Add("tipo_audit");
            camposRequeridos.Add("clave_audit");
            camposRequeridos.Add("numero_auditoria");
            camposRequeridos.Add("titulo_auditoria");
            camposRequeridos.Add("siglas_ente");
            camposRequeridos.Add("ente_fiscalizado");
            camposRequeridos.Add("clave_depend");
            camposRequeridos.Add("clave_tipo_ent");
            camposRequeridos.Add("clave_entidad");
            camposRequeridos.Add("nombre_sector");
            bool hasNumR = false, hasOrdenar = false, hasLLaveGpo = false, hasGrupoFuncional = false;
            foreach (String s in columns)
            {
                bool encontrado = false;
                for (int i = 0; i < camposRequeridos.Count; i++)
                    if (s.ToLower() == camposRequeridos[i])
                    {
                        camposRequeridos.RemoveAt(i);
                        encontrado = true;
                        break;
                    }
                if (encontrado)
                    continue;

                if (!hasNumR && s.ToLower() == "num_r")
                {
                    hasNumR = true;
                    continue;
                }
                if (!hasOrdenar && s.ToLower() == "ordenar")
                {
                    hasOrdenar = true;
                    continue;
                }
                if (!hasLLaveGpo && s.ToLower() == "llave_gpo")
                {
                    hasLLaveGpo = true;
                    continue;
                }
                if (!hasGrupoFuncional && s.ToLower() == "grupo_funcional")
                {
                    hasGrupoFuncional = true;
                    continue;
                }
                return false;
            }
            if (camposRequeridos.Count != 0 || (hasNumR != hasOrdenar) || (hasLLaveGpo != hasGrupoFuncional))
                return false;
            return true;
        }
        public bool isAutoIndex(List<string> columns)
        {
            if (columns.Contains("ordenar"))
                return false;
            return true;
        }
        public bool hasGrupos(List<string> columns)
        {
            return columns.Contains("grupo_funcional");
        }

    }
}
