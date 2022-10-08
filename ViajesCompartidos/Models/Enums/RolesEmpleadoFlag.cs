using System;

namespace SistemaViajesCompartidos.Models
{
    [Flags]
    public enum RolesFlag
    {
        EMPLEADO = 0,
        RRHH = 1,
        CONDUCTOR = 2,
        ADMINISTRADOR = 64
    }
}