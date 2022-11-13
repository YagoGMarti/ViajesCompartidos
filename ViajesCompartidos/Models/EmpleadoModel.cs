using SistemaViajesCompartidos.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaViajesCompartidos.Models
{
    [Table("Empleados")]
    public class EmpleadoModel : BaseModel
    {
        [DisplayName("Nombre")]
        [Required(ErrorMessage = "Es necesario un nombre", AllowEmptyStrings = false)]
        public string Nombre { get; set; }

        [NotMapped]
        [DisplayName("Email")]
        [Required(ErrorMessage = "Es necesario un email", AllowEmptyStrings = false)]
        [EmailAddress(ErrorMessage = "El email no cumple con el formato esperado")]
        public string CorreoElectronico { get; set; }

        public string CorreoElectronicoEncriptado { get; set; }

        public byte[] ClaveEncriptada { get; set; }

        [DisplayName("Rol")]
        public RolesEmpleadoFlag Roles { get; set; }

        [NotMapped]
        [DisplayName("Telefono")]
        public string Telefono { get; set; }
        public string TelefonoEncriptado { get; set; }

        [DisplayName("Pertenece a RRHH")]
        [NotMapped]
        public bool RRHH { get; set; }

        public VehiculoModel Vehiculo { get; set; }

        [DisplayName("Vehiculo")]
        public bool TieneVehiculo => (Vehiculo != null);

        [DisplayName("Horario")]
        public TimeSpan HorarioIngreso { get; set; }
        public TimeSpan HorarioSalida { get; set; }

        [NotMapped]
        public string HorarioIngresoTexto { get; set; }
        [NotMapped]
        public string HorarioSalidaTexto { get; set; }

        public string Horario => HorarioIngreso.Hours.ToString() + "Hs - " + HorarioSalida.Hours.ToString() + "Hs";

        public UbicacionModel Ubicacion { get; set; }

        [DisplayName("Empresa")]
        public Guid EmpresaModel_ID { get; set; }

        [DisplayName("Recorrido")]
        public Guid Recorrido_ID { get; set; }
        public bool RecorridoActivo { get; set; } = false;
        public virtual RecorridoModel Recorrido { get; set; }

        [DisplayName("Sucursal")]
        public Guid SucursalModel_ID { get; set; }
        [ForeignKey("SucursalModel_ID")]
        public virtual SucursalModel Sucursal { get; set; }
        public double DistanciaSucursal { get; set; }

        public void Update(EmpleadoModel empleadoModel)
        {
            Nombre = empleadoModel.Nombre;
            //CorreoElectronicoEncriptado = empleadoModel.CorreoElectronicoEncriptado;
            TelefonoEncriptado = empleadoModel.TelefonoEncriptado;
            Roles = empleadoModel.Roles;
            SucursalModel_ID = empleadoModel.SucursalModel_ID;
            DistanciaSucursal = empleadoModel.DistanciaSucursal;
            HorarioIngreso = empleadoModel.HorarioIngreso;
            HorarioSalida = empleadoModel.HorarioSalida;
            Ubicacion.Update(empleadoModel.Ubicacion);
        }

        public void ActualizarRoles()
        {
            if (RRHH && !Roles.HasFlag(RolesEmpleadoFlag.RRHH))
                Roles = Roles | RolesEmpleadoFlag.RRHH;

            if (!RRHH && Roles.HasFlag(RolesEmpleadoFlag.RRHH))
                Roles &= ~RolesEmpleadoFlag.RRHH;
        }
    }
}