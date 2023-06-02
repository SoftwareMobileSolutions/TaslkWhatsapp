using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TalkWhatsapp.Services;
using TalkWhatsapp.Util;
using TalkWhatsapp.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using TaslkWhatsapp.Util;

namespace TalkWhatsapp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            EnvioMensajeService EnvioMensajeService = new EnvioMensajeService();
            AccessKeyJson AccessKeyJson = new AccessKeyJson();

            int plataforma = int.Parse(AccessKeyJson.getKey("plataforma", "valor"));
            string apiToken = AccessKeyJson.getKey("plantilla", "Api-Token");
            string urlRequest = AccessKeyJson.getKey("plantilla", "urlRequest");
            string idPlantilla = AccessKeyJson.getKey("plantilla", "idPlantilla");
            int idBotRedes = int.Parse(AccessKeyJson.getKey("plantilla", "idBotRedes"));
            int idEmpresa = int.Parse(AccessKeyJson.getKey("plantilla", "idEmpresa"));
            bool altAsSMS = Convert.ToBoolean(AccessKeyJson.getKey("plantilla", "altAsSMS"));
            string method = AccessKeyJson.getKey("plantilla", "method");
            string contenttype = AccessKeyJson.getKey("plantilla", "contentType");
            int timeout = int.Parse(AccessKeyJson.getKey("plantilla", "timeout"));

            IEnumerable<EnvioMensajeModel> datos = await EnvioMensajeService.ObtenerMensajes(plataforma);
            envioDeMensajes(urlRequest, idPlantilla, idBotRedes, idEmpresa, altAsSMS, apiToken, method, contenttype, timeout, datos.ToList());
            Console.WriteLine("...DATOS ENVIADOS...");
        }

        static void envioDeMensajes(string urlRequest,string idPlantilla, int idBotRedes, int idEmpresa, bool altAsSMS, string apiToken, string method, string contenttype, int timeout, IEnumerable<EnvioMensajeModel> datos)
        {

            var d = datos.ToList();

            for(int i = 0, len = d.Count(); i < len; i++) //Recorriendo la lista de registros
            {
                string[] telefonos = d[i].telefonos.ToString().Split(',');

                for(int j = 0, lenj = telefonos.Count(); j < lenj; j++) // Recorriendo la cantidad de teléfonos a los cuales se les debe enviar
                {
                    if (telefonos[j] != "" && telefonos[j] != null)
                    {
                        dynamic jsonResponse = null;
                        var data = new
                        {
                            idBotRedes = idBotRedes,
                            idEmpresa = idEmpresa,
                            parametrosPlantilla = new[] {
                            " " + d[i].placa,
                            " " + d[i].dategps,
                            " " + d[i].velocidad + " km",
                            " " + d[i].ubicacion,
                            " " + d[i].lat + "," + d[i].lng
                        },
                            idPlantilla = idPlantilla,
                            altAsSMS = altAsSMS,
                            telefono = telefonos[j]
                        };


                        var settings = new JsonSerializerSettings { Converters = { new ReplacingStringWritingConverter("\n", "") } };
                        var json = JsonConvert.SerializeObject(data, Formatting.None, settings);


                       // var json = JsonConvert.SerializeObject(data);


                        var djson = Encoding.ASCII.GetBytes(json);

                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        WebRequest tRequest = WebRequest.Create(urlRequest);
                        tRequest.Method = method;
                        tRequest.Timeout = timeout;
                        tRequest.ContentType = contenttype;
                        tRequest.Headers["Api-Token"] = apiToken;
                        tRequest.ContentLength = djson.Length;

                        using (var stream = tRequest.GetRequestStream())
                        {
                            stream.Write(djson, 0, djson.Length);
                        }

                        WebResponse response = (HttpWebResponse)tRequest.GetResponse();
                        jsonResponse = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    }
                }
            }
        }
    }
}
