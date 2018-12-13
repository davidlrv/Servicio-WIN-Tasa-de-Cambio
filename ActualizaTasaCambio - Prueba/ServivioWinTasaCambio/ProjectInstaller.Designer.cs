namespace ServivioWinTasaCambio
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.ServicioProcesoInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.ServicioInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // ServicioProcesoInstaller
            // 
            this.ServicioProcesoInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.ServicioProcesoInstaller.Password = null;
            this.ServicioProcesoInstaller.Username = null;
            // 
            // ServicioInstaller
            // 
            this.ServicioInstaller.Description = "Servicio Windows que Consume el Web Service del BCN, y manda Actualizar la Tabla " +
    "TipoCambio";
            this.ServicioInstaller.DisplayName = "CSJ_OBTENER_TIPO_CAMBIO_BCN2";
            this.ServicioInstaller.ServiceName = "CSJ_OBTENER_TIPO_CAMBIO_BCN2";
            this.ServicioInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.ServicioProcesoInstaller,
            this.ServicioInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller ServicioProcesoInstaller;
        private System.ServiceProcess.ServiceInstaller ServicioInstaller;
    }
}