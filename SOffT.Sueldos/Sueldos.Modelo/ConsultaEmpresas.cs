﻿/*  
    ConsultaEmpresas.cs

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

using Sueldos.Data;
using Sueldos.Entidades;


namespace Sueldos.Modelo
{
    public class ConsultaEmpresas
    {


        public ConsultaEmpresas() { }


        public int insert(EmpresaEntity empresa)
        {
            using (EmpresaData empresaData = new EmpresaData())
            {
                return empresaData.insert(empresa);
            }
        }

        public int update(EmpresaEntity empresa)
        {
            using (EmpresaData empresaData = new EmpresaData())
            {
                return empresaData.update(empresa);
            }
        }

        public int delete(EmpresaEntity empresa)
        {
            using (EmpresaData empresaData = new EmpresaData())
            {
                return empresaData.delete(empresa);
            }
        }

        public EmpresaEntity getById(int id)
        {
            using (EmpresaData empresaData = new EmpresaData())
            {
                return empresaData.getById(id);
            }
        }

        public List<EmpresaEntity> getAll()
        {
            using (EmpresaData empresaData = new EmpresaData())
            {
                return empresaData.GetAll();
            }
        }


    }
}