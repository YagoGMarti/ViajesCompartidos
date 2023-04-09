using SistemaViajesCompartidos.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using SistemaViajesCompartidos.Models.Transactional;

namespace SistemaViajesCompartidos.Models
{
    [Table("Recorridos")]
    public class RecorridoModel : BaseModel
    {
        public RecorridoModel() : base()
        {
            Ubicaciones = new List<UbicacionModel>();
            Pasajeros = new List<EmpleadoModel>();
        }

        public Guid Empresa_ID { get; set; }
        
        public Guid Conductor_ID { get; set; }
        //[ForeignKey("Conductor_ID")]
        //public virtual EmpleadoModel Conductor { get; set; }

        public List<RecorridoEmpleado> RecorridoEmpleado { get; set; }
        public virtual List<EmpleadoModel> Pasajeros { get; set; }
        
        public Guid Sucursal_ID { get; set; }
        //[ForeignKey("Sucursal_ID")]
        //public virtual SucursalModel Sucursal { get; set; }

        public List<RecorridoUbicacion> RecorridoUbicacion { get; set; }
        public virtual List<UbicacionModel> Ubicaciones { get; set; }

        public EstadoRecorridoEnum EstadoRecorrido { get; set; }

        public double LatitudCentro { get; set; }
        public string LatitudCentroString => LatitudCentro.ToString().Replace(',', '.');
        public double LongitudCentro { get; set; }
        public string LongitudCentroString => LongitudCentro.ToString().Replace(',', '.');
    }
}