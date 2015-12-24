//
//  HuellaData.cs
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
using System.Data;
using System.Text;
using Hamekoz.Data;
using Sueldos.Data;

namespace Reloj.Core
{
	public class HuellaData
	{
		const string tabla = "huellas";

		#region ABM

		public int Insert (Huella huella)
		{
			var sql = new StringBuilder ();
			sql.Append (" INSERT INTO ");
			sql.Append (tabla);
			sql.Append (" VALUES(");
			sql.Append (huella.Legajo);
			sql.Append (", ");
			sql.Append (huella.DedoHuella.Contenido);
			sql.Append (", '");
			sql.Append (huella.Patron);
			sql.Append ("') ");
			return DB.Instancia.Sql (sql.ToString ());
		}

		public int Update (Huella huella)
		{
			var sql = new StringBuilder ();
			sql.Append (" UPDATE ");
			sql.Append (tabla);
			sql.Append (" SET");
			sql.Append (" huella = '");
			sql.Append (huella.Patron);
			sql.Append ("' WHERE ");
			sql.Append (" legajo = ");
			sql.Append (huella.Legajo);
			sql.Append (" and idHuella = ");
			sql.Append (huella.DedoHuella.Contenido);
			return DB.Instancia.Sql (sql.ToString ());
		}

		public int Delete (Huella huella)
		{
			var sql = new StringBuilder ();
			sql.Append ("DELETE");
			sql.Append (" FROM ");
			sql.Append (tabla);
			sql.Append (" WHERE ");
			sql.Append (" legajo = ");
			sql.Append (huella.Legajo);
			sql.Append (" and idHuella = ");
			sql.Append (huella.DedoHuella.Contenido);
			return DB.Instancia.Sql (sql.ToString ());
		}

		/// <summary>
		/// borra todas las huellas de un legajo
		/// </summary>
		/// <param name = "legajo"></param>
		/// <returns></returns>
		public int Delete (int legajo)
		{
			var sql = new StringBuilder ();
			sql.Append ("DELETE");
			sql.Append (" FROM ");
			sql.Append (tabla);
			sql.Append (" WHERE ");
			sql.Append (" legajo = ");
			sql.Append (legajo);
			return DB.Instancia.Sql (sql.ToString ());
		}

		#endregion

		#region SELECTs

		public Huella GetBy (int legajo, int idHuella)
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT legajo");
			sql.Append (", idHuella");
			sql.Append (", huella");
			sql.Append (" FROM ");
			sql.Append (tabla);
			sql.Append (" WHERE ");
			sql.Append (" legajo = ");
			sql.Append (legajo);
			sql.Append (" and idHuella = ");
			sql.Append (idHuella);
			using (IDataReader reader = DB.Instancia.SqlToDbDataReader (sql.ToString ())) {
				return (reader.Read ()) ? make (reader) : null;
			}
		}

		public Huella GetBy (string huella)
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT legajo");
			sql.Append (", idHuella");
			sql.Append (", huella");
			sql.Append (" FROM ");
			sql.Append (tabla);
			sql.Append (" WHERE ");
			sql.Append (" huella = '");
			sql.Append (huella);
			sql.Append ("'");
			using (IDataReader reader = DB.Instancia.SqlToDbDataReader (sql.ToString ())) {
				return (reader.Read ()) ? make (reader) : null;
			}
		}

		public List<Huella> GetBy (int legajo)
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT legajo");
			sql.Append (", idHuella");
			sql.Append (", huella");
			sql.Append (" FROM ");
			sql.Append (tabla);
			sql.Append (" WHERE ");
			sql.Append (" legajo = ");
			sql.Append (legajo);
			return GetLista (sql.ToString ());
		}

		public List<Huella> GetAll ()
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT legajo");
			sql.Append (", idHuella");
			sql.Append (", huella");
			sql.Append (" FROM ");
			sql.Append (tabla);
			sql.Append (" ORDER BY legajo, idHuella ");
			return GetLista (sql.ToString ());
		}

		#endregion

		#region PRIVATE

		static Huella make (IDataRecord reader)
		{
			var huella = new Huella (int.Parse (reader ["legajo"].ToString ()));

			var tablaData = new TablaData ();
			int idHuella = int.Parse (reader ["idHuella"].ToString ());
			huella.DedoHuella = tablaData.getById ("reloj", 4, idHuella);
			huella.Patron = reader ["huella"].ToString ();
			return huella;
		}

		static List<Huella> GetLista (string sql)
		{
			var huellas = new List<Huella> ();
			using (IDataReader reader = DB.Instancia.SqlToDbDataReader (sql)) {
				while (reader.Read ())
					huellas.Add (make (reader));
			}
			return huellas;
		}

		#endregion
	}
}
