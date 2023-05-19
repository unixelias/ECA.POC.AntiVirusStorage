using Azure.Storage.Blobs.Models;
using Azure;
using ECA.POC.AntiVirusStorage.Interface.Repositorio;
using ECA.POC.AntiVirusStorage.Util.Configuracoes;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace ECA.POC.AntiVirusStorage.Repositorio
{
    public class BlobEspecificaRepositorio : BlobRepositorioBase, IBlobEspecificaRepositorio
    {
        protected override sealed BlobClientBaseConfig BlobClientBaseConfig => Configuracao.Obter().BlobEspecificaConfig;

        public BlobEspecificaRepositorio(ILogger<BlobEspecificaRepositorio> logger) : base(logger)
        {
        }

        public async Task<Response<BlobContentInfo>> EnviarArquivo(Stream conteudoArquivo,string nomeArquivo)
        {
            return await CriarArquivoBlobConteiner(BlobClientBaseConfig, conteudoArquivo, nomeArquivo);
        }

        public async Task<Response<GetBlobTagResult>> ObterMetadadosArquivo(string nomeArquivo)
        {
            return await ObterMetadadosBlobConteiner(BlobClientBaseConfig, nomeArquivo);
        }
        public async Task<byte[]> ObterArquivo(string nomeArquivo)
        {
            return await ObterBlobDoConteiner(BlobClientBaseConfig, nomeArquivo);
        }

        public async Task DeletarArquivo(string nomeArquivo)
        {
            await DeletarArquivoBlobDoContainer(BlobClientBaseConfig, nomeArquivo);
        }
    }
}