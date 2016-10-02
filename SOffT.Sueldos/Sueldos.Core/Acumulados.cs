//
//  Acumulados.cs
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

using System.Collections;

namespace Sueldos.Core
{
	public class Acumulados : CollectionBase
	{
		public int Add(Acumulado item)
		{
			return List.Add(item);
		}

		public void Insert(int index, Acumulado item)
		{
			List.Insert(index, item);
		}

		public void Remove(Acumulado item)
		{
			List.Remove(item);
		}

		public bool Contains(Acumulado item)
		{
			return List.Contains(item);
		}

		public int IndexOf(Acumulado item)
		{
			return List.IndexOf(item);
		}

		public void CopyTo(Acumulado[] array, int index)
		{
			List.CopyTo(array, index);
		}

		public Acumulado this[int index]
		{
			get { return (Acumulado)List[index]; }
			set { List[index] = value; }
		}

		/// <summary>
		/// verifica si un codigo existe en la coleccion
		/// </summary>
		/// <param name="ce"></param>
		/// <returns></returns>
		public bool existeCodigo(Acumulado ce)
		{
			bool salida = false;
			foreach (Acumulado c in this)
			{
				salida = c.Codigo == ce.Codigo;
			}
			return salida;
		}
	}
}
