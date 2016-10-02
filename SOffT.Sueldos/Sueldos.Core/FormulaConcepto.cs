//
//  FormulaConcepto.cs
//
//  Author:
//       Claudio Rodrigo Pereyra Diaz <claudiorodrigo@pereyradiaz.com.ar>
//
//  Copyright (c) 2010 Hamekoz - www.hamekoz.com.ar
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
using Hamekoz.Data;

namespace Sueldos.Core
{
	class FormulaConcepto
	{
		readonly Queue<string> formulaCompilador;

		public FormulaConcepto()
		{
			formulaCompilador = new Queue<string>();
		}

		public string CadenaCompilador
		{
			get
			{
				string cadena = "";
				foreach (string elemento in formulaCompilador)
				{
					cadena = cadena + elemento + " ";
				}
				return cadena;
			}
			set
			{
				string[] expresiones;
				expresiones = value.Split(" ".ToCharArray());
				for (int i = 0; i < expresiones.Length; i++)
				{
					formulaCompilador.Enqueue(expresiones[i]);
				}
			}
		}

		public string CadenaNatural
		{
			get
			{
				string cadena = "";
				foreach (string elemento in formulaCompilador)
				{
					cadena = cadena + Traducir(elemento) + " ";
				}
				return cadena;
			}
		}

		static string Traducir(string instruccion)
		{
			switch (instruccion)
			{
				case "?":
					return "entonces";
				case ":":
					return "sino";
				case "||":
					return "o";
				case "&&":
					return "y";
				case "<>":
					return "disntinto de";
				case ">=":
					return "mayor o igual que";
				case "<=":
					return "menor o igual que";
				case ">":
					return "mayor que";
				case "<":
					return "menor que";
				case "+":
					return instruccion;
				case "-":
					return instruccion;
				case "/":
					return instruccion;
				case "*":
					return instruccion;
				case "(":
					return instruccion;
				case ")":
					return instruccion;
				case ",":
					return instruccion;
				case "":
					return " ";
				case " ":
					return " ";
				default:
					return Consultar(instruccion);
			}
		}

		static string Consultar(string instrucion)
		{
			string[] cadena = instrucion.Split("(),\"".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

			switch (cadena[0])
			{
				case "campoEmpleado":
					return DB.Instancia.SPToScalar("tablaConsultarDescipcion", "tabla", "empleadosSueldos", "indice", cadena[1]).ToString();
				case "variable":
					return DB.Instancia.SPToScalar("tablaConsultarDescipcion", "tabla", "VAR", "indice", cadena[1]).ToString();
				case "acumulador":
					return DB.Instancia.SPToScalar("tablaConsultarDescipcion", "tabla", "ACU", "indice", cadena[1]).ToString();
				case "novedadEmpleado":
					return DB.Instancia.SPToScalar("tablaConsultarDescipcion", "tabla", "novedades", "indice", cadena[1]).ToString();
				case "tablas":
					return DB.Instancia.SPToScalar("tablaConsultarDescipcion", "tabla", cadena[1], "indice", cadena[2]).ToString();
				case "fechaEmpleado":
					return DB.Instancia.SPToScalar("tablaConsultarDescipcion", "tabla", "empleadosSueldos", "indice", cadena[1]).ToString();
				case "acumuladoEmpleado":
					return DB.Instancia.SPToScalar("tablaConsultarDescipcion", "tabla", "acumulados", "indice", cadena[1]).ToString();
				case "asistenciaEmpleadoEntreDias":
					return DB.Instancia.SPToScalar("tablaConsultarDescipcion", "tabla", "asistencia", "indice", cadena[1])
					+ " entre " + cadena[2] + " y " + cadena[3];
				case "acumuladoMayorEntreFechas":
					return DB.Instancia.SPToScalar("tablaConsultarDescipcion", "tabla", "acumulados", "indice", cadena[1])
					+ " entre " + cadena[2] + " y " + cadena[3];
				case "saltar":
					return cadena[0] + " a ";
				default:
					return instrucion;
			}
		}

		public void Agregar(string valor)
		{
			formulaCompilador.Enqueue(valor);
		}

		public string Quitar()
		{
			return formulaCompilador.Dequeue();
		}
	}
}
