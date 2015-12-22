//
//  ConceptoData.cs
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
	public class ConceptoData
	{
		const string tabla = "calculo";

		#region ABM

		public int insert (ConceptoEntity conce)
		{
			var sql = new StringBuilder ();
			sql.Append (" INSERT INTO ");
			sql.Append (tabla);
			sql.Append (" VALUES(");
			sql.Append (conce.IdCalculo);
			sql.Append (", ");
			sql.Append (conce.OrdenDeProceso);
			sql.Append (", ");
			sql.Append (conce.Codigo);
			sql.Append (", '");
			sql.Append (conce.Descripcion);
			sql.Append ("', '");
			sql.Append (conce.Formula);
			sql.Append ("', ");
			sql.Append (conce.Tipo);
			sql.Append (", ");
			sql.Append (conce.Imprime ? 1 : 0);
			sql.Append (", ");
			sql.Append (conce.ImprimeCantidad ? 1 : 0);
			sql.Append (", ");
			sql.Append (conce.ImprimeVU ? 1 : 0);
			sql.Append (", ");
			sql.Append (conce.Desactivado ? 1 : 0);
			sql.Append (", ");
			sql.Append (conce.IdTipoLiquidacion);
			sql.Append (", ");
			sql.Append (conce.idAplicacion);
			sql.Append (", ");
			sql.Append (conce.IdCuentaContable);
			sql.Append (")");
			return DB.Instancia.Sql (sql.ToString ());
		}

		public int update (ConceptoEntity conce)
		{
			var sql = new StringBuilder ();
			sql.Append (" UPDATE ");
			sql.Append (tabla);
			sql.Append (" SET");
			sql.Append (" descripcion = '");
			sql.Append (conce.Descripcion);
			sql.Append ("', formula = '");
			sql.Append (conce.Formula);
			sql.Append ("', tipo = ");
			sql.Append (conce.Tipo);
			sql.Append (", imprime = ");
			sql.Append (conce.Imprime ? 1 : 0);
			sql.Append (", imprimeCantidad = ");
			sql.Append (conce.ImprimeCantidad ? 1 : 0);
			sql.Append (", imprimeVU = ");
			sql.Append (conce.ImprimeVU ? 1 : 0);
			sql.Append (", desactivado = ");
			sql.Append (conce.Desactivado ? 1 : 0);
			sql.Append (", idTipoLiquidacion = ");
			sql.Append (conce.IdTipoLiquidacion);
			sql.Append (", idAplicacion = ");
			sql.Append (conce.idAplicacion);
			sql.Append (", idCuentaContable = ");
			sql.Append (conce.IdCuentaContable);
			sql.Append (" WHERE ");
			sql.Append (" idCalculo = ");
			sql.Append (conce.IdCalculo);
			sql.Append (" and OrdenProceso = ");
			sql.Append (conce.OrdenDeProceso);
			sql.Append (" and Codigo = ");
			sql.Append (conce.Codigo);
			return DB.Instancia.Sql (sql.ToString ());
		}

		public int delete (ConceptoEntity conce)
		{
			var sql = new StringBuilder ();
			sql.Append ("DELETE");
			sql.Append (" FROM ");
			sql.Append (tabla);
			sql.Append (" WHERE ");
			sql.Append (" idCalculo = ");
			sql.Append (conce.IdCalculo);
			sql.Append (" and OrdenProceso = ");
			sql.Append (conce.OrdenDeProceso);
			sql.Append (" and Codigo = ");
			sql.Append (conce.Codigo);
			return DB.Instancia.Sql (sql.ToString ());
		}

		#endregion

		#region SELECTs

		[Obsolete ("Usar GetById")]
		public ConceptoEntity getById (int idCalculo, int ordenProceso, double codigo)
		{
			return GetById (idCalculo, ordenProceso, codigo);
		}

		public ConceptoEntity GetById (int idCalculo, int ordenProceso, double codigo)
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT idCalculo");
			sql.Append (", ordeProceso");
			sql.Append (", codigo");
			sql.Append (", descripcion");
			sql.Append (", formula");
			sql.Append (", tipo");
			sql.Append (", imprime");
			sql.Append (", imprimeCantidad");
			sql.Append (", imprimeVU");
			sql.Append (", desactivado");
			sql.Append (", idTipoLiquidacion");
			sql.Append (", idAplicacion");
			sql.Append (", idCuentaContable");
			sql.Append (" FROM ");
			sql.Append (tabla);
			sql.Append (" WHERE ");
			sql.Append (" idCalculo = ");
			sql.Append (idCalculo);
			sql.Append (" and ordeProceso = ");
			sql.Append (ordenProceso);
			sql.Append (" and codigo = ");
			sql.Append (codigo);
			using (IDataReader reader = DB.Instancia.SqlToDbDataReader (sql.ToString ())) {
				return (reader.Read ()) ? Make (reader) : null;
			}
		}

		public List<ConceptoEntity> GetAll ()
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT idCalculo");
			sql.Append (", OrdeProceso");
			sql.Append (", Codigo");
			sql.Append (", Descripcion");
			sql.Append (", Formula");
			sql.Append (", Tipo");
			sql.Append (", Imprime");
			sql.Append (", ImprimeCantidad");
			sql.Append (", ImprimeVU");
			sql.Append (", desactivado");
			sql.Append (", idTipoLiquidacion");
			sql.Append (", idAplicacion");
			sql.Append (", idCuentaContable");
			sql.Append (" FROM ");
			sql.Append (tabla);
			return GetLista (sql.ToString ());
		}

		/// <summary>
		/// Devuelve dataset de conceptos filtrado por idCalculo
		/// </summary>
		public DataSet GetAll (int idCalculo)
		{
			DataSet ds;
			var sql = new StringBuilder ();
			sql.Append ("SELECT idCalculo");
			sql.Append (", OrdeProceso");
			sql.Append (", Codigo");
			sql.Append (", Descripcion");
			sql.Append (", Formula");
			sql.Append (", Tipo");
			sql.Append (", Imprime");
			sql.Append (", ImprimeCantidad");
			sql.Append (", ImprimeVU");
			sql.Append (", desactivado");
			sql.Append (", idTipoLiquidacion");
			sql.Append (", idAplicacion");
			sql.Append (", idCuentaContable");
			sql.Append (" FROM ");
			sql.Append (tabla);
			sql.Append (" where idCalculo = " + idCalculo);
			ds = DB.Instancia.SPToDataSet (sql.ToString ());
			return ds;
		}

		#endregion

		#region PRIVATE

		static ConceptoEntity Make (IDataRecord reader)
		{
			var conce = new ConceptoEntity (int.Parse (reader ["idCalculo"].ToString ()), int.Parse (reader ["ordenProceso"].ToString ()), int.Parse (reader ["codigo"].ToString ()));
			conce.Descripcion = reader ["descripcion"].ToString ();
			conce.Formula = reader ["formula"].ToString ();
			conce.Tipo = int.Parse (reader ["tipo"].ToString ());
			conce.Imprime = Boolean.Parse (reader ["imprime"].ToString ());
			conce.ImprimeCantidad = Boolean.Parse (reader ["imprimeCantidad"].ToString ());
			conce.ImprimeVU = Boolean.Parse (reader ["imprimeVU"].ToString ());
			conce.Desactivado = Boolean.Parse (reader ["desactivado"].ToString ());
			conce.IdTipoLiquidacion = int.Parse (reader ["idTipoLiquidacion"].ToString ());
			conce.idAplicacion = int.Parse (reader ["idAplicacion"].ToString ());
			conce.IdCuentaContable = int.Parse (reader ["idCuentaContable"].ToString ());
			return conce;
		}

		static List<ConceptoEntity> GetLista (string sql)
		{
			var conceptos = new List<ConceptoEntity> ();
			using (IDataReader reader = DB.Instancia.SqlToDbDataReader (sql)) {
				while (reader.Read ())
					conceptos.Add (Make (reader));
			}
			return conceptos;
		}

		#endregion
	}
}
