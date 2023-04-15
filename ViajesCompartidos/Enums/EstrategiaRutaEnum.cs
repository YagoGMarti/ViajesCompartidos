namespace SistemaViajesCompartidos.Enums
{
    public enum EstrategiaRutaEnum
    {
        Estandar = 1, // pasajeros más lejanos a la sucursal.
        SoloCercanosDomicilio = 2, // busca en un perímetro cercano al punto inicial sólamente.
        SinConductores = 3,  // ignora otros pasajeros con vehículo.
        SoloMasCercano = 4,  // solo el pasajero mas cerca a la sucursal
    }
}