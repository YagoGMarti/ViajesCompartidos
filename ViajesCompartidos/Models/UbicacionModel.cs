using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaViajesCompartidos.Models
{
    [Table("Ubicaciones")]
    public class UbicacionModel : BaseModel
    {
        public double Latitud { get; set; }
        public double Longitud { get; set; }

        public string UbicacionTexto { get; set; }

        public byte[] MapaDescargado { get; set; }
        public string TipoImagenMapa { get; set; }

        public double CalcularDistanciaEntrePuntos(UbicacionModel otraUbicacion) { return Latitud + Longitud; }
    }
}