using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaViajesCompartidos.Models
{
    [Table("Empleados")]
    public class EmpleadoModel : BaseModel
    {
        [DisplayName("Nombre o apodo")]
        public string Nombre { get; set; }
        public UbicacionModel Ubicacion { get; set; }
        
        [DisplayName("Correo electrónico")]
        public string CorreoElectronicoEncriptado { get; set; }
        public string ClaveHash { get; set; }
        public RolesFlag Roles { get; set; }
        public string TelefonoEncriptado { get; set; }

        [NotMapped]
        [DisplayName("Pertenece a RRHH")]
        public bool RRHH { get; set; }

        public VehiculoModel Vehiculo { get; set; }
        public List<JornadaTrabajoModel> JornadasTrabajo { get; set; }

        public bool ValidarLogin(string Clave) { return true; }
    }
}