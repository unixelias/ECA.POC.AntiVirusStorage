using Azure;
using Azure.Storage.Blobs.Models;
using ECA.POC.AntiVirusStorage.Excecao;
using ECA.POC.AntiVirusStorage.Interface.Repositorio;
using ECA.POC.AntiVirusStorage.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ECA.POC.AntiVirusStorage.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ArquivoController : Controller
    {
        private readonly IBlobEspecificaRepositorio _blobEspecificaRepositorio;
        private readonly ILogger<ArquivoController> _logger;

        public ArquivoController(ILogger<ArquivoController> logger, IBlobEspecificaRepositorio blobEspecificaRepositorio)
        {
            _logger = logger;
            _blobEspecificaRepositorio = blobEspecificaRepositorio;
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> EnviarArquivoAsync([FromBody][Required] ArquivoModel arquivo)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(arquivo.ConteudoArquivoBase64);
                using var streamArquivo = new MemoryStream(bytes);
                using StreamContent conteudo = new(streamArquivo);

                _logger.LogInformation("Enviando arquivo ao Blob Conteiner");
                _ = await _blobEspecificaRepositorio.EnviarArquivo(conteudo.ReadAsStream(), arquivo.NomeArquivo);
                _logger.LogInformation("Aguardando análise do anti vírus");
                await Task.Delay(10000);
                Response<GetBlobTagResult> metadadosArquivoBlob = await _blobEspecificaRepositorio.ObterMetadadosArquivo(arquivo.NomeArquivo);

                foreach (KeyValuePair<string, string> tag in metadadosArquivoBlob.Value.Tags)
                {
                    if (tag.Key == "Malware Scanning scan result" && tag.Value == "No threats found")
                    {
                        return Created(arquivo.NomeArquivo, metadadosArquivoBlob.Value.Tags);
                    }
                }
                _logger.LogError("Arquivo comprometido");
                await _blobEspecificaRepositorio.DeletarArquivo(arquivo.NomeArquivo);
                return BadRequest(metadadosArquivoBlob.Value.Tags);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Mensagem de erro: {mensagem}", ex.Message);
                await _blobEspecificaRepositorio.DeletarArquivo(arquivo.NomeArquivo);
                return InternalServerError(ex);
            }
        }

        private IActionResult InternalServerError(Exception ex)
        {
            DetalhesExcecao errorResult = ex.ToProblemDetails();
            return StatusCode(errorResult.Status, errorResult);
        }
    }
}