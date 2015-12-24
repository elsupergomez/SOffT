﻿//
//  FondoDesempleo.cs
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
using System.IO;
using System.Windows.Forms;
using Hamekoz.Data;
using System.Data.Common;

namespace Sueldos.View
{
	/// <summary>
	/// Clase que define estructura de archivos para acreditacion del fondo desempleo
	/// en banco nacion
	/// </summary>
	static class FondoDesempleo
	{
		public struct movi
		{
			public const string casa = "11000";
			public static string cuenta = "";
			public const string transaccion = "128";
			public const string empresa = "KIOS";
			public const string filler = "               ";
			public static string importe = "";
		}

		public static void generaArchivo (int anioMes, string archivo)
		{
			string importe;
			var sw = new StreamWriter (archivo);
			//recorrer netos por legajo
			//consultar netos por idliquidacion.
			//repoteLiquidacionesNetoPorLegajo. calcular haberes+adicionales-retenciones
			using (DbDataReader rsLegajos = DB.Instancia.SPToDbDataReader ("sindicatosConsultarPorCodigo", "@anioMes", anioMes, "@codigo", 11)) {
				while (rsLegajos.Read ()) {  //para cada legajo recorro todos los campos definidos en tablas
					importe = string.Format ("{0:#0.00}", Convert.ToDouble (rsLegajos ["Valor"]));
					importe = importe.Replace (".", "");
					movi.importe = importe.PadLeft (11, '0');
					try {
						movi.cuenta = DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "legajo", Convert.ToInt32 (rsLegajos ["Legajo"]), "codigo", 105).ToString ();
					} catch (Exception e) {
						MessageBox.Show ("error al obtener cuenta legajo " + int.Parse (rsLegajos ["legajo"].ToString ()) + "\n" + e);
					}
					movi.cuenta = movi.cuenta.PadLeft (9, '0');
					if (movi.cuenta.Length == 0)
						MessageBox.Show ("ATENCIÓN: el legajo " + int.Parse (rsLegajos ["legajo"].ToString ()) + " no tiene cuenta o banco asignado. No se exportara el importe.");
					else {
						try {
							sw.WriteLine (movi.casa + movi.cuenta + movi.transaccion + movi.empresa + movi.filler + movi.importe);
						} catch (Exception e) {
							MessageBox.Show ("Error al insertar registro en archivo txt. \n" + e);
						}
					}
				}
			}
			sw.Close ();
		}
	}
}
