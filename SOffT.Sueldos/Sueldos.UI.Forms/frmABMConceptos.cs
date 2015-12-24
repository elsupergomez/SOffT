//
//  frmABMConceptos.cs
//
//  Author:
//       Hernan Vivani <hernan@vivani.com.ar> - http://hvivani.com.ar
//       Claudio Rodrigo Pereyra Diaz <claudiorodrigo@pereyradiaz.com.ar>
//
//  Copyright (c) 2010 SOffT - http://www.sofft.com.ar
//  Copyright (c) 2015 Hamekoz - www.hamekoz.com.ar
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Windows.Forms;
using Hamekoz.Data;
using Sofft.Utils;
using Sofft.UI.Forms;
using Sueldos.Core;

namespace Sueldos.View
{
	public partial class frmABMConceptos : Form
	{
		Concepto concepto;

		public frmABMConceptos ()
		{
			InitializeComponent ();
			dgvCalculo.Click += dgvCalculo_Click;
			dgvCalculo.KeyDown += dgvCalculo_KeyDown;
		}

		void dgvCalculo_KeyDown (object sender, KeyEventArgs e)
		{
			cargaConcepto ();
		}

		void dgvCalculo_Click (object sender, EventArgs e)
		{
			cargaConcepto ();
		}

		void mostrarDatosEnForm ()
		{
			txtCodigo.Text = concepto.Codigo.ToString ();
			txtOrdPro.Text = concepto.OrdenDeProceso.ToString ();
			txtDescripcion.Text = concepto.Descripcion;
			cmbTipo.SelectedValue = concepto.Tipo;
			chkImprime.Checked = concepto.Imprime;
			chkImprimeCantidad.Checked = concepto.ImprimeCantidad;
			chkImprimeValorUnitario.Checked = concepto.ImprimeValorUnitario;
			chkDesactivado.Checked = concepto.Desactivado;
			txtFormula.Text = concepto.Formula;
			cmbTipoLiquidacion.SelectedValue = concepto.IdTipoLiquidacion;
			cmbAplicacion.SelectedValue = concepto.IdAplicacion;
			cmbCuentaContable.SelectedValue = concepto.IdCuentaContable;
		}

		public void abrirParaSueldos ()
		{
			Text = "ABM de Conceptos";
			cargarCombos ();
			Controles.CargaComboBox (cmbTablas, "descripcion", "indice", DB.Instancia.SPToDataSet ("tablasConsultarParaSueldos", "@nombre", "Calculo"));
			ShowDialog ();
		}

		public void abrirParaIndice (int indice)
		{
			Text = "Edición de Formulas";
			cargarCombos ();
			Controles.CargaComboBox (cmbTablas, "descripcion", "indice", DB.Instancia.SPToDataSet ("tablasConsultarPorIndice", "@nombre", "Calculo", "@indice", indice));
			ShowDialog ();
		}

		public void abrirParaAsientosDeSueldos ()
		{
			Text = "Formulas para Asientos de Sueldos";
			cargarCombos ();
			Controles.CargaComboBox (cmbTablas, "descripcion", "indice", DB.Instancia.SPToDataSet ("tablasConsultarParaAsientos", "@nombre", "Calculo"));
			ShowDialog ();
		}

		void btnCerrar_Click (object sender, EventArgs e)
		{
			Close ();
		}

		void cargaConcepto ()
		{
			concepto = null;
			concepto = new Concepto (Convert.ToInt32 (Controles.ConsultaCampoRenglon (dgvCalculo, 0)), Convert.ToInt32 (cmbTablas.SelectedValue.ToString ()));
			concepto.cargarDatosConcepto ();
			mostrarDatosEnForm ();
		}

		void btnModificar_Click (object sender, EventArgs e)
		{
			accionModificar ();
		}

		void btnCancelar_Click (object sender, EventArgs e)
		{
			accionCancelar ();
			limpiaCampos ();
			actualizaGrillaFormulas ();
		}

		void cmbTablas_SelectedIndexChanged (object sender, EventArgs e)
		{
			actualizaGrillaFormulas ();
		}

		void frmABMConceptos_Load (object sender, EventArgs e)
		{
			accionCancelar ();           
		}

		void cargarCombos ()
		{
			Controles.CargaComboBox (cmbTipo, "detalle", "contenido", DB.Instancia.SPToDataSet ("tablasConsultarContenidoyDetalle", "tabla", "Varios", "indice", 50));
			Controles.CargaComboBox (cmbTipoLiquidacion, "descripcion", "indice", DB.Instancia.SPToDataSet ("tablasConsultar", "@nombre", "tiposDeLiquidacion"));
			Controles.CargaComboBox (cmbAplicacion, "descripcion", "indice", DB.Instancia.SPToDataSet ("tablasConsultar", "@nombre", "aplicacionLiquida"));
			Controles.CargaComboBox (cmbCuentaContable, "descripcion", "indice", DB.Instancia.SPToDataSet ("tablasConsultar", "@nombre", "CuentasContables"));
		}

		void btnAgregar_Click (object sender, EventArgs e)
		{
			accionAgregar ();
		}

