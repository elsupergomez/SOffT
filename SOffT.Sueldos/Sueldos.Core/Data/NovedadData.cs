//
//  NovedadData.cs
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


﻿using System.Collections.Generic;
using System.Data;
using System.Text;
using Hamekoz.Data;
using Sueldos.Entidades;

namespace Sueldos.Data
{
	public class NovedadData
	{
		const string tabla = "novedades";

		#region ABM

		public int insert (NovedadEntity nove)
		{
			var sql = new StringBuilder ();
			sql.Append (" INSERT INTO ");
			sql.Append (tabla);
			sql.Append (" VALUES(");
			sql.Append (nove.IdLiquidacion);
			sql.Append (", ");
			sql.Append (nove.Legajo);
			sql.Append (", ");
			sql.Append (nove.Codigo);
			sql.Append (", '");
			sql.Append (nove.Valor);
			sql.Append ("')");
			return DB.Instancia.Sql (sql.ToString ());
		}

		public int update (NovedadEntity nove)
		{
			var sql = new StringBuilder ();
			sql.Append (" UPDATE ");
			sql.Append (tabla);
			sql.Append (" SET");
			sql.Append (" valor = '");
			sql.Append (nove.Valor);
			sql.Append ("' WHERE ");
			sql.Append (" idLiquidacion = ");
			sql.Append (nove.IdLiquidacion);
			sql.Append (" and legajo = ");
			sql.Append (nove.Legajo);
			sql.Append (" and codigo = ");
			sql.Append (nove.Codigo);
			return DB.Instancia.Sql (sql.ToString ());
		}

		public int delete (NovedadEntity nove)
		{
			var sql = new StringBuilder ();
			sql.Append ("DELETE");
			sql.Append (" FROM ");
			sql.Append (tabla);
			sql.Append (" WHERE ");
			sql.Append (" idLiquidacion = ");
			sql.Append (nove.IdLiquidacion);
			sql.Append (" and legajo = ");
			sql.Append (nove.Legajo);
			sql.Append (" and codigo = ");
			sql.Append (nove.Codigo);
			return DB.Instancia.Sql (sql.ToString ());
		}

		#endregion

		#region SELECTs

		/// <summary>
		/// Devuelve una Novedad a partir de su (Id): idLiquidacion, legajo, codigo. 
		/// </summary>
		public NovedadEntity getById (int IdLiquidacion, int legajo, double codigo)
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT idLiquidacion");
			sql.Append (", legajo");
			sql.Append (", codigo");
			sql.Append (", valor");
			sql.Append (" FROM ");
			sql.Append (tabla);
			sql.Append (" WHERE ");
			sql.Append (" idLiquidacion = ");
			sql.Append (IdLiquidacion);
			sql.Append (" and legajo = ");
			sql.Append (legajo);
			sql.Append (" and codigo = ");
			sql.Append (codigo);
			using (IDataReader reader = DB.Instancia.SqlToDbDataReader (sql.ToString ())) {
				return (reader.Read ()) ? make (reader) : null;
			}
		}

		public List<NovedadEntity> GetAll ()
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT idLiquidacion");
			sql.Append (", legajo");
			sql.Append (", codigo");
			sql.Append (", valor");
			sql.Append (" FROM ");
			sql.Append (tabla);
			return GetLista (sql.ToString ());
		}

		/// <summary>
		/// Devuelve dataset de novedades filtrada por idLiquidacion
		/// y otros criterios
		/// </summary>
		/// <param name="idLiquidacion"></param>
		/// <param name="filtro"></param>
		/// <returns></returns>
		public DataSet GetAll (int idLiquidacion, string filtro)
		{
			DataSet ds;
			var sql = new StringBuilder ();
			sql.Append ("SELECT novedades.idLiquidacion ");
			sql.Append (", empleados.legajo as Legajo");
			sql.Append (", empleados.nyap as Nombres");
			sql.Append (", novedades.codigo as Codigo");
			sql.Append (", tablas.descripcion as Descripcion");
			sql.Append (", novedades.valor as Valor");
			sql.Append (" FROM ");
			sql.Append (tabla);
			sql.Append (" INNER JOIN empleados ON empleados.legajo = novedades.legajo ");
			sql.Append (" INNER JOIN tablas ON tablas.nombre = 'Novedades' AND tablas.indice = novedades.Codigo ");
			sql.Append (" where novedades.idLiquidacion = " + idLiquidacion);
			if (filtro.Length > 0)
				sql.Append (" and " + filtro);
			ds = DB.Instancia.SqlToDataSet (sql.ToString ());
			return ds;
		}

		#endregion

		#region PRIVATE

		static NovedadEntity make (IDataRecord reader)
		{
			var nove = new NovedadEntity (int.Parse (reader ["idLiquidacion"].ToString ()), int.Parse (reader ["legajo"].ToString ()), int.Parse (reader ["codigo"].ToString ()));
			nove.Valor = reader ["valor"].ToString ();
			return nove;
		}

		static List<NovedadEntity> GetLista (string sql)
		{
			var novedades = new List<NovedadEntity> ();
			using (IDataReader reader = DB.Instancia.SqlToDbDataReader (sql)) {
				while (reader.Read ())
					novedades.Add (make (reader));
			}
			return novedades;
		}

		#endregion
	}
}
