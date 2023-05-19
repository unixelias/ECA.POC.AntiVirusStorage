using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ECA.POC.AntiVirusStorage.Excecao;
using ECA.POC.AntiVirusStorage.Util.Configuracoes;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ECA.POC.AntiVirusStorage.Repositorio
{
    public class AzureStorageRepositorio<T> : IDisposable
    {
        #region Propriedades

        private readonly BlobClient _blobClient;

        protected readonly JsonSerializerOptions JSONOPTIONS = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
        };

        private MemoryStream _stream;
        private MemoryStream _input;
        private MemoryStream _output;
        private DeflateStream _deflateStream;
        private bool _disposedValue;

        #endregion Propriedades

        public AzureStorageRepositorio(BlobClientBaseConfig blobClientBaseConfig, string nomeArquivo)
        {
            _stream = new MemoryStream();
            var credenciais = new AzureSasCredential(blobClientBaseConfig.ChaveAcesso);
            var caminhoArquivo = new Uri($"{blobClientBaseConfig.UriConteiner}/{nomeArquivo}");
            _blobClient = new BlobClient(caminhoArquivo, credenciais);
        }

        public async Task<byte[]> ObterAquivoBinario()
        {
            try
            {
                if (await _blobClient.ExistsAsync())
                {
                    await _blobClient.DownloadToAsync(_stream);
                    return _stream.ToArray();
                }
                throw new FileNotFoundException($"Arquivo {_blobClient.Name} não encontrado no conteiner de armazenamento", _blobClient.Name);
            }
            catch (Exception ex)
            {
                throw new BlobStorageException(ex.Message, ex);
            }
        }

        public async Task<Response<GetBlobTagResult>> ObterMetadataArquivo()
        {
            try
            {
                if (await _blobClient.ExistsAsync())
                {
                    Response<GetBlobTagResult> tagsArquivo = await _blobClient.GetTagsAsync();
                    return tagsArquivo;
                }
                throw new FileNotFoundException($"Arquivo {_blobClient.Name} não encontrado no conteiner de armazenamento", _blobClient.Name);
            }
            catch (Exception ex)
            {
                throw new BlobStorageException(ex.Message, ex);
            }
        }

        public async Task DeletarArquivo()
        {
            try
            {
                await _blobClient.DeleteIfExistsAsync();
            }
            catch (Exception ex)
            {
                throw new BlobStorageException(ex.Message, ex);
            }
        }

        public async Task<Response<BlobContentInfo>> EnviarArquivo(Stream conteudoArquivo)
        {
            try
            {
                if (await _blobClient.ExistsAsync())
                {
                    throw new ConflictException($"Arquivo {_blobClient.Name} já existe no conteiner de armazenamento");
                }
                Response<BlobContentInfo> respostaEnvio = await _blobClient.UploadAsync(conteudoArquivo);
                return respostaEnvio;
            }
            catch (Exception ex)
            {
                throw new BlobStorageException(ex.Message, ex);
            }
        }

        public async Task<T> ObterAquivoDesserializado()
        {
            try
            {
                if (await _blobClient.ExistsAsync())
                {
                    await _blobClient.DownloadToAsync(_stream);
                    return DecompressCache<T>(_stream);
                }
                throw new FileNotFoundException($"Arquivo {_blobClient.Name} não encontrado no conteiner de armazenamento", _blobClient.Name);
            }
            catch (Exception ex)
            {
                throw new BlobStorageException(ex.Message, ex);
            }
        }

        private U DecompressCache<U>(MemoryStream conteudo)
        {
            return JsonSerializer.Deserialize<U>(Encoding.Default.GetString(Decompress(conteudo.ToArray())), JSONOPTIONS);
        }

        private byte[] Decompress(byte[] dados)
        {
            _input = new MemoryStream(dados);
            _output = new MemoryStream();
            _deflateStream = new DeflateStream(_input, CompressionMode.Decompress);
            _deflateStream.CopyTo(_output);
            return _output.ToArray();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (_stream != null)
                    {
                        _stream.Dispose();
                        _stream = null;
                    }
                    if (_output != null)
                    {
                        _output.Dispose();
                        _output = null;
                    }
                    if (_stream != null)
                    {
                        _input.Dispose();
                        _input = null;
                    }
                    if (_deflateStream != null)
                    {
                        _deflateStream.Dispose();
                        _deflateStream = null;
                    }
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        ~AzureStorageRepositorio() => this.Dispose(false);
    }
}