using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace SistemaViajesCompartidos.Models.Temporal
{
    public class ReporteSucursalModel
    {
        public Guid RecorridoID { get; set; }

        public string Rol { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string Dirección { get; set; }
        [DisplayName("Vigencia")]
        public string Vencimiento { get; set; }

        public List<ReporteSucursalModel> Pasajeros { get; set; } = new List<ReporteSucursalModel>();
    }
}