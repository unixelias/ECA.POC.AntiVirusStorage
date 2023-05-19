using System;
using System.Runtime.Serialization;

namespace ECA.POC.AntiVirusStorage.Excecao
{
    [Serializable]
    public class BlobStorageException : PocStorageBaseException
    {
        public BlobStorageException() : base("Erro ao realizar operação no conteiner de armazenamento")
        {
        }

        public BlobStorageException(string mensagem) : base(mensagem)
        {
        }

        public BlobStorageException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BlobStorageException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}