/*  
    frmSeleccionValor.cs

    Author:
    Hernan Vivani <hernan@vivani.com.ar> - http://hvivani.com.ar

    Copyright � SOffT 2010 - http://www.sofft.com.ar

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sofft.ViewComunes.Dialogos
{
    public partial class frmSeleccionValor : Form
    {
        public frmSeleccionValor()
        {
            InitializeComponent();
            txtValor.Text= "0";

        }

        /// <summary>
        /// Obtiene el el valor seleccionado
        /// </summary>
        public object getValor
        {
            get { return txtValor.Text; }
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void frmSeleccionValor_Load(object sender, EventArgs e)
        {
            this.txtValor.Focus();
        }
    }
}