//
//  Calculo.cs
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
	public class Calculo
	{
		//idTabla en calculo
		//legajo que se esta liquidando
		//OrdPro a saltar en calculo
		//AnioMes a liquidar
		static double[] acu;
		//arreglo de acumuladores
		static double[] var;
		//arreglo de variables
		static double imprime;
		static double imprimeCantidad;
		static double imprimeValorUnitario;
		static int idTipoLiquidacion;
		//normal/vacaciones/etc

		public static int IdTabla {
			get;
			set;
		}

		public static int Legajo {
			get;
			set;
		}

		public static int Salto {
			get;
			set;
		}

		public static int AnioMes {
			get;
			set;
		}

		static int AnioCalculo {
			get { return AnioMes / 100; }
		}

		static int MesCalculo {
			get { return AnioMes % 100; }
		}

		public static int IdAplicacion {
			get;
			set;
		}

		public static int IdLiquidacion {
			get;
			set;
		}

		static void inicializaACUs ()
		{
			acu.Initialize ();
		}

		static void inicializaVARs ()
		{
			var.Initialize ();
		}

		/// <summary>
		/// actualiza tabla verificando si es una tabla de la db
		/// o de la tabla tablas.
		/// la tabla debe llamarse nombretablaActualizar
		/// </summary>
		/// <param name="nombreTabla"></param>
		/// <param name="codigo"></param>
		/// <param name="valor"></param>
		/// <returns></returns>
		public double actualizarTabla (string nombreTabla, int codigo, string valor)
		{
			//consulto identidad
			byte identidad = 0;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("tablasConsultarContenidoyDetalle", "@tabla", nombreTabla, "@indice", codigo)) {
				if (rs.Read ())
					identidad = Convert.ToByte (rs ["identidad"]);
			}
			//si es tabla con historico
			if (identidad == 2)
				DB.Instancia.SP (nombreTabla + "Actualizar", "@AnioMes", Calculo.AnioMes, "@Indice", IdTabla, "@Legajo", Calculo.Legajo, "@Codigo", codigo, "@valor", valor);
			return 0;
		}

		/// <summary>
		/// actualiza la tabla de campos historicos de empleados 
		/// dado el codigo, lo copia de la tabla de empleados
		/// y lo almacena en empleadosHistoricoLiquidacion
		/// </summary>
		/// <param name="codigo"></param>
		/// <returns></returns>
		public int actualizarHistoricoEmpleado (int codigo)
		{
			DB.Instancia.SP ("empleadosHistoricoLiquidacionActualizaDesdeEmpleado", "@idLiquidacion", Calculo.IdLiquidacion, "@legajo", Calculo.Legajo, "@codigo", codigo);
			return 0;
		}

		/// <summary>
		/// devuelve un valor numerico con un numero de decimales dados.
		/// </summary>
		/// <param name="valor"></param>
		/// <param name="dec"></param>
		/// <returns></returns>
		public string decimales (string valor, int dec)
		{
			string cad = "{0:#0.";
			for (int i = 0; i < dec; i++)
				cad = cad + '0';
			cad = cad + '}';
			return string.Format (cad, Redondear (Convert.ToDouble (valor), dec));
		}

		/// <summary>
		/// Consulta cantidad de familiares para un legajo con un determinado
		/// parentesco
		/// </summary>
		/// <param name="idParentesco"></param>
		/// <returns></returns>
		public double familiaresConsultarCantidad (int idParentesco)
		{
			double salida;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("familiaresConsultarCantidadPorParentesco", "@Legajo", Calculo.Legajo, "@idParentesco", idParentesco)) {
				salida = rs.Read () ? Convert.ToDouble (rs ["cantidad"]) : 0;
			}
			return salida;
		}

		/// <summary>
		/// consulta importe de anticipos a descontar.
		/// </summary>
		/// <returns></returns>
		public double anticiposConsultarImporte ()
		{
			double salida;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("anticiposConsultarParaLiquidacion", "@Legajo", Calculo.Legajo, "@anioMes", Calculo.AnioMes)) {
				salida = rs.Read () ? Convert.ToDouble (rs ["importe"]) : 0;
			}
			return salida;
		}

		public double anticiposConsultarImportePorTipo (int tipo)
		{
			double salida;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("anticiposConsultarParaLiquidacionPorTipo", "@Legajo", Calculo.Legajo, "@anioMes", Calculo.AnioMes, "@idTipoAnticipo", tipo)) {
				salida = rs.Read () ? Convert.ToDouble (rs ["importe"]) : 0;
			}
			return salida;
		}

		/// <summary>
		/// acumula en acumulador acu[]
		/// </summary>
		/// <param name="valor"></param>
		/// <param name="indice"></param>
		public double acumular (double valor, int indice)
		{
			acu [indice] = acu [indice] + valor;
			Console.WriteLine ("     ACU[" + indice + "]=" + valor);
			return valor;
		}

		/// <summary>
		///  guarda el valor en variable var[] no cumula
		/// </summary>
		/// <param name="valor"></param>
		/// <param name="indice"></param>
		public double guardar (double valor, int indice)
		{
			var [indice] = valor;
			Console.WriteLine ("     VAR[" + indice + "]=" + valor);
			return valor;
		}

		public double potencia (double Base, double exponente)
		{
			double result;
			result = Math.Pow (Base, exponente);
			return result;
		}

		/// <summary>
		/// guarda en acumulados
		/// </summary>
		/// <param name="valor"></param>
		/// <param name="indice"></param>
		/// <returns></returns>
		public double guardarEnAcumulado (double valor, int indice)
		{
			DB.Instancia.SP ("acumuladosActualizar", "@AnioMes", Calculo.AnioMes, "@Indice", IdTabla, "@Legajo", Calculo.Legajo, "@Codigo", indice, "@Valor", valor);
			return valor;
		}

		/// <summary>
		/// consulta valor almacenado en acumulado
		/// </summary>
		/// <param name="indice"></param>
		/// <returns></returns>
		public double acumuladoEmpleado (int indice)
		{
			double salida;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("acumuladosConsultar", "@AnioMes", Calculo.AnioMes, "@Indice", IdTabla, "@Legajo", Calculo.Legajo, "@Codigo", indice)) {
				salida = rs.Read () ? Convert.ToDouble (rs ["Valor"]) : 0;
			}
			return salida;
		}

		/// <summary>
		/// Obtiene el importe total liquidado de un concepto dado.
		/// </summary>
		/// <param name="codigo"></param>
		/// <returns></returns>
		public double totalConceptoImporte (int codigo)
		{
			double salida;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("liquidacionesConsultarConcepto", "@idLiquidacion", Calculo.IdLiquidacion, "@Legajo", Calculo.Legajo, "@Codigo", codigo)) {
				salida = rs.Read () ? Convert.ToDouble (rs ["Importe"]) : 0;
			}
			return salida;
		}

		/// <summary>
		/// Consulta el importe total liquidado para un concepto de acuerdo al tipo de liquidacion.
		/// </summary>
		/// <param name="codigo"></param>
		/// <param name="tipo"></param>
		/// <returns></returns>
		public double totalConceptoImportePorTipo (int codigo, int tipo)
		{
			double salida;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("liquidacionesConsultarConceptoPorTipo", "@idLiquidacion", Calculo.IdLiquidacion, "@idTipoLiquidacion", tipo, "@Legajo", Calculo.Legajo, "@Codigo", codigo)) {
				salida = rs.Read () ? Convert.ToDouble (rs ["Importe"]) : 0;
			}
			return salida;
		}

		public double totalConceptoImporteMes (int codigo)
		{
			double salida;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("liquidacionesConsultarConceptoAnioMes", "@anioMes", Calculo.AnioMes, "@Legajo", Calculo.Legajo, "@Codigo", codigo)) {
				salida = rs.Read () ? Convert.ToDouble (rs ["Importe"]) : 0;
			}
			return salida;
		}

		public double totalConceptoImporteAnioMes (int codigo, int AnioMes)
		{
			double salida;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("liquidacionesConsultarConceptoAnioMes", "@anioMes", AnioMes, "@Legajo", Calculo.Legajo, "@Codigo", codigo)) {
				salida = rs.Read () ? Convert.ToDouble (rs ["Importe"]) : 0;
			}
			return salida;
		}

		public double totalConceptoHaberImporteMes (int codigo)
		{
			double salida;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("liquidacionesConsultarConceptoAnioMesPosicion", "@anioMes", Calculo.AnioMes, "@Legajo", Calculo.Legajo, "@Codigo", codigo, "@posicion", 1)) {
				salida = rs.Read () ? Convert.ToDouble (rs ["Importe"]) : 0;
			}
			return salida;
		}

		public double totalConceptoAdicionalImporteMes (int codigo)
		{
			double salida;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("liquidacionesConsultarConceptoAnioMesPosicion", "@anioMes", Calculo.AnioMes, "@Legajo", Calculo.Legajo, "@Codigo", codigo, "@posicion", 3)) {
				salida = rs.Read () ? Convert.ToDouble (rs ["Importe"]) : 0;
			}
			return salida;
		}

		public double totalConceptoHaberCantidadMes (int codigo)
		{
			double salida;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("liquidacionesConsultarConceptoAnioMesPosicion", "@anioMes", Calculo.AnioMes, "@Legajo", Calculo.Legajo, "@Codigo", codigo, "@posicion", 1)) {
				salida = rs.Read () ? Convert.ToDouble (rs ["Cantidad"]) : 0;
			}
			return salida;
		}

		/// <summary>
		/// consulta el mayor acumulado entre fechas
		/// </summary>
		/// <param name="codigo"></param>
		/// <param name="fDesde"></param>
		/// <param name="fHasta"></param>
		/// <returns></returns>
		public double acumuladoMayorEntreFechas (int codigo, DateTime fDesde, DateTime fHasta)
		{
			double salida;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("acumuladosConsultarMayorEntreFechas", "@legajo", Calculo.Legajo, "@codigo", codigo, "@fDesde", fDesde, "@fHasta", fHasta)) {
				salida = rs.Read () ? Convert.ToDouble (rs ["Valor"]) : 0;
			}
			return salida;
		}

		/// <summary>
		/// devuelve el contenido de una variable
		/// </summary>
		/// <param name="indice"></param>
		/// <returns></returns>
		public double variable (int indice)
		{
			return var [indice];
		}

		/// <summary>
		/// devuelve el contenido de un acumulador
		/// </summary>
		/// <param name="indice"></param>
		/// <returns></returns>
		public double acumulador (int indice)
		{
			return acu [indice];
		}

		/// <summary>
		/// consulta tablas en tablas
		/// </summary>
		/// <param name="tabla"></param>
		/// <param name="indice"></param>
		/// <returns></returns>
		public double tablas (string tabla, int indice)
		{
			double salida;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("tablasConsultarContenidoYdetalle", "@tabla", tabla, "@indice", indice)) {
				salida = rs.Read () ? Convert.ToDouble (rs ["Contenido"]) : 0;
			}
			return salida;
		}

		/// <summary>
		/// busca en tablas un importe en la columna detalle, comprendido en el rango delimitado por indice y contenido
		/// </summary>
		/// <param name="tabla"></param>
		/// <param name="valor"></param>
		/// <returns></returns>
		public double buscarImporte (string tabla, double valor)
		{
			double salida;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("tablasBuscarImporteEntreRangos", "@tabla", tabla, "@valor", valor)) {
				salida = rs.Read () ? Convert.ToDouble (rs ["detalle"]) : 0;
			}
			return salida;
		}

		/// <summary>
		/// busca un determinado valor comprendido entre los campos contenido y detalle, aplicando un coeficiente (dividiendo) los campos 
		/// contenido y detalle y devuelve el campo detalle.
		/// Este metodo es explicito para ganancias. Habría que trabajarlo un poco mas.
		/// Se realiza acá el pasaje de coeficientes para no aumentar la complejidad en 
		/// las formulas.
		/// </summary>
		/// <param name="tabla"></param>
		/// <param name="valor"></param>
		/// <returns></returns>
		public double buscarValorCoeficiente (string tabla, double valor)
		{
			//campo utilizado para enviar el mes de liquidacion al sp. (mes + 1 para ganancias)
			int mm;
			if (mesLiquidacion () < 12)
				mm = mesLiquidacion () + 1;
			else
				mm = 1;
			
			double salida;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("tablasBuscarValorCoeficiente", "@tabla", tabla, "@valor", valor, "@coeficiente", 12, "@mesLiquidacion", mm)) {
				salida = rs.Read () ? Convert.ToDouble (rs ["detalle"]) : 0;
			}
			return salida;
		}

		/// <summary>
		/// busca un determinado valor comprendido entre los campos contenido y detalle, y devuelve el campo detalle.
		/// </summary>
		/// <param name="tabla"></param>
		/// <param name="valor"></param>
		/// <returns></returns>
		public double buscarValor (string tabla, double valor)
		{
			double salida;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("tablasBuscarValor", "@tabla", tabla, "@valor", valor)) {
				salida = rs.Read () ? Convert.ToDouble (rs ["detalle"]) : 0;
			}
			return salida;
		}

		/// <summary>
		/// busca un determinado valor comprendido entre los campos contenido y detalle, aplicando un coeficiente (dividiendo) los campos 
		/// contenido y detalle y devuelve el campo indice dividido el coeficiente.
		/// Este metodo es explicito para ganancias. Habría que trabajarlo un poco mas.
		/// Se realiza acá el pasaje de coeficientes para no aumentar la complejidad en 
		/// las formulas.
		/// </summary>
		/// <param name="tabla"></param>
		/// <param name="valor"></param>
		/// <returns></returns>
		public double buscarIndiceCoeficiente (string tabla, double valor)
		{
			//campo utilizado para enviar el mes de liquidacion al sp. (mes + 1 para ganancias)
			int mm;
			if (mesLiquidacion () < 12)
				mm = mesLiquidacion () + 1;
			else
				mm = 1;

			double salida;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("tablasBuscarIndiceCoeficiente", "@tabla", tabla, "@valor", valor, "@coeficiente", 12, "@mesLiquidacion", mm)) {
				salida = rs.Read () ? Convert.ToDouble (rs ["indice"]) : 0;
			}
			return salida;
		}

		/// <summary>
		/// consulta acumulados entre rango de anios mes
		/// </summary>
		/// <returns></returns>
		public double acumuladosEntreRangos (int anioMesDesde, int anioMesHasta, int codigo)
		{
			double salida;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("acumuladosConsultarEntreRangos", "@anioMesDesde", anioMesDesde, "@anioMesHasta", anioMesHasta, "@legajo", Calculo.Legajo, "@codigo", codigo)) {
				salida = rs.Read () ? Convert.ToDouble (rs ["valor"]) : 0;
			}
			return salida;
		}

		/// <summary>
		/// consulta acumulados del ultimo anio desde mes actual - 1
		/// </summary>
		/// <param name = "codigo"></param>
		/// <returns></returns>
		public double acumuladosUltimoAnio (int codigo)
		{
			double salida;
			int anioMesDesde;
			int anioMesHasta;
			//armo aniomesdesde y aniomeshasta
			if (mesLiquidacion () == 1) {
				anioMesDesde = Int32.Parse (ArmaAnioMes (anioLiquidacion () - 1, 1));
				anioMesHasta = Int32.Parse (ArmaAnioMes (anioLiquidacion () - 1, 12));
			} else {
				anioMesDesde = Int32.Parse (ArmaAnioMes (anioLiquidacion () - 1, byte.Parse ((mesLiquidacion ()).ToString ())));
				anioMesHasta = Int32.Parse (ArmaAnioMes (anioLiquidacion (), byte.Parse ((mesLiquidacion () - 1).ToString ())));
			}
			//consulto
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("acumuladosConsultarEntreRangos", "@anioMesDesde", anioMesDesde, "@anioMesHasta", anioMesHasta, "@legajo", Calculo.Legajo, "@codigo", codigo)) {
				salida = rs.Read () ? Convert.ToDouble (rs ["valor"]) : 0;
			}
			return salida;
		}

		/// <summary>
		/// Sigue tablas de calculo.
		/// </summary>
		/// <param name="idT"></param>
		/// <param name = "idTipoLiquidacion"></param>
		/// <param name = "reciboSeparado"></param>
		public static void sigueCalculo (int idT, int idTipoLiquidacion, Boolean reciboSeparado)
		{
			acu = new double[61];
			var = new double[61];
			//  inicializaACUs();
			//  inicializaVARs();
			Calculo.idTipoLiquidacion = idTipoLiquidacion;
			Console.WriteLine ("Tabla: {0} idTipoLiquidacion: {1} reciboSeparado: {2}", idT, idTipoLiquidacion, reciboSeparado);
			if (Calculo.AnioMes == 0 || Calculo.Legajo == 0)
				throw new Exception ("No se puede seguir el calculo, no se ha seteado AnioMes o Legajo.");

			IdTabla = idT;      //este valor viene del tablas: calculo
			//HACK Elegir que version de Eval usar
			var ev = new Eval4.CSharpEvaluator ();
			//var ev = new Eval3.Evaluator (Eval3.eParserSyntax.cSharp, false);
			ev.AddEnvironmentFunctions (new Calculo ());
			Salto = 0;
			double salida = 0;    //contiene el id de la linea a saltar
			DbDataReader rs = DB.Instancia.SPToDbDataReader ("calculoConsultarParaLiquidar", "@idCalculo", IdTabla, "@OrdenProceso", 0, "@idAplicacion", IdAplicacion, "@idTipoLiquidacion", idTipoLiquidacion, "@reciboSeparado", reciboSeparado, "@idLiquidacion", IdLiquidacion);
			while (rs.Read ()) {
				//if (Convert.ToInt32(rs["OrdPro"]) == 91)
				Console.WriteLine ("Concepto: " + rs ["OrdenProceso"]);
				if (Salto == 0) {
					try {
						salida = Convert.ToDouble (ev.Parse (rs ["Formula"].ToString ()).ObjectValue);
						Console.WriteLine ("salida: " + salida);
					} catch (Exception ex) {
						throw new Exception ("OrdenProceso: " + Convert.ToInt32 (rs ["OrdenProceso"].ToString ()) + " " + (ex.Message + "\r\n"));
					}

					if (Convert.ToInt32 (rs ["ImprimeCantidad"]) == 1 && var [58] > 0) {
						Console.WriteLine ("var[58]: " + var [58]);
						imprimeCantidad = var [58];
						var [58] = 0;
					} else
						imprimeCantidad = 0;
					if (Convert.ToInt32 (rs ["ImprimeVU"]) == 1 && var [59] > 0) {
						Console.WriteLine ("var[59]: " + var [59]);
						imprimeValorUnitario = var [59];
						var [59] = 0;
					} else
						imprimeValorUnitario = 0;
                    
					if (Convert.ToInt32 (rs ["Imprime"]) == 1 && salida != 0) {
						imprime = salida;
						DB.Instancia.SP ("liquidacionesInsertar", "@idLiquidacion", Calculo.IdLiquidacion, "@idTipoLiquidacion", idTipoLiquidacion, "@Legajo", Calculo.Legajo, "@Codigo", Convert.ToInt32 (rs ["Codigo"]), "@AnioMes", Calculo.AnioMes, "@idCalculo", IdTabla, "idAplicacion", Calculo.IdAplicacion, "@Posicion", Convert.ToInt32 (rs ["Tipo"]), "@Cantidad", imprimeCantidad, "@VU", imprimeValorUnitario, "@Importe", imprime, "@Letra", "", "@OrdenProceso", Convert.ToInt32 (rs ["OrdenProceso"]));
						imprimeCantidad = 0;
						imprimeValorUnitario = 0;
					}
				} else {
					if (Salto > 0) {
						Console.WriteLine ("salto: " + Salto);
						rs = DB.Instancia.SPToDbDataReader ("calculoConsultarParaLiquidar", "@idCalculo", IdTabla, "@OrdenProceso", Salto, "@idAplicacion", IdAplicacion, "@idTipoLiquidacion", idTipoLiquidacion, "@reciboSeparado", reciboSeparado, "@idLiquidacion", IdLiquidacion); 
						Salto = 0;
					} else {
						//error
					}
				}
			}
			rs = null;
		}

		public int saltar (int codigo)
		{
			Salto = codigo;
			return 0;
		}

		public int seguir ()
		{
			return 0;
		}

		public double Redondear (double num, int decimales)
		{
			return Math.Round (num, decimales);
		}

		/// <summary>
		/// Obtiene el valor del campo del maestro del empleado a
		/// partir del codigo de campo
		/// </summary>
		/// <param name="codigo"></param>
		/// <returns></returns>
		public double campoEmpleado (int codigo)
		{
			string cad;
			cad = (string)DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "@legajo", Calculo.Legajo, "@codigo", codigo);
			return cad != null ? Convert.ToDouble (cad) : 0;
		}

		public double actualizarCampoEmpleado (int codigo, double valor)
		{
			DB.Instancia.SP ("empleadosSueldosActualizar", "@legajo", Calculo.Legajo, "@codigo", codigo, "@valor", valor);
			return valor;
		}

		/*   public double actualizarAsiento(int cuenta, double debe, double haber)
        {
            DB.ejecutarProceso(TipoComando.SP, "asientosDeSueldosActualizar", "@idLiquidacion",Calculo.IdLiquidacion , "@anioMes",Calculo.AnioMes, "@legajo", Calculo.Legajo, "@idCuenta", cuenta, "@debe", debe, "@haber", haber);
            return 0;
        }*/

		public double actualizarDebeAsiento (int cuenta, double debe)
		{
			if (cuenta == 935 && Calculo.Legajo == 459)
				Console.WriteLine ("estoy");
			if (double.IsNaN (debe)) //para los casos en que venga un NaN o infinito
                debe = 0;
			DB.Instancia.SP ("asientosDeSueldosActualizar", "@idLiquidacion", Calculo.IdLiquidacion, "@anioMes", Calculo.AnioMes, "@legajo", Calculo.Legajo, "@idCuenta", cuenta, "@debe", debe, "@haber", 0);
			return debe;
		}

		public double actualizarHaberAsiento (int cuenta, double haber)
		{
			if (double.IsNaN (haber)) //para los casos en que venga un NaN o infinito
                haber = 0;
			DB.Instancia.SP ("asientosDeSueldosActualizar", "@idLiquidacion", Calculo.IdLiquidacion, "@anioMes", Calculo.AnioMes, "@legajo", Calculo.Legajo, "@idCuenta", cuenta, "@debe", 0, "@haber", haber);
			return haber;
		}

		public double novedadEmpleado (int codigo)
		{
			string cad;
			cad = (string)DB.Instancia.SPToScalar ("novedadesConsultarValorLegajo", "@idLiquidacion", Calculo.IdLiquidacion, "@legajo", Calculo.Legajo, "@codigo", codigo);
			return cad != null ? Convert.ToDouble (cad) : 0;
		}

		public double asistenciaEmpleado (int codigo)
		{
			string cad;
			cad = DB.Instancia.SPToScalar ("asistenciaConsultarLegajoCodigo", "@AnioMes", Calculo.AnioMes, "@legajo", Calculo.Legajo, "@codigo", codigo).ToString ();
			return cad != "" ? Convert.ToDouble (cad) : 0;
		}

		/// <summary>
		/// Consulta la cantidad de dias de asistencia para un empleado entre fechas para un codigo especifico. 
		/// Por ej. asistenciaConsultarLegajoEntreDias(200801,151,1,15)*/
		/// </summary>
		/// <param name="codigo"></param>
		/// <param name="desde"></param>
		/// <param name="hasta"></param>
		/// <returns></returns>
		public double asistenciaEmpleadoEntreDias (int codigo, int desde, int hasta)
		{
			double salida;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("asistenciaConsultarLegajoEntreDias", "@AnioMes", Calculo.AnioMes, "@legajo", Calculo.Legajo, "@codigo", codigo, "@diaDesde", desde, "@diaHasta", hasta)) {
				salida = rs.Read () ? Convert.ToDouble (rs ["cantidad"]) : 0;
			}
			return salida;
		}

		/// <summary>
		/// Calcula la suma total de haberes dado un idLiquidacion
		/// </summary>
		/// <returns></returns>
		public double totalHaberes ()
		{
			double salida;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("liquidacionesConsultarTotalPosicion", "@idLiquidacion", Calculo.IdLiquidacion, "@idTipoLiquidacion", Calculo.idTipoLiquidacion, "@legajo", Calculo.Legajo, "@posicion", 1)) {
				salida = rs.Read () ? Convert.ToDouble (rs ["total"]) : 0;
			}
			return salida;
		}

		public double totalHaberesMesPorTipo (int tipo)
		{
			double salida;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("liquidacionesConsultarTotalPosicionTipoMes", "@anioMes", Calculo.AnioMes, "@idTipoLiquidacion", tipo, "@legajo", Calculo.Legajo, "@posicion", 1, "@idAplicacion", Calculo.IdAplicacion)) {
				salida = rs.Read () ? Convert.ToDouble (rs ["importe"]) : 0;
			}
			return salida;
		}

		public double totalAdicionalesMesPorTipo (int tipo)
		{
			double salida;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("liquidacionesConsultarTotalPosicionTipoMes", "@anioMes", Calculo.AnioMes, "@idTipoLiquidacion", tipo, "@legajo", Calculo.Legajo, "@posicion", 3, "@idAplicacion", Calculo.IdAplicacion)) {
				salida = rs.Read () ? Convert.ToDouble (rs ["importe"]) : 0;
			}
			return salida;
		}

		public double totalRetencionesMesPorTipo (int tipo)
		{
			double salida;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("liquidacionesConsultarTotalPosicionTipoMes", "@anioMes", Calculo.AnioMes, "@idTipoLiquidacion", tipo, "@legajo", Calculo.Legajo, "@posicion", 2, "@idAplicacion", Calculo.IdAplicacion)) {
				salida = rs.Read () ? Convert.ToDouble (rs ["importe"]) : 0;
			}
			return salida;
		}

		/// <summary>
		/// Calcula la suma total de haberes dado un AnioMes de Liquidacion
		/// </summary>
		/// <returns></returns>
		public double totalHaberesMes ()
		{
			double salida;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("liquidacionesConsultarTotalPosicionAnioMes", "@anioMes", Calculo.AnioMes, "@legajo", Calculo.Legajo, "@posicion", 1)) {
				salida = rs.Read () ? Convert.ToDouble (rs ["total"]) : 0;
			}
			return salida;
		}



		public double totalAdicionales ()
		{
			double salida;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("liquidacionesConsultarTotalPosicion", "@idLiquidacion", Calculo.IdLiquidacion, "@idTipoLiquidacion", Calculo.idTipoLiquidacion, "@legajo", Calculo.Legajo, "@posicion", 3)) {
				salida = rs.Read () ? Convert.ToDouble (rs ["total"]) : 0;
			}
			return salida;
		}


		public double totalAdicionalesMes ()
		{
			double salida;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("liquidacionesConsultarTotalPosicionAnioMes", "@anioMes", Calculo.AnioMes, "@legajo", Calculo.Legajo, "@posicion", 3)) {
				salida = rs.Read () ? Convert.ToDouble (rs ["total"]) : 0;
			}
			return salida;
		}


		public double totalRetenciones ()
		{
			double salida;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("liquidacionesConsultarTotalPosicion", "@idLiquidacion", Calculo.IdLiquidacion, "@idTipoLiquidacion", Calculo.idTipoLiquidacion, "@legajo", Calculo.Legajo, "@posicion", 2)) {
				salida = rs.Read () ? Convert.ToDouble (rs ["total"]) : 0;
			}
			return salida;
		}

		public double totalRetencionesMes ()
		{
			double salida;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("liquidacionesConsultarTotalPosicionAnioMes", "@anioMes", Calculo.AnioMes, "@legajo", Calculo.Legajo, "@posicion", 2)) {
				salida = rs.Read () ? Convert.ToDouble (rs ["total"]) : 0;
			}
			return salida;
		}

		/// <summary>
		/// consulta la existencia de un codigo de situacion de revista
		/// en la asistencia pasandole el numero de ocurrencia a consultar
		/// es decir, si es la 1ra, la 2da, etc. 
		/// devuelve el codigo de asistencia de dicha ocurrencia
		/// </summary>
		/// <param name="ocurrencia"></param>
		/// <returns></returns>
		public double codigoSituacionDeRevista (int ocurrencia)
		{
			double salida = 0;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("asistenciaConsultarOcurrenciaSituacionDeRevista", "@legajo", Calculo.Legajo, "@anioMes", Calculo.AnioMes, "@ocurrencia", ocurrencia)) {
				if (rs.Read ()) //este recorrido se supone realizarse siempre sobre 3 o cuatro registros
                salida = Convert.ToDouble (rs ["codigo"]);
			}
			return salida;
		}

		/// <summary>
		/// consulta el dia de inicio de un codigo de situacion de revista
		/// en la asistencia pasandole el numero de ocurrencia a consultar
		/// es decir, si es la 1ra, la 2da, etc. 
		/// devuelve el codigo de asistencia de dicha ocurrencia
		/// </summary>
		/// <param name="ocurrencia"></param>
		/// <returns></returns>
		public double diaInicioSituacionDeRevista (int ocurrencia)
		{
			double salida = 0;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("asistenciaConsultarOcurrenciaSituacionDeRevista", "@legajo", Calculo.Legajo, "@anioMes", Calculo.AnioMes, "@ocurrencia", ocurrencia)) {
				if (rs.Read ()) //este recorrido se supone realizarse siempre sobre 3 o cuatro registros
                salida = Convert.ToDouble (rs ["DiaInicio"]);
			}
			return salida;
		}

		/// <summary>
		/// Consulta el saldo total del debe de una cuenta asociada a un concepto
		/// </summary>
		/// <param name="idCuenta"></param>
		/// <returns></returns>
		public double consultarDebeConceptoLiquidado (int idCuenta)
		{
			double salida = 0;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("asientoDeSueldosConsultarSaldoCuenta", "@anioMes", Calculo.AnioMes, "@legajo", Calculo.Legajo, "@idCuenta", idCuenta)) {
				if (rs.Read ()) //este recorrido se supone realizarse siempre sobre 3 o cuatro registros
                salida = Convert.ToDouble (rs ["debe"]);
			}
			return salida;
		}

		public double consultarHaberConceptoLiquidado (int idCuenta)
		{
			double salida = 0;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("asientoDeSueldosConsultarSaldoCuenta", "@anioMes", Calculo.AnioMes, "@legajo", Calculo.Legajo, "@idCuenta", idCuenta)) {
				if (rs.Read ()) //este recorrido se supone realizarse siempre sobre 3 o cuatro registros
                salida = Convert.ToDouble (rs ["haber"]);
			}
			return salida;
		}

		/// <summary>
		/// calcula la antiguedad del empleado en días, en base a los períodos trabajados
		/// detallados en dicha tabla
		/// </summary>
		/// <returns></returns>
		public double consultarDiasAntiguedad (DateTime fechaTope)
		{
			double salida = 0;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("periodosTrabajadosCalculaAntiguedad", "@fechaTope", fechaTope, "@legajo", Calculo.Legajo)) {
				if (rs.Read ())
					salida = Convert.ToDouble (rs ["totalDias"]);
			}
			return salida;
		}

		/// <summary>
		/// consulta temporadas trabajadas de acuerdo a periodos trabajados completos
		/// </summary>
		/// <returns></returns>
		public double temporadasTrabajadas ()
		{
			double salida = 0;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("periodosTrabajadosConsultaTemporadas", "@legajo", Calculo.Legajo)) {
				if (rs.Read ())
					salida = Convert.ToDouble (rs ["periodos"]);
			}
			return salida;
		}

		public double empresaConsultarAlicuotaLRT ()
		{
			double salida = 0;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("empresaConsultar", "@idEmpresa", 1)) {
				if (rs.Read ())
					salida = Convert.ToDouble (rs ["porcentajeAlicuotaLRT"]);
			}
			return salida;
		}

		public double empresaConsultarCuotaLRT ()
		{
			double salida = 0;
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("empresaConsultar", "@idEmpresa", 1)) {
				if (rs.Read ())
					salida = Convert.ToDouble (rs ["cuotaFijaLRT"]);
			}
			return salida;
		}

		/// <summary>
		/// devuelve la parte entera de un numero
		/// </summary>
		/// <param name="numero"></param>
		/// <returns></returns>
		public int entero (double numero)
		{
			return (int)numero;
		}

		/// <summary>
		/// Devuelve el total de días que trae un mes.
		/// </summary>
		/// <returns></returns>
		public int diasDelMes ()
		{
			var d = new DateTime (AnioCalculo, MesCalculo, DateTime.DaysInMonth (AnioCalculo, MesCalculo));
			return d.Day;
		}

		public int mesLiquidacion ()
		{
			return MesCalculo;
		}

		public int anioLiquidacion ()
		{
			return AnioCalculo;
		}

		public int anioMesLiquidacion ()
		{
			return Convert.ToInt32 (Calculo.AnioMes.ToString ());
		}

		/// <summary>
		/// Arma la cadena AnioMes, a partir de un año y mes dados.
		/// formato aaaamm
		/// </summary>
		/// <returns></returns>
		public string ArmaAnioMes (int anio, byte mes)
		{
			string strMes;
			if (mes > 9)
				strMes = mes.ToString ();
			else
				strMes = 0 + mes.ToString ();
			return anio + strMes;
		}

		public int legajoEmpleado ()
		{
			return Calculo.Legajo;
		}

		/// <summary>
		/// Devuelve la fecha correspondiente al ultimo dia del mes.
		/// </summary>
		/// <returns></returns>
		public DateTime fechaUltimoDiaDelMes ()
		{
			var dd = new DateTime (AnioCalculo, MesCalculo, DateTime.DaysInMonth (AnioCalculo, MesCalculo));
			return dd;
		}


		public DateTime fechaDia15DelMes ()
		{
			var dd = new DateTime (AnioCalculo, MesCalculo, 15);
			return dd;
		}

		/// <summary>
		/// retorna el valor almacenado en un campo con formato fecha del empleado.
		/// </summary>
		/// <param name="codigo"></param>
		/// <returns></returns>
		public DateTime fechaEmpleado (int codigo)
		{
			DateTime salida;
			salida = Convert.ToDateTime (DB.Instancia.SPToScalar ("empleadosSueldosConsultarValorLegajo", "@legajo", Calculo.Legajo, "@codigo", codigo).ToString ());
			return salida;
		}

		/// <summary>
		/// Devuelve fecha Nula
		/// </summary>
		/// <returns></returns>
		public DateTime fechaNula ()
		{
			return Convert.ToDateTime ("01/01/0001");
		}

		/// <summary>
		/// Devuelve =0 si es igual 
		/// >0 si fecha1 > fecha2 
		/// <0 si fecha1 < fecha2
		/// </summary>
		/// <param name="fecha1"></param>
		/// <param name="fecha2"></param>
		/// <returns></returns>
		public int comparaFecha (DateTime fecha1, DateTime fecha2)
		{
			return fecha1.CompareTo (fecha2);
		}

		public object Muevo (object expr, int campo)
		{
			return "Muevo " + expr + " a " + campo;
		}

		public DateTime fechaHoy ()
		{
			return DateTime.Now.Date;
		}

		/// <summary>
		/// Devuelve la cantidad de Años entre dos fechas.
		/// </summary>
		/// <param name="fDesde"></param>
		/// <param name="fHasta"></param>
		/// <returns></returns>
		public long Anios (DateTime fDesde, DateTime fHasta)
		{
			long i;
			i = DateDiff (DateInterval.Year, fDesde, fHasta);
			return i;
		}

		/// <summary>
		/// Devuelve el año de una fecha.
		/// </summary>
		/// <returns></returns>
		public int Anio (DateTime fecha)
		{
			return fecha.Year;
		}

		/// <summary>
		/// Devuelve el mes de una fecha.
		/// </summary>
		/// <returns></returns>
		public int Mes (DateTime fecha)
		{
			return fecha.Month;
		}

		/// <summary>
		/// Devuelve el dia de una fecha.
		/// </summary>
		/// <returns></returns>
		public int Dia (DateTime fecha)
		{
			return fecha.Day;
		}

		public long diasEntreFechas (DateTime fDesde, DateTime fHasta)
		{
			long i;
			i = DateDiff (DateInterval.Day, fDesde, fHasta);
			return i;
		}

		public DateTime inicioSemestre ()
		{
			DateTime fecha;
			int mesInicio = MesCalculo <= 6 ? 1 : 7;
			fecha = new DateTime (AnioCalculo, mesInicio, 1);
			return fecha;
		}

		public DateTime finSemestre ()
		{
			DateTime fecha;
			int mesFin = MesCalculo <= 6 ? 6 : 12;
			fecha = new DateTime (AnioCalculo, mesFin, 30);
			return fecha;
		}

		public object SI (bool cond, object ValorVerdadero, object ValorFalso)
		{
			return cond ? ValorVerdadero : ValorFalso;
		}

		public enum DateInterval
		{
			Second,
			Minute,
			Hour,
			Day,
			Week,
			Month,
			Quarter,
			Year
		}

		public long DateDiff (DateInterval Interval, DateTime StartDate, DateTime EndDate)
		{
			long lngDateDiffValue = 0;
			var TS = new TimeSpan (EndDate.Ticks - StartDate.Ticks);
			switch (Interval) {
			case DateInterval.Day:
				lngDateDiffValue = (long)TS.Days;
				break;
			case DateInterval.Hour:
				lngDateDiffValue = (long)TS.TotalHours;
				break;
			case DateInterval.Minute:
				lngDateDiffValue = (long)TS.TotalMinutes;
				break;
			case DateInterval.Month:
				lngDateDiffValue = (long)(TS.Days / 30.4375);
				break;
			case DateInterval.Quarter:
				lngDateDiffValue = (long)((TS.Days / 30) / 3);
				break;
			case DateInterval.Second:
				lngDateDiffValue = (long)TS.TotalSeconds;
				break;
			case DateInterval.Week:
				lngDateDiffValue = (long)(TS.Days / 7);
				break;
			case DateInterval.Year:
				lngDateDiffValue = (long)(TS.Days / 365.25);
				break;
			}
			return (lngDateDiffValue);
		}
	}
}
