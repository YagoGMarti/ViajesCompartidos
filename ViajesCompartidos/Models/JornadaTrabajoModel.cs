using SistemaViajesCompartidos.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace SistemaViajesCompartidos.Models
{
    public class JornadaTrabajoModel
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        public DiasTrabajoFlag DiasTrabajo { get; set; }
        
        public HorarioTrabajoEnum HorarioIngreso { get; set; }
        
        public HorarioTrabajoEnum HorarioSalida { get; set; }
    }
}