		void actualizaGrillaFormulas ()
		{
			if (cmbTablas.SelectedValue != null) {
				var ds = DB.Instancia.SPToDataSet ("calculoConsultarParaGrilla", "@idCalculo", cmbTablas.SelectedValue);
				Controles.CargaDataGridView (dgvCalculo, ds, true);
				Controles.DataGridViewEstandar (dgvCalculo);
			} else
				dgvCalculo.DataSource = "";
		}

		public virtual void accionAgregar ()
		{
			cmbTablas.Enabled = false;
			txtOrdPro.Enabled = true;
			txtCodigo.Enabled = true;
			txtDescripcion.Enabled = true;
			gbFormula.Enabled = true;
			gbPropiedades.Enabled = true;

			btnAgregar.Enabled = false;
			btnEliminar.Enabled = false;
			btnGrabar.Enabled = true;
			btnCancelar.Enabled = true;
			btnModificar.Enabled = true;
			btnCerrar.Enabled = false;

			txtOrdPro.Focus ();
		}

		public virtual void accionCancelar ()
		{
			cmbTablas.Enabled = true;
			txtOrdPro.Enabled = false;
			txtCodigo.Enabled = false;
			txtDescripcion.Enabled = false;
			gbPropiedades.Enabled = false;
			gbFormula.Enabled = false;

			btnAgregar.Enabled = true;
			btnEliminar.Enabled = true;
			btnGrabar.Enabled = false;
			btnCancelar.Enabled = false;
			btnModificar.Enabled = true;
			btnCerrar.Enabled = true;
		}


		public virtual void accionModificar ()
		{
			cmbTablas.Enabled = false;
			txtOrdPro.Enabled = false;
			txtCodigo.Enabled = false;
			txtDescripcion.Enabled = true;
			gbPropiedades.Enabled = true;
			gbFormula.Enabled = true;

			btnAgregar.Enabled = false;
			btnEliminar.Enabled = false;
			btnGrabar.Enabled = true;
			btnCancelar.Enabled = true;
			btnModificar.Enabled = false;
			btnCerrar.Enabled = false;
		}

		void limpiaCampos ()
		{
			txtOrdPro.Text = "0";
			txtCodigo.Text = "0";
			txtDescripcion.Text = "";
		}

		void btnGrabar_Click (object sender, EventArgs e)
		{
			if (datosValidos ()) {
				DB.Instancia.SP ("calculoActualizar", "@idCalculo", Convert.ToInt32 (cmbTablas.SelectedValue), "@ordenProceso", Convert.ToInt32 (txtOrdPro.Text), "@codigo", Convert.ToInt32 (txtCodigo.Text), "@descripcion", txtDescripcion.Text, "@formula", txtFormula.Text, "@tipo", Convert.ToInt32 (cmbTipo.SelectedValue), "@imprime", chkImprime.Checked, "@imprimeCantidad", chkImprimeCantidad.Checked, "@imprimeVU", chkImprimeValorUnitario.Checked, "@desactivado", chkDesactivado.Checked, "idTipoLiquidacion", Convert.ToInt32 (cmbTipoLiquidacion.SelectedValue), "idAplicacion", Convert.ToInt32 (cmbAplicacion.SelectedValue), "idCuentaContable", Convert.ToInt32 (cmbCuentaContable.SelectedValue));
				btnCancelar_Click (sender, e);
			}
		}

		bool datosValidos ()
		{
			Boolean ok = true;
			if (!Varios.IsNumeric (txtCodigo.Text) || Convert.ToInt32 (txtCodigo.Text) < 0) {
				MessageBox.Show ("Debe ingresar un Codigo valido");
				ok = false;
				txtCodigo.Focus ();
			} else {
				if (txtDescripcion.Text.Length == 0) {
					MessageBox.Show ("Debe ingresar una descripcion valida");
					ok = false;
					txtDescripcion.Focus ();
				} else {
					if (!Varios.IsNumeric (txtOrdPro.Text) || Convert.ToInt32 (txtOrdPro.Text) < 0) {
						MessageBox.Show ("Debe ingresar un Orden de Proceso valido");
						ok = false;
						txtOrdPro.Focus ();
					}
				}
			}
			return ok;
		}

		void btnEliminar_Click (object sender, EventArgs e)
		{
			if (Varios.ConfirmaEliminarRegistro ()) {
				DB.Instancia.SP ("calculoEliminar", "@idCalculo", Convert.ToInt32 (cmbTablas.SelectedValue), "@ordenProceso", Convert.ToInt32 (txtOrdPro.Text), "@codigo", Convert.ToInt32 (txtCodigo.Text));
				MessageBox.Show ("el registro se elimino con éxito");
				btnCancelar_Click (sender, e);
			}
		}

		void cmbTipoLiquidacion_SelectedIndexChanged (object sender, EventArgs e)
		{
			//Controles.CargaComboBox(cmbTablas, "descripcion", "indice", "calculoConsultarTablasPorTipoLiquidacion", "@idTipoLiquidacion", cmbTipoLiquidacion.SelectedValue);
		}

		void btnEdicionFomula_Click (object sender, EventArgs e)
		{
			var frmec = new frmEdicionConcepto (Convert.ToInt32 (cmbTablas.SelectedValue));
			frmec.ShowDialog ();
			txtFormula.Text = frmec.FormulaCompilador;
		}

		void btnConsultarTablas_Click (object sender, EventArgs e)
		{
			var frmABMTablas = new frmABMTablas ();
			frmABMTablas.ShowDialog ();
		}
	}
}