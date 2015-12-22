using System.Collections.Generic;
using Sueldos.Data;
using Sueldos.Entidades;

namespace Sueldos.Modelo
{
	public class ConsultaTablas
	{
		readonly TablaData persistencia = new TablaData ();

		public int insert (TablaEntity tabla)
		{
			return persistencia.insert (tabla);
		}

		public int update (TablaEntity tabla)
		{
			return persistencia.update (tabla);
		}

		public int delete (TablaEntity tabla)
		{
			return persistencia.delete (tabla);
		}

		/// <summary>
		/// Crea una tabla y carga datos de tabla preexistente.
		/// </summary>
		public TablaEntity getById (string nombre, int indice, double contenido)
		{
			TablaEntity tabla;
			tabla = persistencia.getById (nombre, indice, contenido);
			return tabla;
		}

		/// <summary>
		/// Obtiene la lista de tablas 
		/// </summary>
		/// <returns>Lista de tablas</returns>
		public List<TablaEntity> getContenidoYdetalle (string nombreTabla, int indice)
		{
			List<TablaEntity> tablas = persistencia.getByContenidoYdetalle (nombreTabla, indice);
			return tablas;
		}

		/// <summary>
		/// Obtiene los nombres de las tablas
		/// </summary>
		/// <returns></returns>
		public List<TablaEntity> getNombres ()
		{
			List<TablaEntity> tablas = persistencia.getNombresTablas ();
			return tablas;
		}

		/// <summary>
		/// Obtiene el contenido de una tabla por su nombre
		/// </summary>
		/// <returns></returns>
		public List<TablaEntity> getByNombre (string nombre)
		{
			List<TablaEntity> tablas = persistencia.getByNombre (nombre);
			return tablas;
		}

		/// <summary>
		/// Obtiene el contenido de una tabla por su nombre
		/// Debe especificarse campo/s de ordenacion
		/// </summary>
		/// <returns></returns>
		public List<TablaEntity> getByNombre (string nombre, string orden)
		{
			List<TablaEntity> tablas = persistencia.getByNombre (nombre, orden);
			return tablas;
		}

		/// <summary>
		/// Obtiene el contenido de una tabla por su nombre
		/// Se aceptan filtros
		/// Debe especificarse campo/s de ordenacion
		/// </summary>
		/// <returns></returns>
		public List<TablaEntity> getByNombre (string nombre, string filtro, string orden)
		{
			List<TablaEntity> tablas = persistencia.getByNombre (nombre, filtro, orden);
			return tablas;
		}
	}
}
