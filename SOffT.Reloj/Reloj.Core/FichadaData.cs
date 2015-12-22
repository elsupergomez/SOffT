//
//  FichadaData.cs
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


using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Hamekoz.Data;
using Sueldos.Data;

namespace Reloj.Core
{
	public class FichadaData
	{
		const string tabla = "reloj";

		#region ABM

		public int Insert (Fichada fichada)
		{
			return DB.Instancia.SP ("relojInsertarCaptura",
				"legajo", fichada.Legajo,
				"fecha", fichada.Fecha,
				"hora", fichada.Hora,
				"idReloj", fichada.Reloj == null ? 0 : fichada.Reloj.Contenido
			);
		}

		//TODO DESARROLLAR PARA USAR EN EL ABM DE FICHADAS
		/*    public int Update(FichadaEntity fichada)
        {
            MyLog4Net.Instance.getCustomLog(this.GetType()).Info("Actualizando: " + fichada.ToString());
            try
            {
                return Model.DB.ejecutarProceso(Model.TipoComando.SP, "relojActualizar", "@legajo", fichada.Legajo, "@fecha", fichada.Fecha, "@hora", fichada.Hora , "@idTipoMovimiento", Convert.ToInt32(this.cmbMovimientos.SelectedValue), "@idEstadoFichada", 3);
            }
            catch (Exception ex)
            {
                MyLog4Net.Instance.getCustomLog(this.GetType()).Error("update(). " + ex.Message, ex);
                throw;
            }
        }*/

		//TODO DESARROLLAR PARA USAR EN EL ABM DE FICHADAS
		/*public int Delete(FichadaEntity fichada)
        {
            MyLog4Net.Instance.getCustomLog(this.GetType()).Info("Borrando idFichada: " + fichada.GetId());
            StringBuilder sql = new StringBuilder();
            try
            {
                return Model.DB.ejecutarProceso(Model.TipoComando.SP, "relojEliminar", "@id", Controles.consultaCampoRenglon(this.dgvFichadas, 0));
            }
            catch (Exception ex)
            {
                MyLog4Net.Instance.getCustomLog(this.GetType()).Error("delete(). " + ex.Message, ex);
                throw;
            }
        }*/

		#endregion

		#region SELECTs

		public Fichada GetBy (int id)
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT id");
			sql.Append (", legajo");
			sql.Append (", fecha");
			sql.Append (", hora");
			sql.Append (", idReloj");
			sql.Append (", idTipoMovimiento");
			sql.Append (", idEstadoFichada");
			sql.Append (", horaServidor");
			sql.Append (" FROM ");
			sql.Append (tabla);
			sql.Append (" WHERE ");
			sql.Append (" id = " + id);
			using (IDataReader reader = DB.Instancia.SqlToDbDataReader (sql.ToString ())) {
				return (reader.Read ()) ? make (reader) : null;
			}
		}

		public List<Fichada> GetAll ()
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT id");
			sql.Append (", legajo");
			sql.Append (", fecha");
			sql.Append (", hora");
			sql.Append (", idReloj");
			sql.Append (", idTipoMovimiento");
			sql.Append (", idEstadoFichada");
			sql.Append (", horaServidor");
			sql.Append (" FROM ");
			sql.Append (tabla);
			sql.Append (" ORDER BY fecha, hora ");
			return GetLista (sql.ToString ());
		}

		public List<Fichada> GetEntreFechas (DateTime desde, DateTime hasta)
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT id");
			sql.Append (", reloj.fecha");
			sql.Append (", Convert(varchar(50),reloj.hora,108)  as hora");
			sql.Append (", reloj.legajo");
			sql.Append (", empleados.nyap");
			sql.Append (@", REPLACE(foto, '\\curie\sistemas\personal\Fotos\', './fotos/') AS foto");
			sql.Append (", reloj.idReloj");
			sql.Append (" FROM ");
			sql.Append (tabla);
			sql.Append (" INNER JOIN empleados ");
			sql.Append (" ON empleados.legajo = reloj.legajo");
			sql.Append (" WHERE reloj.fecha >= '" + desde.ToShortDateString () + "'");
			sql.Append (" AND reloj.fecha <= '" + hasta.ToShortDateString () + "'");
			sql.Append (" AND (empleados.eliminado = 0) ");
			sql.Append (" ORDER BY fecha, hora desc");
			return GetLista (sql.ToString ());
		}

		public DataSet GetAll (DateTime desde, DateTime hasta)
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT id");
			sql.Append (", reloj.fecha");
			sql.Append (", Convert(varchar(50),reloj.hora,108)  as hora");
			sql.Append (", reloj.legajo");
			sql.Append (", empleados.nyap");
			sql.Append (@", REPLACE(foto, '\\curie\sistemas\personal\Fotos\', './fotos/') AS foto");
			sql.Append (", reloj.idReloj");
			sql.Append (" FROM ");
			sql.Append (tabla);
			sql.Append (" INNER JOIN empleados ");
			sql.Append (" ON empleados.legajo = reloj.legajo");
			sql.Append (" WHERE reloj.fecha >= '" + desde + "'");
			sql.Append (" AND reloj.fecha <= '" + hasta + "'");
			sql.Append (" AND (empleados.eliminado = 0) ");
			sql.Append (" ORDER BY fecha, hora desc");
			return DB.Instancia.SPToDataSet (sql.ToString ());
		}

		#endregion

		#region PRIVATE

		static Fichada make (IDataRecord reader)
		{
			var fichada = new Fichada (int.Parse (reader ["id"].ToString ()));
			fichada.Legajo = int.Parse (reader ["legajo"].ToString ());
			var fecha = DateTime.Parse (reader ["fecha"].ToString ());
			var hora = DateTime.Parse (reader ["hora"].ToString ());
			fichada.FechaHora = new DateTime (fecha.Year, fecha.Month, fecha.Day, hora.Hour, hora.Minute, hora.Second);
			fichada.Foto = reader ["foto"].ToString ().Replace ("\\\\curie\\Sistemas\\Personal\\Fotos\\", "./fotos/");
			var empleadoData = new EmpleadoData ();
			fichada.ApellidoYnombres = empleadoData.getNombresByLegajo (fichada.Legajo);
			var tablaData = new TablaData ();
			int idReloj = int.Parse (reader ["idReloj"].ToString ());
			fichada.Reloj = tablaData.getById ("reloj", 3, idReloj);
			//falta asignar:
			//reloj.TipoMovimiento
			//reloj.EstadoFichada
			//reloj.HoraServidor

			return fichada;
		}

		static List<Fichada> GetLista (string sql)
		{
			var fichadas = new List<Fichada> ();
			using (IDataReader reader = DB.Instancia.SqlToDbDataReader (sql)) {
				while (reader.Read ())
					fichadas.Add (make (reader));
			}
			return fichadas;
		}

		#endregion
	}
}
