/*
    Controles.cs

    Author:
    Claudio Rodrigo Pereyra Diaz <claudiorodrigo@pereyradiaz.com.ar>
    Hernan Vivani <hernan@vivani.com.ar> - http://hvivani.com.ar

    Copyright © SOffT 2010 - http://www.sofft.com.ar

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
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Hamekoz.Data;

namespace Sofft.ViewComunes
{
	/// <summary>
	/// Clase de manejo de la presentacion, carga y estetica de varios controles.
	/// </summary>
	[Obsolete ("Usar Sofft.UI.Forms.Controles")]
	public static class Controles
	{
		/// <summary>
		/// Devuelve un objeton imágen desde un byte[]
		/// </summary>
		/// <param name="byteArrayIn"></param>
		/// <returns></returns>
		public static Image ByteArrayToImage (byte[] byteArrayIn)
		{
			return UI.Forms.Controles.ByteArrayToImage (byteArrayIn);
		}

		/// <summary>
		/// Totaliza una columna de un DataGridView
		/// </summary>
		/// <param name="columna"></param>
		/// <returns></returns>
		/// <param name = "dgv"></param>
		public static double TotalizarColumnaGrilla (int columna, DataGridView dgv)
		{
			return UI.Forms.Controles.TotalizarColumnaGrilla (columna, dgv);
		}

		//Open file in to a filestream and read data in a byte array.
		public static byte[] ReadFile (string sPath)
		{
			return UI.Forms.Controles.ReadFile (sPath);
		}

		/// <summary>
		/// Devuelve un byte[] desde una imágen
		/// </summary>
		/// <param name="imageIn"></param>
		/// <returns></returns>
		public static byte[] ImageToByteArray (Image imageIn)
		{
			return UI.Forms.Controles.ImageToByteArray (imageIn);
		}

		/// <summary>
		/// Carga un datagrid con el resultado de la ejecucion de un sp.
		/// </summary>
		[Obsolete ("usar metodo con otros parametros")]
		public static void cargaDataGridView (DataGridView dgv, string sp, params object[] parametros)
		{

			dgv.AutoGenerateColumns = true;
			dgv.DataSource = DB.Instancia.SPToDataSet (sp, parametros).Tables [sp];
			for (int i = 0; i < dgv.Columns.Count - 1; i++) {
				dgv.Columns [i].Visible = !dgv.Columns [i].Name.StartsWith ("id") || !dgv.Columns [i].Name.StartsWith ("Id");
			}

			//TODO: sacar esto de acá !!!
		}


		/// <summary>
		/// Carga un datagrid con el resultado de la ejecucion de un dataset.
		/// </summary>
		/// <param name = "dgv"></param>
		/// <param name = "ds"></param>
		/// <param name = "columnaCeroVisible"></param>
		public static void cargaDataGridView (DataGridView dgv, DataSet ds, Boolean columnaCeroVisible)
		{
			UI.Forms.Controles.CargaDataGridView (dgv, ds, columnaCeroVisible);
		}

		/// <summary>
		/// Carga un datagrid con una coleccion.
		/// </summary>
		/// <param name = "dgv"></param>
		/// <param name = "lista"></param>
		/// <param name = "columnaCeroVisible"></param>
		public static void cargaDataGridView (DataGridView dgv, object lista, Boolean columnaCeroVisible)
		{
			UI.Forms.Controles.CargaDataGridView (dgv, lista, columnaCeroVisible);
		}

		/// <summary>
		/// carga un cmb dado con el resultado de un sp.
		/// setea el dropdownstyle a dropdownlist.
		/// </summary>
		/// <param name="cmb"></param>
		/// <param name="displayCol"></param>
		/// <param name="idCol"></param>
		/// <param name="sp"></param>
		/// <param name="parametrosSp"></param>
		[Obsolete ("usar metodo con otros parametros")]
		public static void cargaComboBox (ComboBox cmb, string displayCol, string idCol, string sp, params object[] parametrosSp)
		{
			var datos = DB.Instancia.SPToDataSet (sp, parametrosSp).Tables [sp];
			UI.Forms.Controles.CargaComboBox (cmb, displayCol, idCol, datos);
		}

		/// <summary>
		/// Carga combo box desde lista de objetos, indicando columna display y columna id
		/// </summary>
		/// <param name="cmb"></param>
		/// <param name="displayCol"></param>
		/// <param name="idCol"></param>
		/// <param name="lista"></param>
		public static void cargaComboBox (ComboBox cmb, string displayCol, string idCol, object lista)
		{
			UI.Forms.Controles.CargaComboBox (cmb, displayCol, idCol, lista);
		}

		/// <summary>
		/// Carga combo box desde dataset, indicando columna display y columna id
		/// </summary>
		/// <param name="cmb"></param>
		/// <param name="displayCol"></param>
		/// <param name="idCol"></param>
		/// <param name = "ds"></param>
		public static void cargaComboBox (ComboBox cmb, string displayCol, string idCol, DataSet ds)
		{
			UI.Forms.Controles.CargaComboBox (cmb, displayCol, idCol, ds);
		}

		/// <summary>
		/// carga listbox con el resultado de la ejecucion de un sp
		/// </summary>
		/// <param name="lb"></param>
		/// <param name="displayCol"></param>
		/// <param name="idCol"></param>
		/// <param name="sp"></param>
		/// <param name="parametrosSp"></param>
		[Obsolete ("usar metodo con otros parametros")]
		public static void cargaListBox (ListBox lb, string displayCol, string idCol, string sp, params object[] parametrosSp)
		{
			var lista = DB.Instancia.SPToDataSet (sp, parametrosSp).Tables [sp];
			UI.Forms.Controles.CargaListBox (lb, displayCol, idCol, lista);
		}

		/// <summary>
		/// Carga list box con una lista
		/// </summary>
		/// <param name="lb"></param>
		/// <param name="displayCol"></param>
		/// <param name="idCol"></param>
		/// <param name="lista"></param>
		public static void cargaListBox (ListBox lb, string displayCol, string idCol, object lista)
		{
			UI.Forms.Controles.CargaListBox (lb, displayCol, idCol, lista);
		}

		/// <summary>
		/// Crea y carga combobox seteado con propiedades por defecto. Lo utilizo para crear arrays
		/// </summary>
		/// <param name="displayCol"></param>
		/// <param name="idCol"></param>
		/// <param name="sp"></param>
		/// <param name="parametrosSp"></param>
		/// <returns></returns>
		[Obsolete ("usar metodo con otros parametros")]
		public static ComboBox creaComboBox (string displayCol, string idCol, string sp, params object[] parametrosSp)
		{
			var cmb = new ComboBox ();
			var lista = DB.Instancia.SPToDataSet (sp, parametrosSp).Tables [sp];
			UI.Forms.Controles.CargaComboBox (cmb, displayCol, idCol, lista);
			cmb.SelectedIndex = -1; //para q muestre el primero en blanco
			return cmb;
		}

		/// <summary>
		/// setea valores estandard en datagrid con ajuste automatico de columnas.
		/// </summary>
		/// <remarks>Desabilita la autogeneracion de columnas</remarks>
		/// <param name="dgv"></param>
		public static void setEstandarDataGridView (DataGridView dgv)
		{
			UI.Forms.Controles.DataGridViewEstandar (dgv);
		}

		/// <summary>
		/// carga dtPicker teniendo en cuenta si no hay una fecha seteada.
		/// esteblece automaticamente propiedad checked. Pide showcheckbox.
		/// Comparo con "01/01/1900".
		/// </summary>
		/// <param name="dtp"></param>
		/// <param name="fecha"></param>
		/// <param name = "showCheckBox"></param>
		public static void seteaDTPicker (DateTimePicker dtp, DateTime fecha, Boolean showCheckBox)
		{
			UI.Forms.Controles.SeteaDTPicker (dtp, fecha, showCheckBox);
		}

		/// <summary>
		/// carga dtPicker teniendo en cuenta si no hay una fecha seteada.
		/// esteblece automaticamente propiedad checked y showcheckbox.
		///
		/// Comparo con "01/01/1900".
		/// </summary>
		/// <param name="dtp"></param>
		/// <param name="fecha"></param>
		public static void seteaDTPicker (DateTimePicker dtp, DateTime fecha)
		{
			UI.Forms.Controles.SeteaDTPicker (dtp, fecha);
		}

		/// <summary>
		/// consulta el contenido de un campo de un renglon de un dgv.
		/// </summary>
		/// <param name = "dgv"></param>
		/// <param name = "idColumna">id de columna de la grilla</param>
		/// <returns></returns>
		public static string consultaCampoRenglon (DataGridView dgv, int idColumna)
		{
			return UI.Forms.Controles.ConsultaCampoRenglon (dgv, idColumna);
		}

		/// <summary>
		/// consulta el id del renglon seleccionado en una grilla
		/// </summary>
		/// <param name="dgv"></param>
		/// <returns></returns>
		public static int consultaRenglonSeleccionado (DataGridView dgv)
		{
			return UI.Forms.Controles.ConsultaRenglonSeleccionado (dgv);
		}

		/// <summary>
		/// Carga la imagen solicitada en un control bitmap.
		/// </summary>
		/// <param name="pathAbsoluto">Pasar Application.StartupPath + pathRelativo o pathAbsoluto</param>
		/// <returns></returns>
		public static Bitmap cargarImagen (string pathAbsoluto)
		{
			return UI.Forms.Controles.CargarImagen (pathAbsoluto);
		}
	}
}
