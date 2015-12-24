//
//  ReciboRenglon.cs
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
using System.Data.Common;
using Hamekoz.Data;

namespace Sueldos.Core
{
	public class ReciboRenglon
	{
		int idLiquidacion;
		int codigo;
		int posicion;
		char letra;
		int anioMes;
		int idAplicacion;
		int ordenProceso;

		public int IdLiquidacion {
			get { return idLiquidacion; }
			set {
				idLiquidacion = value;
				DbDataReader rs;
				using (rs = DB.Instancia.SPToDbDataReader ("liquidacionesDetalleConsultar",
					       "id", idLiquidacion
				       )) {
					if (rs.Read ()) {
						//this.anioMes = anioMes;
						//this.idAplicacion = idAplicacion;
						/* idTipoSalario
                     * descripcion
                     * fechaLiquidacion
                     * periodoLiquidado
                     * lugarDePago
                     * fechaDePago
                     * periodoDepositado
                     * bancoDepositado
                     */
					}
				}
			}
		}

		public int IdTipoLiquidacion {
			get;
			set;
		}

		public int Legajo {
			get;
			set;
		}

		public int Codigo {
			get { return codigo; }
			set {
				codigo = value;
				using (
					DbDataReader rs = DB.Instancia.SPToDbDataReader ("calculoConsultar",
						                  "codigo", codigo,
						                  "idCalculo", IdCalculo
					                  ))
					if (rs.Read ()) {
						//idCalculo, OrdenProceso, Codigo, Descripcion, Formula, Tipo, Imprime, ImprimeCantidad, ImprimeVU, desactivado, idTipoLiquidacion, idAplicacion
						ordenProceso = Convert.ToInt32 (rs ["OrdenProceso"]);
						posicion = Convert.ToInt32 (rs ["Tipo"]);
						IdTipoLiquidacion = Convert.ToInt32 (rs ["idTipoLiquidacion"]);
						idAplicacion = Convert.ToInt32 (rs ["idAplicacion"]);
					}
			}
		}

		public double Cantidad {
			get;
			set;
		}

		public double VU {
			get;
			set;
		}

		public double Importe {
			get;
			set;
		}

		public int IdCalculo {
			get;
			set;
		}

		#region BaseDatos

		public void CargarDatosConcepto ()
		{

		}

		public void CargarDatos ()
		{
			using (DbDataReader
			       rs = DB.Instancia.SPToDbDataReader ("liquidacionesConsultarConcepto",
				            "@idLiquidacion", idLiquidacion,
				            "@legajo", Legajo,
				            "@codigo", codigo
			            )) {
				if (rs.Read ()) {
					Cantidad = Convert.ToDouble (rs ["Cantidad"]);
					VU = Convert.ToDouble (rs ["VU"]);
					Importe = Convert.ToDouble (rs ["Importe"]);
				}
			}
		}

		public void Actualizar ()
		{
			DB.Instancia.SP ("liquidacionesActualizar",
				"idLiquidacion", idLiquidacion,
				"idTipoLiquidacion", IdTipoLiquidacion,
				"legajo", Legajo,
				"codigo", codigo,
				"cantidad", Cantidad,
				"UV", VU,
				"importe", Importe);
		}

		public void Grabar ()
		{
			DB.Instancia.SP ("liquidacionesInsertar",
				"idLiquidacion", idLiquidacion,
				"idTipoLiquidacion", IdTipoLiquidacion,
				"Legajo", Legajo,
				"Codigo", codigo,
				"AnioMes", anioMes,
				"idCalculo", IdCalculo,
				"idAplicacion", idAplicacion,
				"Posicion", posicion,
				"Cantidad", Cantidad,
				"VU", VU,
				"Importe", Importe,
				"Letra", letra,
				"OrdenProceso", ordenProceso);
		}

		#endregion
	}
}
