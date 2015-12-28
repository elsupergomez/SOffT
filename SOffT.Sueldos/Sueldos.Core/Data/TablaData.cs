//
//  TablaData.cs
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

﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Hamekoz.Data;
using Sueldos.Entidades;

namespace Sueldos.Data
{
	public class TablaData
	{
		const string tabladb = "tablas";

		#region ABM

		public int insert (TablaEntity tabla)
		{
			var sql = new StringBuilder ();
			sql.Append (" INSERT INTO ");
			sql.Append (tabladb);
			sql.Append (" VALUES('");
			sql.Append (tabla.Nombre);
			sql.Append ("', ");
			sql.Append (tabla.Indice);
			sql.Append (", '");
			sql.Append (tabla.Descripcion);
			sql.Append ("', ");
			sql.Append (tabla.Contenido);
			sql.Append (", '");
			sql.Append (tabla.Detalle);
			sql.Append ("', ");
			sql.Append (tabla.Identidad);
			sql.Append (")");
			return DB.Instancia.Sql (sql.ToString ());
		}

		public int update (TablaEntity tabla)
		{
			var sql = new StringBuilder ();
			sql.Append (" UPDATE ");
			sql.Append (tabladb);
			sql.Append (" SET");
			sql.Append (" indice = ");
			sql.Append (tabla.Indice);
			sql.Append (", descripcion = '");
			sql.Append (tabla.Descripcion);
			sql.Append ("', contenido = ");
			sql.Append (tabla.Contenido);
			sql.Append (", detalle = '");
			sql.Append (tabla.Detalle);
			sql.Append ("', identidad = ");
			sql.Append (tabla.Identidad);
			sql.Append (" WHERE ");
			sql.Append (" id = ");
			sql.Append (tabla.Id);
			sql.Append (" and nombre = '");
			sql.Append (tabla.Nombre);
			sql.Append ("'");
			return DB.Instancia.Sql (sql.ToString ());
		}

		public int delete (TablaEntity tabla)
		{
			var sql = new StringBuilder ();
			sql.Append ("DELETE");
			sql.Append (" FROM ");
			sql.Append (tabladb);
			sql.Append (" WHERE ");
			sql.Append (" id = ");
			sql.Append (tabla.Id);
			return DB.Instancia.Sql (sql.ToString ());
		}

		#endregion

		#region SELECTs

		/// <summary>
		/// Devuelve una lista de contenido y detalle de Tablas a partir de su nombre/indice. 
		/// </summary>
		public List<TablaEntity> getByContenidoYdetalle (string nombreTabla, int indice)
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT id");
			sql.Append (", nombre");
			sql.Append (", indice");
			sql.Append (", descripcion");
			sql.Append (", contenido");
			sql.Append (", detalle");
			sql.Append (", identidad");
			sql.Append (" FROM ");
			sql.Append (tabladb);
			sql.Append (" WHERE ");
			sql.Append (" nombre = '" + nombreTabla);
			sql.Append ("' and indice = " + indice);
			var tablas = new List<TablaEntity> ();
			using (IDataReader reader = DB.Instancia.SqlToDbDataReader (sql.ToString ())) {
				while (reader.Read ())
					tablas.Add (make (reader));
			}
			return tablas;
		}


