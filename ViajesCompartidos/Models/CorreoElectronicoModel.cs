using SistemaViajesCompartidos.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string Destinatario { get; set; }
        public string Asunto { get; set; }
        public string CorreoElectronicoEncriptado { get; set; }
        public string Mensaje { get; set; }
        
        [NotMapped]
        public string MensajeFlattened => Mensaje?.ToString();

        public bool FalloEnvio { get; set; } = false;
        public string Excepcion { get; set; }
    }
}