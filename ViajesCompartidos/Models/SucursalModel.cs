using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaViajesCompartidos.Models
{
    [Table("Sucursales")]
    public class SucursalModel : BaseModel
    {
        [Required(ErrorMessage = "Debe indicar un nombre", AllowEmptyStrings = false)]
        public string Nombre { get; set; }
        
        public UbicacionModel Ubicacion { get; set; }

        public Guid EmpresaModel_ID { get; set; }

        public IEnumerable<EmpleadoModel> Empleados { get; set; }

        internal void Update(SucursalModel sucursalModel)
        {
            Nombre = sucursalModel.Nombre;
            Ubicacion.Update(sucursalModel.Ubicacion);
        }
    }
}