//
//  LoginForm.cs
//
//  Author:
//       Claudio Rodrigo Pereyra Diaz <claudiorodrigo@pereyradiaz.com.ar>
//       Hernan Vivani <hernan@vivani.com.ar> - http://hvivani.com.ar
//
//  Copyright (c) 2015 Hamekoz - www.hamekoz.com.ar
//  Copyright (c) 2010 SOffT - http://www.sofft.com.ar
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
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Sofft.Utils;

namespace Sofft.UI.Forms
{
	/// <summary>
	/// Fomulario utilizado para verificar usuario
	/// </summary>
	public partial class LoginForm : Form
	{
		public LoginForm()
		{
			InitializeComponent();
            Icon = Icon = Controles.Icono;
            picLogo.SizeMode = PictureBoxSizeMode.StretchImage;
			picLogo.Image = Controles.Logo;
            //TODO: verificar si valida login o no
            //Modulo.ValidaLogin = false;
            txtUsuario.Text = Environment.UserName;
            if (txtUsuario.Text != string.Empty)
                txtPwd.Focus();
		}

		void btnAceptar_Click(object sender, EventArgs e)
		{
			int idUsuario;
			var usuario = new Usuario();
			idUsuario = usuario.ExisteUsuario(txtUsuario.Text, txtPwd.Text);
			if (idUsuario > 0)
			{
				usuario.Id = idUsuario;
				usuario.Login = txtUsuario.Text;
				usuario.Password = txtPwd.Text;
				Usuario.Actual = usuario;
			}
			else
				MessageBox.Show("Usuario inexistente - Consulte con el administrador");
			Close();
		}

		void btnCancelar_Click(object sender, EventArgs e)
		{
            Application.Exit();
		}

		void txtUsuario_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r')
			{
				txtPwd.Focus();
				e.Handled = true; //No Beep}
			}
		}

		void txtPwd_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r')
			{
				btnAceptar.Focus();
				e.Handled = true; //No Beep}
			}
		}
	}
}
