using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Threading;
using System.Globalization;
using System.Configuration;
using SincronizarConcordancia.ModeloDatos;
//using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Dynamic;



namespace SincronizarConcordancia
{
    public partial class SincronizarConcordanciaI : ServiceBase
    {       
        string OrigenLog = "Sincronizar Concordancia";
        string Aplicacion = "Application";
       
        private System.Timers.Timer timer;
        //Intervalo por defecto hasta encontrar el intervalo correcto-1H
        //sc delete ServiceName, Para despues de desintalar
        public double Intervalo = 36000000; //(3600000:UNA HORA)(86400000:UN DIA)
        public DateTime FechaInicial = DateTime.Now;
        public DateTime FechaFinal = DateTime.Now;
        
        public int BufferPagos = 100;

        public SincronizarConcordanciaI()
        {
            InitializeComponent();
        }
                    
        protected override void OnStart(string[] args)
        {
            //Thread.Sleep(int.Parse(ConfigurationSettings.AppSettings["RetardoMilisegundos"]));                        
            Intervalo = double.Parse(ConfigurationSettings.AppSettings["IntervaloMilisegundos"]);              
           
            this.timer = new System.Timers.Timer(Intervalo);
            this.timer.AutoReset = true;
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
            this.timer.Start();
                        
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            FechaInicial = DateTime.Parse(ConfigurationSettings.AppSettings["FechaInicial"]); 
            CultureInfo Cultura = new CultureInfo("es-NI");

            var r = SincronizarConcordancia(Convert.ToDateTime(FechaInicial).ToString("dd/MM/yyyy"));


            if (!EventLog.SourceExists(OrigenLog))
                EventLog.CreateEventSource(OrigenLog, Aplicacion);
            
            EventLog.WriteEntry(OrigenLog, "Sincronizacion  Concordancia Completada: " , EventLogEntryType.SuccessAudit);           
        }

