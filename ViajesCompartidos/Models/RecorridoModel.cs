using SistemaViajesCompartidos.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SistemaViajesCompartidos.Models
{
    [Table("Recorridos")]
    public class RecorridoModel : BaseModel
    {
        public Guid EmpresaId { get; set; }
        
        public EmpleadoModel Conductor { get; set; }

        public List<EmpleadoModel> Pasajeros { get; set; } = new List<EmpleadoModel>();
        
        public SucursalModel Sucursal { get; set; }

        public List<UbicacionModel> Ubicaciones { get; set; } = new List<UbicacionModel>();

        public EstadoRecorridoEnum EstadoRecorrido { get; set; }
    }
}