using SistemaViajesCompartidos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaViajesCompartidos.Models.Transactional
{
    public class RecorridoUbicacion : BaseModel
    {
        public RecorridoUbicacion()
        {

        }

        public RecorridoUbicacion(Guid Recorrido_ID, Guid Empleado_ID, int Orden) : base()
        {
            this.Recorrido_ID = Recorrido_ID;
            this.Ubicacion_ID = Empleado_ID;
            this.Orden = Orden;
        }

        public Guid Recorrido_ID { get; set; }
        public Guid Ubicacion_ID { get; set; }
        public int Orden { get; set; }
    }
}