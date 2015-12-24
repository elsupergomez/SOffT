//
//  BancoGalicia.cs
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
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Windows.Forms;
using Hamekoz.Data;
using Sofft.Utils;

namespace Sueldos.View
{
	/// <summary>
	/// Clase que define estructura de archivos para acreditacion de sueldos en cuentas
	/// </summary>
	static class BancoGalicia
	{
		public const string nombreArchivo = "balcarce-galicia.txt";

		public struct cabecera
		{
			public const string tipoRegistro = "H";
			//H:header
			public const string empresas = "52097";
			//Código de prestación de la empresa
			public const string tipoCuenta = "0";
			//Indica si la cuenta de débito es CC (0) o CA (4)
			public const string folio = "025002";
			//Folio de la cuenta de débito
			public const string digito1 = "3";
			//Dígito 1 de la cuenta de débito
			public const string sucursal = "083";
			//Sucursal de la cuenta de débito
			public const string digito2 = "4";
			//Dígito 2 de la cuenta de débito
			public static string importe = "              ";
			//Importe total del archivo (12 enteros + 2 decimales)
			public static string fecAcred;
			//Fecha de acreditación
			public const string filler = "                         ";
			//25
		}

		public struct movimiento
		{
			public const string tipoRegistro = "D";
			//D: detalle
			public static string empresas = "52097";
			//Código de prestación de la empresa
			public static string tipoCuenta = "";
			//0 ctacte - 4 C.A.
			public static string folio = "      ";
			//Folio de la cuenta de crédito
			public static string digito1;
			//digito 1 de la cuenta de crédito
			public static string sucursal = "";
			//sucursal cuenta credito
			public static string digito2 = "";
			//digito 2 de la cuenta de crédito
			public static string nombre = "                    ";
			//Nombre del titular de la cuenta de crédito
			public static string importe = "              ";
			//Importe a acreditar (12 enteros + 2 decimales).
			public static string concepto = "  ";
			//00 si la empresa no manda nada //01 HABERES - 04 SAC
			public const string filler = "           ";
			//11
		}

		public struct final
		{
			public const string tipoRegistro = "F";
			//F:footer
			public const string empresas = "52097";
			//Código de prestación de la empresa
			public static string cantRegistros;
			//Cantidad de registros
			public const string filler = "                                                    ";
			//52
		}

