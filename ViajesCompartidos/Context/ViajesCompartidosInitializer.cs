using SistemaViajesCompartidos.Context;
using SistemaViajesCompartidos.Enums;
using SistemaViajesCompartidos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViajesCompartidos.Handlers;

namespace ViajesCompartidos.Context
{
    public class ViajesCompartidosInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ViajesCompartidosContext>
    {
        public void CallSeed(ViajesCompartidosContext context)
        {
            Seed(context);
        }

        protected override void Seed(ViajesCompartidosContext context)
        {
            EncriptadoHandler encriptadoHandler = new EncriptadoHandler();

            #region contactos
            var contactos = new List<ContactoModel>
            {
                new ContactoModel() {
                    CorreoElectronicoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("contacto1@gmail.com")),
                    Mensaje = "Deseo realizar una prueba, telefono 3515999999, Juan."
                },
                new ContactoModel() {
                    CorreoElectronicoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("contacto2@asd.com")),
                    Mensaje = "Soy de la empresa ASD, quisieramos una demo."
                }
            };
            contactos.ForEach(c => context.Contactos.Add(c));
            context.SaveChanges();
            #endregion

            #region empresas
            var empresas = new List<EmpresaModel>
            {
                new EmpresaModel () {
                    Nombre = "Demo",
                    RazonSocial = "Empresa Demo S.R.L.",
                    CUIT = Int64.Parse("20355727395"),
                    TipoEmpresa = TipoEmpresaEnum.DEMO
                }
            };

            empresas.ForEach(e => context.Empresas.Add(e));
            context.SaveChanges();
            #endregion

            #region sucursales
            var sucursales = new List<SucursalModel>
            {
                new SucursalModel () {
                    Nombre = "Casa Central",
                    EmpresaModel_ID = empresas.FirstOrDefault(e => e.Nombre == "Demo").ID,
                    Ubicacion = new UbicacionModel() { 
                        UbicacionTexto = "Buenos Aires 106, Cordoba, Argentina",
                        Latitud = -31.417520,
                        LatitudTexto = "-31.417520",
                        Longitud = -64.183365,
                        LongitudTexto = "-64.183365"
                    }
                },
                new SucursalModel () {
                    Nombre = "Cerro",
                    EmpresaModel_ID = empresas.FirstOrDefault(e => e.Nombre == "Demo").ID,
                    Ubicacion = new UbicacionModel() { 
                        UbicacionTexto = "Avenida Recta Martinolli 5243, Cordoba, Argentina",
                        Latitud = -31.352805,
                        LatitudTexto = "-31.352805",
                        Longitud = -64.248318,
                        LongitudTexto = "-64.248318"
                    }
                }
            };

            sucursales.ForEach(s => context.Sucursales.Add(s));
            context.SaveChanges();
            #endregion

            #region empleados
            var empleados = new List<EmpleadoModel>
                {
                new EmpleadoModel()  {
                    Nombre = "Yago",
                    ClaveEncriptada = EncriptadoHandler.Encriptar("123123"),
                    CorreoElectronicoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("yaguito_marti@hotmail.com")),
                    Roles = RolesEmpleadoFlag.ADMINISTRADOR,
                    EmpresaModel_ID = empresas.FirstOrDefault(e => e.Nombre == "Demo").ID,
                    SucursalModel_ID = sucursales.FirstOrDefault(s => s.Nombre == "Cerro").ID,
                    HorarioIngreso = new TimeSpan(9,0,0),
                    HorarioSalida = new TimeSpan(17,0,0),
                    Ubicacion = new UbicacionModel() { 
                        UbicacionTexto = "Alfredo Garcia Voglino 5457, Cordoba, Argentina",
                        Latitud = -31.346839,
                        LatitudTexto = "-31.346839",
                        Longitud = -64.245077,
                        LongitudTexto = "-64.245077" 
                    }
                },
                new EmpleadoModel()  {
                    Nombre = "Marti",
                    ClaveEncriptada = EncriptadoHandler.Encriptar("123123"),
                    CorreoElectronicoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("yago.g.marti@gmail.com")),
                    Roles = RolesEmpleadoFlag.RRHH | RolesEmpleadoFlag.CONDUCTOR,
                    EmpresaModel_ID = empresas.FirstOrDefault(e => e.Nombre == "Demo").ID,
                    SucursalModel_ID = sucursales.FirstOrDefault(s => s.Nombre == "Casa Central").ID,
                    Vehiculo = new VehiculoModel() {
                        Patente = "OSU997",
                        AsientosLibres = 2,
                        FechaVencimientoCarnetConducir = DateTime.Today.AddDays(120),
                        FechaVencimientoComprobantePoliza = DateTime.Today.AddDays(120)
                    },
                    HorarioIngreso = new TimeSpan(9,0,0),
                    HorarioSalida = new TimeSpan(17,0,0),
                    Ubicacion = new UbicacionModel() { 
                        UbicacionTexto = "La Rioja 2359, Cordoba, Argentina",
                        Latitud = -31.402764,
                        LatitudTexto = "-31.402764",
                        Longitud = -64.212213,
                        LongitudTexto = "-64.212213"
                    } 
                },
                new EmpleadoModel()  {
                    Nombre = "Gonzalo",
                    ClaveEncriptada = EncriptadoHandler.Encriptar("123123"),
                    CorreoElectronicoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("marti@mail.com")),
                    Roles = RolesEmpleadoFlag.EMPLEADO,
                    EmpresaModel_ID = empresas.FirstOrDefault(e => e.Nombre == "Demo").ID,
                    SucursalModel_ID = sucursales.FirstOrDefault(s => s.Nombre == "Casa Central").ID,
                    HorarioIngreso = new TimeSpan(9,0,0),
                    HorarioSalida = new TimeSpan(17,0,0),
                    Ubicacion = new UbicacionModel() { 
                        UbicacionTexto = "Av. Colon 3932, Cordoba, Argentina",
                        Latitud = -31.398708,
                        LatitudTexto = "-31.398708",
                        Longitud = -64.232047,
                        LongitudTexto = "-64.232047"
                    }
                }
            };

            foreach (var item in empleados)
                if (item.Vehiculo != null)
                    item.Vehiculo.Empleado_ID = item.ID;

            empleados.ForEach(e => context.Empleados.Add(e));
            context.SaveChanges();
            #endregion
        }
    }
}