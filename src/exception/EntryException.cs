using System;
using System.Collections.Generic;
using System.Web;

namespace asf.cms.exception
{
    public class EntryException : ApplicationException
    {
        //Constructor por Default
        public EntryException() { }

        //Constructor que coloca Message en la herencia
        public EntryException(string Mensaje) : base(Mensaje) { }

        //Constructor que maneja excepciones internas
        public EntryException(string Mensaje, Exception inner) : base(Mensaje, inner) { }

        //Constructor que maneja la serialización del tipo
        protected EntryException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}