		/// <summary>
		/// Genera archivo con formato galica.
		/// </summary>
		public static void generaArchivo (int idliquidacion, string archivo, List<int> tiposLiquidacionesSeleccionados, DateTime fechaAcreditacion, bool todosLosConveios, int idConvenio)
		{
			//00 si la empresa no manda nada //01 HABERES - 04 SAC - 02 HS EXTRAS - 05 VACACIONES
			/*si el indice 0 es 1, se seleccionó la liquidacion normal.*/
			if (tiposLiquidacionesSeleccionados [0] == 4) //SAC
                movimiento.concepto = "04";
			else if (tiposLiquidacionesSeleccionados [0] == 3) // VACACIONES
                    movimiento.concepto = "05";
			else
				movimiento.concepto = "01"; //HABERES
			string importe;
			int cantRegistros = 0;
			double importeTotal = 0;
			DbDataReader rsLegajos;
			var sw = new StreamWriter (archivo);

			//recorro netos para obtener total del archivo. este dato va en cabecera
			if (todosLosConveios)
				rsLegajos = DB.Instancia.SPToDbDataReader ("liquidacionesNetosPorLegajoPorBanco", "@idLiquidacion", idliquidacion, "@idFormaDePago", 2, "@idBanco", 2);
			else
				rsLegajos = DB.Instancia.SPToDbDataReader ("liquidacionesNetosPorLegajoPorBancoPorConvenio", "@idLiquidacion", idliquidacion, "@idFormaDePago", 2, "@idBanco", 2, "@idConvenio", idConvenio);

			while (rsLegajos.Read ()) {  //para cada legajo recorro todos los campos definidos en tablas
				importe = string.Format ("{0:#0.00}", Convert.ToDouble (rsLegajos ["Neto"]));
				importeTotal = importeTotal + Convert.ToDouble (importe);

			}
			cabecera.importe = string.Format ("{0:#0.00}", importeTotal).Replace (".", "").PadLeft (14, '0');
			cabecera.fecAcred = Varios.Right (fechaAcreditacion.Year.ToString (), 4) + fechaAcreditacion.Month.ToString ().PadLeft (2, '0') + fechaAcreditacion.Day.ToString ().PadLeft (2, '0');
			//GRABA CABECERA.
			sw.WriteLine (cabecera.tipoRegistro + cabecera.empresas + cabecera.tipoCuenta + cabecera.folio + cabecera.digito1 + cabecera.sucursal + cabecera.digito2 + cabecera.importe + cabecera.fecAcred + cabecera.filler);

			//recorrer netos por legajo
			//consultar netos por idliquidacion.
			//repoteLiquidacionesNetoPorLegajo. calcular haberes+adicionales-retenciones
			//rsLegajos = Model.DB.ejecutarDataReader(Model.TipoComando.SP, "liquidacionesNetosPorLegajoPorBanco", "@idLiquidacion", idliquidacion, "@idFormaDePago", 2, "@idBanco", 2);
			if (todosLosConveios)
				rsLegajos = DB.Instancia.SPToDbDataReader ("liquidacionesNetosPorLegajoPorBanco", "@idLiquidacion", idliquidacion, "@idFormaDePago", 2, "@idBanco", 2);
			else
				rsLegajos = DB.Instancia.SPToDbDataReader ("liquidacionesNetosPorLegajoPorBancoPorConvenio", "@idLiquidacion", idliquidacion, "@idFormaDePago", 2, "@idBanco", 2, "@idConvenio", idConvenio);
			while (rsLegajos.Read ()) {  //para cada legajo recorro todos los campos definidos en tablas
				cantRegistros++;
				movimiento.nombre = Varios.Left (rsLegajos ["Apellidos y nombres"].ToString (), 20).PadRight (20, ' ');
				importe = string.Format ("{0:#0.00}", Convert.ToDouble (rsLegajos ["Neto"]));
				importe = importe.Replace (".", "");
				movimiento.importe = importe.PadLeft (14, '0');
				movimiento.sucursal = Varios.Right (DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "legajo", Convert.ToInt32 (rsLegajos ["legajo"]), "codigo", 22).ToString (), 4);
				movimiento.digito2 = Varios.Right (movimiento.sucursal, 1);
				movimiento.sucursal = Varios.Left (movimiento.sucursal, 3);
				//movimiento.sucursal = Varios.Right(movimiento.sucursal, 3);
				movimiento.folio = Varios.Left (DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "legajo", Convert.ToInt32 (rsLegajos ["legajo"]), "codigo", 22).ToString (), 8);
				movimiento.digito1 = Varios.Right (movimiento.folio, 1);
				movimiento.tipoCuenta = Varios.Left (movimiento.folio, 1);
				movimiento.folio = Varios.Right (Varios.Left (movimiento.folio, 7), 6);
				//movimiento.folio = Varios.Right(movimiento.folio, 6);
				//movimiento.digito1 = Varios.Left(movimiento.folio, 1);
				//movimiento.digito2 = Varios.Right( Varios.Left(movimiento.folio, 2),1);
				if (movimiento.folio.Length == 0 || movimiento.sucursal == "0")
					MessageBox.Show ("ATENCIÓN: el empleado " + movimiento.nombre + " no tiene cuenta o banco asignado. No se exportara la liquidación.");
				else {
					//GRABA DETALLE
					sw.WriteLine (movimiento.tipoRegistro + movimiento.empresas + movimiento.tipoCuenta + movimiento.folio + movimiento.digito1 + movimiento.sucursal + movimiento.digito2 + movimiento.nombre + movimiento.importe + movimiento.concepto + movimiento.filler);
					//actualiza liquidacionesEstados
					for (int i = 0; i < tiposLiquidacionesSeleccionados.Count; i++)
						DB.Instancia.SP ("liquidacionesEstadosActualizar", "@idLiquidacion", idliquidacion, "@idTipoLiquidacion", tiposLiquidacionesSeleccionados [i], "@legajo", Convert.ToInt32 (rsLegajos ["legajo"].ToString ()), "@acreditada", true, "fechaAcreditacion", fechaAcreditacion);
				}
			}
			final.cantRegistros = cantRegistros.ToString ().PadLeft (7, '0');
			//grabar final.
			sw.WriteLine (final.tipoRegistro + final.empresas + final.cantRegistros + final.filler);
			sw.Close ();
		}

		public static void generaArchivoDesdeAnticipos (int anioMes, int idTipoAnticipo, string archivo, DateTime fechaAcreditacion, bool todosLosConveios, int idConvenio)
		{
			//00 si la empresa no manda nada //01 HABERES - 04 SAC - 02 HS EXTRAS - 05 VACACIONES
			/*si el indice 0 es 1, se seleccionó la liquidacion normal.*/

			movimiento.concepto = "01"; //HABERES
			string importe;
			int cantRegistros = 0;
			double importeTotal = 0;
			DbDataReader rsLegajos;
			var sw = new StreamWriter (archivo);

			//recorro netos para obtener total del archivo. este dato va en cabecera
			if (todosLosConveios)
				rsLegajos = DB.Instancia.SPToDbDataReader ("anticiposConsultarParaAcreditarPorBanco", "@anioMes", anioMes, "@idTipoAnticipo", idTipoAnticipo, "@idBanco", 2);
			else
				rsLegajos = DB.Instancia.SPToDbDataReader ("anticiposConsultarParaAcreditarPorBancoPorConvenio", "@anioMes", anioMes, "@idTipoAnticipo", idTipoAnticipo, "@idBanco", 2, "@idConvenio", idConvenio);

			while (rsLegajos.Read ()) {  //para cada legajo recorro todos los campos definidos en tablas
				importe = string.Format ("{0:#0.00}", Convert.ToDouble (rsLegajos ["Neto"]));
				importeTotal = importeTotal + Convert.ToDouble (importe);

			}
			cabecera.importe = string.Format ("{0:#0.00}", importeTotal).Replace (".", "").PadLeft (14, '0');
			cabecera.fecAcred = Varios.Right (fechaAcreditacion.Year.ToString (), 4) + fechaAcreditacion.Month.ToString ().PadLeft (2, '0') + fechaAcreditacion.Day.ToString ().PadLeft (2, '0');
			//GRABA CABECERA.
			sw.WriteLine (cabecera.tipoRegistro + cabecera.empresas + cabecera.tipoCuenta + cabecera.folio + cabecera.digito1 + cabecera.sucursal + cabecera.digito2 + cabecera.importe + cabecera.fecAcred + cabecera.filler);

			//recorrer netos por legajo
			//consultar netos por idliquidacion.
			//repoteLiquidacionesNetoPorLegajo. calcular haberes+adicionales-retenciones
			if (todosLosConveios)
				rsLegajos = DB.Instancia.SPToDbDataReader ("anticiposConsultarParaAcreditarPorBanco", "@anioMes", anioMes, "@idTipoAnticipo", idTipoAnticipo, "@idBanco", 2);
			else
				rsLegajos = DB.Instancia.SPToDbDataReader ("anticiposConsultarParaAcreditarPorBancoPorConvenio", "@anioMes", anioMes, "@idTipoAnticipo", idTipoAnticipo, "@idBanco", 2, "@idConvenio", idConvenio);

			while (rsLegajos.Read ()) {  //para cada legajo recorro todos los campos definidos en tablas
				cantRegistros++;
				movimiento.nombre = Varios.Left (rsLegajos ["Apellidos y nombres"].ToString (), 20).PadRight (20, ' ');
				importe = string.Format ("{0:#0.00}", Convert.ToDouble (rsLegajos ["Neto"]));
				importe = importe.Replace (".", "");
				movimiento.importe = importe.PadLeft (14, '0');
				movimiento.sucursal = Varios.Right (DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "legajo", Convert.ToInt32 (rsLegajos ["legajo"]), "codigo", 22).ToString (), 4);
				movimiento.digito2 = Varios.Right (movimiento.sucursal, 1);
				movimiento.sucursal = Varios.Left (movimiento.sucursal, 3);
				movimiento.folio = Varios.Left (DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "legajo", Convert.ToInt32 (rsLegajos ["legajo"]), "codigo", 22).ToString (), 8);
				movimiento.digito1 = Varios.Right (movimiento.folio, 1);
				movimiento.tipoCuenta = Varios.Left (movimiento.folio, 1);
				movimiento.folio = Varios.Right (Varios.Left (movimiento.folio, 7), 6);
				if (movimiento.folio.Length == 0 || movimiento.sucursal == "0")
					MessageBox.Show ("ATENCIÓN: el empleado " + movimiento.nombre + " no tiene cuenta o banco asignado. No se exportara la liquidación.");
				else {
					//GRABA DETALLE
					sw.WriteLine (movimiento.tipoRegistro + movimiento.empresas + movimiento.tipoCuenta + movimiento.folio + movimiento.digito1 + movimiento.sucursal + movimiento.digito2 + movimiento.nombre + movimiento.importe + movimiento.concepto + movimiento.filler);
				}
			}
			final.cantRegistros = cantRegistros.ToString ().PadLeft (7, '0');
			//grabar final.
			sw.WriteLine (final.tipoRegistro + final.empresas + final.cantRegistros + final.filler);
			sw.Close ();
			rsLegajos.Close ();
		}
	}
}
