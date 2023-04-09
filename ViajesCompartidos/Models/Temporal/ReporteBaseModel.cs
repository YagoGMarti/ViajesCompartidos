using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace SistemaViajesCompartidos.Models.Temporal
{
    public abstract class ReporteBaseModel
    {
        public string Empleados { get; set; }
        public string Recorridos { get; set; }
        [DisplayName("Usuarios")]
        public string EmpleadosRecorrido { get; set; }
    }
}