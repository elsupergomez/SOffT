//
//  PlanillaDeCalculo.cs
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
using System.Data.Common;
using System.Windows.Forms;
using ExcelLibrary;
using System.Diagnostics;

namespace Sueldos.View
{
	public static class PlanillaDeCalculo
	{
		public static void XLS (this DataSet dataSet)
		{
			dataSet.XLS (dataSet.DataSetName);
		}

		public static void XLS (this DataSet dataSet, string nombreArchivo)
		{
			var saveFileDialog = new SaveFileDialog ();
			saveFileDialog.Filter = "Planilla de Calculo (*.xls)|*.xls";
			saveFileDialog.FileName = nombreArchivo;
			if (saveFileDialog.ShowDialog () == DialogResult.OK) {
				DataSetHelper.CreateWorkbook (saveFileDialog.FileName, dataSet);
				MessageBox.Show ("Reporte exportado con exito" + Environment.NewLine + saveFileDialog.FileName);
				Process.Start (saveFileDialog.FileName);
			}
		}

		static readonly DbProviderFactory factory = DbProviderFactories.GetFactory ("System.Data.OleDb");

		/// <summary>
		/// Lee la estructura del esquema del libro excel y devuelve dicha estructura en un dataTable.
		/// Se utiliza para obtener los nombres de las "hojas" en los libros excel.
		/// </summary>
		/// <param name="archivo"></param>
		/// <returns></returns>
		public static DataTable LeerEsquemaLibro (string archivo)
		{
			//Los archivos de excel lo manejamos con el namespace "System.Data.OleDb".
			//Crearemos un dataset y un providerfactories el cual utilizaremos para leer es esquela del libro de excel:

			DataTable worksheets = null;
			string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + archivo + ";Extended Properties=Excel 8.0;";

			//Con esta instrución leeremos el esquema del libro de excel
			try {
				DbConnection connection = factory.CreateConnection ();
				connection.ConnectionString = connectionString;
				connection.Open ();
				worksheets = connection.GetSchema ("Tables");
			} catch (Exception) {
				MessageBox.Show ("Se produjo un error. Puede ser que la hoja de calculo a abrir no exista o posea un esquema diferente.");
			}
			return worksheets;
		}

		[Obsolete ("Usar LeerEsquemaLibro()")]
		public static DataTable leerEsquemaLibro (string archivo)
		{
			return LeerEsquemaLibro (archivo);
		}

		/// <summary>
		/// Lee la estructura de columnas de una hoja excel.
		/// Antes de utilizarse deben obtenerse los nombres de las hojas.
		/// </summary>
		/// <param name="archivo"></param>
		/// <param name="nombreHoja"></param>
		/// <returns></returns>
		public static DataTable LeerEsquemaHoja (string archivo, string nombreHoja)
		{
			DataTable columns = null;
			string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + archivo + ";Extended Properties=Excel 8.0;";

			//una vez leido el esquema del libro de excel, leeremos el esquema de la hoja,
			//para obtener las calumnas. Mediante la siguiente instrucción obtendremos lo mencionado.
			try {
				string[] restrictions = { null, null, nombreHoja, null };
				DbConnection connection = factory.CreateConnection ();
				connection.ConnectionString = connectionString;
				connection.Open ();
				columns = connection.GetSchema ("Columns", restrictions);
				/*this.lstColumnas.Items.Clear();
                int i = 0;
                while (i < columns.Rows.Count)
                {
                    this.lstColumnas.Items.Add(columns.Rows[i].ItemArray[3].ToString());
                    i++;
                }
                this.dgvColumnas.DataSource = columns;*/
			} catch (Exception) {
				MessageBox.Show ("Se produjo un error. Puede ser que la hoja de calculo a abrir no exista o posea un esquema diferente.");
			}
			return columns;
		}

		[Obsolete ("USar LeerEsquemaHoja")]
		public static DataTable leerEsquemaHoja (string archivo, string nombreHoja)
		{
			return LeerEsquemaHoja (archivo, nombreHoja);
		}

		/// <summary>
		/// Devuelve en un dataTable una hoja completa de excel. Se debe obtener previamente el
		/// nombre de la hoja.
		/// </summary>
		/// <param name="archivo"></param>
		/// <param name="hoja"></param>
		/// <returns></returns>
		public static DataTable LeerDatosHoja (string archivo, string hoja)
		{
			var dsMsExcel = new DataSet ();
			//leeremos los datos de la hoja de calculo de excel, con la siguiente instrucción
			//y lo mostraremos en un DataGridView
			try {
				string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + archivo + ";Extended Properties=Excel 8.0;";
				DbDataAdapter adapter = factory.CreateDataAdapter ();
				DbCommand selectCommand = factory.CreateCommand ();

				selectCommand.CommandText = "SELECT * FROM [" + hoja + "]";
				DbConnection connection = factory.CreateConnection ();
				connection.ConnectionString = connectionString;
				selectCommand.Connection = connection;

				adapter.SelectCommand = selectCommand;
				dsMsExcel.Tables.Clear ();
				adapter.Fill (dsMsExcel);

			} catch (Exception ex) {
				MessageBox.Show (ex.Message + "Se produjo un error.");
			}
			return dsMsExcel.Tables [0];
		}

		[Obsolete ("Usar LeerDatosHoja()")]
		public static DataTable leerDatosHoja (string archivo, string hoja)
		{
			return LeerDatosHoja (archivo, hoja);
		}
	}
}

