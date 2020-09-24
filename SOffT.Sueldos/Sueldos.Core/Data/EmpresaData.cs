//
//  EmpresaData.cs
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
	public class EmpresaData
	{
		const string tabla = "empresa";

		#region ABM

		public int insert (EmpresaEntity empresa)
		{
			var sql = new StringBuilder ();
			sql.Append (" INSERT INTO ");
			sql.Append (tabla);
			sql.Append (" VALUES('");
			sql.Append (empresa.RazonSocial);
			sql.Append ("', '");
			sql.Append (empresa.CUIT);
			sql.Append ("', '");
			sql.Append (empresa.Domicilio);
			sql.Append ("', ");
			sql.Append (empresa.UltimaRubrica);
			sql.Append (", ");
			sql.Append (empresa.CorrespondeReduccion ? 1 : 0);
			sql.Append (", ");
			sql.Append (empresa.CodigoActividad);
			sql.Append (", ");
			sql.Append (empresa.TipoEmpleador);
			sql.Append (", ");
			sql.Append (empresa.PorcentajeAlicuotaLRT);
			sql.Append (", ");
			sql.Append (empresa.CuotaFijaLRT);
			sql.Append (", '");
			sql.Append (empresa.Pais);
			sql.Append ("', '");
			sql.Append (empresa.Provincia);
			sql.Append ("', '");
			sql.Append (empresa.Localidad);
			sql.Append ("', '");
			sql.Append (empresa.Imagen);
			sql.Append ("')");
			return DB.Instancia.Sql (sql.ToString ());
		}

		public int update (EmpresaEntity empresa)
		{
			var sql = new StringBuilder ();
			sql.Append (" UPDATE ");
			sql.Append (tabla);
			sql.Append (" SET");
			sql.Append (" razonsocial = '");
			sql.Append (empresa.RazonSocial);
			sql.Append ("', cuit = '");
			sql.Append (empresa.CUIT);
			sql.Append ("', domicilio = '");
			sql.Append (empresa.Domicilio);
			sql.Append ("', ultimaRubrica = ");
			sql.Append (empresa.UltimaRubrica);
			sql.Append (", correspondeReduccion = ");
			sql.Append (empresa.CorrespondeReduccion ? 1 : 0);
			sql.Append (", codigoActividad = ");
			sql.Append (empresa.CodigoActividad);
			sql.Append (", tipoEmpleador = ");
			sql.Append (empresa.TipoEmpleador);
			sql.Append (", porcentajeAlicuotaLRT = ");
			sql.Append (empresa.PorcentajeAlicuotaLRT);
			sql.Append (", cuotaFijaLRT = ");
			sql.Append (empresa.CuotaFijaLRT);
			sql.Append (", Pais = '");
			sql.Append (empresa.Pais);
			sql.Append ("', Provincia = '");
			sql.Append (empresa.Provincia);
			sql.Append ("', Localidad = '");
			sql.Append (empresa.Localidad);
			sql.Append ("', Logotipo = '");
            sql.Append(empresa.Imagen);
			sql.Append (" WHERE ");
			sql.Append (" idEmpresa = ");
			sql.Append (empresa.IdEmpresa);
			return DB.Instancia.Sql(sql.ToString ());
		}

		public int delete (EmpresaEntity empresa)
		{
			var sql = new StringBuilder ();
			sql.Append ("DELETE");
			sql.Append (" FROM ");
			sql.Append (tabla);
			sql.Append (" WHERE ");
			sql.Append (" idEmpresa = " + empresa.IdEmpresa);
			return DB.Instancia.Sql (sql.ToString ());
		}

		#endregion

		#region SELECTs

		/// <summary>
		/// Devuelve una Empresa a partir de su idEmpresa. 
		/// </summary>
		/// <param name="idEmpresa"></param>
		/// <returns></returns>
		public EmpresaEntity getById (int idEmpresa)
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT idEmpresa");
			sql.Append (", razonSocial");
			sql.Append (", cuit");
			sql.Append (", domicilio");
			sql.Append (", ultimaRubrica");
			sql.Append (", correspondeReduccion");
			sql.Append (", codigoActividad");
			sql.Append (", tipoEmpleador");
			sql.Append (", porcentajeAlicuotaLRT");
			sql.Append (", cuotaFijaLRT");
			sql.Append (", Pais");
			sql.Append (", Provincia");
			sql.Append (", Localidad");
			sql.Append (", Logotipo");
			sql.Append (", (SELECT detalle FROM tablas WHERE nombre = 'empleadosSueldos' AND indice = 62 AND contenido = codigoActividad) AS actividad");
			sql.Append (" FROM ");
			sql.Append (tabla);
			sql.Append (" WHERE ");
			sql.Append (" idEmpresa = " + idEmpresa);
			using (IDataReader reader = DB.Instancia.SqlToDbDataReader (sql.ToString ())) {
				return (reader.Read ()) ? make (reader) : null;
			}
		}

		public List<EmpresaEntity> GetAll ()
		{
			var sql = new StringBuilder ();
			sql.Append ("SELECT idEmpresa");
			sql.Append (", razonSocial");
			sql.Append (", cuit");
			sql.Append (", domicilio");
			sql.Append (", ultimaRubrica");
			sql.Append (", correspondeReduccion");
			sql.Append (", codigoActividad");
			sql.Append (", tipoEmpleador");
			sql.Append (", porcentajeAlicuotaLRT");
			sql.Append (", cuotaFijaLRT");
			sql.Append (", Pais");
			sql.Append (", Provincia");
			sql.Append (", Localidad");
			sql.Append (", Logotipo");
			sql.Append (", (SELECT detalle FROM tablas WHERE nombre = 'empleadosSueldos' AND indice = 62 AND contenido = codigoActividad) AS actividad");
			sql.Append (" FROM ");
			sql.Append (tabla);
			return GetLista (sql.ToString ());
		}

		#endregion

		#region PRIVATE

		static EmpresaEntity make (IDataRecord reader)
		{
			var empresa = new EmpresaEntity (int.Parse (reader ["idEmpresa"].ToString ()));
			empresa.RazonSocial = reader ["razonSocial"].ToString ();
			empresa.CUIT = reader ["CUIT"].ToString ();
			empresa.Domicilio = reader ["domicilio"].ToString ();
			empresa.UltimaRubrica = Convert.ToInt32 (reader ["ultimaRubrica"]);
			empresa.CorrespondeReduccion = Convert.ToBoolean (reader ["correspondeReduccion"]);
			empresa.CodigoActividad = Convert.ToInt32 (reader ["codigoActividad"]);
			empresa.TipoEmpleador = Convert.ToInt32 (reader ["tipoEmpleador"]);
			empresa.PorcentajeAlicuotaLRT = Convert.ToDouble (reader ["porcentajeAlicuotaLRT"]);
			empresa.CuotaFijaLRT = Convert.ToDouble (reader ["cuotaFijaLRT"]);
			empresa.Pais = reader ["pais"].ToString ();
			empresa.Provincia = reader ["provincia"].ToString ();
			empresa.Localidad = reader ["localidad"].ToString ();
			empresa.Imagen = reader ["logotipo"].ToString ().Length == 0 ? null : (byte[])reader ["logotipo"];
			empresa.Actividad = reader ["actividad"].ToString ();
			return empresa;
		}

		static List<EmpresaEntity> GetLista (string sql)
		{
			var empresas = new List<EmpresaEntity> ();
			using (IDataReader reader = DB.Instancia.SqlToDbDataReader (sql)) {
				while (reader.Read ())
					empresas.Add (make (reader));
			}
			return empresas;
		}

		#endregion
	}
}
