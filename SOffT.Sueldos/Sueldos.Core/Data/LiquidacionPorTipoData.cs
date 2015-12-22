//
//  LiquidacionPorTipoData.cs
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
	public class LiquidacionPorTipoData
	{
		const string tabla = "liquidacionesPorTipo";

		#region ABM

		public int insert (LiquidacionPorTipoEntity liqui)
		{
			var sql = new StringBuilder ();
			sql.Append (" INSERT INTO ");
			sql.Append (tabla);
			sql.Append (" VALUES(");
			sql.Append (liqui.IdLiquidacion);
			sql.Append (", ");
			sql.Append (liqui.IdTipoLiquidacion);
			sql.Append (", ");
			sql.Append (liqui.Seleccionado ? 1 : 0);
			sql.Append (") ");

			return DB.Instancia.Sql (sql.ToString ());
		}

		public int update (LiquidacionPorTipoEntity liqui)
		{
			var sql = new StringBuilder ();
			sql.Append (" UPDATE ");
			sql.Append (tabla);
			sql.Append (" SET");
			sql.Append (" seleccionado = ");
			sql.Append (liqui.Seleccionado ? 1 : 0);
			sql.Append (" WHERE ");
			sql.Append (" idLiquidacion = ");
			sql.Append (liqui.IdLiquidacion);
			sql.Append (" and idTipoLiquidacion = ");
			sql.Append (liqui.IdTipoLiquidacion);
			return DB.Instancia.Sql (sql.ToString ());
		}

		public int delete (LiquidacionPorTipoEntity liqui)
		{
			var sql = new StringBuilder ();
			sql.Append ("DELETE");
			sql.Append (" FROM ");
			sql.Append (tabla);
			sql.Append (" WHERE ");
			sql.Append (" idLiquidacion = ");
			sql.Append (liqui.IdLiquidacion);
			sql.Append (" and idTipoLiquidacion = ");
			sql.Append (liqui.IdTipoLiquidacion);
			return DB.Instancia.Sql (sql.ToString ());
		}

		#endregion

		#region SELECTS

		public LiquidacionPorTipoEntity getById (int idLiquidacion, int idTipoLiquidacion)
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT idLiquidacion");
			sql.Append (", idTipoLiquidacion");
			sql.Append (", seleccionado");
			sql.Append (" FROM ");
			sql.Append (tabla);
			sql.Append (" WHERE ");
			sql.Append (" idLiquidacion = " + idLiquidacion);
			sql.Append (" and idTipoLiquidacion = " + idTipoLiquidacion);
			using (IDataReader reader = DB.Instancia.SqlToDbDataReader (sql.ToString ())) {
				return (reader.Read ()) ? make (reader) : null;
			}
		}

		public List<LiquidacionPorTipoEntity> GetAll ()
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT idLiquidacion");
			sql.Append (", idTipoLiquidacion");
			sql.Append (", seleccionado");
			sql.Append (" FROM ");
			sql.Append (tabla);
			return GetLista (sql.ToString ());
		}

		public List<LiquidacionPorTipoEntity> GetAll (int idLiquidacion)
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT idLiquidacion");
			sql.Append (", idTipoLiquidacion");
			sql.Append (", seleccionado");
			sql.Append (" FROM ");
			sql.Append (tabla);
			sql.Append (" WHERE ");
			sql.Append (" idLiquidacion = " + idLiquidacion);
			return GetLista (sql.ToString ());
		}

		#endregion

		#region PRIVATE

		static LiquidacionPorTipoEntity make (IDataRecord reader)
		{
			var liqui = new  LiquidacionPorTipoEntity (int.Parse (reader ["idLiquidacion"].ToString ()), int.Parse (reader ["idTipoLiquidacion"].ToString ()));
			liqui.Seleccionado = Boolean.Parse (reader ["seleccionado"].ToString ());
			return liqui;
		}

		static List<LiquidacionPorTipoEntity> GetLista (string sql)
		{
			var liquis = new List<LiquidacionPorTipoEntity> ();
			using (IDataReader reader = DB.Instancia.SqlToDbDataReader (sql)) {
				while (reader.Read ())
					liquis.Add (make (reader));
			}
			return liquis;
		}

		#endregion
	}
}
