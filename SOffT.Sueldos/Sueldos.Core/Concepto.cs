//
//  Concepto.cs
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
using System.Data.Common;
using Hamekoz.Data;

namespace Sueldos.Core
{
	public class Concepto
	{
		public Concepto()
		{
		}

		/// <summary>
		/// constructor para cargar datos de un concepto existente
		/// </summary>
		/// <param name="codigo"></param>
		/// <param name = "idcalculo"></param>
		public Concepto(int codigo, int idcalculo)
		{
			Codigo = codigo;
			IdCalculo = idcalculo;
		}

		public int IdCalculo
		{
			get;
			set;
		}

		public int OrdenDeProceso
		{
			get;
			set;
		}

		public int Codigo
		{
			get;
			set;
		}

		public string Descripcion
		{
			get;
			set;
		}

		public string Formula
		{
			get;
			set;
		}

		public byte Tipo
		{
			get;
			set;
		}

		public bool Imprime
		{
			get;
			set;
		}

		public bool ImprimeCantidad
		{
			get;
			set;
		}

		public bool ImprimeValorUnitario
		{
			get;
			set;
		}

		public bool Desactivado
		{
			get;
			set;
		}

		public int IdTipoLiquidacion
		{
			get;
			set;
		}

		public int IdAplicacion
		{
			get;
			set;
		}

		public int IdCuentaContable
		{
			get;
			set;
		}

		public void cargarDatosConcepto()
		{
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader("calculoConsultar", "@codigo", Codigo, "@idCalculo", IdCalculo))
			{
				if (rs.Read())
				{
					OrdenDeProceso = Convert.ToInt32(rs["OrdenProceso"]);
					IdCalculo = Convert.ToInt32(rs["idCalculo"]);
					Descripcion = rs["Descripcion"].ToString();
					Formula = rs["formula"].ToString();
					Tipo = Convert.ToByte(rs["tipo"]);
					Imprime = Convert.ToBoolean(rs["imprime"]);
					ImprimeCantidad = Convert.ToBoolean(rs["imprimeCantidad"]);
					ImprimeValorUnitario = Convert.ToBoolean(rs["imprimeVU"]);
					Desactivado = Convert.ToBoolean(rs["desactivado"]);
					IdTipoLiquidacion = Convert.ToInt32(rs["idTipoLiquidacion"]);
					IdAplicacion = Convert.ToInt32(rs["idAplicacion"]);
					IdCuentaContable = Convert.ToInt32(rs["idCuentaContable"]);
				}
			}
		}
	}
}
