namespace SistemaViajesCompartidos.Enums
{
    public enum TipoCorreoEnum
    {
        NuevaRuta, // Un conductor incluye al pasajero en las opciones de ruta. 
        RutaCancelada, // Conductor se da de baja a nivel ruta. 
        DesasociadoRuta,  // Conductor dá de baja a un empleado en particular.
        BajaRuta  // Empleado se desasocia de la ruta.
    }
}