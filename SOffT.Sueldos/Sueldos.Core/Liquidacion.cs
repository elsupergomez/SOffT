//
//  Liquidacion.cs
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
using System.Data.Common;
using Hamekoz.Data;

namespace Sueldos.Core
{
	public class Liquidacion
	{
		public Liquidacion ()
		{
			FechasDePago = new List<DateTime> ();
		}

		/// <summary>
		/// Constructor para Liquidacion existente.
		/// </summary>
		/// <param name = "idLiquidacion"></param>
		public Liquidacion (int idLiquidacion)
		{
			Id = idLiquidacion;
			FechasDePago = new List<DateTime> ();
			cargarDatos ();
		}

		public int Id {
			get;
			set;
		}

		public int AnioMes {
			get;
			set;
		}

		public int IdAplicacion {
			get;
			set;
		}

		public int IdTipoSalario {
			get;
			set;
		}

		public string Descripcion {
			get;
			set;
		}

		public DateTime FechaLiquidacion {
			get;
			set;
		}

		public string PeriodoLiquidado {
			get;
			set;
		}

		public string LugarDePago {
			get;
			set;
		}

		public IList<DateTime> FechasDePago {
			get;
			set;
		}

		public string PeriodoDepositado {
			get;
			set;
		}

		public string BancoDepositado {
			get;
			set;
		}

		public DateTime FechaDepositado {
			get;
			set;
		}


		public Boolean IdEstado {
			get;
			set;
		}

		public Boolean RecibosSeparados {
			get;
			set;
		}

		protected void cargarDatos ()
		{
			//cargo datos liquidacion
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("liquidacionesDetalleConsultar", "@id", Id)) {
				if (rs.Read ()) {
					AnioMes = Convert.ToInt32 (rs ["anioMes"]);
					IdAplicacion = Convert.ToInt32 (rs ["idAplicacion"]);
					IdTipoSalario = Convert.ToInt32 (rs ["idTipoSalario"]);
					Descripcion = rs ["Descripcion"].ToString ();
					FechaLiquidacion = Convert.ToDateTime (rs ["fechaLiquidacion"]);
					PeriodoLiquidado = rs ["periodoLiquidado"].ToString ();
					LugarDePago = rs ["lugarDePago"].ToString ();
					//  this.fechasDePago[0] = Convert.ToDateTime(rs["fechaDePago"]);
					//  this.cargarFechasDePago();
					PeriodoDepositado = rs ["periodoDepositado"].ToString ();
					BancoDepositado = rs ["bancoDepositado"].ToString ();
					FechaDepositado = Convert.ToDateTime (rs ["fechaDepositado"]);
					IdEstado = Convert.ToBoolean (rs ["estado"]);
					RecibosSeparados = Convert.ToBoolean (rs ["recibosSeparados"]);
				}
			}
		}

		/*    private void cargarFechasDePago()
        {
            DbDataReader rs = Model.DB.ejecutarDataReader(Model.TipoComando.SP, "fechasDePagoConsultar", "@idLiquidacion", this.id);
            while (rs.Read())
                this.fechasDePago.Add(Convert.ToDateTime(rs["fechaDePago"]));
            Model.DB.desconectarDB();
        }*/

		/*     private void grabarFechasDePago()
        {
            //primero elimino las actuales
            Model.DB.ejecutarProceso(Model.TipoComando.SP, "fechasDePagoEliminarTodas", "@idLiquidacion", this.id);
            for (int i = 0; i < this.fechasDePago.Count; i++)
                Model.DB.ejecutarProceso(Model.TipoComando.SP, "fechasDePagoInsertar", "@idLiquidacion", this.id, "@fechaDePago", this.fechasDePago[i]);
        }*/

		/*     public void grabar()
        {   //graba y luego obtiene el id grabado para ser asignado al objeto.
            Model.DB.ejecutarProceso(Model.TipoComando.SP, "liquidacionesDetalleActualizar", "@AnioMes", this.anioMes, "@idAplicacion", this.idAplicacion, "@idTipoSalario", this.idTipoSalario, "@descripcion", this.descripcion, "@fechaLiquidacion", this.fechaLiquidacion, "@periodoLiquidado", this.periodoLiquidado, "@lugarDePago", this.lugarDePago, "@periodoDepositado", this.periodoDepositado, "@bancoDepositado", this.bancoDepositado, "@fechaDepositado", this.fechaDepositado, "@estado", this.idEstado, "@recibosSeparados", this.recibosSeparados);
            this.id = Convert.ToInt32(Model.DB.ejecutarScalar(Model.TipoComando.SP, "liquidacionesDetalleConsultaNueva",  "@AnioMes",this.anioMes, "@idAplicacion", this.idAplicacion, "@fechaLiquidacion", this.FechaLiquidacion ));
            this.grabarFechasDePago();
        }*/

		/*     public static Boolean consultarAnioMesAbierta(int anioMes)
        {
            DbDataReader rs = Model.DB.ejecutarDataReader(Model.TipoComando.SP, "liquidacionesDetalleConsultarAnioMesAbierta", "@anioMes", anioMes);
            if (rs.Read())
            {
                Model.DB.desconectarDB();
                return true;
            }
            else
            {
                Model.DB.desconectarDB();
                return false;
            }
            
        }*/

		/*    public static Boolean hayConveniosAsociados(int idTipoLiq)
        {
            DbDataReader rsLiquidaciones;
            rsLiquidaciones = Model.DB.ejecutarDataReader(Model.TipoComando.SP, "tablasConsultarConveniosAsociadosAliquidaciones", "@idTipoLiquidacion", idTipoLiq);
            if (rsLiquidaciones.Read())
            {
                if (Convert.ToInt32(rsLiquidaciones["contenido"]) > 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }*/
	}
}
