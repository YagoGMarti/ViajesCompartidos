using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace SistemaViajesCompartidos.Models
{
    [Table("Vehiculos")]
    public class VehiculoModel : BaseModel
    {
        public string Patente { get; set; }
        public int AsientosLibres { get; set; }

        public byte[] ComprobantePoliza { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImagenComprobantePoliza { get; set; }

        public string TipoImagenComprobantePoliza { get; set; }
        
        [DisplayName("Vencimiento Comprobante Poliza")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")] 
        public DateTime FechaVencimientoComprobantePoliza { get; set; }
        [DisplayName("Poliza Validada")] 
        public bool FechaVencimientoComprobantePolizaActiva { get; set; }
        public EmpleadoModel EmpleadoValidoComprobantePoliza { get; set; }

        public byte[] CarnetConducir { get; set; }
        
        [NotMapped]
        public HttpPostedFileBase ImagenCarnetConducir { get; set; }

        public string TipoImagenCarnetConducir { get; set; }

        [DisplayName("Vencimiento Carnet Conducir")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime FechaVencimientoCarnetConducir { get; set; }
        [DisplayName("Carnet Validado")] 
        public bool FechaVencimientoCarnetConducirActiva { get; set; }
        public EmpleadoModel EmpleadoValidoCarnetConducir { get; set; }

        public bool ValidarVigenciaPoliza() { return true; }
        public bool ValidarVigenciaCarnetConducir() { return true; }
    }
}