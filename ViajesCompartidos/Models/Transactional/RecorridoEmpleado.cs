using SistemaViajesCompartidos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViajesCompartidos.Models.Transactional
{
    public class RecorridoEmpleado : BaseModel
    {
        public RecorridoEmpleado()
        {

        }

        public RecorridoEmpleado(Guid Recorrido_ID, Guid Empleado_ID, int Orden) : base()
        {
            this.Recorrido_ID = Recorrido_ID;
            this.Empleado_ID = Empleado_ID;
            this.Orden = Orden;
        }

        public Guid Recorrido_ID { get; set; }
        public Guid Empleado_ID { get; set; }
        public int Orden { get; set; }
    }
}