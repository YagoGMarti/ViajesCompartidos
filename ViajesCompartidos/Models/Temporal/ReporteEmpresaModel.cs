using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace SistemaViajesCompartidos.Models.Temporal
{
    public class ReporteEmpresaModel : ReporteBaseModel
    {
        public string Sucursal { get; set; }
        public Guid SucursalID { get; set; }
        [DisplayName("Dirección")]
        public string SucursalDireccion { get; set; }
        public bool Activo { get; set; }
    }
}