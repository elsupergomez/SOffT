//
//  FingerPrintUtilsForm.cs
//
//  Author:
//       Hernan Vivani <hernan@vivani.com.ar> - http://hvivani.com.ar
//       Claudio Rodrigo Pereyra Diaz <claudiorodrigo@pereyradiaz.com.ar>
//
//  Copyright (c) 2011 SOffT - http://www.sofft.com.ar
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
using System.Collections.Generic;
using System.Windows.Forms;
using Reloj.Core;
using Sueldos.Entidades;
using Sueldos.Modelo;
using WisSensorNLibLib;

namespace Reloj.UI.Forms
{
	/// <summary>
	/// Formulario para enrolamiento e identificación de usuarios.
	/// </summary>
	/// <remarks>
	/// Programado para el lector Wison OR200N v2.5
	/// es necesario realizar el registro manual de la librería: regsvr32 WisSensorNLib.dll
	/// antes de utilizar el dispositivo.
	/// Debe ademas, agregarse como referencia (objeto COM) en el presente proyecto.
	/// </remarks>
	public partial class FingerPrintUtilsForm : Form
	{
		WisSensorN WisObj = new WisSensorN ();
		ConsultaEmpleados consuemple = new ConsultaEmpleados ();
		TablaController consutab = new TablaController ();
		HuellaController huellasNegocio = new HuellaController ();

		Huella huella;
		Boolean capturando;
		int idDedo;

		string[] huellaIdentificada = new string[1000];

		public FingerPrintUtilsForm ()
		{
			InitializeComponent ();
			CargarCombos ();
		}

		void btnCerrar_Click (object sender, EventArgs e)
		{
			Close ();
		}

