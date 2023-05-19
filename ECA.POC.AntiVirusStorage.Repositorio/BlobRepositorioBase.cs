using Azure;
using Azure.Storage.Blobs.Models;
using ECA.POC.AntiVirusStorage.Excecao;
using ECA.POC.AntiVirusStorage.Util.Configuracoes;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ECA.POC.AntiVirusStorage.Repositorio
{
    public abstract class BlobRepositorioBase
    {
        protected readonly ILogger<BlobRepositorioBase> Logger;
        protected abstract BlobClientBaseConfig BlobClientBaseConfig { get; }

        protected BlobRepositorioBase(ILogger<BlobRepositorioBase> logger)
        {
            Logger = logger;
        }

        protected async Task<Response<BlobContentInfo>> CriarArquivoBlobConteiner(BlobClientBaseConfig blobClientBaseConfig, Stream conteudoArquivo, string nomeArquivo)
        {
            try
            {
                Logger.LogInformation("Criando arquivo {nomeArquivo} no Container: {nomeContainer}", nomeArquivo, blobClientBaseConfig.UriConteiner);

                if (!string.IsNullOrEmpty(nomeArquivo))
                {
                    using var blobClient = new AzureStorageRepositorio<byte[]>(BlobClientBaseConfig, nomeArquivo);
                    Response<BlobContentInfo> respostaEnvio = await blobClient.EnviarArquivo(conteudoArquivo);
                    return respostaEnvio;
                }
                return default;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Erro ao chamar conteiner de armazenamento Container: {nomeContainer}; Exceção: {mensagem}", blobClientBaseConfig.UriConteiner, ex.Message);
                throw new AcessoStorageException(ex.Message, ex);
            }
        }

        protected async Task<byte[]> ObterBlobDoConteiner(BlobClientBaseConfig blobClientBaseConfig, string nomeArquivo)
        {
            try
            {
                Logger.LogInformation("Buscando arquivo {nomeArquivo} no Container: {nomeContainer}", nomeArquivo, blobClientBaseConfig.UriConteiner);

                if (!string.IsNullOrEmpty(nomeArquivo))
                {
                    using var blobClient = new AzureStorageRepositorio<byte[]>(BlobClientBaseConfig, nomeArquivo);
                    return await blobClient.ObterAquivoBinario();
                }
                return Array.Empty<byte>();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Erro ao chamar conteiner de armazenamento Container: {nomeContainer}; Exceção: {mensagem}", blobClientBaseConfig.UriConteiner, ex.Message);
                throw new AcessoStorageException(ex.Message, ex);
            }
        }

        protected async Task<Response<GetBlobTagResult>> ObterMetadadosBlobConteiner(BlobClientBaseConfig blobClientBaseConfig, string nomeArquivo)
        {
            try
            {
                Logger.LogInformation("Buscando arquivo {nomeArquivo} no Container: {nomeContainer}", nomeArquivo, blobClientBaseConfig.UriConteiner);

                if (!string.IsNullOrEmpty(nomeArquivo))
                {
                    using var blobClient = new AzureStorageRepositorio<byte[]>(BlobClientBaseConfig, nomeArquivo);
                    return await blobClient.ObterMetadataArquivo();
                }
                return default;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Erro ao chamar conteiner de armazenamento Container: {nomeContainer}; Exceção: {mensagem}", blobClientBaseConfig.UriConteiner, ex.Message);
                throw new AcessoStorageException(ex.Message, ex);
            }
        }

        protected async Task DeletarArquivoBlobDoContainer(BlobClientBaseConfig blobClientBaseConfig, string nomeArquivo)
        {
            try
            {
                Logger.LogInformation("Apagando arquivo {nomeArquivo} do Container: {nomeContainer}", nomeArquivo, blobClientBaseConfig.UriConteiner);

                if (!string.IsNullOrEmpty(nomeArquivo))
                {
                    using var blobClient = new AzureStorageRepositorio<byte[]>(BlobClientBaseConfig, nomeArquivo);
                    await blobClient.DeletarArquivo();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Erro ao chamar conteiner de armazenamento Container: {nomeContainer}; Exceção: {mensagem}", blobClientBaseConfig.UriConteiner, ex.Message);
                throw new AcessoStorageException(ex.Message, ex);
            }
        }
    }
}