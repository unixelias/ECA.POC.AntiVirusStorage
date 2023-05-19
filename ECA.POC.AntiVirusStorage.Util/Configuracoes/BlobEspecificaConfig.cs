using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.POC.AntiVirusStorage.Util.Configuracoes
{
    public class BlobEspecificaConfig : BlobClientBaseConfig
    {
        public BlobEspecificaConfig(IConfiguration configuracao) : base(configuracao, UtilConstants.CONFIG_NAME_BLOB_ESPECIFICA)
        {
        }
    }
}
