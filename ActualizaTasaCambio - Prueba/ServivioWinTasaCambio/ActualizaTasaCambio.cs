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

namespace ServivioWinTasaCambio
{
    public partial class ActualizaTasaCambio : ServiceBase
    {       
        string OrigenLog = "Servicio Tasa de Cambio - BCN";
        string Aplicacion = "Application";

        public ActualizaTasaCambio()
        {
            InitializeComponent();
        }
        private System.Timers.Timer timer;
        //Intervalo por defecto hasta encontrar el intervalo correcto-1H
        public double Intervalo = 864000000; //(36000000:UNA HORA)(864000000:UN DIA)

        protected override void OnStart(string[] args)
        {
            //Se detien 1 1/2 Minutos para que inicien todos los demas Servicio del Sistema Operativo(Tarjeta de Red)
            Thread.Sleep(90000);
            Intervalo = ObtenerPeriodoEjecucion();
            //DATOS POS PARAMETRIZADOS
            EventLog.WriteEntry(OrigenLog, "Intervalo: " + ObtenerPeriodoEjecucion() + " Host: " + ObtenerSmtpHost() + " Correo: " + ObtenerSmtpCorreo() + " Clave: " + ObtenerSmtpClave() + " Puerto: " + ObtenerSmtpPuerto());
             
             
            this.timer = new System.Timers.Timer(Intervalo);
            this.timer.AutoReset = true;
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
            this.timer.Start();

            if (!HayInternet())
                EventLog.WriteEntry(OrigenLog, "Error de Comunicacion, No hay Comunicacion con el Servicio Web https://servicios.bcn.gob.ni/Tc_Servicio/ServicioTC.asmx", EventLogEntryType.Error);
            else
            {
                if (!ExisteMesTipoCambio(DateTime.Now.Year, DateTime.Now.Month))
                {//Sino Exite el año-mes Actual en la Base de Datos se manda a Guardar
                    if (GuardarMesTipoCambio(DateTime.Now.Year, DateTime.Now.Month) == true)
                    {
                        EnviarCorreo("Tasa de Cambio Actualizada Correctamente al iniciar el Servicio <br/> " + DateTime.Now.Month + " - " + DateTime.Now.Year + ObtenerTablaTipoCambio(DateTime.Now.Year, DateTime.Now.Month));
                        EventLog.WriteEntry(OrigenLog, "Se Guardo con exito la tasa de cambio del mes: " + DateTime.Now.Month + " - " + DateTime.Now.Year, EventLogEntryType.SuccessAudit);
                    }
                }
            }

        }
        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ConsumirServicioWeb();

