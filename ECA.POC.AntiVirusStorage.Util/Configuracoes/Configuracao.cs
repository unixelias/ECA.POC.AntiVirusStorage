using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace ECA.POC.AntiVirusStorage.Util.Configuracoes
{
    public class Configuracao
    {
        private static Configuracao CONFIGURACAO;

        public BlobEspecificaConfig BlobEspecificaConfig { get; }

        private Configuracao(IConfiguration configuracao)
        {
            BlobEspecificaConfig = new BlobEspecificaConfig(configuracao);
        }

        public static Configuracao Obter()
        {
            if (CONFIGURACAO == null)
            {
                CONFIGURACAO = ObterConfiguracoesAtuais();
            }

            return CONFIGURACAO;
        }

        public static IConfiguration ObterConfigurationBuilderBase()
        {
            string environmentName = Environment.GetEnvironmentVariable(UtilConstants.ASPNETCORE_ENVIRONMENT);

            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            return builder.Build();
        }

        private static Configuracao ObterConfiguracoesAtuais()
        {
            return new Configuracao(ObterConfigurationBuilderBase());
        }
    }
}