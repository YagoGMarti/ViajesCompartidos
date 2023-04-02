using SistemaViajesCompartidos.Enums;
using SistemaViajesCompartidos.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ViajesCompartidos.Context;
using ViajesCompartidos.Handlers;
using ViajesCompartidos.Models.Transactional;
using ViajesCompartidos.Temporal;

namespace SistemaViajesCompartidos.Context
{
    public partial class ViajesCompartidosContext : DbContext
    {
        public ViajesCompartidosContext(bool Initialize = false) : base("SistemaViajesCompartidos")
        {
            // Evita la validación de atributos con decorador NotMapped... 
            Configuration.ValidateOnSaveEnabled = false;

            // Desactivar inicializador
            //Database.SetInitializer<ViajesCompartidosContext>(null);

            // Inicializar con una firma propia y datos de prueba
            if (Initialize)
            {
                Database.SetInitializer<ViajesCompartidosContext>(new DropCreateDatabaseAlways<ViajesCompartidosContext>());
                base.Database.CommandTimeout = 300;
                new ViajesCompartidosInitializer().CallSeed(this);
            }
            else
            {
                Database.SetInitializer(new ViajesCompartidosInitializer());
                base.Database.Initialize(true);
            }
        }

        public DbSet<EmpresaModel> Empresas { get; set; }
        #region EmpresasCRUD
        public static IEnumerable<EmpresaModel> GetEmpresas()
        {
            IEnumerable<EmpresaModel> empresas;
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                empresas = context.Empresas.ToList();
            }
            return empresas;
        }

