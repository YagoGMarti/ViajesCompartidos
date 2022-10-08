using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SistemaViajesCompartidos.Models
{
    [Table("Recorridos")]
    public class RecorridoModel : BaseModel
    {
        public List<UbicacionModel> Ubicaciones { get; set; }

        public Guid EmpresaId { get; set; }

        public SucursalModel Sucursal { get; set; }
        public EmpleadoModel Conductor { get; set; }

        public List<EmpleadoModel> Pasajeros { get; set; }

        public EstadoRecorridoEnum EstadoRecorrido { get; set; }

        public double CalcularDistanciaEntrePuntos() { return Ubicaciones.Sum(x => x.CalcularDistanciaEntrePuntos(Sucursal.Ubicacion)); }
    }
}