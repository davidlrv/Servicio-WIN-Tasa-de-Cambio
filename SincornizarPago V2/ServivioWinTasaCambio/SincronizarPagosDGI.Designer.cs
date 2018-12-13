namespace ServivioWinTasaCambio
{
    partial class SincronizarPagosDGI
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
            // 
            // ActualizaTasaCambio
            // 
            this.CanPauseAndContinue = true;
            this.ServiceName = "SincronizarPagosDGI";

        }

        #endregion


        public System.EventHandler timer1_Tick { get; set; }
    }
}
