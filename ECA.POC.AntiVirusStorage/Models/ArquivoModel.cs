using System.Text.Json.Serialization;

namespace ECA.POC.AntiVirusStorage.Models
{
    public class ArquivoModel
    {
        [JsonPropertyName("conteudo-arquivo")]
        public string ConteudoArquivoBase64 { get; set; }
        [JsonPropertyName("mime-type")]
        public string MimeType { get; set; }
        [JsonPropertyName("nome-arquivo")]
        public string NomeArquivo { get; set; }
    }
}