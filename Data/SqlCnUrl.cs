using System.IO;
using Microsoft.Extensions.Configuration;

namespace TalkWhatsapp.Data
{
    public class SqlCnUrl
    {
        public string getCon()
        {
            var builder = new ConfigurationBuilder()
                          .SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true);

            IConfiguration _configuration = builder.Build();
            string sqlCon = _configuration.GetConnectionString("con_Talk_Whatsapp");
            return sqlCon;
        }
    }
}
