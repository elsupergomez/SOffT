//
//  ConsultaEmpresas.cs
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
	public class ConsultaEmpresas
	{
		readonly EmpresaData persistencia = new EmpresaData ();

		public int insert (EmpresaEntity empresa)
		{
			return persistencia.insert (empresa);
		}

		public int update (EmpresaEntity empresa)
		{
			return persistencia.update (empresa);
		}

		public int delete (EmpresaEntity empresa)
		{
			return persistencia.delete (empresa);
		}

		public EmpresaEntity getById (int id)
		{
			return persistencia.getById (id);
		}

		public List<EmpresaEntity> getAll ()
		{
			return persistencia.GetAll ();
		}
	}
}
