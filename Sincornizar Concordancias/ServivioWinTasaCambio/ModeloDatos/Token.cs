using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace SincronizarConcordancia.ModeloDatos
{
    public class Token
    {
        public class CToken
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public string expires_in { get; set; }
        }

        public string ObtenerToken(string url, string credenciales)
        {
            CToken tokens = new CToken();
            HttpWebRequest solicitud = WebRequest.Create(url) as HttpWebRequest;
            solicitud.Method = "POST";
            solicitud.ContentType = "application/x-www-form-urlencoded";
            try
            {                
                byte[] byteArray = Encoding.UTF8.GetBytes(credenciales);

                Stream dataStream = solicitud.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                HttpWebResponse Repuesta = solicitud.GetResponse() as HttpWebResponse;
                StreamReader Transmisor = new StreamReader(Repuesta.GetResponseStream());
                string Resultado = Transmisor.ReadToEnd();
                tokens = JsonConvert.DeserializeObject<CToken>(Resultado);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return tokens.access_token;
        }
    }
}
