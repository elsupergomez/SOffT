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
		public LoginForm ()
		{
			InitializeComponent ();
			picLogo.SizeMode = PictureBoxSizeMode.StretchImage;
			string pathLogo = Application.StartupPath + Path.DirectorySeparatorChar + Modulo.pathImagenes + Path.DirectorySeparatorChar + Modulo.nombreLogo;
			picLogo.Image = Controles.CargarImagen (pathLogo);
			//TODO: verificar si valida login o no
			//Modulo.ValidaLogin = false;
			LeeArchivoLogin ();
		}

		void btnAceptar_Click (object sender, EventArgs e)
		{
			int idUsuario;
			var usuario = new Usuario ();
			idUsuario = usuario.ExisteUsuario (txtUsuario.Text, txtPwd.Text);
			if (idUsuario > 0) {
				usuario.IdUsuario = idUsuario;
				usuario.Login = txtUsuario.Text;
				usuario.Password = txtPwd.Text;
				Modulo.Usuario = usuario;
				EscribeArchivoLogin ();
			} else
				MessageBox.Show ("Usuario inexistente - Consulte con el administrador");
			Close ();
		}

		void btnCancelar_Click (object sender, EventArgs e)
		{
			Close ();
		}

		void txtUsuario_KeyPress (object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r') {
				txtPwd.Focus ();
				e.Handled = true; //No Beep}
			}
		}

		void txtPwd_KeyPress (object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r') {
				btnAceptar.Focus ();
				e.Handled = true; //No Beep}
			}
		}

		void LeeArchivoLogin ()
		{
			try {
				string strLine;
				//TODO generalizar SO
				var objReader = new StreamReader ("C:\\lastusr.dat");
				strLine = objReader.ReadLine ();
				if (strLine != null) {
					txtUsuario.Text = strLine;
					txtPwd.Focus ();
				}
				objReader.Close ();
			} catch {
				Console.WriteLine ("no se encontró archivo login.");
			}
		}

		void EscribeArchivoLogin ()
		{
			try {
				//TODO generalizar SO
				var objWriter = new StreamWriter ("C:\\lastusr.dat");
				if (txtUsuario.Text.Length > 0)
					objWriter.WriteLine (txtUsuario.Text);
				objWriter.Close ();
			} catch {
				Console.WriteLine ("No pudo crear archivo login.");
			}
		}
	}
}

//TODO Remover
namespace Sofft.ViewComunes
{
	/// <summary>
	/// Fomulario utilizado para verificar usuario
	/// </summary>
	[Obsolete ("Usar Sofft.UI.Forms.LoginForm")]
	public class frmLogin : Sofft.UI.Forms.LoginForm
	{
	}
}