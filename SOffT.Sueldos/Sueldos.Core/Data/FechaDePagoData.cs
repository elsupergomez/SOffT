//
//  FechaDePagoData.cs
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
	public class FechaDePagoData
	{
		const string tabla = "fechasDePago";

		#region ABM

		public int insert (FechaDePagoEntity fecha)
		{
			var sql = new StringBuilder ();
			sql.Append (" INSERT INTO ");
			sql.Append (tabla);
			sql.Append (" VALUES(");
			sql.Append (fecha.IdLiquidacion);
			sql.Append (", '");
			sql.Append (fecha.FechaDePago);
			sql.Append ("')");
			return DB.Instancia.Sql (sql.ToString ());
		}

		public int update (FechaDePagoEntity fecha)
		{
			var sql = new StringBuilder ();
			sql.Append (" UPDATE ");
			sql.Append (tabla);
			sql.Append (" SET");
			sql.Append (" fechaDePago = '");
			sql.Append (fecha.FechaDePago);
			sql.Append ("'");
			sql.Append (" WHERE ");
			sql.Append (" idLiquidacion = ");
			sql.Append (fecha.IdLiquidacion);
			return DB.Instancia.Sql (sql.ToString ());
		}

		public int delete (FechaDePagoEntity fecha)
		{
			var sql = new StringBuilder ();
			sql.Append ("DELETE");
			sql.Append (" FROM ");
			sql.Append (tabla);
			sql.Append (" WHERE ");
			sql.Append (" idLiquidacion = ");
			sql.Append (fecha.IdLiquidacion);
			sql.Append (" and fechaDePago = '");
			sql.Append (fecha.FechaDePago + "'");
			return DB.Instancia.Sql (sql.ToString ());
		}

		public int delete (int idLiquidacion)
		{
			var sql = new StringBuilder ();
			sql.Append ("DELETE");
			sql.Append (" FROM ");
			sql.Append (tabla);
			sql.Append (" WHERE ");
			sql.Append (" idLiquidacion = ");
			sql.Append (idLiquidacion);
			return DB.Instancia.Sql (sql.ToString ());
		}

		#endregion

		#region SELECTs

		public FechaDePagoEntity getById (int id, DateTime fechadepago)
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT idLiquidacion");
			sql.Append (", fechaDePago");
			sql.Append (" FROM ");
			sql.Append (tabla);
			sql.Append (" WHERE ");
			sql.Append (" idLiquidacion = " + id);
			sql.Append (" and fechaDePago = '" + fechadepago + "'");
			using (IDataReader reader = DB.Instancia.SqlToDbDataReader (sql.ToString ())) {
				return (reader.Read ()) ? make (reader) : null;
			}
		}

		public List<FechaDePagoEntity> GetAll ()
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT idLiquidacion");
			sql.Append (", fechaDePago");
			sql.Append (" FROM ");
			sql.Append (tabla);
			return getLista (sql.ToString ());
		}

		public List<FechaDePagoEntity> GetAll (int idLiquidacion)
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT idLiquidacion");
			sql.Append (", fechaDePago");
			sql.Append (" FROM ");
			sql.Append (tabla);
			sql.Append (" WHERE ");
			sql.Append (" idLiquidacion = " + idLiquidacion);
			return getLista (sql.ToString ());
		}

		#endregion

		#region PRIVATE

		static FechaDePagoEntity make (IDataRecord reader)
		{
			var fecha = new FechaDePagoEntity (int.Parse (reader ["idLiquidacion"].ToString ()), DateTime.Parse (reader ["fechaDePago"].ToString ()));
			return fecha;
		}

		List<FechaDePagoEntity> getLista (string sql)
		{
			var fechas = new List<FechaDePagoEntity> ();
			using (IDataReader reader = DB.Instancia.SqlToDbDataReader (sql)) {
				while (reader.Read ())
					fechas.Add (make (reader));
			}
			return fechas;
		}

		#endregion
	}
}
