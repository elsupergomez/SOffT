//
//  Controles.cs
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
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace Sofft.UI.Forms
{
	/// <summary>
	/// Clase de manejo de la presentacion, carga y estetica de varios controles.
	/// </summary>
	public static class Controles
	{
		/// <summary>
		/// Devuelve un objeton imágen desde un byte[]
		/// </summary>
		/// <param name="byteArrayIn"></param>
		/// <returns></returns>
		public static Image ByteArrayToImage (byte[] byteArrayIn)
		{
			Image newImage;
			//Read image data into a memory stream
			using (var ms = new MemoryStream (byteArrayIn, 0, byteArrayIn.Length)) {
				ms.Write (byteArrayIn, 0, byteArrayIn.Length);

				//Set image variable value using memory stream.
				newImage = Image.FromStream (ms, true);
			}
			return newImage;
		}

		/// <summary>
		/// Totaliza una columna de un DataGridView
		/// </summary>
		/// <param name="columna"></param>
		/// <returns></returns>
		/// <param name = "dgv"></param>
		public static double TotalizarColumnaGrilla (int columna, DataGridView dgv)
		{
			double total = 0;
			foreach (DataGridViewRow row in dgv.Rows) {
				total += double.Parse (row.Cells [columna].Value.ToString ());
			}
			return total;
		}

		//Open file in to a filestream and read data in a byte array.
		public static byte[] ReadFile (string sPath)
		{
			//Initialize byte array with a null value initially.
			byte[] data;

			//Use FileInfo object to get file size.
			var fInfo = new FileInfo (sPath);
			long numBytes = fInfo.Length;

			//Open FileStream to read file
			var fStream = new FileStream (sPath, FileMode.Open, FileAccess.Read);

			//Use BinaryReader to read file stream into byte array.
			var br = new BinaryReader (fStream);

			//When you use BinaryReader, you need to supply number of bytes
			//to read from file.
			//In this case we want to read entire file.
			//So supplying total number of bytes.
			data = br.ReadBytes ((int)numBytes);

			return data;
		}

		/// <summary>
		/// Devuelve un byte[] desde una imágen
		/// </summary>
		/// <param name="imageIn"></param>
		/// <returns></returns>
		public static byte[] ImageToByteArray (Image imageIn)
		{
			var ms = new MemoryStream ();
			imageIn.Save (ms, ImageFormat.Bmp);
			return ms.ToArray ();
		}

		/// <summary>
		/// Carga un datagrid con el resultado de la ejecucion de un dataset.
		/// </summary>
		/// <param name = "dgv"></param>
		/// <param name = "ds"></param>
		/// <param name = "columnaCeroVisible"></param>
		public static void CargaDataGridView (DataGridView dgv, DataSet ds, Boolean columnaCeroVisible)
		{
			CargaDataGridView (dgv, ds.Tables [0], columnaCeroVisible);
		}

		/// <summary>
		/// Carga un datagrid con una coleccion.
		/// </summary>
		/// <param name = "dgv"></param>
		/// <param name = "lista"></param>
		/// <param name = "columnaCeroVisible"></param>
		public static void CargaDataGridView (DataGridView dgv, object lista, Boolean columnaCeroVisible)
		{
			dgv.AutoGenerateColumns = true;
			dgv.DataSource = lista;
			dgv.Columns [0].Visible = columnaCeroVisible;
		}

		/// <summary>
		/// Carga combo box desde lista de objetos, indicando columna display y columna id
		/// </summary>
		/// <param name="cmb"></param>
		/// <param name="displayCol"></param>
		/// <param name="idCol"></param>
		/// <param name="lista"></param>
		public static void CargaComboBox (ComboBox cmb, string displayCol, string idCol, object lista)
		{
			cmb.ValueMember = idCol;
			cmb.DisplayMember = displayCol;
			cmb.DataSource = lista;
			// cmb.SelectedIndex = -1; //para q muestre el primero en blanco
			cmb.DropDownStyle = ComboBoxStyle.DropDownList;
		}

		/// <summary>
		/// Carga combo box desde dataset, indicando columna display y columna id
		/// </summary>
		/// <param name="cmb"></param>
		/// <param name="displayCol"></param>
		/// <param name="idCol"></param>
		/// <param name = "ds"></param>
		public static void CargaComboBox (ComboBox cmb, string displayCol, string idCol, DataSet ds)
		{
			CargaComboBox (cmb, displayCol, idCol, ds.Tables [0]);
		}

		/// <summary>
		/// Carga list box con una lista
		/// </summary>
		/// <param name="lb"></param>
		/// <param name="displayCol"></param>
		/// <param name="idCol"></param>
		/// <param name="lista"></param>
		public static void CargaListBox (ListBox lb, string displayCol, string idCol, object lista)
		{
			lb.ValueMember = idCol;
			lb.DisplayMember = displayCol;
			lb.DataSource = lista;
		}

		/// <summary>
		/// setea valores estandard en datagrid con ajuste automatico de columnas.
		/// </summary>
		/// <remarks>Desabilita la autogeneracion de columnas</remarks>
		/// <param name="dgv"></param>
		public static void DataGridViewEstandar (DataGridView dgv)
		{
			//cpereyra: desabilita la autogeneracion de columnas para que no se modifique el orden de las mismas.
			//Es necesario haber asignado previamente la fuente de datos.
			dgv.AutoGenerateColumns = true;
			dgv.AllowUserToAddRows = false;
			dgv.AllowUserToDeleteRows = false;
			dgv.AllowUserToResizeRows = false;
			//dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
			dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			dgv.BorderStyle = BorderStyle.Fixed3D;
			dgv.CellBorderStyle = DataGridViewCellBorderStyle.Sunken;
			dgv.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
			dgv.ColumnHeadersHeight = 20;
			dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			dgv.MultiSelect = false;
			dgv.ReadOnly = true;
			dgv.RowHeadersWidth = 21;
			dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			dgv.RowTemplate.Height = 20;
			dgv.RowTemplate.ReadOnly = true;
			dgv.RowTemplate.Resizable = DataGridViewTriState.False;
			dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dgv.TabIndex = 0;

			for (int i = 0; i < dgv.Columns.Count - 1; i++) {
				dgv.Columns [i].Visible = !(dgv.Columns [i].Name.StartsWith ("id") || dgv.Columns [i].Name.StartsWith ("Id"));
			}
		}

		/// <summary>
		/// carga dtPicker teniendo en cuenta si no hay una fecha seteada.
		/// esteblece automaticamente propiedad checked. Pide showcheckbox.
		/// Comparo con "01/01/1900".
		/// </summary>
		/// <param name="dtp"></param>
		/// <param name="fecha"></param>
		/// <param name = "showCheckBox"></param>
		public static void SeteaDTPicker (DateTimePicker dtp, DateTime fecha, Boolean showCheckBox)
		{
			if (fecha > new DateTime(1900,01,01)) {
				dtp.ShowCheckBox = showCheckBox;
				dtp.Value = fecha;
			} else {
				dtp.ShowCheckBox = showCheckBox;
				dtp.Checked = false;
			}
		}

		/// <summary>
		/// carga dtPicker teniendo en cuenta si no hay una fecha seteada.
		/// esteblece automaticamente propiedad checked y showcheckbox.
		///
		/// Comparo con "01/01/1900".
		/// </summary>
		/// <param name="dtp"></param>
		/// <param name="fecha"></param>
		public static void SeteaDTPicker (DateTimePicker dtp, DateTime fecha)
		{
			SeteaDTPicker (dtp, fecha, false);
		}

		/// <summary>
		/// consulta el contenido de un campo de un renglon de un dgv.
		/// </summary>
		/// <param name = "dgv"></param>
		/// <param name = "idColumna">id de columna de la grilla</param>
		/// <returns></returns>
		public static string ConsultaCampoRenglon (DataGridView dgv, int idColumna)
		{
			return dgv.SelectedRows [0].Cells [idColumna].Value.ToString ();
		}

		/// <summary>
		/// consulta el id del renglon seleccionado en una grilla
		/// </summary>
		/// <param name="dgv"></param>
		/// <returns></returns>
		public static int ConsultaRenglonSeleccionado (DataGridView dgv)
		{
			return dgv.SelectedRows [0].Cells [0].RowIndex;
		}

		/// <summary>
		/// Carga la imagen solicitada en un control bitmap.
		/// </summary>
		/// <param name="pathAbsoluto">Pasar Application.StartupPath + pathRelativo o pathAbsoluto</param>
		/// <returns></returns>
		public static Bitmap CargarImagen (string pathAbsoluto)
		{
			Bitmap bmp = null;
			try {
				bmp = new Bitmap (pathAbsoluto);
			} catch (Exception ex) {
				Console.WriteLine ("Error al cargar la imagen solicitada" + ex);
			}
			return bmp;
		}

		/// <summary>
		/// Habilitar o deshabilita los controles hijos de forma recursiva
		/// </summary>
		/// <param name="control">Control contenedor</param>
		/// <param name="estado">True para habilitar controles, False para deshabilitar controles</param>
		/// <autor>Claudio Rodrigo Pereyra Diaz</autor>
		public static void HabilitarControles (Control control, bool estado)
		{
			foreach (Control c in control.Controls) {
				HabilitarControles (c, estado);
			}
			if (control is TextBox)
				control.Enabled = estado;
			if (control is ComboBox)
				control.Enabled = estado;
			if (control is Button)
				control.Enabled = estado;
			if (control is CheckBox)
				control.Enabled = estado;
			if (control is DateTimePicker)
				control.Enabled = estado;
			if (control is MaskedTextBox)
				control.Enabled = estado;
			var dataGridView = control as DataGridView;
			if (dataGridView != null)
				dataGridView.ReadOnly = !estado;
		}
	}
}

