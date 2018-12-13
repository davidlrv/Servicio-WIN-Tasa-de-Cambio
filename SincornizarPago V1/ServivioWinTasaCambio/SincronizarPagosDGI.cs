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
using ServivioWinTasaCambio.wsPagoDGI;

namespace ServivioWinTasaCambio
{
    public partial class SincronizarPagosDGI : ServiceBase
    {       
        string OrigenLog = "Sincronizar Pago DGI";
        string Aplicacion = "Application";
       
        private System.Timers.Timer timer;
        //Intervalo por defecto hasta encontrar el intervalo correcto-1H
        public double Intervalo = 36000000; //(36000000:UNA HORA)(864000000:UN DIA)

        public SincronizarPagosDGI()
        {
            InitializeComponent();
        }
       
                     
        protected override void OnStart(string[] args)
        {
            Intervalo = double.Parse(ConfigurationSettings.AppSettings["Intervalo"]);
            
            this.timer = new System.Timers.Timer(Intervalo);
            this.timer.AutoReset = true;
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
            this.timer.Start();
            
            int r = SincronizarPagoDGI();
            if (r > 0)
            {
                if (!EventLog.SourceExists(OrigenLog))
                    EventLog.CreateEventSource(OrigenLog, Aplicacion);
                EventLog.WriteEntry(OrigenLog, "Se Inicio con Exito el Servicio Web, Proxima Ejecucion : " + DateTime.Now.AddMilliseconds(Intervalo).ToString("dd/MM/yyyy hh:mm:ss tt"), EventLogEntryType.Information);
                EventLog.WriteEntry(OrigenLog, "Se Sincronizaron  : " + r + " Pagos", EventLogEntryType.SuccessAudit);
                
            }
            else
            { 
                if (!EventLog.SourceExists(OrigenLog))
                    EventLog.CreateEventSource(OrigenLog, Aplicacion);
                EventLog.WriteEntry(OrigenLog, "Se Inicio con Exito el Servicio Web, Proxima Ejecucion : " + DateTime.Now.AddMilliseconds(Intervalo).ToString("dd/MM/yyyy hh:mm:ss tt"), EventLogEntryType.Information);
             
            }
            
            
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            int r = SincronizarPagoDGI();
            if (r > 0)
            {
                if (!EventLog.SourceExists(OrigenLog))
                    EventLog.CreateEventSource(OrigenLog, Aplicacion);
                EventLog.WriteEntry(OrigenLog, "Se Ejecuto con Exito el Ciclo del Servicio Web, Proxima Ejecucion : " + DateTime.Now.AddMilliseconds(Intervalo).ToString("dd/MM/yyyy hh:mm:ss tt"), EventLogEntryType.Information);
                EventLog.WriteEntry(OrigenLog, "Se Sincronizaron  : " + r + " Pagos", EventLogEntryType.SuccessAudit);
            }
            else
            {
                if (!EventLog.SourceExists(OrigenLog))
                    EventLog.CreateEventSource(OrigenLog, Aplicacion);
                EventLog.WriteEntry(OrigenLog, "Se Ejecuto con Exito el Ciclo del Servicio Web, Proxima Ejecucion : " + DateTime.Now.AddMilliseconds(Intervalo).ToString("dd/MM/yyyy hh:mm:ss tt"), EventLogEntryType.Information);
            }

        }

        private int SincronizarPagoDGI()
        {
            int resultado = 0;
            try
            {
                CultureInfo Cultura = new CultureInfo("es-NI");

                WsTramitesInstitucionLineaClient wcfClient = new WsTramitesInstitucionLineaClient();

                string userws = ConfigurationSettings.AppSettings["wcfTramiteDGIUser"];
                string passws = ConfigurationSettings.AppSettings["wcfTramiteDGIPass"];

                int anio = DateTime.Now.Year;
                int mes = DateTime.Now.Month;
                int dia = DateTime.Now.Day;
                DateTime Hoy = DateTime.Parse(anio.ToString() + "/" + mes.ToString() + "/" + dia);

                List<ServivioWinTasaCambio.wsPagoDGI.Pagos> pago = wcfClient.listPagos(Hoy, userws, passws).ToList();

                foreach (var p in pago)
                {
                    //Insertar si no existe el pago en la base de dato local y si es activo en la DGI
                    if (!ExistePagoLocal(p.nodocumento) && p.activo == true)
                    {
                        try
                        {
                            TasacionEnLineaEntities DATO = new TasacionEnLineaEntities();
                            PagoDocumento pd = new PagoDocumento();

                            pd.IdPago = Guid.NewGuid();
                            pd.NoDocSoporteBco = p.voucher;

                            var IdCotizacionWeb = ObtenerIdCotizacionWeb(p.nodocumento);
                            if (IdCotizacionWeb != Guid.Empty)
                                pd.IdCotizacionWeb = IdCotizacionWeb;

                            pd.MontoPago = p.monto;
                            pd.IndPago = "1";
                            pd.FechaPago = p.fechapago;
                            pd.HoraPago = p.fechapago.ToShortTimeString();
                            pd.IdEstadoPago = ObtenerCodigoValorDominio("Pago Correcto");
                            pd.SucBco = "N/D";
                            pd.CajaBanco = "N/D";
                            pd.PagoConciliado = false;
                            pd.Identificacion = p.cedula;
                            pd.Comentario = "N/D";
                            pd.siglasBanco = p.banco;
                            pd.NoDocumentoPagado = p.nodocumento;

                            DATO.AddToPagoDocumento(pd);
                            DATO.SaveChanges();

                            resultado++;
                        }
                        catch (Exception Error)
                        {

                            if (!EventLog.SourceExists(OrigenLog))
                                EventLog.CreateEventSource(OrigenLog, Aplicacion);
                            EventLog.WriteEntry(OrigenLog, "Error al Guardar en la tabla <TasacionEnLinea.WEB.PagoDocumento>" + Error.Message, EventLogEntryType.Error);
                        }
                        
                    }

                }
            }
            catch (Exception e)
            {

                if (!EventLog.SourceExists(OrigenLog))
                    EventLog.CreateEventSource(OrigenLog, Aplicacion);
                EventLog.WriteEntry(OrigenLog, "Error al consumir el Servicio: <http://wstramiteslineapruebas.dgi.gob.ni/WsTramitesInstitucionLinea.svc> " + e.Message, EventLogEntryType.Error);
            }
            
            return resultado;
        }

