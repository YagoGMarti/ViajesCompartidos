using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaViajesCompartidos.Models
{
    [Table("CorreoElectrinicoRespaldos")]
    public class CorreoElectrinicoRespaldoModel : BaseModel
    {
        public EmpleadoModel Destinatario { get; set; }
        public string Mensaje { get; set; }
    }
}