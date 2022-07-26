using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.IO;
using System.Collections.Generic;
using log4net;
using asf.cms.util;

namespace asf.cms.bll
{
    public class RecuperacionTree
    {
        private RecuperacionFile processfiles(String pathDir,String pathNode)
        {
            RecuperacionFile item=new RecuperacionFile(pathNode);
            
            List<RecuperacionFile> todo = new List<RecuperacionFile>();
            foreach (string file in Directory.GetFiles(pathDir, "*.htm"))
            {
                todo.Add(new RecuperacionFile(file));
            }
            foreach (string file in Directory.GetFiles(pathDir, "*.html"))
            {
                todo.Add(new RecuperacionFile(file));
            }
            foreach (string file in Directory.GetFiles(pathDir, "*.pdf"))
            {
                todo.Add(new RecuperacionFile(file));
            }
            foreach (string dir in Directory.GetDirectories(pathDir))
            {
                string node = dir;
                for (int i = todo.Count - 1; i >= 0; i--)
                {
                    RecuperacionFile rf = todo[i];
                    if (rf.title == Path.GetFileNameWithoutExtension(dir))
                    {
                        todo.Remove(rf);
                        node = rf.path;
                    }
                }
                todo.Add(processfiles(dir,node));
            }

            item.children = todo;
            return item;
        }
        public string getTree(String path)
        {
            if (!Directory.Exists(path))
                return "{"+Encoder.getJSONNameValue("data","ERROR: La ruta especificada no existe")+"}";
            if (Directory.GetDirectories(path).Length != 1)
                return "{" + Encoder.getJSONNameValue("data", "ERROR: La ruta dada debe contener solo una carpeta") + "}";
            if (Directory.GetFiles(path).Length < 1)
                return "{" + Encoder.getJSONNameValue("data", "ERROR: La ruta dada debe contener al menos 1 archivo") + "}";
            RecuperacionFile root = processfiles(path,"");
            if (root == null || root.children.Count == 0)
                return "";
            int i=0;
            string res="";
            foreach (RecuperacionFile rf in root.children)
            {
                if(i>0)
                    res+=",";
                res+=rf.toJson();
                i++;
            }
            return res;
        }
    }
    internal class RecuperacionFile {

        static String applicationRoot = HttpContext.Current.Server.MapPath("~").Replace("\\", "/");


        public RecuperacionFile(string path)
        {
            
            title=Path.GetFileNameWithoutExtension(path);
            if (!Directory.Exists(path))
            {
                this.path=path.Replace("\\","/").Replace(applicationRoot,"../");
//                this.path = path.Replace(HttpContext.Current.Server.MapPath("~"), "../").Replace("\\", "/");
 //               this.path = path.Replace("\\", "/").Replace(HttpContext.Current.Server.MapPath("~").Replace("\\", "/"), "../");
            }
                children = new List<RecuperacionFile>();
        }
        public String title { get; set; }
        public String path { get; set; }
        public List<RecuperacionFile> children { get; set; }

        public string toJson()
        { 
          String res="{\"data\": \""+title+"\",\"metadata\":{\"path\":\""+path+"\"},\"attr\":{\"class\":\"visible\"}";
            if(children.Count>0)
            { 
                res+=",\"state\":\"open\",\"children\":[";
                int i=0;
                string child="";
                foreach (RecuperacionFile rf in children)
                {
                    if(i>0)
                        child+=",";
                    child+=rf.toJson();
                    i++;
                }
                res += child;
                res += "]";
            }
            res += "}";
            return res;
        }
    
    }

}
