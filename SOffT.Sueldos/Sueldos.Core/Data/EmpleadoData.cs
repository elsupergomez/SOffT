//
//  EmpleadoData.cs
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
using System.Data;
using System.Text;
using Hamekoz.Data;
using Sueldos.Entidades;

namespace Sueldos.Data
{
	public class EmpleadoData
	{
		const string tabla = "empleadosSueldos";

		#region ABM

		public int insert (EmpleadoEntity empleado)
		{
			int salida;
			salida = actualizaDatos (empleado);
			return salida;
		}

		public int update (EmpleadoEntity empleado)
		{
			int salida;
			salida = actualizaDatos (empleado);
			return salida;
		}

		public int delete (int legajo)
		{
			int salida;
			salida = DB.Instancia.SP ("empleadosEliminar", "@legajo", legajo);
			return salida;
		}

		#endregion

		#region SELECTs

		/// <summary>
		/// Devuelve un Empleado a partir de su idPersona. idEmpleado=idPersona
		/// </summary>
		/// <param name="idEmpleado"></param>
		/// <returns></returns>
		public EmpleadoEntity getById (int idEmpleado)
		{
			using (IDataReader reader = DB.Instancia.SPToDbDataReader ("empleadosConsultarParaCargar", "@idEmpleado", idEmpleado)) {
				return (reader.Read ()) ? make (reader) : null;
			}
		}

		/// <summary>
		/// Consulta el legajo de un empleado a partir del cuil.
		/// El cuil debe pasarse sin guiones
		/// </summary>
		/// <param name="cuil"></param>
		/// <returns></returns>
		public int getLegajoByCUIL (string cuil)
		{
			using (IDataReader rs = DB.Instancia.SPToDbDataReader ("empleadosSueldosConsultarLegajoCUIL", "@cuil", cuil)) {
				return rs.Read () ? Convert.ToInt32 (rs ["legajo"]) : 0;
			}
		}

		/// <summary>
		/// Dado un legajo, consulta el apellido y nombres del empleado.
		/// </summary>
		/// <param name="legajo"></param>
		/// <returns></returns>
		public string getNombresByLegajo (int legajo)
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT empleados.nyap");
			sql.Append (" FROM ");
			sql.Append (tabla);
			sql.Append (" INNER JOIN empleados ON empleados.legajo = empleadosSueldos.legajo");
			sql.Append (" AND empleadosSueldos.codigo = 10 ");
			sql.Append (" WHERE empleados.legajo= " + legajo + " and empleadosSueldos.valor <> 1 AND empleados.eliminado = 0");
			using (IDataReader rs = DB.Instancia.SqlToDbDataReader (sql.ToString ())) {
				return rs.Read () ? rs ["nyap"].ToString () : "SIN DATO";
			}
		}

