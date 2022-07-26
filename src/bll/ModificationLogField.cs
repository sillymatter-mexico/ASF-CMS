using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.util;
using Newtonsoft.Json;

namespace asf.cms.bll
{
    public class ModificationLogField : IJsonable
    {
        public ModificationLogField()
        {
        }

        public ModificationLogField(string fieldname, object from, object to, string type)
        {
            Field = fieldname;
            From = from;
            To = to;
            Type = type;
        }

        public string Field { set; get; }
        public object From { set; get; }
        public object To { set; get; }
        public string Type { set; get; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static string ListToJson(List<ModificationLogField> fields)
        {
            return JsonConvert.SerializeObject(fields);
        }

        public static List<ModificationLogField> ListFromJson(string json)
        {
            return JsonConvert.DeserializeObject<List<ModificationLogField>>(json);
        }
    }
}