        private dynamic SincronizarConcordancia(string Ini)
        {   
            try 
	        {
                Conexion DATA = new Conexion();
                DataTable Result = DATA.EjecutarProcedimiento("prSincronizarFincasConcordadas", "<Parametros> <Parametro FechaIni =\"" + Ini + "\" ></Parametro> </Parametros>");

                var FincasColeccion = (from DataRow d in Result.Rows
                            select Result.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => d[col])).ToList();

                foreach (var Fincas in FincasColeccion)
                {
                    //Retardar las paticiones alos servicios de Catastro, para que no nos bloqueen 
                    Thread.Sleep(int.Parse(ConfigurationSettings.AppSettings["RetardoMilisegundos"])); 

                    //string dataCatastroString = FolioAdministrativo(Fincas["NumeroFinca"].ToString(), Fincas["UbicacionGeografica"].ToString());
                                        
                    //dataCatastroString = dataCatastroString.Replace(@"\", "");
                    //dataCatastroString = dataCatastroString.Replace("$", "");
                    //dataCatastroString = dataCatastroString.Replace(";", "");

                    //dataCatastroString = dataCatastroString.Replace("Area Catastral","AreaCatastral");
                    //dataCatastroString = dataCatastroString.Replace("Mapa Parcela","MapaParcela");

                    //dataCatastroString = dataCatastroString.Replace(" ", "");

                    string dataCatastroString = CargarDatosCatastroFincaParcela(Fincas["NumeroFinca"].ToString(), Fincas["UbicacionGeografica"].ToString());

                    if (dataCatastroString.Contains("AreaCatastral") && dataCatastroString.Length > 15)//Si hay datos en catastro de la finca
                    {
                        var json = dataCatastroString.Remove(0, 1).Remove(dataCatastroString.Length - 2, 1);
                        var DatosC = JsonConvert.DeserializeObject<DatosCatastrales>(json);
                        DataTable concordancia;
                        if(Double.Parse(DatosC.AreaCatastral) > 0 )
                                  concordancia = Concordar(DatosC, Fincas["UbicacionGeografica"].ToString(), Fincas["IdFinca"].ToString());  
                    }                    
                }
                return Result;
	        }
	        catch (Exception ex)
	        {
                EventLog.WriteEntry(OrigenLog, "SincronizarConcordancia(string FechaINI) " + ex.Message, EventLogEntryType.SuccessAudit); 
                return ex.Message;
	        }
        }
        private dynamic Concordar(DatosCatastrales DatosC, String UbicacionGeografica, String IdFinca)
        {
            try
            {
                Conexion DATA = new Conexion();
                string parametros = @"<Parametros> <Parametro " + " FINCA=\"" + DatosC.Finca + "\"  IDFINCA = \"" + IdFinca + "\"  UBICACIONGEOGRAFICA = \"" + UbicacionGeografica + "\" PARCELA = \"" + DatosC.MapaParcela + "\" AREACATASTRAL=\"" + DatosC.AreaCatastral + "\" ></Parametro> </Parametros>";
                parametros = parametros.Replace(@"\", "");
                                
                var Result = DATA.EjecutarProcedimiento("prSincronizarFincasConcordadas", parametros);
                return Result;
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(OrigenLog, "Concordar(cResumenParcela DatosC, String UbicacionGeografica, String IdFinca) " + ex.Message, EventLogEntryType.SuccessAudit);
                return ex.Message;
            }                    
        }

        public dynamic FolioAdministrativo(String numFinca, String nomDepartamento)
        {
            //Declaraciones
            //String resultado = "";
            HttpWebRequest webRequest;
            String sUrlRequest;
            String resultado = String.Empty;

            //Operaciones
            // Create a request using a URL that can receive a post. 
            sUrlRequest = ConfigurationSettings.AppSettings["UrlServiciosRegistroIneter"] + "Api/ConsultarDatos/FolioAdministrativo?numFinca=" + numFinca + "&UbicacionGeografica=" + nomDepartamento;            
            webRequest = (HttpWebRequest)System.Net.WebRequest.Create(sUrlRequest);

            //Agregando Seguridad/Token
            webRequest.Method = "GET";
            Token t = new Token();
            var token = t.ObtenerToken(ConfigurationSettings.AppSettings["UrlServiciosRegistroIneterToken"], ConfigurationSettings.AppSettings["CredencialesRegistroToken"]);
            webRequest.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + token);
            webRequest.Credentials = new System.Net.NetworkCredential(ConfigurationSettings.AppSettings["User"], ConfigurationSettings.AppSettings["Clave"]);

            try
            {
                using (var response = webRequest.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    resultado = reader.ReadToEnd().ToString();
                }
            }
            catch (Exception ex)
            {
                //XtraMessageBox.Show(ex.Message, this.Text);
            }

            //Resultado
            return resultado;
        }

        public dynamic CargarDatosCatastroFincaParcela(String numFinca, String UbicaionGeografica)
        {
            //Declaraciones
            HttpWebRequest webRequest;
            String sUrlRequest;
            String resultado = String.Empty;

            sUrlRequest = ConfigurationSettings.AppSettings["UrlServiciosRegistroIneter"] + "Api/CargarDatosCatastroFincaParcela/" + numFinca + "/" + UbicaionGeografica;
            webRequest = (HttpWebRequest)System.Net.WebRequest.Create(sUrlRequest);           
            webRequest.Method = "GET";            
            try
            {
                using (var response = webRequest.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    resultado = reader.ReadToEnd().ToString();
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(OrigenLog, "CargarDatosCatastroFincaParcela(String numFinca, String UbicaionGeografica) " + ex.Message, EventLogEntryType.SuccessAudit); 
            }

            return resultado;
        }


        private Boolean ValidarCertificado(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        protected override void OnStop()
        {
            this.timer.Stop();
            this.timer = null;
        }

        public class DatosCatastrales
        {
            public string AreaCatastral { get; set; }
            public string MapaParcela { get; set; }
            public string Finca { get; set; }
        } 
        //public class DatosCatastrales
        //{
        //    public List<cResumenParcela> ResumenParcela { get; set; }
        //}
        //public class cResumenParcela 
        //{
        //    public string AreaCatastral { get; set; }
        //    public string MapaParcela { get; set; }
        //    public string Finca { get; set; }
            
           
        //} 
    }
    
}