//TODO Remover
namespace Sofft.ViewComunes
{
	/// <summary>
	/// Clase de manejo de la presentacion, carga y estetica de varios controles.
	/// </summary>
	[Obsolete ("Usar Sofft.UI.Forms.Controles")]
	public static class Controles
	{
		/// <summary>
		/// Carga un datagrid con el resultado de la ejecucion de un sp.
		/// </summary>
		[Obsolete ("Usar Sofft.UI.Forms.Controles.CargaDataGridView()")]
		public static void cargaDataGridView (DataGridView dgv, string sp, params object[] parametros)
		{
			dgv.AutoGenerateColumns = true;
			dgv.DataSource = Hamekoz.Data.DB.Instancia.SPToDataSet (sp, parametros).Tables [sp];
			for (int i = 0; i < dgv.Columns.Count - 1; i++) {
				dgv.Columns [i].Visible = !dgv.Columns [i].Name.StartsWith ("id") || !dgv.Columns [i].Name.StartsWith ("Id");
			}
			//TODO: sacar esto de acá !!!
		}

		/// <summary>
		/// setea valores estandard en datagrid con ajuste automatico de columnas.
		/// </summary>
		/// <remarks>Desabilita la autogeneracion de columnas</remarks>
		/// <param name="dgv"></param>
		[Obsolete ("Usar Sofft.UI.Forms.Controles.DataGridViewEstandar()")]
		public static void setEstandarDataGridView (DataGridView dgv)
		{
			UI.Forms.Controles.DataGridViewEstandar (dgv);
		}
	}
}