        public static EmpresaModel GetEmpresa(Guid ID)
        {
            EmpresaModel empresa;
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                empresa = context.Empresas.Find(ID);
            }
            return empresa;
        }

        public static void CrearEmpresa(EmpresaModel empresaModel)
        {
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                context.Empresas.Add(empresaModel);
                context.SaveChanges();
            }
        }

        public static void EditarEmpresa(EmpresaModel empresaModel)
        {
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                EmpresaModel empresa = GetEmpresa(empresaModel.ID);
                empresa.Update(empresaModel);
                context.Entry(empresa).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public static void CambiarEstadoActivoEmpresa(Guid ID, bool estado)
        {
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                EmpresaModel empresa = context.Empresas.Find(ID);
                empresa.Activo = estado;
                context.SaveChanges();
            }
        }
        #endregion

        public DbSet<SucursalModel> Sucursales { get; set; }
        #region Sucursales
        internal static IEnumerable<SucursalModel> GetSucursalesPorEmpresa(Guid empresaID)
        {
            IEnumerable<SucursalModel> sucursales;
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                sucursales = context.Sucursales.Where(x => x.EmpresaModel_ID == empresaID).Include(x => x.Ubicacion).ToList();
            }
            return sucursales;
        }

        internal static SucursalModel GetSucursal(Guid ID)
        {
            SucursalModel sucursal;
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                sucursal = context.Sucursales.Include(x => x.Ubicacion).FirstOrDefault(x => x.ID == ID);
            }
            return sucursal;
        }

        public static void CrearSucursal(SucursalModel sucursalModel)
        {
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                context.Sucursales.Add(sucursalModel);
                context.SaveChanges();
            }
        }

        public static void EditarSucursal(SucursalModel sucursalModel)
        {
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                SucursalModel sucursal = GetSucursal(sucursalModel.ID);
                sucursal.Update(sucursalModel);
                context.Entry(sucursal).State = EntityState.Modified;
                context.Entry(sucursal.Ubicacion).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public static void CambiarEstadoActivoSucursal(Guid ID, bool estado)
        {
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                SucursalModel sucursal = context.Sucursales.Find(ID);
                sucursal.Activo = estado;
                context.SaveChanges();
            }
        }
        #endregion
        public DbSet<EmpleadoModel> Empleados { get; set; }
        #region empleados
        internal static IEnumerable<EmpleadoModel> GetEmpleadosPorEmpresa(Guid empresaID)
        {
            IEnumerable<EmpleadoModel> empleados;
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                empleados = context.Empleados.Where(x => x.EmpresaModel_ID == empresaID)
                    .Include(x => x.Ubicacion)
                    .Include(x => x.Vehiculo)
                    .Include(x => x.Sucursal).ToList();
            }
            return empleados;
        }

        internal static IEnumerable<EmpleadoModel> GetEmpleadosPorSucursal(Guid sucursalID)
        {
            IEnumerable<EmpleadoModel> empleados;
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                empleados = context.Empleados.Where(x => x.SucursalModel_ID == sucursalID)
                    .Include(x => x.Ubicacion)
                    .Include(x => x.Vehiculo)
                    .Include(x => x.Sucursal).ToList();
            }
            return empleados;
        }

        internal static void CancelarRuta(Guid recorrido_ID)
        {
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                RecorridoModel recorrido = GetRecorrido(recorrido_ID);

                foreach (var pasajero in recorrido.RecorridoEmpleado)
                {
                    RemoverPasajero(recorrido_ID, pasajero.Empleado_ID);
                }

                RemoverPasajero(recorrido_ID, recorrido.Conductor_ID);
            }
        }

        public DbSet<RecorridoEmpleado> RecorridoEmpleado { get; set; }
        public DbSet<RecorridoUbicacion> RecorridoUbicacion { get; set; }


        internal static void RemoverPasajero(Guid recorrido_ID, Guid pasajero_ID)
        {
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                EmpleadoModel empleado = GetEmpleado(pasajero_ID);
                empleado.Recorrido = null;
                empleado.RecorridoActivo = false;
                empleado.Recorrido_ID = Guid.Empty;
                context.Entry(empleado).State = EntityState.Modified;
                context.SaveChanges();

                var recorridosEmpleados = context.RecorridoEmpleado.Where(x => x.Empleado_ID == empleado.ID && x.Recorrido_ID == recorrido_ID);
                foreach (var recorridoEmpleado in recorridosEmpleados)
                {
                    context.RecorridoEmpleado.Remove(recorridoEmpleado);
                    context.Entry(recorridoEmpleado).State = EntityState.Deleted;
                }

                var recorridosUbicaciones = context.RecorridoUbicacion.Where(x => x.Ubicacion_ID == empleado.Ubicacion.ID && x.Recorrido_ID == recorrido_ID);
                foreach (var recorridoUbicacion in recorridosUbicaciones)
                {
                    context.RecorridoUbicacion.Remove(recorridoUbicacion);
                    context.Entry(recorridoUbicacion).State = EntityState.Deleted;
                }

                context.SaveChanges();
            }
        }

        public static void CrearEmpleado(EmpleadoModel empleadoModel)
        {
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                context.Empleados.Add(empleadoModel);
                context.SaveChanges();
            }
        }

        public static void EditarEmpleado(EmpleadoModel empleadoModel)
        {
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                EmpleadoModel empleado = GetEmpleado(empleadoModel.ID);
                empleado.Update(empleadoModel);
                context.Entry(empleado).State = EntityState.Modified;
                context.Entry(empleado.Ubicacion).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public static void AgregarVehiculo(EmpleadoModel empleadoModel)
        {
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                EmpleadoModel empleado = GetEmpleado(empleadoModel.ID);
                empleado.Vehiculo = empleadoModel.Vehiculo;
                empleado.Vehiculo_ID = empleadoModel.Vehiculo.ID;
                context.Entry(empleado).State = EntityState.Modified; 
                context.Entry(empleado.Vehiculo).State = EntityState.Added;
                context.SaveChanges();
            }
        }

        public static void CambiarEstadoActivoEmpleado(Guid ID, bool estado)
        {
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                EmpleadoModel empleado = context.Empleados.Find(ID);
                empleado.Activo = estado;
                context.SaveChanges();
            }
        }
        #endregion
        #region Login
        public static EmpleadoModel GetEmpleado(Guid ID)
        {
            EmpleadoModel empleado;
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                empleado = context.Empleados
                    .Include(x => x.Ubicacion)
                    .Include(x => x.Vehiculo)
                    .Include(x => x.Sucursal)
                    .FirstOrDefault(x => x.ID == ID);
            }
            return empleado;
        }

        public static EmpleadoModel GetEmpleadoPorCorreoElectronico(string emailEncriptado)
        {
            EmpleadoModel empleado;
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                empleado = context.Empleados.FirstOrDefault(x => x.CorreoElectronicoEncriptado == emailEncriptado);
            }
            return empleado;
        }

        public static RolesEmpleadoFlag GetRolEmpleado(Guid empleadoID)
        {
            RolesEmpleadoFlag rolesEmpleado;
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                rolesEmpleado = context.Empleados.FirstOrDefault(x => x.ID == empleadoID).Roles;
            }
            return rolesEmpleado;
        }
        #endregion

        public DbSet<VehiculoModel> Vehiculos { get; set; }
        #region vehiculos
        internal static VehiculoModel GetVehiculo(Guid ID)
        {
            VehiculoModel vehiculo;
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                vehiculo = context.Vehiculos.Find(ID);
            }
            return vehiculo;
        }

        public static IEnumerable<VehiculoModel> GetVehiculos()
        {
            IEnumerable<VehiculoModel> vehiculos;
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                vehiculos = context.Vehiculos.ToList();
            }
            return vehiculos;
        }

        public static void CrearVehiculo(VehiculoModel vehiculoModel)
        {
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                context.Vehiculos.Add(vehiculoModel);
                context.SaveChanges();
            }
        }

        public static void EditarVehiculo(VehiculoModel vehiculoModel)
        {
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                VehiculoModel vehiculo = GetVehiculo(vehiculoModel.ID);
                vehiculo.Update(vehiculoModel);
                context.Entry(vehiculo).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public static void ValidarVehiculo(VehiculoModel vehiculoModel)
        {
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                VehiculoModel vehiculo = GetVehiculo(vehiculoModel.ID);
                vehiculo.UpdateValidar(vehiculoModel);
                context.Entry(vehiculo).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
        #endregion
        public DbSet<UbicacionModel> Ubicaciones { get; set; }
        public static UbicacionModel GetUbicacion(Guid ubicacion_ID)
        {
            UbicacionModel ubicacion;
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                ubicacion = context.Ubicaciones.Find(ubicacion_ID);
            }
            return ubicacion;
        }
        public DbSet<RecorridoModel> Recorridos { get; set; }
        #region recorridos
        public static void GrabarRecorrido(RecorridoModel recorrido)
        {
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                //context.Entry(recorrido.Pasajeros).State = EntityState.Unchanged;
                //recorrido.Ubicaciones.ForEach(u => context.Entry(u).State = EntityState.Unchanged);
                //context.Entry(recorrido.Ubicaciones).State = EntityState.Unchanged;

                //var pasajeros_ID = recorrido.Pasajeros.Select(p => p.ID);
                //recorrido.Pasajeros = new List<EmpleadoModel>();
                //foreach (var empleado_ID in pasajeros_ID)
                //{
                //    recorrido.Pasajeros.Add(GetEmpleado(empleado_ID));
                //}

                recorrido.Pasajeros = null;
                recorrido.Ubicaciones = null;

                context.Recorridos.Add(recorrido);
                context.SaveChanges();
            }
        }

        public static void GrabarRecorrido(Guid empleado_ID, Guid recorrido_ID)
        {
            EmpleadoModel empleado = GetEmpleado(empleado_ID);
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                empleado.Recorrido_ID = recorrido_ID;
                empleado.RecorridoActivo = true;
                context.Entry(empleado).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public static void BorrarRecorrido(Guid empleado_ID)
        {
            EmpleadoModel empleado = GetEmpleado(empleado_ID);
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                empleado.RecorridoActivo = false;
                context.Entry(empleado).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public static RecorridoModel GetRecorrido(Guid recorrido_ID)
        {
            RecorridoModel recorrido;
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                recorrido = context.Recorridos
                    .Include(x => x.RecorridoEmpleado)
                    .Include(x => x.Pasajeros)
                    .Include(x => x.RecorridoUbicacion)
                    .Include(x => x.Ubicaciones)
                    .FirstOrDefault(x => x.ID == recorrido_ID);

                recorrido.Pasajeros = recorrido.RecorridoEmpleado.OrderBy(x => x.Orden).Select(x => GetEmpleado(x.Empleado_ID)).ToList();
                recorrido.Ubicaciones = recorrido.RecorridoUbicacion.OrderBy(x => x.Orden).Select(x => GetUbicacion(x.Ubicacion_ID)).ToList();

            }
            return recorrido;
        }

        public static IEnumerable<RecorridoModel> GetRecorridosPorSucursal(Guid sucursal_ID)
        {
            IEnumerable<RecorridoModel> recorridos;
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                recorridos = context.Recorridos
                    .Where(x => x.Sucursal_ID == sucursal_ID).ToList();

            }
            return recorridos;
        }
        
        #endregion
        public DbSet<ContactoModel> Contactos { get; set; }
        #region ContactosCRUD
        public static IEnumerable<ContactoModel> GetContactos(bool filtrarProcesados)
        {
            IEnumerable<ContactoModel> contactos;
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                contactos = context.Contactos.ToList();
            }
            return contactos;
        }

        public static void MarcarContactoRealizado(Guid ID)
        {
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                ContactoModel contacto = context.Contactos.Find(ID);
                contacto.Procesado = true;
                context.Entry(contacto).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public static void GrabarContacto(ContactoModel contactoModel)
        {
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                context.Contactos.Add(contactoModel);
                context.SaveChanges();
            }
        }
        #endregion

        public DbSet<CorreoElectronicoModel> CorreosElectronicos { get; set; }
        #region ContactosCRUD
        public static void GrabarCorreoElectronico(CorreoElectronicoModel correoElectronicoModel)
        {
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                context.CorreosElectronicos.Add(correoElectronicoModel);
                context.SaveChanges();
            }
        }

        internal static Guid GetEmpresaBySucursal(Guid sucursalID)
        {
            Guid empresaID;
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                empresaID = context.Sucursales.Find(sucursalID).EmpresaModel_ID;
            }
            return empresaID;
        }

        internal static string ReiniciarClave(Guid empladoID, byte[] claveEncriptada)
        {
            string email;
            using (ViajesCompartidosContext context = new ViajesCompartidosContext())
            {
                var empleado = context.Empleados.Find(empladoID);
                empleado.ClaveEncriptada = claveEncriptada;
                context.SaveChanges();
                email = empleado.CorreoElectronicoEncriptado;
            }

            return email;
        }
        #endregion
    }
}