//
//  LiquidacionDetalleData.cs
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
	public class LiquidacionDetalleData
	{
		const string tabla = "LiquidacionesDetalle";

		#region ABM

		public int insert (LiquidacionDetalleEntity liqdet)
		{
			int salida;
			var sql = new StringBuilder ();
			sql.Append (" INSERT INTO ");
			sql.Append (tabla);
			sql.Append (" VALUES(");
			// sql.Append(liqdet.Id);
			// sql.Append(", ");
			sql.Append (liqdet.AnioMes);
			sql.Append (", ");
			sql.Append (liqdet.IdAplicacion);
			sql.Append (", ");
			sql.Append (liqdet.IdTipoSalario);
			sql.Append (", '");
			sql.Append (liqdet.Descripcion);
			sql.Append ("', '");
			sql.Append (liqdet.FechaLiquidacion);
			sql.Append ("', '");
			sql.Append (liqdet.PeriodoLiquidado);
			sql.Append ("', '");
			sql.Append (liqdet.LugarDePago);
			sql.Append ("', '");
			sql.Append (string.Empty);
			sql.Append ("', '");
			sql.Append (liqdet.PeriodoDepositado);
			sql.Append ("', '");
			sql.Append (liqdet.BancoDepositado);
			sql.Append ("', '");
			sql.Append (liqdet.FechaDepositado);
			sql.Append ("', ");
			sql.Append (liqdet.Estado ? 1 : 0);
			sql.Append (", ");
			sql.Append (liqdet.RecibosSeparados ? 1 : 0);
			sql.Append (", ");
			sql.Append (liqdet.Eliminado ? 1 : 0);
			sql.Append (")");

			salida = DB.Instancia.Sql (sql.ToString ());

			return salida;
		}

		public int insertFechasDePago (List<FechaDePagoEntity> fechas)
		{
			var fechadepagoData = new FechaDePagoData ();
			foreach (FechaDePagoEntity f in fechas)
				fechadepagoData.insert (f);
			return 0;
		}

		public int update (LiquidacionDetalleEntity liqdet)
		{
			int salida;
			int salidaFechas;
			var sql = new StringBuilder ();
			sql.Append (" UPDATE ");
			sql.Append (tabla);
			sql.Append (" SET");
			sql.Append (" aniomes = ");
			sql.Append (liqdet.AnioMes);
			sql.Append (", idAplicacion = ");
			sql.Append (liqdet.IdAplicacion);
			sql.Append (", idTipoSalario = ");
			sql.Append (liqdet.IdTipoSalario);
			sql.Append (", descripcion = '");
			sql.Append (liqdet.Descripcion);
			sql.Append ("', fechaLiquidacion = '");
			sql.Append (liqdet.FechaLiquidacion);
			sql.Append ("', periodoLiquidado = '");
			sql.Append (liqdet.PeriodoLiquidado);
			sql.Append ("', lugarDePago = '");
			sql.Append (liqdet.LugarDePago);
			//sql.Append("', fechaDePago = '");
			//sql.Append(liqdet.FechaDePago);
			sql.Append ("', periodoDepositado = '");
			sql.Append (liqdet.PeriodoDepositado);
			sql.Append ("', bancoDepositado = '");
			sql.Append (liqdet.BancoDepositado);
			sql.Append ("', fechaDepositado = '");
			sql.Append (liqdet.FechaDepositado);
			sql.Append ("', estado = ");
			sql.Append (liqdet.Estado ? 1 : 0);
			sql.Append (", recibosSeparados = ");
			sql.Append (liqdet.RecibosSeparados ? 1 : 0);
			sql.Append (", eliminado = ");
			sql.Append (liqdet.Eliminado ? 1 : 0);
			sql.Append (" WHERE ");
			sql.Append (" id = ");
			sql.Append (liqdet.Id);
			salida = DB.Instancia.Sql (sql.ToString ());
			deleteFechasDePago (liqdet.Id);//primero elimino las existentes
			salidaFechas = insertFechasDePago (liqdet.FechasDePago);//luego inserto
			return salida + salidaFechas;
		}

		public int updateFechasDePago (List<FechaDePagoEntity> fechas)
		{
			var fechadepagoData = new FechaDePagoData ();
			foreach (FechaDePagoEntity f in fechas)
				fechadepagoData.update (f);
			return 0;
		}

		public int delete (LiquidacionDetalleEntity liqdet)
		{
			int salida;
			int salidaFechas;
			var sql = new StringBuilder ();
			sql.Append ("update ");
			sql.Append (tabla);
			sql.Append (" set eliminado = 1 where");
			sql.Append (" id = ");
			sql.Append (liqdet.Id);

			salida = DB.Instancia.Sql (sql.ToString ());
			salidaFechas = deleteFechasDePago (liqdet.Id);
			return salida + salidaFechas;
		}

		public int deleteFechasDePago (List<FechaDePagoEntity> fechas)
		{
			var fechadepagoData = new FechaDePagoData ();
			foreach (FechaDePagoEntity f in fechas)
				fechadepagoData.delete (f);
			return 0;
		}

		public int deleteFechasDePago (int idLiquidacion)
		{
			var fechadepagoData = new FechaDePagoData ();
			fechadepagoData.delete (idLiquidacion);
			return 0;
		}

		#endregion

		#region SELECTs

		public LiquidacionDetalleEntity getById (int id)
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT id");
			sql.Append (", aniomes");
			sql.Append (", idAplicacion");
			sql.Append (", idTipoSalario");
			sql.Append (", descripcion");
			sql.Append (", fechaLiquidacion");
			sql.Append (", periodoLiquidado");
			sql.Append (", lugarDePago");
			// sql.Append(", fechaDePago");
			sql.Append (", periodoDepositado");
			sql.Append (", bancoDepositado");
			sql.Append (", fechaDepositado");
			sql.Append (", estado");
			sql.Append (", recibosSeparados");
			sql.Append (", eliminado");
			sql.Append (" FROM ");
			sql.Append (tabla);
			sql.Append (" WHERE ");
			sql.Append (" id = " + id);
			using (IDataReader reader = DB.Instancia.SqlToDbDataReader (sql.ToString ())) {
				return (reader.Read ()) ? make (reader) : null;
			}
		}

		public LiquidacionDetalleEntity getById (int aniomes, int idAplicacion, DateTime fechaLiquidacion)
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT id");
			sql.Append (", aniomes");
			sql.Append (", idAplicacion");
			sql.Append (", idTipoSalario");
			sql.Append (", descripcion");
			sql.Append (", fechaLiquidacion");
			sql.Append (", periodoLiquidado");
			sql.Append (", lugarDePago");
			//  sql.Append(", fechaDePago");
			sql.Append (", periodoDepositado");
			sql.Append (", bancoDepositado");
			sql.Append (", fechaDepositado");
			sql.Append (", estado");
			sql.Append (", recibosSeparados");
			sql.Append (", eliminado");
			sql.Append (" FROM ");
			sql.Append (tabla);
			sql.Append (" WHERE ");
			sql.Append (" aniomes = " + aniomes);
			sql.Append (" and idAplicacion = " + idAplicacion);
			sql.Append (" and fechaLiquidacion = '" + fechaLiquidacion + "'");
			using (IDataReader reader = DB.Instancia.SqlToDbDataReader (sql.ToString ())) {
				return (reader.Read ()) ? make (reader) : null;
			}
		}

		/// <summary>
		/// Consulta si una liquidación por estado y aniomes. Devuelve el id o cero si no la encuentra
		/// </summary>
		/// <param name="aniomes"></param>
		/// <returns></returns>
		/// <param name = "estado"></param>
		public int getIdByEstado (int aniomes, int estado)
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT isnull(id,0) as id");
			sql.Append (" FROM ");
			sql.Append (tabla);
			sql.Append (" WHERE ");
			sql.Append (" aniomes = " + aniomes);
			sql.Append (" and estado = " + estado);
			return int.Parse (DB.Instancia.SqlToScalar (sql.ToString ()).ToString ());
		}

		public List<LiquidacionDetalleEntity> getByEstado (int estado)
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT id");
			sql.Append (", aniomes");
			sql.Append (", idAplicacion");
			sql.Append (", idTipoSalario");
			sql.Append (", descripcion");
			sql.Append (", fechaLiquidacion");
			sql.Append (", periodoLiquidado");
			sql.Append (", lugarDePago");
			//   sql.Append(", fechaDePago");
			sql.Append (", periodoDepositado");
			sql.Append (", bancoDepositado");
			sql.Append (", fechaDepositado");
			sql.Append (", estado");
			sql.Append (", recibosSeparados");
			sql.Append (", eliminado");
			sql.Append (" FROM ");
			sql.Append (tabla);
			sql.Append (" WHERE ");
			sql.Append (" eliminado = 0 and estado=" + estado);
			return getLista (sql.ToString ());
		}

		public List<LiquidacionDetalleEntity> GetAll ()
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT id");
			sql.Append (", aniomes");
			sql.Append (", idAplicacion");
			sql.Append (", idTipoSalario");
			sql.Append (", descripcion");
			sql.Append (", fechaLiquidacion");
			sql.Append (", periodoLiquidado");
			sql.Append (", lugarDePago");
			//   sql.Append(", fechaDePago");
			sql.Append (", periodoDepositado");
			sql.Append (", bancoDepositado");
			sql.Append (", fechaDepositado");
			sql.Append (", estado");
			sql.Append (", recibosSeparados");
			sql.Append (", eliminado");
			sql.Append (" FROM ");
			sql.Append (tabla);
			return getLista (sql.ToString ());
		}

		public DataSet GetAllGrilla ()
		{
			DataSet ds;
			ds = DB.Instancia.SPToDataSet ("liquidacionesDetalleConsultarParaGrilla");
			return ds;
		}

		#endregion

		#region PRIVATE

		static LiquidacionDetalleEntity make (IDataRecord reader)
		{
			var liqdet = new LiquidacionDetalleEntity (int.Parse (reader ["aniomes"].ToString ()), int.Parse (reader ["idAplicacion"].ToString ()), DateTime.Parse (reader ["fechaLiquidacion"].ToString ()));
			liqdet.Id = int.Parse (reader ["id"].ToString ());
			liqdet.IdTipoSalario = int.Parse (reader ["idTipoSalario"].ToString ());
			liqdet.Descripcion = reader ["descripcion"].ToString ();
			liqdet.PeriodoLiquidado = reader ["periodoLiquidado"].ToString ();
			liqdet.LugarDePago = reader ["lugarDePago"].ToString ();
			//liqdet.FechaDePago = reader["fechaDePago"] == DBNull.Value ? new DateTime(0) : DateTime.Parse(reader["fechaDePago"].ToString());
			liqdet.PeriodoDepositado = reader ["periodoDepositado"].ToString ();
			liqdet.BancoDepositado = reader ["bancoDepositado"].ToString ();
			liqdet.FechaDepositado = reader ["fechaDepositado"] == DBNull.Value ? new DateTime (0) : DateTime.Parse (reader ["fechaDepositado"].ToString ());
			liqdet.Estado = Boolean.Parse (reader ["Estado"].ToString ());
			liqdet.RecibosSeparados = Boolean.Parse (reader ["recibosSeparados"].ToString ());
			liqdet.Eliminado = Boolean.Parse (reader ["eliminado"].ToString ());

			//cargo fechas de pago
			var fechadepagoData = new FechaDePagoData ();
			liqdet.FechasDePago = fechadepagoData.GetAll (liqdet.Id);
			return liqdet;
		}

		List<LiquidacionDetalleEntity> getLista (string sql)
		{
			var liquidaciones = new List<LiquidacionDetalleEntity> ();
			using (IDataReader reader = DB.Instancia.SqlToDbDataReader (sql)) {
				while (reader.Read ())
					liquidaciones.Add (make (reader));
			}
			return liquidaciones;
		}

		#endregion
	}
}
