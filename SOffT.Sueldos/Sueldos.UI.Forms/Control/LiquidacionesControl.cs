//
//  LiquidacionesControl.cs
//
//  Author:
//       Claudio Rodrigo Pereyra Diaz <claudiorodrigo@pereyradiaz.com.ar>
//
//  Copyright (c) 2010 SOffT - http://www.sofft.com.ar
//  Copyright (c) 2010 Hamekoz
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
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Hamekoz.Data;

namespace Sueldos.UI.Forms
{
	public partial class LiquidacionesControl : UserControl
	{
		/// <summary>
		/// Control para seleccionar una liquidacion activa, y uno o mas tipos de liquidacion correspondiente
		/// </summary>
		public LiquidacionesControl ()
		{
			InitializeComponent ();

			comboBoxEstado.ValueMember = "contenido";
			comboBoxEstado.DisplayMember = "detalle";

			comboBoxLiquidaciones.ValueMember = "id";
			comboBoxLiquidaciones.DisplayMember = "descripcion";

			listBoxTiposLiquidacion.ValueMember = "idTipoLiquidaciones";
			listBoxTiposLiquidacion.DisplayMember = "descripcion";
		}

		#region Propiedades

		/// <summary>
		/// Obtiene o Establece el id de la liquidacion seleccionada en el combo
		/// </summary>
		public int LiquidacionId {
			get {
				return Convert.ToInt32 (comboBoxLiquidaciones.SelectedValue);
			}
			set {
				comboBoxLiquidaciones.SelectedValue = value;
			}
		}

		/// <summary>
		/// Obtiene la descripcion de la liquidacion seleccionda
		/// </summary>
		public string LiquidacionDescripcion
        { get { return comboBoxLiquidaciones.Text; } }

		/// <summary>
		/// Obtiene una lista con los id de los tipos de liquidacion seleccionados
		/// </summary>
		public IList<int> TiposSeleccionados {
			get {
				var lista = new List<int> (listBoxTiposLiquidacion.SelectedItems.Count);
				foreach (DataRowView item in listBoxTiposLiquidacion.SelectedItems) {
					lista.Add (Convert.ToInt32 (item [0]));
				}
				return lista;
			}
		}

		#endregion

		#region Metodos

		public event EventHandler LiquidacionId_Changed;

		void comboBoxEstado_SelectedIndexChanged (object sender, EventArgs e)
		{
			comboBoxLiquidaciones.DataSource = DB.Instancia.SPToDataSet ("liquidacionesDetalleParaCombo", "estado", Convert.ToInt32 (comboBoxEstado.SelectedValue)).Tables [0];
			comboBoxLiquidaciones.Refresh ();
			listBoxTiposLiquidacion.Refresh ();
			listBoxTiposLiquidacion.ClearSelected ();
		}

		void comboBoxLiquidaciones_SelectedIndexChanged (object sender, EventArgs e)
		{
			listBoxTiposLiquidacion.DataSource = DB.Instancia.SPToDataSet ("liquidacionesPorTipoConsultar", "idLiquidacion", LiquidacionId).Tables [0];
			listBoxTiposLiquidacion.Refresh ();
			listBoxTiposLiquidacion.ClearSelected ();
			if (LiquidacionId_Changed != null) {
				LiquidacionId_Changed (sender, e);
			}
		}

		/// <summary>
		/// Llena el combo y la lista con la informacion de las liquidaciones
		/// </summary>
		public void LiquidacionesCargar ()
		{
			comboBoxEstado.DataSource = DB.Instancia.SPToDataSet ("tablasConsultarContenidoyDetalle", "tabla", "LiquidacionesDetalle", "indice", 0).Tables [0];
			comboBoxEstado.Refresh ();
			comboBoxLiquidaciones.Refresh ();
			listBoxTiposLiquidacion.Refresh ();
			listBoxTiposLiquidacion.ClearSelected ();
		}

		/// <summary>
		/// Actualiza el estado de seleccion de los tipos de liquidacion seleccionados, para su posterior uso
		/// </summary>
		public void GrabarTipoSeleccionados ()
		{
			foreach (DataRowView item in listBoxTiposLiquidacion.Items) {
				DB.Instancia.SP (
					"liquidacionesPorTipoActualizarSeleccionado",
					"idLiquidacion", LiquidacionId,
					"idTipoLiquidacion", item [0],
					"seleccionado", listBoxTiposLiquidacion.GetSelected (listBoxTiposLiquidacion.Items.IndexOf (item))
				);
			}
		}

		#endregion
	}
}

//TODO Remover
namespace Sueldos.View.Control
{
	[Obsolete ("Usar Sueldos.UI.Forms.LiquidacionesControl")]
	public class LiquidacionesCtrl : Sueldos.UI.Forms.LiquidacionesControl
	{
		
	}
}