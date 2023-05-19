using System;
using System.Runtime.Serialization;

namespace ECA.POC.AntiVirusStorage.Excecao
{
    [Serializable]
    public class PocStorageBaseException : Exception
    {
        public PocStorageBaseException() : base("Excessão na API")
        {
        }

        public PocStorageBaseException(string mensagem) : base(mensagem)
        {
        }

        public PocStorageBaseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PocStorageBaseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}