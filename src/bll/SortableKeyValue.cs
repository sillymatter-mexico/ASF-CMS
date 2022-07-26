using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;

namespace asf.cms.bll
{
    public class SortableKeyValue: IComparable
    {
        public string Key
        { set; get; }
        public string Value
        { set; get; }
        public SortableKeyValue()
        {
 
        }
        public SortableKeyValue(string Key, String Value)
        {
            this.Key = Key;
            this.Value = Value;
        }

        public SortableKeyValue(KeyValuePair<string,string> val)
        {
            this.Key = val.Key;
            this.Value = val.Value;
        }


        public static List<SortableKeyValue> GetListFromDictionary(Dictionary<string, string> dict)
        {
            List<SortableKeyValue> lista = new List<SortableKeyValue>();
            foreach (KeyValuePair<string, string> item in dict)
                lista.Add(new SortableKeyValue(item));
            return lista;
        }


        #region IComparable Members

       public  int CompareTo(object obj)
        {
            return this.Value.CompareTo( ((SortableKeyValue)obj).Value);
        }

        #endregion
    }
}
