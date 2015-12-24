//
//  frmSeleccionValor.cs
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
	public partial class frmSeleccionValor : Form
	{
		public string Texto { 
			get { 
				return txtValor.Text;
			}
		}

		public int Valor {
			get {
				try {
					return Convert.ToInt32 (txtValor.Text);
				} catch (Exception) {
					return 0;
				}
			}
		}

		public frmSeleccionValor (string descripcion)
		{
			InitializeComponent ();
			lblDescripcion.Text = descripcion;
			ShowDialog ();
		}

		void txtValor_KeyDown (object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				btnAceptar.Focus ();
		}

		void btnAceptar_Click (object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close ();
		}
	}
}