//
//  ABMCForm.cs
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

namespace Sofft.UI.Forms
{
	public partial class ABMCForm : Form
	{
		protected string codigoBusqueda = string.Empty;
		ToolTip toolTip = new ToolTip();

		public ABMCForm()
		{
			InitializeComponent();
			btnBuscar.MouseHover += btnBuscar_MouseHover;
			Text = "A.B.M. Generico";
		}

		void btnBuscar_MouseHover(object sender, EventArgs e)
		{
			toolTip.SetToolTip(btnBuscar, "Buscar...");
		}

		protected virtual void frmABM_Load(object sender, EventArgs e)
		{
			HabilitarEdicion(false);
		}

		void btnCerrar_Click(object sender, EventArgs e)
		{
			Close();
		}

		protected virtual void btnAgregar_Click(object sender, EventArgs e)
		{
			HabilitarEdicion(true);
			LimpiarControles(gbDatos);
		}

		protected virtual void HabilitarEdicion(bool habilitado)
		{
			Controles.HabilitarControles(gbDatos, habilitado);

			btnBuscar.Enabled = !habilitado;

			btnAgregar.Enabled = !habilitado;
			btnEliminar.Enabled = !habilitado;
			btnModificar.Enabled = !habilitado;
			btnGrabar.Enabled = habilitado;
			btnCancelar.Enabled = habilitado;
			if (habilitado)
			{
				btnAgregar.Focus();
			}
		}

		void LimpiarControles(Control control)
		{
			foreach (Control c in control.Controls)
			{
				LimpiarControles(c);
			}
			var textBox = control as TextBox;
			if (textBox != null)
				textBox.Text = string.Empty;
			var listBox = control as ListBox;
			if (listBox != null)
			{
				listBox.Items.Clear();
			}
			var comboBox = control as ComboBox;
			if (comboBox != null)
			{
				if (comboBox.ValueMember != string.Empty)
				{
					comboBox.SelectedValue = -1;
				}
			}
			var checkBox = control as CheckBox;
			if (checkBox != null)
				checkBox.Checked = false;
		}

		protected virtual void btnCancelar_Click(object sender, EventArgs e)
		{
			HabilitarEdicion(false);
		}

		protected virtual void btnGrabar_Click(object sender, EventArgs e)
		{
			HabilitarEdicion(false);
			btnAgregar.Focus();
		}

		protected virtual void btnEliminar_Click(object sender, EventArgs e)
		{

		}

		protected virtual void btnModificar_Click(object sender, EventArgs e)
		{
			HabilitarEdicion(true);
		}

		protected virtual void btnBuscar_Click(object sender, EventArgs e)
		{

		}
	}
}