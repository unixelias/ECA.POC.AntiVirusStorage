using Microsoft.Extensions.Configuration;

namespace ECA.POC.AntiVirusStorage.Util.Configuracoes
{
    public abstract class BlobClientBaseConfig
    {
        public string ChaveAcesso { get; }
        public string UriConteiner { get; }

        #region Construtor

        protected BlobClientBaseConfig(IConfiguration configs, string sectionName)
        {
            IConfigurationSection sectionApiConfiguration = configs.GetSection(sectionName);

            ChaveAcesso = sectionApiConfiguration.GetValue<string>(UtilConstants.CHAVE_ACESSO);
            UriConteiner = sectionApiConfiguration.GetValue<string>(UtilConstants.URI_CONTEINER);
        }

        #endregion Construtor
    }
}