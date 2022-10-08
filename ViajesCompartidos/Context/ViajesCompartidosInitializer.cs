using SistemaViajesCompartidos.Context;
using SistemaViajesCompartidos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViajesCompartidos.Context
{
    public class ViajesCompartidosInitializer : System.Data.Entity.DropCreateDatabaseAlways<ViajesCompartidosContext>
    {
        protected override void Seed(ViajesCompartidosContext context)
        {
            var empresas = new List<EmpresaModel>
            {
            };

            empresas.ForEach(s => context.Empresas.Add(s));
            context.SaveChanges();

            var empleados = new List<EmpleadoModel>
                {
                new EmpleadoModel()  {
                    Nombre = "Yago",
                    CorreoElectronicoEncriptado = "yaguito_marti@hotmail.com",
                    Roles = RolesFlag.EMPLEADO | RolesFlag.CONDUCTOR,
                    
                    Vehiculo = new VehiculoModel() {
                        Patente = "OSU997",
                        AsientosLibres = 3,
                        FechaVencimientoComprobantePoliza = DateTime.Today.AddDays(300),
                        FechaVencimientoComprobantePolizaActiva = true,
                        FechaVencimientoCarnetConducir = DateTime.Today.AddDays(300)
                    }
                }
            };

            empleados.ForEach(s => context.Empleados.Add(s));
            context.SaveChanges();
        }
    }
}