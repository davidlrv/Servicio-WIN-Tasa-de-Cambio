﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServivioWinTasaCambio.wsPagoDGI {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DetalleCatalogoInstitucion", Namespace="http://schemas.datacontract.org/2004/07/Entity")]
    [System.SerializableAttribute()]
    public partial class DetalleCatalogoInstitucion : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ServivioWinTasaCambio.wsPagoDGI.Enti_CatalogoConcepto[] ListaCatalogoConceptoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string nombreField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public ServivioWinTasaCambio.wsPagoDGI.Enti_CatalogoConcepto[] ListaCatalogoConcepto {
            get {
                return this.ListaCatalogoConceptoField;
            }
            set {
                if ((object.ReferenceEquals(this.ListaCatalogoConceptoField, value) != true)) {
                    this.ListaCatalogoConceptoField = value;
                    this.RaisePropertyChanged("ListaCatalogoConcepto");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string nombre {
            get {
                return this.nombreField;
            }
            set {
                if ((object.ReferenceEquals(this.nombreField, value) != true)) {
                    this.nombreField = value;
                    this.RaisePropertyChanged("nombre");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Enti_CatalogoConcepto", Namespace="http://schemas.datacontract.org/2004/07/Entity")]
    [System.SerializableAttribute()]
    public partial class Enti_CatalogoConcepto : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ServivioWinTasaCambio.wsPagoDGI.Enti_DetalleConceptos[] ListDetalleConceptosField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int catalogoconcepto_idField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string codigoconceptoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string codigoimpuestoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int tipoField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public ServivioWinTasaCambio.wsPagoDGI.Enti_DetalleConceptos[] ListDetalleConceptos {
            get {
                return this.ListDetalleConceptosField;
            }
            set {
                if ((object.ReferenceEquals(this.ListDetalleConceptosField, value) != true)) {
                    this.ListDetalleConceptosField = value;
                    this.RaisePropertyChanged("ListDetalleConceptos");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int catalogoconcepto_id {
            get {
                return this.catalogoconcepto_idField;
            }
            set {
                if ((this.catalogoconcepto_idField.Equals(value) != true)) {
                    this.catalogoconcepto_idField = value;
                    this.RaisePropertyChanged("catalogoconcepto_id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string codigoconcepto {
            get {
                return this.codigoconceptoField;
            }
            set {
                if ((object.ReferenceEquals(this.codigoconceptoField, value) != true)) {
                    this.codigoconceptoField = value;
                    this.RaisePropertyChanged("codigoconcepto");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string codigoimpuesto {
            get {
                return this.codigoimpuestoField;
            }
            set {
                if ((object.ReferenceEquals(this.codigoimpuestoField, value) != true)) {
                    this.codigoimpuestoField = value;
                    this.RaisePropertyChanged("codigoimpuesto");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int tipo {
            get {
                return this.tipoField;
            }
            set {
                if ((this.tipoField.Equals(value) != true)) {
                    this.tipoField = value;
                    this.RaisePropertyChanged("tipo");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Enti_DetalleConceptos", Namespace="http://schemas.datacontract.org/2004/07/Entity")]
    [System.SerializableAttribute()]
    public partial class Enti_DetalleConceptos : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string descripcionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string incisoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<decimal> montoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> tipomontoField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string descripcion {
            get {
                return this.descripcionField;
            }
            set {
                if ((object.ReferenceEquals(this.descripcionField, value) != true)) {
                    this.descripcionField = value;
                    this.RaisePropertyChanged("descripcion");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string inciso {
            get {
                return this.incisoField;
            }
            set {
                if ((object.ReferenceEquals(this.incisoField, value) != true)) {
                    this.incisoField = value;
                    this.RaisePropertyChanged("inciso");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<decimal> monto {
            get {
                return this.montoField;
            }
            set {
                if ((this.montoField.Equals(value) != true)) {
                    this.montoField = value;
                    this.RaisePropertyChanged("monto");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> tipomonto {
            get {
                return this.tipomontoField;
            }
            set {
                if ((this.tipomontoField.Equals(value) != true)) {
                    this.tipomontoField = value;
                    this.RaisePropertyChanged("tipomonto");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Pagos", Namespace="http://schemas.datacontract.org/2004/07/Entity")]
    [System.SerializableAttribute()]
    public partial class Pagos : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool activoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string bancoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string cedulaField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string codigoimpuestoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string codigoservicioField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime fechapagoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal montoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string nodocumentoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string nombrebeneficiarioField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string voucherField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool activo {
            get {
                return this.activoField;
            }
            set {
                if ((this.activoField.Equals(value) != true)) {
                    this.activoField = value;
                    this.RaisePropertyChanged("activo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string banco {
            get {
                return this.bancoField;
            }
            set {
                if ((object.ReferenceEquals(this.bancoField, value) != true)) {
                    this.bancoField = value;
                    this.RaisePropertyChanged("banco");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string cedula {
            get {
                return this.cedulaField;
            }
            set {
                if ((object.ReferenceEquals(this.cedulaField, value) != true)) {
                    this.cedulaField = value;
                    this.RaisePropertyChanged("cedula");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string codigoimpuesto {
            get {
                return this.codigoimpuestoField;
            }
            set {
                if ((object.ReferenceEquals(this.codigoimpuestoField, value) != true)) {
                    this.codigoimpuestoField = value;
                    this.RaisePropertyChanged("codigoimpuesto");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string codigoservicio {
            get {
                return this.codigoservicioField;
            }
            set {
                if ((object.ReferenceEquals(this.codigoservicioField, value) != true)) {
                    this.codigoservicioField = value;
                    this.RaisePropertyChanged("codigoservicio");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime fechapago {
            get {
                return this.fechapagoField;
            }
            set {
                if ((this.fechapagoField.Equals(value) != true)) {
                    this.fechapagoField = value;
                    this.RaisePropertyChanged("fechapago");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal monto {
            get {
                return this.montoField;
            }
            set {
                if ((this.montoField.Equals(value) != true)) {
                    this.montoField = value;
                    this.RaisePropertyChanged("monto");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string nodocumento {
            get {
                return this.nodocumentoField;
            }
            set {
                if ((object.ReferenceEquals(this.nodocumentoField, value) != true)) {
                    this.nodocumentoField = value;
                    this.RaisePropertyChanged("nodocumento");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string nombrebeneficiario {
            get {
                return this.nombrebeneficiarioField;
            }
            set {
                if ((object.ReferenceEquals(this.nombrebeneficiarioField, value) != true)) {
                    this.nombrebeneficiarioField = value;
                    this.RaisePropertyChanged("nombrebeneficiario");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string voucher {
            get {
                return this.voucherField;
            }
            set {
                if ((object.ReferenceEquals(this.voucherField, value) != true)) {
                    this.voucherField = value;
                    this.RaisePropertyChanged("voucher");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="wsPagoDGI.IWsTramitesInstitucionLinea")]
    public interface IWsTramitesInstitucionLinea {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWsTramitesInstitucionLinea/listCatalogos", ReplyAction="http://tempuri.org/IWsTramitesInstitucionLinea/listCatalogosResponse")]
        ServivioWinTasaCambio.wsPagoDGI.DetalleCatalogoInstitucion[] listCatalogos(string usr, string pwd);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWsTramitesInstitucionLinea/listPagos", ReplyAction="http://tempuri.org/IWsTramitesInstitucionLinea/listPagosResponse")]
        ServivioWinTasaCambio.wsPagoDGI.Pagos[] listPagos(System.DateTime fecha, string usr, string pwd);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWsTramitesInstitucionLinea/listPagosServicio", ReplyAction="http://tempuri.org/IWsTramitesInstitucionLinea/listPagosServicioResponse")]
        ServivioWinTasaCambio.wsPagoDGI.Pagos[] listPagosServicio(System.DateTime fechainicial, System.DateTime fechafinal, string codigoservicio, string codigoimpuesto, string usr, string pwd);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWsTramitesInstitucionLinea/pagoServicio", ReplyAction="http://tempuri.org/IWsTramitesInstitucionLinea/pagoServicioResponse")]
        ServivioWinTasaCambio.wsPagoDGI.Pagos pagoServicio(string sif, string usr, string pwd);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWsTramitesInstitucionLinea/listAnuladosPagos", ReplyAction="http://tempuri.org/IWsTramitesInstitucionLinea/listAnuladosPagosResponse")]
        ServivioWinTasaCambio.wsPagoDGI.Pagos[] listAnuladosPagos(System.DateTime fecha, string usr, string pwd);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWsTramitesInstitucionLinea/listPagosDocumento", ReplyAction="http://tempuri.org/IWsTramitesInstitucionLinea/listPagosDocumentoResponse")]
        ServivioWinTasaCambio.wsPagoDGI.Pagos[] listPagosDocumento(string nodocumento, string usr, string pwd);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IWsTramitesInstitucionLineaChannel : ServivioWinTasaCambio.wsPagoDGI.IWsTramitesInstitucionLinea, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WsTramitesInstitucionLineaClient : System.ServiceModel.ClientBase<ServivioWinTasaCambio.wsPagoDGI.IWsTramitesInstitucionLinea>, ServivioWinTasaCambio.wsPagoDGI.IWsTramitesInstitucionLinea {
        
        public WsTramitesInstitucionLineaClient() {
        }
        
        public WsTramitesInstitucionLineaClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WsTramitesInstitucionLineaClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WsTramitesInstitucionLineaClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WsTramitesInstitucionLineaClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public ServivioWinTasaCambio.wsPagoDGI.DetalleCatalogoInstitucion[] listCatalogos(string usr, string pwd) {
            return base.Channel.listCatalogos(usr, pwd);
        }
        
        public ServivioWinTasaCambio.wsPagoDGI.Pagos[] listPagos(System.DateTime fecha, string usr, string pwd) {
            return base.Channel.listPagos(fecha, usr, pwd);
        }
        
        public ServivioWinTasaCambio.wsPagoDGI.Pagos[] listPagosServicio(System.DateTime fechainicial, System.DateTime fechafinal, string codigoservicio, string codigoimpuesto, string usr, string pwd) {
            return base.Channel.listPagosServicio(fechainicial, fechafinal, codigoservicio, codigoimpuesto, usr, pwd);
        }
        
        public ServivioWinTasaCambio.wsPagoDGI.Pagos pagoServicio(string sif, string usr, string pwd) {
            return base.Channel.pagoServicio(sif, usr, pwd);
        }
        
        public ServivioWinTasaCambio.wsPagoDGI.Pagos[] listAnuladosPagos(System.DateTime fecha, string usr, string pwd) {
            return base.Channel.listAnuladosPagos(fecha, usr, pwd);
        }
        
        public ServivioWinTasaCambio.wsPagoDGI.Pagos[] listPagosDocumento(string nodocumento, string usr, string pwd) {
            return base.Channel.listPagosDocumento(nodocumento, usr, pwd);
        }
    }
}