        private bool ExistePagoLocal(string NoDocumentoPagado)
        {
            bool Resultado = false;

            try
            {
                TasacionEnLineaEntities DATA = new TasacionEnLineaEntities();
                var IdEstadoPago = ObtenerCodigoValorDominio("Pago Correcto");
                var Resul = DATA.PagoDocumento.Where(x => x.NoDocumentoPagado.Equals(NoDocumentoPagado) && x.IdEstadoPago.Equals(IdEstadoPago)).FirstOrDefault();

                if (Resul != null)
                {
                    Resultado = true;
                }
                else Resultado = false;
            }
            catch (Exception e)
            {
                if (!EventLog.SourceExists(OrigenLog))
                    EventLog.CreateEventSource(OrigenLog, Aplicacion);
                EventLog.WriteEntry(OrigenLog, "Error al Leer la tabla <TasacionEnLinea.WEB.PagoDocumento> NoDocumentoPagado: " + NoDocumentoPagado + " Catch: " + e.Message, EventLogEntryType.Error);

                Resultado = false;
            }


            return Resultado;
        }

        private int ObtenerCodigoValorDominio(string NombreValor)
        {
            int CodigoValorDominio = 0;

            try
            {
                TasacionEnLineaEntities DATA = new TasacionEnLineaEntities();
                CodigoValorDominio = DATA.vwXSAValorDominio.Where(x => x.NombreValor.Equals(NombreValor)).FirstOrDefault().CodigoValor;

            }
            catch (Exception e)
            {
                if (!EventLog.SourceExists(OrigenLog))
                    EventLog.CreateEventSource(OrigenLog, Aplicacion);
                EventLog.WriteEntry(OrigenLog, "Error al Obtener Valor Dominio de " + NombreValor + " Catch: " + e.Message, EventLogEntryType.Error);

                throw;
            }

            return CodigoValorDominio;

        }

        private Guid ObtenerIdCotizacionWeb(string Nocumento)
        {
            Guid IdCotizacionWeb = Guid.Empty;

            try
            {
                TasacionEnLineaEntities DATA = new TasacionEnLineaEntities();
                var cw = DATA.CotizacionWeb.Where(x => x.NoDocumento.Equals(Nocumento)).FirstOrDefault();
                if (cw != null)
                    IdCotizacionWeb = cw.IdCotizacionWeb;


            }
            catch (Exception e)
            {

                if (!EventLog.SourceExists(OrigenLog))
                    EventLog.CreateEventSource(OrigenLog, Aplicacion);
                EventLog.WriteEntry(OrigenLog, "Error al Obtener Valor CotizacionWeb.IdCotizacionWeb del Nocumento: " + Nocumento + " Carch: " + e.Message, EventLogEntryType.Error);

                throw;
            }
            return IdCotizacionWeb;
        }

        private Guid ObtenerIdCuenta(string CodCta)
        {
            Guid IdCuenta;

            try
            {
                TasacionEnLineaEntities DATA = new TasacionEnLineaEntities();
                IdCuenta = DATA.BancoCuenta.Where(x => x.CodCta.Equals(CodCta)).FirstOrDefault().IdCuenta;
            }
            catch (Exception e)
            {
                if (!EventLog.SourceExists(OrigenLog))
                    EventLog.CreateEventSource(OrigenLog, Aplicacion);
                EventLog.WriteEntry(OrigenLog, "Error al Obtener Valor BancoCuenta.CodCta " + CodCta + " Carch: " + e.Message, EventLogEntryType.Error);

                throw;
            }
            return IdCuenta;
        }
           
        protected override void OnStop()
        {
            this.timer.Stop();
            this.timer = null;
        }
    }
    
}
