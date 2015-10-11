using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sofft.ViewComunes;

namespace Sueldos.View.Dialogos
{
    /// <summary>
    /// Formulario para la seleccion de A�o y Mes definidos en tablas, utilizando combos
    /// </summary>
    public partial class frmSeleccionAnioMes : Form
    {
        /// <summary>
        /// Crea una nueva instancia del formulario de dialog
        /// </summary>
        public frmSeleccionAnioMes()
        { InitializeComponent(); }

        /// <summary>
        /// Obtiene el numero de A�o seleccionado
        /// </summary>
        public int Anio
        { get { return Convert.ToInt32(this.cmbAnios.SelectedValue); } }

        /// <summary>
        /// Obtiene el n�mero de Mes seleccionado
        /// </summary>
        public int Mes
        { get { return Convert.ToInt32(this.cmbMeses.SelectedValue); } }

        /// <summary>
        /// Obtiene el nombre del Mes seleccionado
        /// </summary>
        public int MesDescripcion
        { get { return Convert.ToInt32(this.cmbMeses.Text); } }

        /// <summary>
        /// Obtiene el A�o y Mes en formato numerico AAAAMM
        /// </summary>
        public int AnioMes
        { get { return Convert.ToInt32(this.cmbAnios.SelectedValue) * 100 + Convert.ToInt32(this.cmbMeses.SelectedValue); } }

        /// <summary>
        /// Obtiene el A�o y Mes en formato texto Mes y A�o
        /// </summary>
        public string AnioMesDescripcion
        { get { return this.cmbMeses.Text + " " + this.cmbAnios.Text; } }

        private void frmSeleccionAnioMes_Load(object sender, EventArgs e)
        {
            Controles.cargaComboBox(this.cmbAnios, "contenido", "contenido", "tablasConsultarContenidoyDetalle", "@tabla", "calendario", "indice", 1);
            this.cmbAnios.SelectedValue = DateTime.Now.Year;
            Controles.cargaComboBox(this.cmbMeses, "detalle", "contenido", "tablasConsultarContenidoyDetalle", "@tabla", "calendario", "indice", 2);
            this.cmbMeses.SelectedValue = DateTime.Now.Month;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}