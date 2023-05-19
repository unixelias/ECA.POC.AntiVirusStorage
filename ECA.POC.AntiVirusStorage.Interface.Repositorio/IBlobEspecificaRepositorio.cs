using Azure;
using Azure.Storage.Blobs.Models;
using System.IO;
using System.Threading.Tasks;

namespace ECA.POC.AntiVirusStorage.Interface.Repositorio
{
    public interface IBlobEspecificaRepositorio
    {
        public Task<Response<BlobContentInfo>> EnviarArquivo(Stream conteudoArquivo, string nomeArquivo);

        public Task<Response<GetBlobTagResult>> ObterMetadadosArquivo(string nomeArquivo);

        public Task<byte[]> ObterArquivo(string nomeArquivo);

        public Task DeletarArquivo(string nomeArquivo);
    }
}