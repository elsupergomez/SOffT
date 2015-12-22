//
//  TablaController.cs
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
	public class ConsultaTablas : TablaController
	{

	}

	public class TablaController
	{
		readonly TablaData persistencia = new TablaData ();

		public int insert (TablaEntity tabla)
		{
			return persistencia.insert (tabla);
		}

		public int update (TablaEntity tabla)
		{
			return persistencia.update (tabla);
		}

		public int delete (TablaEntity tabla)
		{
			return persistencia.delete (tabla);
		}

		/// <summary>
		/// Crea una tabla y carga datos de tabla preexistente.
		/// </summary>
		public TablaEntity getById (string nombre, int indice, double contenido)
		{
			TablaEntity tabla;
			tabla = persistencia.getById (nombre, indice, contenido);
			return tabla;
		}

		/// <summary>
		/// Obtiene la lista de tablas 
		/// </summary>
		/// <returns>Lista de tablas</returns>
		public List<TablaEntity> getContenidoYdetalle (string nombreTabla, int indice)
		{
			List<TablaEntity> tablas = persistencia.getByContenidoYdetalle (nombreTabla, indice);
			return tablas;
		}

		/// <summary>
		/// Obtiene los nombres de las tablas
		/// </summary>
		/// <returns></returns>
		public List<TablaEntity> getNombres ()
		{
			List<TablaEntity> tablas = persistencia.getNombresTablas ();
			return tablas;
		}

		/// <summary>
		/// Obtiene el contenido de una tabla por su nombre
		/// </summary>
		/// <returns></returns>
		public List<TablaEntity> getByNombre (string nombre)
		{
			List<TablaEntity> tablas = persistencia.getByNombre (nombre);
			return tablas;
		}

		/// <summary>
		/// Obtiene el contenido de una tabla por su nombre
		/// Debe especificarse campo/s de ordenacion
		/// </summary>
		/// <returns></returns>
		public List<TablaEntity> getByNombre (string nombre, string orden)
		{
			List<TablaEntity> tablas = persistencia.getByNombre (nombre, orden);
			return tablas;
		}

		/// <summary>
		/// Obtiene el contenido de una tabla por su nombre
		/// Se aceptan filtros
		/// Debe especificarse campo/s de ordenacion
		/// </summary>
		/// <returns></returns>
		public List<TablaEntity> getByNombre (string nombre, string filtro, string orden)
		{
			List<TablaEntity> tablas = persistencia.getByNombre (nombre, filtro, orden);
			return tablas;
		}
	}
}
