using System;

namespace SistemaViajesCompartidos.Models
{
    public class JornadaTrabajoModel
    {
        public DiasTrabajoFlag DiasTrabajo { get; set; }
        public HorarioTrabajoEnum HorarioIngreso { get; set; }
        public HorarioTrabajoEnum HorarioSalida { get; set; }
    }
}