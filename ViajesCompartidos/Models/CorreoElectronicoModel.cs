using SistemaViajesCompartidos.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;

namespace SistemaViajesCompartidos.Models
{
    [Table("CorreosElectronicos")]
    public class CorreoElectronicoModel
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public DateTime Fecha { get; set; } = DateTime.Now;
        public bool Enviado { get; set; } = false;
        public TipoCorreoEnum TipoCorreoEnum { get; set; }
        [NotMapped]
        public string CorreoElectronico { get; set; }
        [NotMapped]
        public string Asunto { get; set; }
        public string CorreoElectronicoEncriptado { get; set; }
        public string Mensaje { get; set; }

        public bool FalloEnvio { get; set; } = false;
        public string Excepcion { get; set; }
    }
}