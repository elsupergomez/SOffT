//
//  frmSeleccionLiquidacionPorTipos.cs
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
using System.Windows.Forms;

namespace Sueldos.View.Dialogos
{
	/// <summary>
	/// Dialogo de seleccion de Liquidacion y Tipos de Liquidacion Activas
	/// </summary>
	public partial class frmSeleccionLiquidacionPorTipos : Form
	{
		public frmSeleccionLiquidacionPorTipos ()
		{
			InitializeComponent ();
		}

		void btnAceptar_Click (object sender, EventArgs e)
		{
			if (liquidacionesCtrl.TiposSeleccionados.Count > 0) {
				liquidacionesCtrl.GrabarTipoSeleccionados ();
				DialogResult = DialogResult.OK;
				Close ();
			} else {
				MessageBox.Show ("Debe seleccionar al menos un tipo de liquidación");
			}
		}

		/// <summary>
		/// Obtiene la descripcion de la liquidacion seleccionada
		/// </summary>
		public string LiquidacionDescripcion {
			get { 
				return liquidacionesCtrl.LiquidacionDescripcion;
			}
		}

		/// <summary>
		/// Obtiene o Establece la liquidacion seleccionada
		/// </summary>
		public int LiquidacionId {
			get { 
				return liquidacionesCtrl.LiquidacionId;
			}
			set { 
				liquidacionesCtrl.LiquidacionId = value;
			}
		}

		void btnCancelar_Click (object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close ();
		}

		void frmSeleccionLiquidacionPorTipos_Load (object sender, EventArgs e)
		{
			liquidacionesCtrl.LiquidacionesCargar ();
		}
	}
}