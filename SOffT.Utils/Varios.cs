/*
    Varios.cs

    Author:
    Hernan Vivani <hernan@vivani.com.ar> - http://hvivani.com.ar
    Claudio Rodrigo Pereyra Diaz <claudiorodrigo@pereyradiaz.com.ar>

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
using System.Windows.Forms;
using Hamekoz.Data;

namespace Sofft.Utils
{
	public static class Varios
	{
		public enum DateInterval
		{
			Second,
			Minute,
			Hour,
			Day,
			Week,
			Month,
			Quarter,
			Year
		}

		public static long DateDiff (DateInterval Interval, DateTime StartDate, DateTime EndDate)
		{
			var TS = new TimeSpan (EndDate.Ticks - StartDate.Ticks);
			switch (Interval) {
			case DateInterval.Day:
				return (long)TS.Days;
			case DateInterval.Hour:
				return (long)TS.TotalHours;
			case DateInterval.Minute:
				return (long)TS.TotalMinutes;
			case DateInterval.Month:
				return (long)(TS.Days / 30);
			case DateInterval.Quarter:
				return (long)((TS.Days / 30) / 3);
			case DateInterval.Second:
				return (long)TS.TotalSeconds;
			case DateInterval.Week:
				return (long)(TS.Days / 7);
			case DateInterval.Year:
				return (long)(TS.Days / 365);
			default:
				return 0;
			}
		}
		//end of DateDiff

		public static bool IsNumeric (string theValue)
		{
			try {
				Convert.ToDouble (theValue);
				return true;
			} catch {
				return false;
			}
		}

		//TODO: sacar esto de acá !!!!!!!!!!!!!!!!!!!!
		public static string consultarFechaActual ()
		{
			string fecha;
			fecha = DB.Instancia.SP ("fechaActual").ToString ();
			return fecha;
		}

		/// <summary>
		/// Arma la cadena AnioMes, a partir de un año y mes dados.
		/// formato aaaamm
		/// </summary>
		/// <returns></returns>
		[Obsolete ("Usar string.Format(\"{0:0000}{1:00}, anio, mes\");")]
		public static string anioMes (int anio, byte mes)
		{
			string strMes;
			if (mes > 9)
				strMes = mes.ToString ();
			else
				strMes = 0 + mes.ToString ();
			return anio + strMes;
		}

		/// <summary>
		/// realiza consulta generica para confirmar la eliminacion de un registro
		/// Está seguro de eliminar el registro ?
		/// </summary>
		/// <returns></returns>
		[Obsolete ("Usar ConfirmarEliminarRegitro()")]
		public static bool confirmaEliminarRegistro ()
		{
			return ConfirmaEliminarRegistro ();
		}

		/// <summary>
		/// realiza consulta generica para confirmar la eliminacion de un registro
		/// Está seguro de eliminar el registro ?
		/// </summary>
		/// <returns></returns>
		public static bool ConfirmaEliminarRegistro ()
		{
			DialogResult result = MessageBox.Show ("Está seguro de eliminar el registro ?", "Caption", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			return result == DialogResult.Yes;
		}

		/// <summary>
		/// Obtiene parte izquierda de una cadena
		/// </summary>
		/// <param name="param"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static string Left (this string param, int length)
		{
			string result = param;
			//we start at 0 since we want to get the characters starting from the
			//left and with the specified lenght and assign it to a variable
			if (param.Length > length)
				result = param.Substring (0, length);
			//return the result of the operation
			return result;
		}

		/// <summary>
		/// obtiene parte derecha de una cadena
		/// </summary>
		/// <param name="param"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static string Right (this string param, int length)
		{
			//start at the index based on the lenght of the sting minus
			//the specified lenght and assign it a variable
			string result = param.Substring (param.Length - length, length);
			//return the result of the operation
			return result;
		}

		/// <summary>
		/// obtiene caracteres dada una posicion de inicio y longitud
		/// </summary>
		/// <param name="param"></param>
		/// <param name="startIndex"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static string Mid (this string param, int startIndex, int length)
		{
			//start at the specified index in the string ang get N number of
			//characters depending on the lenght and assign it to a variable
			string result = param.Substring (startIndex, length);
			//return the result of the operation
			return result;
		}

		/// <summary>
		/// Obtiene caracteres a partir de una posicion dada hasta el final
		/// </summary>
		/// <param name="param"></param>
		/// <param name="startIndex"></param>
		/// <returns></returns>
		public static string Mid (this string param, int startIndex)
		{
			//start at the specified index and return all characters after it
			//and assign it to a variable
			string result = param.Substring (startIndex);
			//return the result of the operation
			return result;
		}
	}
}