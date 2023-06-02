using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TalkWhatsapp.Data;
using TalkWhatsapp.Models;


namespace TalkWhatsapp.Services
{
    public class EnvioMensajeService
    {
        public async Task<IEnumerable<EnvioMensajeModel>> ObtenerMensajes(int plataforma)
        {
            SqlCnUrl SqlCnUrl = new SqlCnUrl();
            string conexion = SqlCnUrl.getCon();

            IEnumerable<EnvioMensajeModel> data;
            using (var conn = new SqlConnection(conexion))
            {
                string query = @"exec w_EnviarMensaje_Service @plataforma";
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                data = await conn.QueryAsync<EnvioMensajeModel>(query, new { plataforma }, commandType: CommandType.Text);
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return data;
        }
    }
}
