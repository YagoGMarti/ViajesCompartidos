using System;

namespace SistemaViajesCompartidos.Enums
{
    [Flags]
    public enum RolesEmpleadoFlag
    {
        EMPLEADO = 0,
        RRHH = 1,
        CONDUCTOR = 2,
        //CORREOINSTITUCIONAL = 32,
        ADMINISTRADOR = 64
    }
}