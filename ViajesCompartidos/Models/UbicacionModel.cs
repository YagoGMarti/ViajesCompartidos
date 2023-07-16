using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ViajesCompartidos.Enums;

namespace SistemaViajesCompartidos.Models
{
    [Table("Ubicaciones")]
    public class UbicacionModel : BaseModel
    {
        [DisplayName("Latitud")]
        [Required(ErrorMessage = "Es necesaria una Latitud", AllowEmptyStrings = false)]
        public string LatitudTexto { get; set; }
        public double Latitud { get; set; }

        [DisplayName("Longitud")]
        [Required(ErrorMessage = "Es necesaria una Longitud", AllowEmptyStrings = false)]
        public string LongitudTexto { get; set; }
        public double Longitud { get; set; }

        public TipoUbicacionEnum TipoUbicacion { get; set; } = TipoUbicacionEnum.Pasajero;
        
        [DisplayName("Dirección")]
        public string UbicacionTexto { get; set; }

        internal void Fill()
        {
            Latitud = double.Parse(LatitudTexto.Replace('.', ','));
            Longitud = double.Parse(LongitudTexto.Replace('.', ','));
        }

        internal void Update(UbicacionModel ubicacion)
        {
            LatitudTexto = ubicacion.LatitudTexto;
            LongitudTexto = ubicacion.LongitudTexto;
            UbicacionTexto = ubicacion.UbicacionTexto;
            Fill();
        }

        //public byte[] MapaDescargado { get; set; }

        //public string TipoImagenMapa { get; set; }
    }
}