using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace SistemaViajesCompartidos.Models.Temporal
{
    public class ReporteAdminModel : ReporteBaseModel
    {
        public string Empresa { get; set; }
        public Guid EmpresaID { get; set; }
        
        public string Sucursales { get; set; }
    }
}