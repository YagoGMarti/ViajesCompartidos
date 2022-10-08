using System;

namespace SistemaViajesCompartidos.Models
{
    [Flags]
    public enum DiasTrabajoFlag
    {
        LUNES = 1,
        MARTES = 2,
        MIERCOLES = 4,
        JUEVES = 8,
        VIERNES = 16,
        SABADO = 32,
        DOMINGO = 64
    }
}