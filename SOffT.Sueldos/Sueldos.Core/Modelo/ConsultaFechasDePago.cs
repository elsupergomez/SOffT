﻿//
//  ConsultaFechasDePago.cs
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
using Sueldos.Data;
using Sueldos.Entidades;

namespace Sueldos.Modelo
{
	public class ConsultaFechasDePago
	{
		readonly FechaDePagoData persistencia = new FechaDePagoData ();

		public int insert (FechaDePagoEntity fecha)
		{
			return persistencia.insert (fecha);
		}

		public int update (FechaDePagoEntity fecha)
		{
			return persistencia.update (fecha);
		}

		public int delete (FechaDePagoEntity fecha)
		{
			return persistencia.delete (fecha);
		}

		public FechaDePagoEntity getById (int idLiquidacion, DateTime fechaDePago)
		{
			FechaDePagoEntity fecha;
			fecha = persistencia.getById (idLiquidacion, fechaDePago);
			return fecha;
		}

		public List<FechaDePagoEntity> getAll (int idLiquidacion)
		{
			List<FechaDePagoEntity> fechas = persistencia.GetAll (idLiquidacion);
			return fechas;
		}
	}
}
