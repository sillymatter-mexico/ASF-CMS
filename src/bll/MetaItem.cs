using System;
using Newtonsoft.Json;
using asf.cms.util;
using System.Collections.Generic;
namespace asf.cms.bll {
    public class MetaItem:IJsonable {
        public String Type;
        public String Value;
        public String Preview;
        public MetaItem()
        { }
        public MetaItem(string type)
        {
            this.Type = type;
        }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
        public MetaItem FromJson(string json)
        {
            return JsonConvert.DeserializeObject<MetaItem>(json);
        }
        public static string ListToJson(List<MetaItem> lista)
        {
            return JsonConvert.SerializeObject(lista);
        }
        public static List<MetaItem> ListFromJson(string json)
        {
            return JsonConvert.DeserializeObject<List<MetaItem>>(json);
        }
        public string getPreview()
        {
            switch (this.Type)
            {
                case "title":
                    return "<title>"+this.Value+"</title>";
                case "language":
                    return "<meta http-equiv=\"Content-Language\" content=\""+this.Value+"\"/>";
                case "revisit":
                    return "<meta name=\"Revisit-After\" content=\"" + this.Value + "\" />";
                case "author":
                case "description":
                case "date":
                case "copyright":
                case "keywords":
                case "robots":
                case "distribution":
                case "abstract":
                    return "<meta name=\""+this.Type+"\" content=\"" + this.Value + "\" />";
                case "expires":
                case "cache-control":
                case "pragma":
                case "refresh":
                case "window-target":
                    return "<meta http-equiv=\"" + this.Type + "\" content=\"" + this.Value + "\" />";

            }
            return "";
        }
        public override bool Equals(Object mi) 
        {
            return ((MetaItem)mi).Type == this.Type;
        }
        public override int GetHashCode()
        {
            return this.Type.GetHashCode();
        }

        public static string GetBannerValue(List<MetaItem> items)
        {
            string value = "";

            foreach(MetaItem item in items)
            {
                string itemValue = item.Value.ToLower();
                if (item.Type.Equals("window-target") && (itemValue.Equals("superior") || itemValue.Equals("lateral") || itemValue.Equals("footer")))
                {
                    value = item.Value;
                }
            }

            return value;
        }
    }

}