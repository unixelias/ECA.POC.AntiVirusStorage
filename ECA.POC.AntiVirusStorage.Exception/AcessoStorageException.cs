using System;
using System.Runtime.Serialization;

namespace ECA.POC.AntiVirusStorage.Excecao
{
    [Serializable]
    public class AcessoStorageException : PocStorageBaseException
    {
        public AcessoStorageException() : base("Erro ao chamar conteiner de armazenamento")
        {
        }

        public AcessoStorageException(string mensagem) : base(mensagem)
        {
        }

        public AcessoStorageException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AcessoStorageException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}