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
        [Required(ErrorMessage = "Debe indicar una patente", AllowEmptyStrings = false)]
        public string Patente { get; set; }
        
        [DisplayName("Asientos Disponibles")]
        [Range(1, 7, ErrorMessage = "Los asientos se espera que sean entre 1 y 7")]
        public int AsientosLibres { get; set; }

        [NotMapped]
        [DisplayName("Adjunto Poliza Seguro")]
        public HttpPostedFileBase ImagenComprobantePoliza { get; set; }
        [DisplayName("Adjunto")]
        public byte[] AdjuntoComprobantePoliza { get; set; }
        public string TipoImagenComprobantePoliza { get; set; }
        public string NombreArchivoComprobantePoliza { get; set; }

        [NotMapped]
        [DisplayName("Adjunto Carnet Conducir")]
        public HttpPostedFileBase ImagenCarnetConducir { get; set; }
        [DisplayName("Adjunto")]
        public byte[] AdjuntoCarnetConducir { get; set; }
        public string TipoImagenCarnetConducir { get; set; }
        public string NombreArchivoCarnetConducir { get; set; }

        public Guid Empleado_ID { get; set; }

        public void Update(VehiculoModel vehiculoModel)
        {
            Patente = vehiculoModel.Patente;
            AsientosLibres = vehiculoModel.AsientosLibres;

            if(vehiculoModel.ImagenComprobantePoliza != null)
            {
                AdjuntoComprobantePoliza = vehiculoModel.AdjuntoComprobantePoliza;
                TipoImagenComprobantePoliza = vehiculoModel.TipoImagenComprobantePoliza;
                NombreArchivoComprobantePoliza = vehiculoModel.NombreArchivoComprobantePoliza;
                ComprobantePolizaValidado = false;
            }

            if(vehiculoModel.ImagenCarnetConducir != null)
            {
                AdjuntoCarnetConducir = vehiculoModel.AdjuntoCarnetConducir;
                TipoImagenCarnetConducir = vehiculoModel.TipoImagenCarnetConducir;
                NombreArchivoCarnetConducir = vehiculoModel.NombreArchivoCarnetConducir;
                ComprobanteCarnetValidado = false;
            }
        }

        [DisplayName("Validada")]
        public bool ComprobantePolizaValidado { get; set; }
        [DisplayName("Validada")]
        public bool ValidarComprobantePoliza => (!ComprobantePolizaValidado && !string.IsNullOrWhiteSpace(NombreArchivoComprobantePoliza));

        [DisplayName("Vencimiento Poliza")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? FechaVencimientoComprobantePoliza { get; set; }

        [DisplayName("Validado")]
        public bool ComprobanteCarnetValidado { get; set; }
        [DisplayName("Validado")]
        public bool ValidarCarnetConducir => (!ComprobanteCarnetValidado && !string.IsNullOrWhiteSpace(NombreArchivoCarnetConducir));

        [DisplayName("Vencimiento Carnet")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? FechaVencimientoCarnetConducir { get; set; }

        public bool MostrarValidar => CalcularValidar();
        
        private bool CalcularValidar()
        {
            if (ValidarComprobantePoliza)
                return true;

            if (ValidarCarnetConducir)
                return true;

            return false;
        }

        public void UpdateValidar(VehiculoModel vehiculoModel)
        {
            ComprobantePolizaValidado = vehiculoModel.ComprobantePolizaValidado;
            FechaVencimientoComprobantePoliza = vehiculoModel.FechaVencimientoComprobantePoliza;
            ComprobanteCarnetValidado = vehiculoModel.ComprobanteCarnetValidado;
            FechaVencimientoCarnetConducir = vehiculoModel.FechaVencimientoCarnetConducir;
        }
    }
}