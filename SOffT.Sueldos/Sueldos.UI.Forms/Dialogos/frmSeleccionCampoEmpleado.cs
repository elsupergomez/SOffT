//
//  frmSeleccionCampoEmpleado.cs
//
//  Author:
//       Claudio Rodrigo Pereyra Diaz <claudiorodrigo@pereyradiaz.com.ar>
//
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
using System.Data;
using System.Windows.Forms;
using Hamekoz.Data;
using Sofft.UI.Forms;

namespace Sueldos.View.Dialogos
{
	public partial class frmSeleccionCampoEmpleado : Form
	{
		public frmSeleccionCampoEmpleado ()
		{
			InitializeComponent ();
		}

		/// <summary>
		/// Obtiene el indice de la tabla de tablas seleccionado
		/// </summary>
		public int Indice {
			get {
				return Convert.ToInt32 (cmbCampoEmpleado.SelectedValue);
			}
		}

		/// <summary>
		/// Obtiene el texto descriptivo del Indice seleccionado
		/// </summary>
		public string IndiceDescripcion {
			get {
				return cmbCampoEmpleado.Text;
			}
		}

		/// <summary>
		/// Obtiene el valor de contenido de la tabla de tablas seleccionado
		/// </summary>
		public int Contenido {
			get {
				return cbFiltroDetalle.Checked ? Convert.ToInt32 (cmbCampoEmpleadoDetalle.SelectedValue) : 0;
			}
		}

		/// <summary>
		/// Obtiene el texto descriptivo del Contenido seleccionado
		/// </summary>
		public string ContenidoDescripcion {
			get {
				return cbFiltroDetalle.Checked ? cmbCampoEmpleadoDetalle.Text : string.Empty;
			}
		}

		/// <summary>
		/// Obtiene el valor del Estado de empleado seleccionado
		/// </summary>
		public int Estado {
			get {
				return Convert.ToInt32 (cmbEstadoEmpleado.SelectedValue);
			}
		}

		/// <summary>
		/// Obtiene el texto descriptivo del Estado de empleado seleccionado
		/// </summary>
		public string EstadoDescripcion {
			get {
				return cmbEstadoEmpleado.Text;
			}
		}

		/// <summary>
		/// Indica si se decea filtrar la el resultado segun un valor especifico
		/// </summary>
		public bool Filtrado {
			get {
				return cbFiltroDetalle.Checked;
			}
		}

		bool tipo;

		public bool Tipo {
			get {
				return tipo;
			}
		}

		/// <summary>
		/// Indica si se debe obtener del historico
		/// </summary>
		public bool Historico {
			get {
				return cbHistorico.Checked;
			}
		}

		/// <summary>
		/// Obtiene el id de la liquidacion seleccionada para el historico
		/// </summary>
		public int LiquidacionID {
			get {
				return Convert.ToInt32 (cmbLiquidaciones.SelectedValue);
			}
		}

		/// <summary>
		/// Obtiene la descripcion de la liquidacion seleccionada para el historico
		/// </summary>
		public string LiquidacionDescripcion {
			get {
				return cbHistorico.Checked ? cmbLiquidaciones.Text : string.Empty;
			}
		}

		void cbFiltroDetalle_CheckedChanged (object sender, EventArgs e)
		{
			cmbCampoEmpleadoDetalle.Enabled = cbFiltroDetalle.Checked;
		}

		//TODO completar consulta
		void cmbCampoEmpleado_SelectedIndexChanged (object sender, EventArgs e)
		{
			//Aca realizo la consulta para llenar el combo especifico si es que tiene valore tabulados
			cmbCampoEmpleadoDetalle.Enabled = false;
			cbFiltroDetalle.Checked = false;
			var dr = (DataRowView)cmbCampoEmpleado.SelectedItem;
			tipo = Convert.ToBoolean (dr ["Tipo"]);
			cbFiltroDetalle.Enabled = Tipo;
			if (cbFiltroDetalle.Enabled) {
				Controles.CargaComboBox (cmbCampoEmpleadoDetalle, "detalle", "contenido", DB.Instancia.SPToDataSet ("tablasConsultarContenidoyDetalle", "tabla", "empleadosSueldos", "indice", cmbCampoEmpleado.SelectedValue));
			}
		}

		//Completar codigo de validacion
		void btnAceptar_Click (object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close ();
		}

		void btnCancelar_Click (object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close ();
		}

		//TODO carga inicial de los combos
		void frmSeleccionCampoEmpleado_Load (object sender, EventArgs e)
		{
			Controles.CargaComboBox (cmbEstadoEmpleado, "detalle", "contenido", DB.Instancia.SPToDataSet ("tablasConsultarContenidoyDetalleParaCombo", "tabla", "empleadosSueldos", "indice", 10));
			Controles.CargaComboBox (cmbCampoEmpleado, "descripcion", "indice", DB.Instancia.SPToDataSet ("tablasConsultarDetalleParaCombo"));
			Controles.CargaComboBox (cmbLiquidaciones, "descripcion", "id", DB.Instancia.SPToDataSet ("liquidacionesDetalleParaCombo", "estado", 1));
		}

		void cbHistorico_CheckedChanged (object sender, EventArgs e)
		{
			cmbLiquidaciones.Enabled = cbHistorico.Checked;
			cmbEstadoEmpleado.Enabled = !cbHistorico.Checked;
		}
	}
}