using SistemaViajesCompartidos.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ViajesCompartidos.Context;

namespace SistemaViajesCompartidos.Context
{
    public class ViajesCompartidosContext : DbContext
    {
        public ViajesCompartidosContext() : base("SistemaViajesCompartidos")
        {
            Database.SetInitializer(new ViajesCompartidosInitializer());
            base.Database.Initialize(true);
        }

        public DbSet<EmpresaModel> Empresas { get; set; }
        public DbSet<SucursalModel> Sucursales { get; set; }
        public DbSet<EmpleadoModel> Empleados { get; set; }
        public DbSet<VehiculoModel> Vehiculos { get; set; }
        public DbSet<RecorridoModel> Recorridos { get; set; }
        public DbSet<CorreoElectrinicoRespaldoModel> CorreoElectrinicoRespaldos { get; set; }

        
    }
}