            if (!EventLog.SourceExists(OrigenLog))
                EventLog.CreateEventSource(OrigenLog, Aplicacion);
            EventLog.WriteEntry(OrigenLog, "Se Ejecuto con Exito el Ciclo del Servicio Web, Proxima Ejecucion : " + DateTime.Now.AddMilliseconds(ObtenerPeriodoEjecucion()), EventLogEntryType.Information);           
             
        }
        private void ConsumirServicioWeb()
        {
            if (!EventLog.SourceExists(OrigenLog))
                EventLog.CreateEventSource(OrigenLog, Aplicacion);

            if (!HayInternet())
            {
                EventLog.WriteEntry(OrigenLog, "Error de Comunicacion, No hay Comunicacion con el Servicio Web https://servicios.bcn.gob.ni/Tc_Servicio/ServicioTC.asmx", EventLogEntryType.Error);
                EnviarCorreo("Error de Comunicacion, No hay Comunicacion con el Servicio Web https://servicios.bcn.gob.ni/Tc_Servicio/ServicioTC.asmx");                
            }
            else
            {
                int mes = 0;
                int año = 0;

                if (DateTime.Now.Month < 12)
                {
                    mes = DateTime.Now.Month + 1;
                    año = DateTime.Now.Year;
                }
                else
                {
                    mes = 1;
                    año = DateTime.Now.Year + 1;
                }
                if (!ExisteMesTipoCambio(DateTime.Now.Year, DateTime.Now.Month))//Sino Exite el año-mes Actual en la Base de Datos se manda a Guardar
                {
                    if (GuardarMesTipoCambio(DateTime.Now.Year, DateTime.Now.Month) == true)
                    {
                        EnviarCorreo("Se Ejecuto con Exito el Ciclo del Servicio Web, Tasa de Cambio Actualizada Correctamente <br/> Para el Mes: " + DateTime.Now.Month + " - " + DateTime.Now.Year + ObtenerTablaTipoCambio(DateTime.Now.Year, DateTime.Now.Month));
                        EventLog.WriteEntry(OrigenLog, "Se Guardo con exito la tasa de cambio del mes: " + DateTime.Now.Month + " - " + DateTime.Now.Year, EventLogEntryType.SuccessAudit);
                    }
                }
                if (!ExisteMesTipoCambio(año, mes))//Sino Exite el año-mes Siguiente en la Base de Datos: Se manda a Guardar.
                {
                    if (GuardarMesTipoCambio(año, mes) == true)
                    {
                        EnviarCorreo("Se Ejecuto con Exito el Ciclo del Servicio Web, Tasa de Cambio Actualizada Correctamente <br/> Para el Mes: " + mes + " - " + año + ObtenerTablaTipoCambio(año, mes));
                        EventLog.WriteEntry(OrigenLog, "Se Guardo con exito la tasa de cambio del mes: " + mes + " - " + año, EventLogEntryType.SuccessAudit);
                    }
                }
            }
        }
        protected override void OnStop()
        {
            this.timer.Stop();
            this.timer = null;
        }        
        private bool HayInternet()
        {
            try
            {
                System.Net.IPHostEntry host = System.Net.Dns.GetHostEntry("www.bcn.gob.ni");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }        
        private bool ExisteMesTipoCambio(int año,int mes)
        {
            Entities DATOS = new Entities();
            bool Resultado = false;

            try
            {
                var t = DATOS.TipoCambio.Where(c => c.Fecha.Value.Month == mes && c.Fecha.Value.Year == año);
                if (t.Count() > 0)
                    Resultado = true;
                else
                    Resultado = false;
            }
            catch (Exception error)
            {
                if (!EventLog.SourceExists(OrigenLog))
                    EventLog.CreateEventSource(OrigenLog, Aplicacion);
                EventLog.WriteEntry(OrigenLog, "Error, Fallo inesperado al Leer la Informacion de la Base de Datos:<ExisteMesTipoCambio> " + error, EventLogEntryType.Error);

                EnviarCorreo("Error, Fallo inesperado al Leer la Informacion de la Base de Datos:<ExisteMesTipoCambio> " + error);                
                Resultado = true;//Si Fallo el acceso ala Base de Datos
                throw error;
            }

            return Resultado;
        }
        private bool GuardarMesTipoCambio(int año, int mes)
        {
            bool GuardadoCorrecto = false;
            try
            {//consumir el Servicio WEB de BCN  
                WSBCN.Tipo_Cambio_BCNSoapClient SW = new WSBCN.Tipo_Cambio_BCNSoapClient();
                XElement dox = SW.RecuperaTC_Mes(año, mes);

                int i= 0;

                CultureInfo Cultura = new CultureInfo("es-NI");
                                
                foreach (var c in dox.Elements())
                {//Mandar a insertar ala Tabla Tipo de Cambio
                    try
                    {                                              
                        
                        Entities DATOS = new Entities();
                        TipoCambio TC = new TipoCambio();    

                        TC.IdTipoCambio = System.Guid.NewGuid();//Generando el ID
                        TC.Fecha = Convert.ToDateTime(c.Element("Fecha").Value);
                        TC.Valor = Convert.ToDecimal(c.Element("Valor").Value,Cultura);                        
                        TC.IdEstatus = 1;
                        TC.EsActivo = true;
                        TC.FechaRegistro = DateTime.Now;
                        TC.UserName = System.Net.Dns.GetHostName();
                        TC.UserIP = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName())[1].ToString();
                        TC.UserPC = System.Net.Dns.GetHostName();
                        DATOS.AddToTipoCambio(TC); 
                        DATOS.SaveChanges();
                        i++;
                    }
                    catch (Exception error)
                    {
                        if (!EventLog.SourceExists(OrigenLog))
                            EventLog.CreateEventSource(OrigenLog, Aplicacion);
                        EventLog.WriteEntry(OrigenLog, "Error, Fallo inesperado al Guardar la Informacion en la Base de Datos: " + error, EventLogEntryType.Error);
                        GuardadoCorrecto = false;
                        EnviarCorreo("Error, Fallo inesperado al Guardar la Informacion en la Base de Datos: " + error);
                        throw error;
                    }                    
                          
                } 
               if (i == 0)
                {
                    if (!EventLog.SourceExists(OrigenLog))
                        EventLog.CreateEventSource(OrigenLog, Aplicacion);
                    EventLog.WriteEntry(OrigenLog, "Notificación, No esta disponible la informacion del Tipo de Cambio para el mes:" + mes + "-" + año, EventLogEntryType.Information);
                    GuardadoCorrecto = false;
                    EnviarCorreo("Notificación, No esta disponible la informacion del Tipo de Cambio para el mes: " + mes + "-" + año);
                    return false;
                }


                GuardadoCorrecto = true;
              
            }
            catch (Exception error)
            {
                if (!EventLog.SourceExists(OrigenLog))
                    EventLog.CreateEventSource(OrigenLog, Aplicacion);
                EventLog.WriteEntry(OrigenLog, "Error, Fallo inesperado al consumir el Servicio web: " + error, EventLogEntryType.Error);
                GuardadoCorrecto = false;
                EnviarCorreo("Error, Fallo inesperado al consumir el Servicio web: " + error);
            }

            return GuardadoCorrecto;                                
             
        }
        private double ObtenerPeriodoEjecucion()
        {
            Entities DATOS = new Entities();
            double Resultado = Intervalo;
            try
            {
                var p = DATOS.vwXSAValorDominio.Where(c=> c.NombreDominio == "Periodo Ejecucion Tasa Cambio Segundos" && c.EsActivoDominio == "S" && c.EsActivoValor == "S").FirstOrDefault();
                Resultado = Intervalo = double.Parse(p.NombreValor);  //Retorna el Intervalo y lo actualiza en la variable Gloval               
            }
            catch (Exception error)
            {
                if (!EventLog.SourceExists(OrigenLog))
                    EventLog.CreateEventSource(OrigenLog, Aplicacion);
                EventLog.WriteEntry(OrigenLog, "Error, Fallo inesperado al Leer el  vwXSAValorDominio.NombreValor de <Periodo Ejecucion Tasa Cambio Segundos>: " + error, EventLogEntryType.Error);
                throw error;
            }
            return Resultado;
        
        }
        private void EnviarCorreo(string Mensaje)
        {
            MailMessage Correo = new MailMessage();
            foreach(var d in ObtenerDestinatarios())  
                Correo.To.Add(new MailAddress(d.NombreValor.ToString()));

            Correo.From = new MailAddress(ObtenerSmtpCorreo());
            Correo.Subject = "Actualizacion de la Tasa de Cambio";            
            Correo.IsBodyHtml = true;
            Correo.Body = Mensaje;
            

            SmtpClient clienteSmtp = new SmtpClient(ObtenerSmtpHost());
            clienteSmtp.Credentials = new NetworkCredential(ObtenerSmtpCorreo().Split('@')[0], ObtenerSmtpClave());
            clienteSmtp.Port = int.Parse(ObtenerSmtpPuerto());
            try
            {
                clienteSmtp.Send(Correo);
            }
            catch (Exception error)
            {
                if (!EventLog.SourceExists(OrigenLog))
                    EventLog.CreateEventSource(OrigenLog, Aplicacion);
                EventLog.WriteEntry(OrigenLog, "Error, Fallo inesperado al querer mandar Corro Electronico de la Actualizacion de la Tasa Cambio: " + error, EventLogEntryType.Error);
                throw error;
            }           
        
        }
        private List<vwXSAValorDominio> ObtenerDestinatarios()
        {
            Entities DATOS = new Entities();
            List<vwXSAValorDominio> Resultado;
            try
            {
                var p = DATOS.vwXSAValorDominio.Where(c => c.NombreDominio == "Correo Notificacion Tasa Cambio" && c.EsActivoDominio == "S" && c.EsActivoValor == "S");
               Resultado = p.ToList();                   
            }
            catch (Exception error)
            {
                if (!EventLog.SourceExists(OrigenLog))
                    EventLog.CreateEventSource(OrigenLog, Aplicacion);
                EventLog.WriteEntry(OrigenLog, "Error, Fallo inesperado al Leer el  vwXSAValorDominio.NombreValor de <Correo Notificacion Tasa Cambio>: " + error, EventLogEntryType.Error);
                throw error;
            }
            return Resultado;        
          }
        private string ObtenerSmtpHost()
        {
            Entities DATOS = new Entities();
            string Resultado;
            try
            {
                var p = DATOS.vwXSAValorDominio.Where(c => c.NombreDominio == "ClienteSmtpHost" && c.EsActivoDominio == "S" && c.EsActivoValor == "S").FirstOrDefault();
                Resultado = p.NombreValor;
            }
            catch (Exception error)
            {
                if (!EventLog.SourceExists(OrigenLog))
                    EventLog.CreateEventSource(OrigenLog, Aplicacion);
                EventLog.WriteEntry(OrigenLog, "Error, Fallo inesperado al Leer el  vwXSAValorDominio.NombreValor de <ClienteSmtpHost>: " + error, EventLogEntryType.Error);
                throw error;
            }
            return Resultado;
        }
        private string ObtenerSmtpCorreo()
        {
            Entities DATOS = new Entities();
            string Resultado;
            try
            {
                var p = DATOS.vwXSAValorDominio.Where(c => c.NombreDominio == "ClienteSmtpCorreo" && c.EsActivoDominio == "S" && c.EsActivoValor == "S").FirstOrDefault();
                Resultado = p.NombreValor;
            }
            catch (Exception error)
            {
                if (!EventLog.SourceExists(OrigenLog))
                    EventLog.CreateEventSource(OrigenLog, Aplicacion);
                EventLog.WriteEntry(OrigenLog, "Error, Fallo inesperado al Leer el  vwXSAValorDominio.NombreValor de <ClienteSmtpCorreo>: " + error, EventLogEntryType.Error);
                throw error;
            }
            return Resultado;
        }
        private string ObtenerSmtpClave()
        {
            Entities DATOS = new Entities();
            string Resultado;
            try
            {
                var p = DATOS.vwXSAValorDominio.Where(c => c.NombreDominio == "ClienteSmtpClave" && c.EsActivoDominio == "S" && c.EsActivoValor == "S").FirstOrDefault();
                Resultado = p.NombreValor;
            }
            catch (Exception error)
            {
                if (!EventLog.SourceExists(OrigenLog))
                    EventLog.CreateEventSource(OrigenLog, Aplicacion);
                EventLog.WriteEntry(OrigenLog, "Error, Fallo inesperado al Leer el  vwXSAValorDominio.NombreValor de <ClienteSmtpClave>: " + error, EventLogEntryType.Error);
                throw error;
            }
            return Resultado;
        }
        private string ObtenerSmtpPuerto()
        {
            Entities DATOS = new Entities();
            string Resultado;
            try
            {
                var p = DATOS.vwXSAValorDominio.Where(c => c.NombreDominio == "ClienteSmtpPuerto" && c.EsActivoDominio == "S" && c.EsActivoValor == "S").FirstOrDefault();
                Resultado = p.NombreValor;
            }
            catch (Exception error)
            {
                if (!EventLog.SourceExists(OrigenLog))
                    EventLog.CreateEventSource(OrigenLog, Aplicacion);
                EventLog.WriteEntry(OrigenLog, "Error, Fallo inesperado al Leer el  vwXSAValorDominio.NombreValor de <ClienteSmtpPuerto>: " + error, EventLogEntryType.Error);
                throw error;
            }
            return Resultado;
        }
        private string ObtenerTablaTipoCambio(int año, int mes)
        {
            Entities DATOS = new Entities();
            List<TipoCambio> TC;
            string TablaTC = "" +
                                "<html> " +
                                "<head> " +
                                "<style> " +
                                "table, th, td { " +
                                "    border: 1px solid black; " +
                                "    border-collapse: collapse; " +
                                "} " +
                                "th, td { " +
                                "    padding: 5px; " +
                                "    text-align: left; " +
                                "} " +
                                "</style> " +
                                "</head> " +
                                "<body> " +
                                "<table style=width:20%> " +
                                "  <caption>Tasa de Cambio</caption> " +
                                "  <tr> " +
                                "    <th>Fecha</th> " +
                                "    <th>Valor</th> " +
                                "  </tr> ";

            try
            {
                TC = DATOS.TipoCambio.Where(c => c.Fecha.Value.Month == mes && c.Fecha.Value.Year == año).OrderBy(c=> c.Fecha).ToList();
                foreach (var item in TC)
                     TablaTC +=  "  <tr> " +
                                    "    <td>" + Convert.ToDateTime(item.Fecha).ToString("dd/MM/yyyy") + "</td> " +
                                    "    <td>" + item.Valor.ToString() + "</td> " +
                                    " </tr> ";
            }
            catch (Exception error)
            {
                if (!EventLog.SourceExists(OrigenLog))
                    EventLog.CreateEventSource(OrigenLog, Aplicacion);
                EventLog.WriteEntry(OrigenLog, "Error, Fallo inesperado al Leer el la tabla <TipoCambios>: " + error, EventLogEntryType.Error);
                throw error;
            }

            TablaTC += "</table> " +
                               "</body> " +
                               "</html>";
            return TablaTC;
        
        }

    }
    
}
