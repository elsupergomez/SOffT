//
//  Liquidacion.cs
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
using Hamekoz.Data;

namespace Sueldos.Core
{
	public class Liquidacion
	{
		public Liquidacion()
		{
			FechasDePago = new List<DateTime>();
		}

		/// <summary>
		/// Constructor para Liquidacion existente.
		/// </summary>
		/// <param name = "idLiquidacion"></param>
		public Liquidacion(int idLiquidacion)
		{
			Id = idLiquidacion;
			FechasDePago = new List<DateTime>();
			cargarDatos();
		}

		public int Id
		{
			get;
			set;
		}

		public int AnioMes
		{
			get;
			set;
		}

		public int IdAplicacion
		{
			get;
			set;
		}

		public int IdTipoSalario
		{
			get;
			set;
		}

		public string Descripcion
		{
			get;
			set;
		}

		public DateTime FechaLiquidacion
		{
			get;
			set;
		}

		public string PeriodoLiquidado
		{
			get;
			set;
		}

		public string LugarDePago
		{
			get;
			set;
		}

		public IList<DateTime> FechasDePago
		{
			get;
			set;
		}

		public string PeriodoDepositado
		{
			get;
			set;
		}

		public string BancoDepositado
		{
			get;
			set;
		}

		public DateTime FechaDepositado
		{
			get;
			set;
		}


		public bool IdEstado
		{
			get;
			set;
		}

		public bool RecibosSeparados
		{
			get;
			set;
		}

		protected void cargarDatos()
		{
			//cargo datos liquidacion
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader("liquidacionesDetalleConsultar", "@id", Id))
			{
				if (rs.Read())
				{
					AnioMes = Convert.ToInt32(rs["anioMes"]);
					IdAplicacion = Convert.ToInt32(rs["idAplicacion"]);
					IdTipoSalario = Convert.ToInt32(rs["idTipoSalario"]);
					Descripcion = rs["Descripcion"].ToString();
					FechaLiquidacion = Convert.ToDateTime(rs["fechaLiquidacion"]);
					PeriodoLiquidado = rs["periodoLiquidado"].ToString();
					LugarDePago = rs["lugarDePago"].ToString();
					PeriodoDepositado = rs["periodoDepositado"].ToString();
					BancoDepositado = rs["bancoDepositado"].ToString();
					FechaDepositado = Convert.ToDateTime(rs["fechaDepositado"]);
					IdEstado = Convert.ToBoolean(rs["estado"]);
					RecibosSeparados = Convert.ToBoolean(rs["recibosSeparados"]);
				}
			}
		}
	}
}