		/// <summary>
		/// Devuelve el contenido de una tabla a partir de su nombre. 
		/// </summary>
		public List<TablaEntity> getByNombre (string nombreTabla)
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT id");
			sql.Append (", nombre");
			sql.Append (", indice");
			sql.Append (", descripcion");
			sql.Append (", contenido");
			sql.Append (", detalle");
			sql.Append (", identidad");
			sql.Append (" FROM ");
			sql.Append (tabladb);
			sql.Append (" WHERE ");
			sql.Append (" nombre = '" + nombreTabla);
			sql.Append ("' order by nombre, indice, descripcion, contenido");
			var tablas = new List<TablaEntity> ();
			using (IDataReader reader = DB.Instancia.SqlToDbDataReader (sql.ToString ())) {
				while (reader.Read ())
					tablas.Add (make (reader));
			}
			return tablas;
		}

		/// <summary>
		/// Devuelve el contenido de una tabla a partir de su nombre. 
		/// Se deben especificar campos de ordenacion
		/// </summary>
		public List<TablaEntity> getByNombre (string nombreTabla, string orden)
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT id");
			sql.Append (", nombre");
			sql.Append (", indice");
			sql.Append (", descripcion");
			sql.Append (", contenido");
			sql.Append (", detalle");
			sql.Append (", identidad");
			sql.Append (" FROM ");
			sql.Append (tabladb);
			sql.Append (" WHERE ");
			sql.Append (" nombre = '" + nombreTabla);
			sql.Append ("' order by " + orden);
			var tablas = new List<TablaEntity> ();
			using (IDataReader reader = DB.Instancia.SqlToDbDataReader (sql.ToString ())) {
				while (reader.Read ())
					tablas.Add (make (reader));
			}
			return tablas;
		}

		/// <summary>
		/// Devuelve el contenido de una tabla a partir de su nombre. 
		/// Se aceptan filtros
		/// Se deben especificar campos de ordenacion
		/// </summary>
		public List<TablaEntity> getByNombre (string nombreTabla, string filtro, string orden)
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT id");
			sql.Append (", nombre");
			sql.Append (", indice");
			sql.Append (", descripcion");
			sql.Append (", contenido");
			sql.Append (", detalle");
			sql.Append (", identidad");
			sql.Append (" FROM ");
			sql.Append (tabladb);
			sql.Append (" WHERE ");
			sql.Append (" nombre = '" + nombreTabla + "'");
			if (filtro.Length > 0)
				sql.Append (" and " + filtro);
			sql.Append (" order by " + orden);
			var tablas = new List<TablaEntity> ();
			using (IDataReader reader = DB.Instancia.SqlToDbDataReader (sql.ToString ())) {
				while (reader.Read ())
					tablas.Add (make (reader));
			}
			return tablas;
		}


		public List<TablaEntity> getNombresTablas ()
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT distinct");
			sql.Append (" nombre");
			sql.Append (" FROM ");
			sql.Append (tabladb);
			var tablas = new List<TablaEntity> ();
			using (IDataReader reader = DB.Instancia.SqlToDbDataReader (sql.ToString ())) {
				while (reader.Read ())
					tablas.Add (new TablaEntity (reader ["nombre"].ToString (), 0, 0));
			}
			return tablas;
		}


		/// <summary>
		/// Devuelve una Tabla a partir de su (Id): nombre, indice, contenido. 
		/// </summary>
		public TablaEntity getById (string nombre, int indice, double contenido)
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT id");
			sql.Append (", nombre");
			sql.Append (", indice");
			sql.Append (", descripcion");
			sql.Append (", contenido");
			sql.Append (", detalle");
			sql.Append (", identidad");
			sql.Append (" FROM ");
			sql.Append (tabladb);
			sql.Append (" WHERE ");
			sql.Append (" nombre = '" + nombre);
			sql.Append ("' and indice = " + indice);
			sql.Append (" and contenido = " + contenido);
			using (IDataReader reader = DB.Instancia.SqlToDbDataReader (sql.ToString ())) {
				return (reader.Read ()) ? make (reader) : null;
			}
		}

		/// <summary>
		/// Devuelve el indice Maximo de una tabla de acuerdo a su nombre y descripcion 
		/// </summary>
		public int getMaxIndice (string nombre, string descripcion)
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT max(indice) as indice");
			sql.Append (" FROM ");
			sql.Append (tabladb);
			sql.Append (" WHERE ");
			sql.Append (" nombre = '" + nombre);
			sql.Append ("' and descripcion = '" + descripcion + "'");
			using (IDataReader reader = DB.Instancia.SqlToDbDataReader (sql.ToString ())) {
				return reader.Read () ? Convert.ToInt32 (reader ["indice"]) : 0;
			}
		}

		public TablaEntity getById (int id)
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT id");
			sql.Append (", nombre");
			sql.Append (", indice");
			sql.Append (", descripcion");
			sql.Append (", contenido");
			sql.Append (", detalle");
			sql.Append (", identidad");
			sql.Append (" FROM ");
			sql.Append (tabladb);
			sql.Append (" WHERE ");
			sql.Append (" id = " + id);
			using (IDataReader reader = DB.Instancia.SqlToDbDataReader (sql.ToString ())) {
				return (reader.Read ()) ? make (reader) : null;
			}
		}

		public List<TablaEntity> GetAll ()
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT id");
			sql.Append (", nombre");
			sql.Append (", indice");
			sql.Append (", descripcion");
			sql.Append (", contenido");
			sql.Append (", detalle");
			sql.Append (", identidad");
			sql.Append (" FROM ");
			sql.Append (tabladb);
			return GetLista (sql.ToString ());
		}

		#endregion

		#region PRIVATE

		static TablaEntity make (IDataRecord reader)
		{
			var tablaSueldos = new TablaEntity (reader ["nombre"].ToString (), int.Parse (reader ["indice"].ToString ()), double.Parse (reader ["contenido"].ToString ()));
			tablaSueldos.Id = Convert.ToInt32 (reader ["id"]);
			tablaSueldos.Descripcion = reader ["descripcion"].ToString ();
			tablaSueldos.Detalle = reader ["detalle"].ToString ();
			tablaSueldos.Identidad = Convert.ToInt32 (reader ["identidad"]);
			return tablaSueldos;
		}

		static List<TablaEntity> GetLista (string sql)
		{
			var tablas = new List<TablaEntity> ();
			using (IDataReader reader = DB.Instancia.SqlToDbDataReader (sql)) {
				while (reader.Read ())
					tablas.Add (make (reader));
			}
			return tablas;
		}

		#endregion
	}
}
