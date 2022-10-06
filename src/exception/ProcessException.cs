using System;
using System.Collections.Generic;
using System.Web;

namespace asf.cms.exception
{
    public class ProcessException : ApplicationException
    {
        //Constructor por Default
        public ProcessException() { }

        //Constructor que coloca Message en la herencia
        public ProcessException(string Mensaje) : base(Mensaje) { }

        //Constructor que maneja excepciones internas
        public ProcessException(string Mensaje, Exception inner) : base(Mensaje, inner) { }

        //Constructor que maneja la serialización del tipo
        protected ProcessException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}