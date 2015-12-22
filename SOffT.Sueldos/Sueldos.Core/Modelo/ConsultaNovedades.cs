//
//  ConsultaNovedades.cs
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

using System.Data;
using Sueldos.Data;
using Sueldos.Entidades;

namespace Sueldos.Modelo
{
	public class ConsultaNovedades
	{
		readonly NovedadData persistencia = new NovedadData ();

		public int insert (NovedadEntity nove)
		{
			return persistencia.insert (nove);
		}

		public int update (NovedadEntity nove)
		{
			return persistencia.update (nove);
		}

		public int delete (NovedadEntity nove)
		{
			return persistencia.delete (nove);
		}

		/// <summary>
		/// Inserta o Actualiza nueva novedad
		/// </summary>
		/// <returns></returns>
		public int grabar (NovedadEntity nove)
		{
			if (persistencia.getById (nove.IdLiquidacion, nove.Legajo, nove.Codigo) == null)
				return persistencia.insert (nove);
			else
				return persistencia.update (nove);
		}

		public NovedadEntity getById (int idLiquidacion, int legajo, double codigo)
		{
			NovedadEntity nove;
			nove = persistencia.getById (idLiquidacion, legajo, codigo);
			return nove;
		}

		public DataSet getAll (int idLiquidacion, string filtro)
		{
			DataSet ds = persistencia.GetAll (idLiquidacion, filtro);
			return ds;
		}
	}
}
