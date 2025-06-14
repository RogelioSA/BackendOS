﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nest.core.dominio.General
{
    public class Provincia
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int DepartamentoId { get; set; }

        public Departamento Departamento { get; set; }
        public List<Distrito> Distritos { get; set; }
    }
}
