﻿//
//  ConsultaLiquidacionesPorTipo.cs
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

using System.Collections.Generic;
using Sueldos.Data;
using Sueldos.Entidades;

namespace Sueldos.Modelo
{
	public class ConsultaLiquidacionesPorTipo
	{
		readonly LiquidacionPorTipoData persistencia = new LiquidacionPorTipoData ();

		public int insert (LiquidacionPorTipoEntity liq)
		{
			return persistencia.insert (liq);
		}

		public int update (LiquidacionPorTipoEntity liq)
		{
			return persistencia.update (liq);
		}

		public int delete (LiquidacionPorTipoEntity liq)
		{
			return persistencia.delete (liq);
		}

		public List<LiquidacionPorTipoEntity> getAll (int idLiquidacion)
		{
			List<LiquidacionPorTipoEntity> liquis = persistencia.GetAll (idLiquidacion);
			return liquis;
		}
	}
}