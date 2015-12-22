//
//  RelojForm.cs
//
//  Author:
//       Claudio Rodrigo Pereyra Diaz <claudiorodrigo@pereyradiaz.com.ar>
//       Hernan Vivani <hernan@vivani.com.ar> - http://hvivani.com.ar
//
//  Copyright (c) 2010 Hamekoz - www.hamekoz.com.ar
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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Reloj.Core;

#if !LINUX
using WisSensorNLibLib;
#endif

namespace Reloj.UI.Forms
{
	public partial class RelojForm : Form
	{
		public int RelojId {
			get;
			set;
		}

		#if !LINUX
		WisSensorN WisObj = new WisSensorN ();

		HuellaController huellasController = new HuellaController ();
		bool capturando;
		string[] arrayHuellas;
		List<Huella> huellas;

		#endif

		TarjetaDeFichada tarjetaFichada;

		FichadaController fichadaController = new FichadaController ();

		public RelojForm ()
		{
			InitializeComponent ();
			pictureBoxEmpresa.Image = CargarImagen (Application.StartupPath + @"/imagenes/logoGrande.jpg");
			textBoxStringTarjeta.LostFocus += textBoxStringTarjeta_LostFocus;
			textBoxStringTarjeta.KeyDown += textBoxStringTarjeta_KeyDown;
			lblVersion.Text = "Version: " + Application.ProductVersion;
			var tooltip = new ToolTip ();
			tooltip.SetToolTip (btnExportar, "Exportar fichadas");
		}

		void textBoxStringTarjeta_KeyDown (object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter) {
				tarjetaFichada = new TarjetaDeFichada (textBoxStringTarjeta.Text);
				Procesar (tarjetaFichada.Legajo);
			}
		}

		void textBoxStringTarjeta_LostFocus (object sender, EventArgs e)
		{
			if (textBoxStringTarjeta.Enabled) {
				textBoxStringTarjeta.Focus ();
			}
		}

