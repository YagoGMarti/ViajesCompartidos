namespace SistemaViajesCompartidos.Enums
{
    public enum EstrategiaRutaEnum
    {
        Estandar, // pasajeros más lejanos a la sucursal.
        SoloCercanosDomicilio, // busca en un perímetro cercano al punto inicial sólamente.
        SinConductores,  // ignora otros pasajeros con vehículo.
        SoloMasCercano,  // solo el pasajero mas cerca a la sucursal
    }
}