//
//  ConsultaLiquidacionDetalle.cs
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
using System.Data;
using Sueldos.Data;
using Sueldos.Entidades;

namespace Sueldos.Modelo
{
	public class ConsultaLiquidacionDetalle
	{
		readonly LiquidacionDetalleData persistencia = new LiquidacionDetalleData ();

		public int insert (LiquidacionDetalleEntity liqdet)
		{
			return persistencia.insert (liqdet);
		}

		public int update (LiquidacionDetalleEntity liqdet)
		{
			return persistencia.update (liqdet);
		}

		public int delete (LiquidacionDetalleEntity liqdet)
		{
			return persistencia.delete (liqdet);
		}

		public int insertarFechasDePago (List<FechaDePagoEntity> fechas)
		{
			return persistencia.insertFechasDePago (fechas);
		}

		public LiquidacionDetalleEntity getById (int aniomes, int idAplicacion, DateTime fechaLiquidacion)
		{
			LiquidacionDetalleEntity liqdet;
			liqdet = persistencia.getById (aniomes, idAplicacion, fechaLiquidacion);
			return liqdet;
		}

		public LiquidacionDetalleEntity getById (int id)
		{
			LiquidacionDetalleEntity liqdet;
			liqdet = persistencia.getById (id);
			return liqdet;
		}

		/// <summary>
		/// Consulta si una liquidación por estado y aniomes. Devuelve el id o cero si no la encuentra
		/// </summary>
		/// <returns></returns>
		public int getIdByEstado (int aniomes, int estado)
		{
			int id;
			id = persistencia.getIdByEstado (aniomes, estado);
			return id;
		}

		public List<LiquidacionDetalleEntity> getByEstado (int estado)
		{
			List<LiquidacionDetalleEntity> liquidaciones = persistencia.getByEstado (estado);
			return liquidaciones;
		}

		public DataSet getAllGrilla ()
		{
			DataSet ds = persistencia.GetAllGrilla ();
			return ds;
		}
	}
}
