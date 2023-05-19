using System;
using System.Runtime.Serialization;

namespace ECA.POC.AntiVirusStorage.Excecao
{
    [Serializable]
    public class ConflictException : PocStorageBaseException
    {
        public ConflictException() : base("Arquivo já existe no conteiner de armazenamento")
        {
        }

        public ConflictException(string mensagem) : base(mensagem)
        {
        }

        public ConflictException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ConflictException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}