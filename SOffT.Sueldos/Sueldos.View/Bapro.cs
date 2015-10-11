/*  
    Bapro.cs

    Author:
    Hernan Vivani <hernan@vivani.com.ar> - http://hvivani.com.ar

    Copyright � SOffT 2010 - http://www.sofft.com.ar

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.Common;
using System.Windows.Forms;
using Sofft.Utils;

namespace Sueldos.View
{
    /// <summary>
    /// Clase que define estructura de archivos para acreditacion de sueldos en cuentas
    /// </summary>
    class Bapro
    {
        /*                 XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX                     
                     DISE�O DE ARCHIVO MAGNETICO PARA PAGO DE HABERES               
                                                                                
         SOPORTE:                                                                   
         -------                                                                    
                  - Diskette de 3 1/2 pulgadas.                                     
                                                                                
                  IMPORTANTE: "Deber� emitir un archivo por cada tipo de            
                               moneda o letra, en  soportes  magn�ticos             
                               separados".                                          
                                                                                
         LONGITUD DEL REGISTRO:                                                     
         ---------------------                                                      
                  - 80 caracteres.                                                  

         ROTULO DEL ARCHIVO:                                                        
         ------------------                                                         
                  - IDHXXX (emitir sin la extensi�n TXT)                            
                                                                                
         NIVEL DE REGISTROS:                                                        
         ------------------                                                         
                                                                                
                  - I - Registro inicial.                                           
                  - II - Registro de movimientos.                                   
                  - III - Registro final.                                           
                                                                /.                  
                                                                  - 2 -             
                                                                                
                                 DETALLE DE LOS REGISTROS                           
                                 ------------------------                           
                                                                                
                                   I - REGISTRO INICIAL                             
                                   --------------------                             
                                                                                
         ����������������������������������������������������������������������ͻ   
         �               �              �                                       �   
         �   NRO. CAMPO  �  POSICION/ES �         C  O  N  C  E  P  T  O        �   
         ����������������������������������������������������������������������͹   
         �               �              �                                       �   
         �               �              �                                       �   
         �               �              �                                       �   
         �       01      �    01 a 10   � CEROS (indicativo comienzo de archivo)�   
         ����������������������������������������������������������������������Ķ   
         �               �              �                                       �   
         �       02      �    11 a 16   � ROTULO DEL ARCHIVO:    XXXXX          �   
         ����������������������������������������������������������������������Ķ   
         �               �              �                                       �   
         �       03      �    17 a 17   � NUMERO DE CPD:  X                     �   
         ����������������������������������������������������������������������Ķ   
         �               �              �                                       �   
         �       04      �    18 a 23   � FECHA DE EMISION DEL SOPORTE MAGNETICO�   
         �               �              � FORMATO: DDMMAA                       �   
         ����������������������������������������������������������������������Ķ   
         �               �              �                                       �   
         �       05      �    24 a 27   � HORA DE EMISION DEL SOPORTE MAGNETICO �   
         �               �              � FORMATO: HHMM                         �   
         ����������������������������������������������������������������������Ķ   
         �               �              �                                       �   
         �       06      �    28 a 30   � NUMERO DE BANCO:  014                 �   
         ����������������������������������������������������������������������Ķ   
         �               �              �                                       �   
         �       07      �    31 a 79   � FILLER (BLANCOS)                      �   
         ����������������������������������������������������������������������Ķ   
         �               �              �                                       �   
         �       08      �    80 a 80   � MARCA:  0                             �   
         ����������������������������������������������������������������������ͼ   
                                                                                
                                                                /.                  
                                                                  - 3 -             
                                                                                
                               II - REGISTRO DE MOVIMIENTOS                         
                               ----------------------------                         
                                                                                
         ����������������������������������������������������������������������ͻ   
         �               �              �                                       �   
         �   NRO. CAMPO  �  POSICION/ES �         C  O  N  C  E  P  T  O        �   
         ����������������������������������������������������������������������͹   
         �               �              �                                       �   
         �               �              �                                       �   
         �       01      �    01 a 04   � NUMERO DE CASA BENEFICIARIA: (sucursal�   
         �               �              � en donde  se deber� acreditar el      �   
         �               �              � dep�sito                              �   
         ����������������������������������������������������������������������Ķ   
         �       02      �    05 a 08   � NUMERO  DE  CASA  RECEPTORA: constante�   
         �               �              �  XXXX                                 �   
         ����������������������������������������������������������������������Ķ   
         �       03      �    09 a 12   � CODIGO  DE  OPERACION:                �   
         �               �              �  0082 : movimientos en Pesos          �   
         �               �              �  0101 : movimientos en Patacones S1   �   
         �               �              �  0102 : movimientos en Patacones S2   �   
         �               �              �  0103 : movimientos en Lecop S1       �   
         ����������������������������������������������������������������������Ķ   
         �       04      �    13 a 13   � INDICATIVO TIPO DE CUENTA:            �   
         �               �              �  1 : Si el c�digo de operaci�n es 0082�   
         �               �              �  5 : Si los c�digos de operaci�n son  �   
         �               �              �      101 � 102 � 103.                 �   
         ����������������������������������������������������������������������Ķ   
         �       05      �    14 a 19   � NUMERO DE CUENTA BENEFICIARIA         �   
         ����������������������������������������������������������������������Ķ   
         �       06      �    20 a 20   � DIGITO VERIFICADOR                    �   
         ����������������������������������������������������������������������Ķ   
         �       07      �    21 a 22   � FILLER NUMERICO (INFORMAR CEROS)      �   
         ����������������������������������������������������������������������Ķ   
         �       08      �    23 a 33   � IMPORTE (9 enteros y 2 decimales) (SIN�   
         �               �              � PUNTOS NI COMAS)                      �   
         ����������������������������������������������������������������������Ķ   
         �       09      �    34 a 41   � REFERENCIA NUMERICA (NRO. DE LEGAJO,  �   
         �               �              � AFILIADO, IDENTIFICADOR EMPLEADO, ETC)�   
         ����������������������������������������������������������������������Ķ   
         �       10      �    42 a 63   � TITULAR DE LA CUENTA BENEFICIARIA     �   
         ����������������������������������������������������������������������Ķ   
         �       11      �    64 a 79   � FILLER (BLANCOS)                      �   
         ����������������������������������������������������������������������Ķ   
         �       12      �    80 a 80   � MARCA:  0                             �   
         ����������������������������������������������������������������������ͼ   
                                                                                
                                                                /.                  
                                                                  - 4 -             
                                                                                
                                   III - REGISTRO FINAL                             
                                   --------------------                             
                                                                                
         ����������������������������������������������������������������������ͻ   
         �               �              �                                       �   
         �   NRO.CAMPO   �  POSICION/ES �         C  O  N  C  E  P  T  O        �   
         ����������������������������������������������������������������������͹   
         �               �              �                                       �   
         �               �              �                                       �   
         �               �              �                                       �   
         �       01      �    01 a 10   �  INDICATIVO FIN DE ARCHIVO  (consignar�   
         �               �              �  siempre nueves)                      �   
         ����������������������������������������������������������������������Ķ   
         �               �              �                                       �   
         �       02      �    11 a 16   �  CANTIDAD DE REGISTROS  DE MOVIMIENTOS�   
         �               �              �  GRABADOS (no incluir inicial y final)�   
         ����������������������������������������������������������������������Ķ   
         �               �              �                                       �   
         �       03      �    17 a 30   �  IMPORTE TOTAL  DE INTERDEPOSITOS  (12�   
         �               �              �  enteros y 2 decimales) (SIN PUNTOS NI�   
         �               �              �  COMAS)                               �   
         ����������������������������������������������������������������������Ķ   
         �               �              �                                       �   
         �       04      �    31 a 79   �  FILLER (BLANCOS)                     �   
         ����������������������������������������������������������������������Ķ   
         �               �              �                                       �   
         �       05      �    80 a 80   �  MARCA:  0                            �   
         ����������������������������������������������������������������������ͼ   
                                                                                
         LOS CAMPOS NRO. DE CASA TOMADORA, NRO. DE CASA BENEFICIARIA,  CODIGO  DE   
         OPERACION, NUMERO DE CUENTA  BENEFICIARIA  E  IMPORTE  DEL  REGISTRO  DE   
         MOVIMIENTOS, NO PODRAN SER INFORMADOS CON CEROS.  EN CASO CONTRARIO,       
         NUESTRO SISTEMA RECHAZARA EL ARCHIVO, SIENDO DEVUELTO SIN PROCESAR.        
         LOS CAMPOS NUMERICOS DEBEN CONTENER CEROS A LA IZQUIERDA HASTA COMPLETAR   
         EL CAMPO.                                                                  
                                                                /.                  
                                                                                
                                                                  - 5 -             
                                                                                
                                    LISTADO DE RESPALDO                             
                                    -------------------                             
                                                                                
            Este listado deber�  estar  ordenado  por  sucursal  beneficiaria  y por   
            n�mero de cuenta y estar� configurado de la siguiente manera:              
                                                                                
            - LINEA DE TITULOS:                                                        
            * Fecha de proceso.                                                      
            * T�tulo: "..........(nombre identificatorio del Organismo) ......."     
            * N�mero de hoja.                                                        
                                                                                
            - LINEA DE SUBTITULOS:                                                     
            * Subtitular: SERVICIO DE PAGO DE HABERES                                
                                                                                
            - LINEA DE DETALLE:                                                        
            * N�mero de sucursal beneficiaria.                                       
            * N�mero de cuenta y d�gito verificador.                                 
            * C�digo de operaci�n.                                                   
            * Importe.                                                               
            * Referencia.                                                            
            * Titular de la cuenta beneficiaria.                                     
                                                                                
            - LINEA DE TOTALES:                                                        
                                                                                
            * Total general de movimientos:                                          
             . Cantidad.                                                            
             . Importe.                                                             
                                                                                
            */

        public struct cabecera
        {
            public const string ceros = "0000000000";
            public const string rotuloArchivo = "IDS898";
            public const string numeroCPD = "0";
            public static string fechaEmision; //DDMMAA
            public static string horaEmision; //HHMM
            public const string numeroBanco = "014";
            public const string filler = "                                                 ";
            public const string marca = "0";
        }

        public struct movimiento
        {
            public static string numeroCasaBeneficiaria = "6102";
            public static string numeroCasaReceptora = "6102";
            /*0082 para sueldo del mes - 0096 para cualquier otra acreditacion (sac/vac/etc)*/
            public static string codigoDeOperacion;
            public const string indicativoTipoCuenta = "1";
            public static string numeroCuentaBeneficiaria;
            public static string digitoVerificador;
            public const string filler01 = "00";
            public static string importe;
            public static string legajo;
            public static string nombreTitular;
            public const string filler02 = "                ";
            public const string marca = "0";
        }

        public struct final
        {
            public const string finArchivo = "9999999999";
            //    public static string cantidadRegistrosMovimientos;
            //    public static string  importeTotalInterdepositos;
            public const string filler = "                                                 ";
            public const string marca = "0";
        }

        /// <summary>
        /// Genera archivo con formato bapro. 
        /// Si es liquidacion normal codigo operaci�n:0082
        /// Si es liquidacion vac o sac u otro codigo operaci�n:0096 
        /// </summary>
        /// <param name="idliquidacion"></param>
        /// <param name="archivo"></param>
        /// <param name="esLiquidacionNormal"></param>
        public static void generaArchivo(int idliquidacion, string archivo, List<int> tiposLiquidacionesSeleccionados, DateTime fechaAcreditacion, bool todosLosConveios, int idConvenio)
        {
            /*0082 para sueldo del mes - 0096 para cualquier otra acreditacion (sac/vac/etc)*/
            /*si el indice 0 es 1, se seleccion� la liquidacion normal.*/
            if (tiposLiquidacionesSeleccionados[0] == 1)
                movimiento.codigoDeOperacion = "0082";
            else
                movimiento.codigoDeOperacion = "0096";
            string cuenta = "";
            string importe = "";
            int cantRegistros = 0;
            double importeTotal = 0;
            StreamWriter sw = new StreamWriter(archivo);
            //grabar cabecera.
            cabecera.fechaEmision = System.DateTime.Now.Day.ToString().PadLeft(2, '0') + System.DateTime.Now.Month.ToString().PadLeft(2, '0') + Varios.Right(System.DateTime.Now.Year.ToString(), 2);
            cabecera.horaEmision = System.DateTime.Now.Hour.ToString().PadLeft(2, '0') + System.DateTime.Now.Minute.ToString().PadLeft(2, '0');
            sw.WriteLine(cabecera.ceros + cabecera.rotuloArchivo + cabecera.numeroCPD + cabecera.fechaEmision + cabecera.horaEmision + cabecera.numeroBanco + cabecera.filler + cabecera.marca);
            //recorrer netos por legajo
            //consultar netos por idliquidacion.
            //repoteLiquidacionesNetoPorLegajo. calcular haberes+adicionales-retenciones
            DbDataReader rsLegajos;
            if (todosLosConveios)
                rsLegajos = Model.DB.ejecutarDataReader(Model.TipoComando.SP, "liquidacionesNetosPorLegajoPorBanco", "@idLiquidacion", idliquidacion, "@idFormaDePago", 2, "@idBanco", 1);
            else
                rsLegajos = Model.DB.ejecutarDataReader(Model.TipoComando.SP, "liquidacionesNetosPorLegajoPorBancoPorConvenio", "@idLiquidacion", idliquidacion, "@idFormaDePago", 2, "@idBanco", 1, "@idConvenio", idConvenio);

            while (rsLegajos.Read())  //para cada legajo recorro todos los campos definidos en tablas
            {
                cantRegistros++;
                movimiento.legajo = rsLegajos["legajo"].ToString().PadLeft(8, '0');
                movimiento.nombreTitular = Varios.Left(rsLegajos["Apellidos y nombres"].ToString(), 22).PadRight(22, ' ');
                //importe = string.Format("{0:#0.00}", Convert.ToDouble(rsLegajos["Haberes"]) + Convert.ToDouble(rsLegajos["Adicionales"]) - Convert.ToDouble(rsLegajos["retenciones"]));
                importe = string.Format("{0:#0.00}", Convert.ToDouble(rsLegajos["Neto"]));
                importeTotal = importeTotal + Convert.ToDouble(importe);
                importe = importe.Replace(".", "");
                movimiento.importe = importe.PadLeft(11, '0');
                movimiento.numeroCasaBeneficiaria = Model.DB.ejecutarScalar(Model.TipoComando.SP, "empleadosSueldosConsultarValorLegajo", "legajo", Convert.ToInt32(rsLegajos["legajo"]), "codigo", 21).ToString();
                movimiento.numeroCasaReceptora = movimiento.numeroCasaBeneficiaria;
                cuenta = Model.DB.ejecutarScalar(Model.TipoComando.SP, "empleadosSueldosConsultarValorLegajo", "legajo", Convert.ToInt32(rsLegajos["legajo"]), "codigo", 22).ToString();
                if (cuenta.Length == 0 || movimiento.numeroCasaBeneficiaria == "0")
                    MessageBox.Show("ATENCI�N: el legajo " + movimiento.legajo + " no tiene cuenta o banco asignado. No se exportara la liquidaci�n.");
                else
                {
                    movimiento.numeroCuentaBeneficiaria = Varios.Left(cuenta, cuenta.Length - 1).PadLeft(6, '0');
                    movimiento.digitoVerificador = Varios.Right(Model.DB.ejecutarScalar(Model.TipoComando.SP, "empleadosSueldosConsultarValorLegajo", "legajo", Convert.ToInt32(rsLegajos["legajo"]), "codigo", 22).ToString(), 1);
                    sw.WriteLine(movimiento.numeroCasaBeneficiaria + movimiento.numeroCasaReceptora + movimiento.codigoDeOperacion + movimiento.indicativoTipoCuenta + movimiento.numeroCuentaBeneficiaria + movimiento.digitoVerificador + movimiento.filler01 + movimiento.importe + movimiento.legajo + movimiento.nombreTitular + movimiento.filler02 + movimiento.marca);
                    //actualiza liquidacionesEstados
                    for (int i = 0; i < tiposLiquidacionesSeleccionados.Count; i++)
                        Model.DB.ejecutarProceso(Model.TipoComando.SP, "liquidacionesEstadosActualizar", "@idLiquidacion", idliquidacion, "@idTipoLiquidacion", tiposLiquidacionesSeleccionados[i], "@legajo", Convert.ToInt32(movimiento.legajo), "@acreditada", true, "fechaAcreditacion", fechaAcreditacion);
                }
            }
            //grabar final.
            sw.WriteLine(final.finArchivo + cantRegistros.ToString().PadLeft(6, '0') + string.Format("{0:#0.00}", importeTotal).Replace(".", "").PadLeft(14, '0') + final.filler + final.marca);
            sw.Close();
        }

        public static void generaArchivoDesdeAnticipos(int anioMes, int idTipoAnticipo, string archivo, bool todosLosConveios, int idConvenio)
        {
            /*0082 para sueldo del mes - 0096 para cualquier otra acreditacion (sac/vac/etc)*/
            movimiento.codigoDeOperacion = "0096";
            string cuenta = "";
            string importe = "";
            int cantRegistros = 0;
            double importeTotal = 0;
            StreamWriter sw = new StreamWriter(archivo);
            //grabar cabecera.
            cabecera.fechaEmision = System.DateTime.Now.Day.ToString().PadLeft(2, '0') + System.DateTime.Now.Month.ToString().PadLeft(2, '0') + Varios.Right(System.DateTime.Now.Year.ToString(), 2);
            cabecera.horaEmision = System.DateTime.Now.Hour.ToString().PadLeft(2, '0') + System.DateTime.Now.Minute.ToString().PadLeft(2, '0');
            sw.WriteLine(cabecera.ceros + cabecera.rotuloArchivo + cabecera.numeroCPD + cabecera.fechaEmision + cabecera.horaEmision + cabecera.numeroBanco + cabecera.filler + cabecera.marca);
            //recorrer netos por legajo
            //consultar netos por idliquidacion.
            //repoteLiquidacionesNetoPorLegajo. calcular haberes+adicionales-retenciones
            DbDataReader rsLegajos;
            if (todosLosConveios)
                rsLegajos = Model.DB.ejecutarDataReader(Model.TipoComando.SP, "anticiposConsultarParaAcreditarPorBanco", "@anioMes", anioMes, "@idTipoAnticipo", idTipoAnticipo, "@idBanco", 1);
            else
                rsLegajos = Model.DB.ejecutarDataReader(Model.TipoComando.SP, "anticiposConsultarParaAcreditarPorBancoPorConvenio", "@anioMes", anioMes, "@idTipoAnticipo", idTipoAnticipo, "@idBanco", 1, "@idConvenio", idConvenio);

            while (rsLegajos.Read())  //para cada legajo recorro todos los campos definidos en tablas
            {
                cantRegistros++;
                movimiento.legajo = rsLegajos["Legajo"].ToString().PadLeft(8, '0');
                movimiento.nombreTitular = Varios.Left(rsLegajos["Apellidos y nombres"].ToString(), 22).PadRight(22, ' ');
                importe = string.Format("{0:#0.00}", Convert.ToDouble(rsLegajos["Neto"]));
                importeTotal = importeTotal + Convert.ToDouble(importe);
                importe = importe.Replace(".", "");
                movimiento.importe = importe.PadLeft(11, '0');
                movimiento.numeroCasaBeneficiaria = Model.DB.ejecutarScalar(Model.TipoComando.SP, "empleadosSueldosConsultarValorLegajo", "legajo", Convert.ToInt32(rsLegajos["Legajo"]), "codigo", 21).ToString();
                movimiento.numeroCasaReceptora = movimiento.numeroCasaBeneficiaria;
                cuenta = Model.DB.ejecutarScalar(Model.TipoComando.SP, "empleadosSueldosConsultarValorLegajo", "legajo", Convert.ToInt32(rsLegajos["Legajo"]), "codigo", 22).ToString();
                if (cuenta.Length == 0 || movimiento.numeroCasaBeneficiaria == "0")
                    MessageBox.Show("ATENCI�N: el legajo " + movimiento.legajo + " no tiene cuenta o banco asignado. No se exportara la liquidaci�n.");
                else
                {
                    movimiento.numeroCuentaBeneficiaria = Varios.Left(cuenta, cuenta.Length - 1).PadLeft(6, '0');
                    movimiento.digitoVerificador = Varios.Right(Model.DB.ejecutarScalar(Model.TipoComando.SP, "empleadosSueldosConsultarValorLegajo", "legajo", Convert.ToInt32(rsLegajos["Legajo"]), "codigo", 22).ToString(), 1);
                    sw.WriteLine(movimiento.numeroCasaBeneficiaria + movimiento.numeroCasaReceptora + movimiento.codigoDeOperacion + movimiento.indicativoTipoCuenta + movimiento.numeroCuentaBeneficiaria + movimiento.digitoVerificador + movimiento.filler01 + movimiento.importe + movimiento.legajo + movimiento.nombreTitular + movimiento.filler02 + movimiento.marca);
                }
            }
            //grabar final.
            sw.WriteLine(final.finArchivo + cantRegistros.ToString().PadLeft(6, '0') + string.Format("{0:#0.00}", importeTotal).Replace(".", "").PadLeft(14, '0') + final.filler + final.marca);
            sw.Close();
        }
    }
}
