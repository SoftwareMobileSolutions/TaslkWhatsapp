using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkWhatsapp.Util
{
    public class AccessKeyJson
    {
        //susana gonzalez
        public string getKey(string p1, string p2)
        {
            var builder = new ConfigurationBuilder()
                         .SetBasePath(Directory.GetCurrentDirectory())
                         .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true);

            IConfiguration _configuration = builder.Build();
            string result = _configuration.GetSection(p1)[p2];
            return result;
        }
    }
}
