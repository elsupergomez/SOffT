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
				var objWriter = new StreamWriter ("C:\\lastusr.dat");
				if (txtUsuario.Text.Length > 0)
					objWriter.WriteLine (txtUsuario.Text);
				objWriter.Close ();
			} catch {
				Console.WriteLine ("no pudo crear archivo login.");
			}
		}
	}
}