using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkWhatsapp.Models
{
    public class EnvioMensajeModel
    {
        public string telefonos { get; set; }
        public int statusid { get; set; }
        public string placa { get; set; }
        public string dategps { get; set; }
        public string velocidad { get; set; }
        public string ubicacion { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
    }
}
