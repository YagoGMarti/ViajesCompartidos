using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaViajesCompartidos.Models
{
    [Table("Ubicaciones")]
    public class UbicacionModel : BaseModel
    {
        [DisplayName("Latitud")]
        [Required(ErrorMessage = "Es necesaria una Latitud", AllowEmptyStrings = false)]
        public string LatitudTexto { get; set; }
        public double Latitud { get; set; } = -31.41293;

        [DisplayName("Longitud")]
        [Required(ErrorMessage = "Es necesaria una Longitud", AllowEmptyStrings = false)]
        public string LongitudTexto { get; set; }
        public double Longitud { get; set; } = -64.18585;

        public string UbicacionTextoBusqueda { get; set; }
        
        [DisplayName("Dirección")]
        public string UbicacionTexto { get; set; }

        internal void Update(UbicacionModel ubicacion)
        {
            LatitudTexto = ubicacion.LatitudTexto;
            Latitud = double.Parse(ubicacion.LatitudTexto.Replace('.',','));
            LongitudTexto = ubicacion.LongitudTexto;
            Longitud = double.Parse(ubicacion.LongitudTexto.Replace('.', ','));
            UbicacionTextoBusqueda = ubicacion.UbicacionTextoBusqueda;
            UbicacionTexto = ubicacion.UbicacionTexto;
        }

        //public byte[] MapaDescargado { get; set; }

        //public string TipoImagenMapa { get; set; }
    }
}