		/// <summary>
		/// Dado un legajo, consulta el apellido y nombres del empleado.
		/// </summary>
		/// <param name="legajo"></param>
		/// <returns></returns>
		public string getNombresByLegajoConBajas (int legajo)
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT empleados.nyap");
			sql.Append (" FROM ");
			sql.Append (tabla);
			sql.Append (" INNER JOIN empleados ON empleados.legajo = empleadosSueldos.legajo");
			sql.Append (" AND empleadosSueldos.codigo = 10 ");
			sql.Append (" WHERE empleados.legajo= " + legajo + " AND empleados.eliminado = 0");//" and empleadosSueldos.valor <> 1 AND empleados.eliminado = 0");
			using (IDataReader rs = DB.Instancia.SqlToDbDataReader (sql.ToString ())) {
				return rs.Read () ? rs ["nyap"].ToString () : "SIN DATO";
			}
		}

		/// <summary>
		/// Dado un legajo, consulta la foto del empleado.
		/// </summary>
		/// <param name="legajo"></param>
		/// <returns></returns>
		public string getFotoByLegajo (int legajo)
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT empleados.foto");
			sql.Append (" FROM ");
			sql.Append (tabla);
			sql.Append (" INNER JOIN empleados ON empleados.legajo = empleadosSueldos.legajo");
			sql.Append (" AND empleadosSueldos.codigo = 10 ");
			sql.Append (" WHERE empleados.legajo= " + legajo + " and empleadosSueldos.valor <> 1 AND empleados.eliminado = 0");
			using (IDataReader rs = DB.Instancia.SqlToDbDataReader (sql.ToString ())) {
				return rs.Read () ? rs ["foto"].ToString () : "";
			}
		}

		/// <summary>
		/// Verifica si existe un legajo
		/// </summary>
		/// <param name="legajo"></param>
		/// <returns></returns>
		public bool existeLegajo (int legajo)
		{
			bool existe = false;
			using (IDataReader rs = DB.Instancia.SPToDbDataReader ("existeLegajo", "@legajo", legajo)) {
				if (rs.Read ())
					existe = legajo == Convert.ToInt32 (rs ["Legajo"]);
			}
			return existe;
		}

		public DataSet GetAlldatosBasicos ()
		{
			DataSet ds;
			var sql = new StringBuilder ();
			sql.Append ("SELECT empleados.idEmpleado");
			sql.Append (", empleadosSueldos.legajo AS Legajo");
			sql.Append (", empleados.nyap as Nombres");
			sql.Append (" FROM ");
			sql.Append (tabla);
			sql.Append (" INNER JOIN empleados ON empleados.legajo = empleadosSueldos.legajo");
			sql.Append (" AND empleadosSueldos.codigo = 10 ");
			sql.Append (" WHERE (empleadosSueldos.valor <> 1) AND (empleados.eliminado = 0)");
			sql.Append (" order by empleados.nyap ");
			ds = DB.Instancia.SqlToDataSet (sql.ToString ());
			return ds;
		}

		#endregion

		#region PRIVATE

		EmpleadoEntity make (IDataRecord reader)
		{
			var empleado = new EmpleadoEntity (int.Parse (reader ["idEmpleado"].ToString ()));
			//DATOS DE LA PERSONA (lectura tabla empleados)
			empleado.Legajo = Convert.ToInt32 (reader ["Legajo"]);
			empleado.Nombre = reader ["Apellido y Nombres"].ToString ();
			empleado.Domicilio = reader ["Domicilio"].ToString ();
			empleado.IdPais = Convert.ToInt32 (reader ["idPais"]);
			empleado.IdProvincia = Convert.ToInt32 (reader ["idProvincia"]);
			empleado.IdLocalidad = Convert.ToInt32 (reader ["idLocalidad"]);
			empleado.CP = reader ["CP"].ToString ();
			empleado.Telefono = reader ["Telefono"].ToString ();
			empleado.TelefonoMensajes = reader ["TelefonoMensajes"].ToString ();
			empleado.Foto = reader ["foto"].ToString ();

			empleado.IdEstadoCivil = Convert.ToInt32 (DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "@legajo", empleado.Legajo, "@codigo", 15));
			empleado.IdTipoDocumento = Convert.ToInt32 (DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "@legajo", empleado.Legajo, "@codigo", 16));
			empleado.NumeroDocumento = DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "@legajo", empleado.Legajo, "@codigo", 17).ToString ();
			empleado.FechaNacimiento = Convert.ToDateTime (DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "@legajo", empleado.Legajo, "@codigo", 25).ToString ());
			empleado.IdSexo = Convert.ToInt32 (DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "@legajo", empleado.Legajo, "@codigo", 07));
			empleado.IdNacionalidad = Convert.ToInt32 (DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "@legajo", empleado.Legajo, "@codigo", 05));

			//DATOS DEL EMPLEADO (lectura tabla empleadossueldos)
			/*     empleado.OtrosCampos = new List<CampoEmpleadoEntity>();
            empleado.Familiares = new List<FamiliarEntity>();
            empleado.Periodos = new List<PeriodoTrabajadoEntity>();
            empleado.Horarios = new List<HorarioEntity>();
            empleado.Vacaciones = new List<VacacionEntity>();*/
			empleado.CUIL = DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "@legajo", empleado.Legajo, "@codigo", 18).ToString ();
			empleado.IdAFJP = Convert.ToInt32 (DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "legajo", empleado.Legajo, "codigo", 20));

			cargarFechas (empleado);

			empleado.SueldoOjornal = Convert.ToDouble (DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "@legajo", empleado.Legajo, "@codigo", 98));
			empleado.IdEstado = Convert.ToInt32 (DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "legajo", empleado.Legajo, "codigo", 10));
			empleado.IdTipoLiquidacion = Convert.ToInt32 (DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "@legajo", empleado.Legajo, "codigo", 41));
			empleado.IdCategoria = Convert.ToInt32 (DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "legajo", empleado.Legajo, "codigo", 30));
			empleado.IdContrato = Convert.ToInt32 (DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "legajo", empleado.Legajo, "codigo", 12));
			empleado.IdConvenio = Convert.ToInt32 (DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "legajo", empleado.Legajo, "codigo", 13));
			empleado.IdSindicato = Convert.ToInt32 (DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "legajo", empleado.Legajo, "codigo", 14));
			empleado.IdPuesto = Convert.ToInt32 (DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "legajo", empleado.Legajo, "codigo", 31));
			empleado.IdArea = Convert.ToInt32 (DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "legajo", empleado.Legajo, "codigo", 36));
			empleado.IdCentroDeCosto = Convert.ToInt32 (DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "legajo", empleado.Legajo, "codigo", 35));
			empleado.IdSucursalBanco = Convert.ToInt32 (DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "legajo", empleado.Legajo, "codigo", 21));
			empleado.IdBanco = Convert.ToInt32 (DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "legajo", empleado.Legajo, "codigo", 24));
			empleado.NumeroCuenta = DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "legajo", empleado.Legajo, "codigo", 22).ToString ();
			empleado.IdFormaDePago = Convert.ToInt32 (DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "legajo", empleado.Legajo, "codigo", 23));
			empleado.IdObraSocial = Convert.ToInt32 (DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "legajo", empleado.Legajo, "codigo", 50));
			empleado.LegajoDelConyuge = Convert.ToInt32 (DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "legajo", empleado.Legajo, "codigo", 19));
			empleado.IdSituacionDeRevista = Convert.ToInt32 (DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "legajo", empleado.Legajo, "codigo", 60));
			empleado.IdCodigoDeCondicion = Convert.ToInt32 (DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "legajo", empleado.Legajo, "codigo", 61));
			empleado.IdCodigoDeActividad = Convert.ToInt32 (DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "legajo", empleado.Legajo, "codigo", 62));
			empleado.IdModalidadDeContratacion = Convert.ToInt32 (DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "legajo", empleado.Legajo, "codigo", 63));
			empleado.IdCodigoDeSiniestrado = Convert.ToInt32 (DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "legajo", empleado.Legajo, "codigo", 64));

			IDataReader rs;
			//*****cargo maestros otros campos*****
			CampoEmpleadoEntity m;
			using (rs = DB.Instancia.SPToDbDataReader ("empleadosSueldosConsultarCamposLegajo", "@legajo", empleado.Legajo)) {
				while (rs.Read ()) {
					m = new CampoEmpleadoEntity (Convert.ToInt32 (rs ["codigo"]), rs ["descripcion"].ToString (), rs ["valor"].ToString (), rs ["detalle"].ToString ());
					empleado.OtrosCampos.Add (m);
				}
			}
            
			//*****cargo familiares*****
			DateTime fechaBaja;
			FamiliarEntity f;
			using (rs = DB.Instancia.SPToDbDataReader ("familiaresConsultarParaCargar", "@legajo", empleado.Legajo)) {
				while (rs.Read ()) {
					if (rs ["fechaBaja"] == null) //copio a variable por problemas con las null
                        fechaBaja = new DateTime (0);
					else
						fechaBaja = Convert.ToDateTime (rs ["fechaBaja"]);
					f = new FamiliarEntity (empleado.Legajo, Convert.ToInt32 (rs ["idFamiliar"]), rs ["apellidoYnombres"].ToString (), Convert.ToInt32 (rs ["idParentesco"]), rs ["parentesco"].ToString (), Convert.ToInt32 (rs ["idTipoDocumento"]), rs ["tipoDocumento"].ToString (), rs ["numeroDocumento"].ToString (), Convert.ToDateTime (rs ["fechaNacimiento"]), Convert.ToDateTime (rs ["fechaAlta"]), fechaBaja, Convert.ToInt32 (rs ["idSexo"]), rs ["sexo"].ToString (), Convert.ToInt32 (rs ["idNacionalidad"]), rs ["nacionalidad"].ToString (), Convert.ToInt32 (rs ["idEstadoCivil"]), rs ["EstadoCivil"].ToString (), Convert.ToBoolean (rs ["generaSalario"]), Convert.ToBoolean (rs ["declaradoParaGanancias"]));
					empleado.Familiares.Add (f);
				}
			}
            
			//*****cargo periodos trabajados*****
			DateTime fInicio;
			DateTime fFin;
			PeriodoTrabajadoEntity p;
			using (rs = DB.Instancia.SPToDbDataReader ("periodosTrabajadosConsultar", "@legajo", empleado.Legajo)) {
				while (rs.Read ()) {
					if (rs ["Fecha Inicio"] == DBNull.Value) //copio a variable por problemas con las null
                        fInicio = new DateTime (0);
					else
						fInicio = Convert.ToDateTime (rs ["Fecha Inicio"]);
					if (rs ["Fecha Fin"] == DBNull.Value) //copio a variable por problemas con las null
                        fFin = new DateTime (0);
					else
						fFin = Convert.ToDateTime (rs ["Fecha Fin"]);

					p = new PeriodoTrabajadoEntity (Convert.ToInt32 (rs ["id"]), Convert.ToInt32 (rs ["Legajo"]), fInicio, fFin, Convert.ToInt32 (rs ["idCausa"]), rs ["Causa Egreso"].ToString (), rs ["Observaciones"].ToString ());
					empleado.Periodos.Add (p);
				}
			}
			//

			//*****cargo horarios*****
			HorarioEntity h;
			using (rs = DB.Instancia.SPToDbDataReader ("horariosConsultar", "@legajo", empleado.Legajo)) {
				while (rs.Read ()) {
					h = new HorarioEntity (Convert.ToInt32 (rs ["id"]), Convert.ToInt32 (rs ["legajo"]), Convert.ToInt32 (rs ["LimiteDia"]), Convert.ToInt32 (rs ["IdDia"]), rs ["Dia"].ToString (), Convert.ToInt32 (rs ["idTipoMovimiento"]), rs ["Movimiento"].ToString (), Convert.ToDateTime (rs ["Hora"]).ToShortTimeString (), Convert.ToInt32 (rs ["tolerancia"]));
					empleado.Horarios.Add (h);
				}
			}

			//*****cargo vacaciones*****
			VacacionEntity v;
			using (rs = DB.Instancia.SPToDbDataReader ("vacacionesConsultarParaCargar", "@legajo", empleado.Legajo)) {
				while (rs.Read ()) {
					v = new VacacionEntity ();
					v.Legajo = empleado.Legajo;
					v.AnioVacaciones = rs.GetInt32 (rs.GetOrdinal ("anioVacaciones"));
					v.Codigo = rs.GetInt32 (rs.GetOrdinal ("codigo"));
					v.DiasTomados = Convert.ToInt32 (rs ["diasTomados"]);
					v.Fecha = rs.GetDateTime (rs.GetOrdinal ("fecha"));
					v.TotalDias = Convert.ToInt32 (rs ["totalDias"]);
					empleado.Vacaciones.Add (v);
				}
			}

			return empleado;
		}

		int actualizaDatos (EmpleadoEntity empleado)
		{
			const int salida = 0;
			//DATOS DE LA PERSONA:
			DB.Instancia.SP ("empleadosActualizar", "@legajo", empleado.Legajo, "@nombre", empleado.Nombre, "@domicilio", empleado.Domicilio, "@CP", empleado.CP, "@idPais", empleado.IdPais, "@idProvincia", empleado.IdProvincia, "@idLocalidad", empleado.IdLocalidad, "@telefono", empleado.Telefono, "@telefonoM", empleado.TelefonoMensajes, "@foto", empleado.Foto);
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 15, "@valor", empleado.IdEstadoCivil);
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 16, "@valor", empleado.IdTipoDocumento);
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 17, "@valor", empleado.NumeroDocumento);
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 25, "@valor", empleado.FechaNacimiento.ToShortDateString ());
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 07, "@valor", empleado.IdSexo);
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 05, "@valor", empleado.IdNacionalidad);

			//DATOS DEL EMPLEADO:
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 18, "@valor", empleado.CUIL);
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 20, "@valor", empleado.IdAFJP);
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 29, "@valor", empleado.FechaReconocimiento.ToShortDateString ());
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 98, "@valor", empleado.SueldoOjornal);
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 10, "@valor", empleado.IdEstado);
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 41, "@valor", empleado.IdTipoLiquidacion);
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 30, "@valor", empleado.IdCategoria);
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 12, "@valor", empleado.IdContrato);
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 13, "@valor", empleado.IdConvenio);
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 14, "@valor", empleado.IdSindicato);
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 31, "@valor", empleado.IdPuesto);
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 36, "@valor", empleado.IdArea);
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 35, "@valor", empleado.IdCentroDeCosto);
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 21, "@valor", empleado.IdSucursalBanco);
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 24, "@valor", empleado.IdBanco);
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 22, "@valor", empleado.NumeroCuenta);
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 23, "@valor", empleado.IdFormaDePago);
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 50, "@valor", empleado.IdObraSocial);
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 19, "@valor", empleado.LegajoDelConyuge);
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 60, "@valor", empleado.IdSituacionDeRevista);
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 61, "@valor", empleado.IdCodigoDeCondicion);
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 62, "@valor", empleado.IdCodigoDeActividad);
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 63, "@valor", empleado.IdModalidadDeContratacion);
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 64, "@valor", empleado.IdCodigoDeSiniestrado);
			//*****actualizo otros campos*****
			DB.Instancia.SP ("empleadosSueldosEliminarCamposLegajo", "@legajo", empleado.Legajo); //elimino los existentes
			foreach (CampoEmpleadoEntity c in empleado.OtrosCampos)
				DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", c.Codigo, "@valor", c.Valor);
			//*****actualizo familiares*****
			DB.Instancia.SP ("familiaresEliminarFamiliaresLegajo", "@legajo", empleado.Legajo);
			foreach (FamiliarEntity f in empleado.Familiares)
				DB.Instancia.SP ("familiaresActualizar", "@legajo", empleado.Legajo, "@apellidoYNombres", f.ApellidoYnombres, "@idParentesco", f.IdParentesco, "@idTipoDocumento", f.IdTipoDocumento, "@numeroDocumento", f.NumeroDocumento, "@fechaNacimiento", f.FechaNacimiento, "@fechaAlta", f.FechaAlta, "@fechaBaja", f.FechaBaja, "@idSexo", f.IdSexo, "@idNacionalidad", f.IdNacionalidad, "@idEstadoCivil", f.IdEstadoCivil, "@generaSalario", f.GeneraSalario, "@declaradoParaGanancias", f.DeclaradoParaGanancias);
			//*****actualizo periodos*****
			DB.Instancia.SP ("periodosTrabajadosEliminarLegajo", "@legajo", empleado.Legajo); //elimino los existentes
			System.Data.SqlTypes.SqlDateTime fInicio; //para grabar fecha nula en la db
			System.Data.SqlTypes.SqlDateTime fFin; //para grabar fecha nula en la db
			foreach (PeriodoTrabajadoEntity p in empleado.Periodos) {
				fInicio = System.Data.SqlTypes.SqlDateTime.Null;
				fFin = System.Data.SqlTypes.SqlDateTime.Null;
				if (p.FechaInicio > Convert.ToDateTime ("01/01/1920"))
					fInicio = p.FechaInicio;
				if (p.FechaFin > Convert.ToDateTime ("01/01/1920"))
					fFin = p.FechaFin;
				DB.Instancia.SP ("periodosTrabajadosInsertar", "@legajo", empleado.Legajo, "@fechaInicio", fInicio, "@fechaFin", fFin, "@idCausaEgreso", p.IdCausaEgreso, "@observaciones", p.Observaciones);
			}
			cargarFechas (empleado);
			//*****actualizo horarios*****
			DB.Instancia.SP ("horariosEliminarLegajo", "@legajo", empleado.Legajo); //elimino los existentes
			foreach (HorarioEntity h in empleado.Horarios)
				DB.Instancia.SP ("horariosInsertar", "@legajo", empleado.Legajo, "@limiteDia", h.LimiteDia, "@idDia", h.IdDia, "@idTipoMovimiento", h.IdTipoMovimiento, "@hora", h.Hora, "@tolerancia", h.Tolerancia);

			//Actualizo Vacaciones
			DB.Instancia.SP ("vacacionesEliminarLegajo", "@legajo", empleado.Legajo); //elimino los existentes
			foreach (VacacionEntity v in empleado.Vacaciones)
				DB.Instancia.SP ("vacacionesActualizar", "@legajo", empleado.Legajo, "@fecha", v.Fecha.Date, "@codigo", v.Codigo, "@diasTomados", v.DiasTomados, "@anioVacaciones", v.AnioVacaciones, "@totalDias", v.TotalDias);

			//
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 26, "@valor", empleado.FechaIngreso.ToShortDateString ());
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 27, "@valor", empleado.FechaEgreso.ToShortDateString ());
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 28, "@valor", empleado.FechaReIngreso.ToShortDateString ());
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 29, "@valor", empleado.FechaReconocimiento.ToShortDateString ());
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", empleado.Legajo, "@codigo", 11, "@valor", empleado.IdCausaEgreso);

			return salida;
		}

		/// <summary>
		/// carga las fechas de egreso, ingreso y reingreso y causaEgreso desde períodos trabajados
		/// </summary>
		public void cargarFechas (EmpleadoEntity empleado)
		{
			DateTime fechaReconocimiento;
			IDataReader rs;
			using (rs = DB.Instancia.SPToDbDataReader ("periodosTrabajadosConsultarFechas", "@legajo", empleado.Legajo)) {
				if (rs.Read ()) {
					if (rs ["fechaIngreso"].ToString () == "")
						empleado.FechaIngreso = new DateTime (0).Date;
					else
						empleado.FechaIngreso = Convert.ToDateTime (rs ["fechaIngreso"]).Date;
					if (rs ["fechaReIngreso"].ToString () == "" || Convert.ToDateTime (rs ["fechaReIngreso"]).Date == empleado.FechaIngreso)
						empleado.FechaReIngreso = new DateTime (0).Date;
					else
						empleado.FechaReIngreso = Convert.ToDateTime (rs ["fechaReIngreso"]).Date;
					if (rs ["fechaEgreso"].ToString () == "")
						empleado.FechaEgreso = new DateTime (0).Date;
					else
						empleado.FechaEgreso = Convert.ToDateTime (rs ["fechaEgreso"]).Date;
					if (rs ["idCausaEgreso"].ToString () == "")
						empleado.IdCausaEgreso = 0;
					else
						empleado.IdCausaEgreso = Convert.ToInt32 (rs ["idCausaEgreso"]);

					fechaReconocimiento = Convert.ToDateTime (DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "@legajo", empleado.Legajo, "@codigo", 29));
					if (fechaReconocimiento < Convert.ToDateTime ("01/01/1920"))
						empleado.FechaReconocimiento = empleado.FechaIngreso;
					else
						empleado.FechaReconocimiento = fechaReconocimiento;
				}
			}
		}

		#endregion
	}
}