		void btnEnrolar_Click (object sender, EventArgs e)
		{
			if (cmbEmpleados.SelectedValue == null) {
				MessageBox.Show ("por favor, seleccione un empleado");
			} else if (idDedo <= 0) {
				MessageBox.Show ("por favor, seleccione una huella");
			} else {
				huella = huellasNegocio.getById (int.Parse (cmbEmpleados.SelectedValue.ToString ()), idDedo);
				if (huella != null) {
					DialogResult result = MessageBox.Show ("Esa Huella ya se encuentra registrada. Desea Regrabarla ?", "Caption", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
					if (result == DialogResult.Yes) {
						huellasNegocio.Eliminar (huella);
						lblMensajes.Text = "Por favor, coloque la huella " + cmbHuella.Text + " sobre el lector...";
						WisObj.StartEnroll ();
					}
				} else {
					lblMensajes.Text = "Por favor, coloque la huella " + cmbHuella.Text + " sobre el lector...";
					WisObj.StartEnroll ();
				}
			}
			huella = cmbEmpleados.SelectedValue != null ? new Huella (int.Parse (cmbEmpleados.SelectedValue.ToString ())) : null;

		}

		void btnIdentificar_Click (object sender, EventArgs e)
		{
			SolicitarHuella ();
		}

		void frmFingerPrintUtils_Load (object sender, EventArgs e)
		{
			lblMensajes.Text = "Por favor, verifique que el lector de huellas se encuentre conectado ...";
			WisObj.Open ();
			WisObj.DataEvent += WisObj_DataEvent;
			WisObj.SetDisplay ((int)pbHuella.Handle);
			FormClosing += frmFingerPrintUtils_FormClosing;
			idDedo = 0;
			cmbHuella.SelectedValue = double.Parse (idDedo.ToString ());
		}

		void frmFingerPrintUtils_FormClosing (object sender, FormClosingEventArgs e)
		{
			WisObj.Close ();
		}

		void WisObj_DataEvent (DATA status, string Template)
		{
			switch (status) {
			case DATA.DATA_ENROLL:
				huella.Patron = string.Copy (Template);
				lblMensajes.Text = "Huella Enrolada !!!!";
				huella.DedoHuella = new TablaEntity ("reloj", 4, idDedo);
				huellasNegocio.Guardar (huella);
				break;

			case DATA.DATA_IDENTIFY_CAPTURE:
				int i = 0;
				List<Huella> huellas = huellasNegocio.getLista ();
				foreach (Huella h in huellas) {
					huellaIdentificada [i] = h.Patron;
					i++;
				}

				int nMatched;
				nMatched = WisObj.Identify (Template, huellaIdentificada);
				if (nMatched < 0) {
					lblMensajes.Text = "No se encontró huella válida!!";
					cmbEmpleados.SelectedValue = -1;
				} else {
					lblMensajes.Text = "Huella Identificada ! legajo=" + huellas [nMatched].Legajo;
					cmbEmpleados.SelectedValue = huellas [nMatched].Legajo;
				}
				capturando = false;
				break;

			case DATA.DATA_VERIFY_CAPTURE:
				break;
			}
		}

		void CargarCombos ()
		{
			cmbEmpleados.ValueMember = "Legajo";
			cmbEmpleados.DisplayMember = "Nombres";
			cmbEmpleados.DataSource = consuemple.ListaDatosBasicos ();
			cmbEmpleados.DropDownStyle = ComboBoxStyle.DropDownList;

			cmbHuella.ValueMember = "contenido";
			cmbHuella.DisplayMember = "detalle";
			cmbHuella.DataSource = consutab.getContenidoYdetalle ("reloj", 4);
			cmbHuella.DropDownStyle = ComboBoxStyle.DropDownList;
		}

		void SolicitarHuella ()
		{
			capturando = true;
			lblMensajes.Text = "Por favor, coloque el dedo sobre el lector...";
			WisObj.IdentifyCapture ();
			huella = cmbEmpleados.SelectedValue != null ? new Huella (int.Parse (cmbEmpleados.SelectedValue.ToString ())) : null;
		}

		void timer_Tick (object sender, EventArgs e)
		{
			if (!capturando) {
				Console.WriteLine ("solicitando huella");
				SolicitarHuella ();
			}
		}

		void rbMeniqueIzq_CheckedChanged (object sender, EventArgs e)
		{
			if (rbMeniqueIzq.Checked) {
				idDedo = 1;
				cmbHuella.SelectedValue = double.Parse (idDedo.ToString ());
			}
		}

		void rbAnularIzq_CheckedChanged (object sender, EventArgs e)
		{
			if (rbAnularIzq.Checked) {
				idDedo = 2;
				cmbHuella.SelectedValue = double.Parse (idDedo.ToString ());
			}
		}

		void rbMayorIzq_CheckedChanged (object sender, EventArgs e)
		{
			if (rbMayorIzq.Checked) {
				idDedo = 3;
				cmbHuella.SelectedValue = double.Parse (idDedo.ToString ());
			}
		}

		void rbIndiceIzq_CheckedChanged (object sender, EventArgs e)
		{
			if (rbIndiceIzq.Checked) {
				idDedo = 4;
				cmbHuella.SelectedValue = double.Parse (idDedo.ToString ());
			}
		}

		void rbPulgarIzq_CheckedChanged (object sender, EventArgs e)
		{
			if (rbPulgarIzq.Checked) {
				idDedo = 5;
				cmbHuella.SelectedValue = double.Parse (idDedo.ToString ());
			}
		}

		void rbMeniqueDer_CheckedChanged (object sender, EventArgs e)
		{
			if (rbMeniqueDer.Checked) {
				idDedo = 10;
				cmbHuella.SelectedValue = double.Parse (idDedo.ToString ());
			}

		}

		void rbAnularDer_CheckedChanged (object sender, EventArgs e)
		{
			if (rbAnularDer.Checked) {
				idDedo = 9;
				cmbHuella.SelectedValue = double.Parse (idDedo.ToString ());
			}
		}

		void rbMayorDer_CheckedChanged (object sender, EventArgs e)
		{
			if (rbMayorDer.Checked) {
				idDedo = 8;
				cmbHuella.SelectedValue = double.Parse (idDedo.ToString ());
			}
		}

		void rbIndiceDer_CheckedChanged (object sender, EventArgs e)
		{
			if (rbIndiceDer.Checked) {
				idDedo = 7;
				cmbHuella.SelectedValue = double.Parse (idDedo.ToString ());
			}
		}

		void rbPulgarDer_CheckedChanged (object sender, EventArgs e)
		{
			if (rbPulgarDer.Checked) {
				idDedo = 6;
				cmbHuella.SelectedValue = double.Parse (idDedo.ToString ());
			}
		}

		void btnEliminarTodas_Click (object sender, EventArgs e)
		{
			if (cmbEmpleados.SelectedValue == null) {
				MessageBox.Show ("por favor, seleccione un empleado");
			} else {
				DialogResult result = MessageBox.Show ("Está seguro de eliminar todas las huellas del legajo " + cmbEmpleados.SelectedValue + " ?", "Caption", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (result == DialogResult.Yes) {
					huellasNegocio.Eliminar (int.Parse (cmbEmpleados.SelectedValue.ToString ()));
					MessageBox.Show ("Se eliminaron todas las huellas del legajo " + cmbEmpleados.SelectedValue + ".");
				}
			}
		}
	}
}

//TODO Remover
namespace Reloj.View
{
	[Obsolete ("Usar Reloj.UI.Forms.FingerPrintUtilsForm")]
	public class frmFingerPrintUtils : Reloj.UI.Forms.FingerPrintUtilsForm
	{

	}
}