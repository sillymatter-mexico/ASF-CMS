using System;
using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json;
using asf.cms.util;

namespace asf.cms.model
{
    public class ResponseMessage : IJsonable
    {
        public string Message { get; set; }
        public string Data { get; set; }
        public bool isError { get; set; }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
