﻿/*  
    LiquidacionPorTipoEntity.cs

    Author:
    Hernan Vivani <hernan@vivani.com.ar> - http://hvivani.com.ar

    Copyright © SOffT 2010 - http://www.sofft.com.ar

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

namespace Sueldos.Entidades
{
    public class LiquidacionPorTipoEntity
    {
        public int IdLiquidacion { get; set; }
        public int IdTipoLiquidacion { get; set; }
        public Boolean Seleccionado { get; set; }

        public LiquidacionPorTipoEntity(){}

        public LiquidacionPorTipoEntity(int idliquidacion, int idtipoliquidacion)
        {
            this.IdLiquidacion = idliquidacion;
            this.IdTipoLiquidacion = idtipoliquidacion;
            this.Seleccionado = false;
        }


    }
}