		void timerReloj_Tick (object sender, EventArgs e)
		{
			labelSistemaFecha.Text = DateTime.Now.Date.ToLongDateString ().ToUpper ();
			labelSistemaHora.Text = string.Format ("{0:D2}:{1:D2}:{2:D2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
		}

		void btnIniciar_Click (object sender, EventArgs e)
		{
			#if !LINUX
			timerHuella.Start ();
			#endif
			BotonesHabilitar (btnDetener.Enabled);
			CargarFichadas ();
		}

		void btnDetener_Click (object sender, EventArgs e)
		{
			#if !LINUX
			capturando = false;
			timerHuella.Stop ();
			WisObj.StopCapture ();
			#endif
			BotonesHabilitar (btnDetener.Enabled);
			textBoxStringTarjeta.Clear ();
			lstFichadas.Items.Clear ();
		}

		void btnSalir_Click (object sender, EventArgs e)
		{
			Close ();
		}

		void BotonesHabilitar (Boolean habilita)
		{
			btnIniciar.Enabled = habilita;
			btnSalir.Enabled = habilita;
			btnDetener.Enabled = !habilita;
			textBoxStringTarjeta.Enabled = !habilita;
		}

		void CargarFichadas ()
		{
			lstFichadas.Items.Clear ();
			List<Fichada> fichadas;
			fichadas = fichadaController.getListaEntreFechas (DateTime.Now.Date, DateTime.Now.Date);
			foreach (Fichada f in fichadas) {
				lstFichadas.Items.Add (f.ToCustomString ());
			}
			if (fichadas != null && fichadas.Count > 0) {
				Fichada ultima = fichadas [0];
				MostrarFichada (ultima);
			}
			textBoxStringTarjeta.Clear ();
			textBoxStringTarjeta.Focus ();
		}

		void RelojForm_Shown (object sender, EventArgs e)
		{
			BotonesHabilitar (btnDetener.Enabled);
			CargarFichadas ();
			textBoxStringTarjeta.Focus ();
		}

		void MostrarFichada (Fichada fichada)
		{
			labelUltimaFichadaHora.Text = fichada.Hora;
			labelUltimaFichadaFecha.Text = fichada.Fecha;
			labelUltimaFichadaLegajo.Text = fichada.Legajo.ToString ();
			labelUltimaFichadaApellidoNombre.Text = fichada.ApellidoYnombres;
			pictureBoxUltimaFichadaFoto.Image = fichada.Foto == "" ? null : CargarImagen (fichada.Foto);
		}

		void Procesar (int legajo)
		{
			var fichada = new Fichada {
				Id = 0,
				Legajo = legajo,
				FechaHora = DateTime.Now,
				RelojId = RelojId,
			};
			try {
				fichadaController.Guardar (fichada);
				MostrarFichada (fichada);
				lstFichadas.Items.Insert (0, fichada.ToCustomString ());
			} catch (Exception) {
				labelUltimaFichadaApellidoNombre.Text = "!!!! ERROR !!!!";
				pictureBoxUltimaFichadaFoto.Image = null;
			} finally {
				textBoxStringTarjeta.Clear ();
			}
		}

		void RelojForm_Load (object sender, EventArgs e)
		{
			#if !LINUX
			WisObj.Open ();
			WisObj.DataEvent += WisObj_DataEvent;
			WisObj.SetDisplay ((int)pictureBoxUltimaFichadaFoto.Handle);
			timerHuella.Tick += TimerHuella_Tick;
			FormClosing += RelojForm_FormClosing;

			//cargo vector de huellas
			huellas = huellasController.getLista ();
			timerHuella.Start ();
			#endif
		}

		#if !LINUX
		void RelojForm_FormClosing (object sender, FormClosingEventArgs e)
		{
			WisObj.StopCapture ();
			timerHuella.Stop ();
			WisObj.Close ();
		}

		void WisObj_DataEvent (DATA status, string Template)
		{
			switch (status) {
			case DATA.DATA_ENROLL:
				break;
			case DATA.DATA_IDENTIFY_CAPTURE:
				int i = 0;
				arrayHuellas = new string[huellas.Count];
				foreach (Huella huella in huellas) {
					arrayHuellas [i] = huella.Patron;
					i++;
				}
				int nMatched;
				nMatched = WisObj.Identify (Template, arrayHuellas);
				if (nMatched < 0) {
					labelUltimaFichadaApellidoNombre.Text = "!!!! ERROR !!!!";
				} else {
					Procesar (huellas [nMatched].Legajo);
				}
				capturando = false;
				break;
			case DATA.DATA_VERIFY_CAPTURE:
				break;
			}
		}

		void TimerHuella_Tick (object sender, EventArgs e)
		{
			if (!capturando) {
				Console.WriteLine ("Solicitando huella");
				capturando = true;
				WisObj.IdentifyCapture ();
			}
		}

		#endif

		void BtnExportar_Click (object sender, EventArgs e)
		{
			var saveFileDialog = new SaveFileDialog ();
			saveFileDialog.Filter = "Planilla de Calculo (*.xls)|*.xls";
			saveFileDialog.FileName = "Fichadas";
			if (saveFileDialog.ShowDialog () == DialogResult.OK) {
				fichadaController.ExportarXLS (DateTime.Now.Date, DateTime.Now.Date, saveFileDialog.FileName);
				MessageBox.Show ("Reporte exportado con exito" + Environment.NewLine + saveFileDialog.FileName);
			}
		}

		/// <summary>
		/// Carga la imagen solicitada en un control bitmap.
		/// </summary>
		/// <param name="pathAbsoluto">Pasar Application.StartupPath + pathRelativo o pathAbsoluto</param>
		/// <returns></returns>
		static Bitmap CargarImagen (string pathAbsoluto)
		{
			Bitmap bmp = null;
			try {
				bmp = new Bitmap (pathAbsoluto);
			} catch (Exception ex) {
				Console.WriteLine ("Error al cargar la imagen {0}\n{1}", pathAbsoluto, ex);
			}
			return bmp;
		}
	}
}

//TODO remover
namespace Reloj.View
{
	[Obsolete ("Usar Reloj.UI.Forms.RelojForm")]
	public class frmReloj : Reloj.UI.Forms.RelojForm
	{
	}
}