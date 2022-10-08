using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaViajesCompartidos.Models
{
    [Table("Sucursales")]
    public class SucursalModel : BaseModel
    {
        public string Nombre { get; set; }
        public UbicacionModel Ubicacion { get; set; }
        public List<EmpleadoModel> Empleados { get; set; }

        public List<EmpleadoModel> GetEmpleados() { return this.Empleados; }
    }
}