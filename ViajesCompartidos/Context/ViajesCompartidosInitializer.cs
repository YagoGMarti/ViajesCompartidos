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
                },
                new EmpresaModel () {
                    Nombre = "Admin",
                    RazonSocial = "Admin Demo",
                    CUIT = Int64.Parse("20355727395"),
                    TipoEmpresa = TipoEmpresaEnum.DEMO
                }
            };

            empresas.ForEach(e => context.Empresas.Add(e));
            context.SaveChanges();
            //empresas = context.Empresas.ToList();
            #endregion

            #region sucursales
            var sucursales = new List<SucursalModel>
            {
                new SucursalModel () {
                    Nombre = "Casa Central",
                    EmpresaModel_ID = empresas.FirstOrDefault(e => e.Nombre == "Demo").ID,
                    Ubicacion = new UbicacionModel() {
                        UbicacionTexto = "Buenos Aires 106, Córdoba, Argentina",
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
                        UbicacionTexto = "Avenida Recta Martinolli 5243, Córdoba, Argentina",
                        Latitud = -31.352805,
                        LatitudTexto = "-31.352805",
                        Longitud = -64.248318,
                        LongitudTexto = "-64.248318"
                    }
                },
                new SucursalModel () {
                    Nombre = "Admin",
                    EmpresaModel_ID = empresas.FirstOrDefault(e => e.Nombre == "Admin").ID,
                    Ubicacion = new UbicacionModel() {
                        UbicacionTexto = "Calle Falsa 123",
                        Latitud = -0.0,
                        LatitudTexto = "0.0",
                        Longitud = -0.0,
                        LongitudTexto = "0.0"
                    }
                },
                new SucursalModel () {
                    Nombre = "Nueva Córdoba",
                    EmpresaModel_ID = empresas.FirstOrDefault(e => e.Nombre == "Demo").ID,
                    Ubicacion = new UbicacionModel() {
                        UbicacionTexto = "Ituzaingó 484, Córdoba, Argentina",
                        Latitud = -31.422529,
                        LatitudTexto = "-31.422529",
                        Longitud = -64.183939,
                        LongitudTexto = "-64.183939"
                    }
                }
            };

            sucursales.ForEach(s => context.Sucursales.Add(s));
            context.SaveChanges();
            //sucursales = context.Sucursales.ToList();
            #endregion

            #region empleados
            var empleados = new List<EmpleadoModel>
                {
                new EmpleadoModel()  {
                    Nombre = "Yago",
                    ClaveEncriptada = EncriptadoHandler.Encriptar("empleado1@cerro"),
                    CorreoElectronicoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("empleado1@cerro.com")),
                    Roles = RolesEmpleadoFlag.EMPLEADO,
                    TelefonoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("3515165507")),
                    EmpresaModel_ID = empresas.FirstOrDefault(e => e.Nombre == "Demo").ID,
                    Sucursal = sucursales.FirstOrDefault(s => s.Nombre == "Cerro"),
                    SucursalModel_ID = sucursales.FirstOrDefault(s => s.Nombre == "Cerro").ID,
                    Vehiculo = new VehiculoModel() {
                        Patente = "LAR303",
                        AsientosLibres = 1,
                        FechaVencimientoCarnetConducir = DateTime.Today.AddDays(120),
                        ComprobanteCarnetValidado = true,
                        FechaVencimientoComprobantePoliza = DateTime.Today.AddDays(120),
                        ComprobantePolizaValidado = true,
                        EmpresaModel_ID = empresas.FirstOrDefault(e => e.Nombre == "Demo").ID
                    },
                    HorarioIngreso = new TimeSpan(9,0,0),
                    HorarioSalida = new TimeSpan(17,0,0),
                    Ubicacion = new UbicacionModel() {
                        UbicacionTexto = "Ernesto Piotti 6378, Córdoba, Argentina",
                        Latitud = -31.3435028,
                        LatitudTexto = "-31.3435028",
                        Longitud = -64.2552163,
                        LongitudTexto = "-64.2552163"
                    }
                },
                new EmpleadoModel()  {
                    Nombre = "Marti",
                    ClaveEncriptada = EncriptadoHandler.Encriptar("empleado2@cerro"),
                    CorreoElectronicoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("empleado2@cerro.com")),
                    Roles = RolesEmpleadoFlag.RRHH | RolesEmpleadoFlag.CONDUCTOR,
                    TelefonoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("3515165507")),
                    EmpresaModel_ID = empresas.FirstOrDefault(e => e.Nombre == "Demo").ID,
                    Sucursal = sucursales.FirstOrDefault(s => s.Nombre == "Cerro"),
                    SucursalModel_ID = sucursales.FirstOrDefault(s => s.Nombre == "Cerro").ID,
                    Vehiculo = new VehiculoModel() {
                        Patente = "OSU997",
                        AsientosLibres = 3,
                        FechaVencimientoCarnetConducir = DateTime.Today.AddDays(120),
                        ComprobanteCarnetValidado = true,
                        FechaVencimientoComprobantePoliza = DateTime.Today.AddDays(120),
                        ComprobantePolizaValidado = true,
                        EmpresaModel_ID = empresas.FirstOrDefault(e => e.Nombre == "Demo").ID
                    },
                    HorarioIngreso = new TimeSpan(9,0,0),
                    HorarioSalida = new TimeSpan(17,0,0),
                    Ubicacion = new UbicacionModel() {
                        UbicacionTexto = "Tte. Gral. Donato Alvarez 9135",
                        Latitud = -31.3194174,
                        LatitudTexto = "-31.3194174",
                        Longitud = -64.2726915,
                        LongitudTexto = "-64.2726915"
                    }
                },
                new EmpleadoModel()  {
                    Nombre = "Gonzalo",
                    ClaveEncriptada = EncriptadoHandler.Encriptar("empleado3@cerro"),
                    CorreoElectronicoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("empleado3@cerro.com")),
                    Roles = RolesEmpleadoFlag.EMPLEADO,
                    TelefonoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("3515165507")),
                    EmpresaModel_ID = empresas.FirstOrDefault(e => e.Nombre == "Demo").ID,
                    Sucursal = sucursales.FirstOrDefault(s => s.Nombre == "Cerro"),
                    SucursalModel_ID = sucursales.FirstOrDefault(s => s.Nombre == "Cerro").ID,
                    HorarioIngreso = new TimeSpan(9,0,0),
                    HorarioSalida = new TimeSpan(17,0,0),
                    Ubicacion = new UbicacionModel() {
                        UbicacionTexto = "Heriberto Martínez 7311, Córdoba, Argentina",
                        Latitud = -31.33715,
                        LatitudTexto = "-31.33715",
                        Longitud = -64.26292339999999,
                        LongitudTexto = "-64.26292339999999"
                    }
                }, // input para que el empleado 2 pueda llevar a 1 y 3 al trabajo . 
                new EmpleadoModel()  {
                    Nombre = "Gustavo",
                    ClaveEncriptada = EncriptadoHandler.Encriptar("empleado4@cerro"),
                    CorreoElectronicoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("empleado4@cerro.com")),
                    Roles = RolesEmpleadoFlag.EMPLEADO,
                    TelefonoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("3515165507")),
                    EmpresaModel_ID = empresas.FirstOrDefault(e => e.Nombre == "Demo").ID,
                    Sucursal = sucursales.FirstOrDefault(s => s.Nombre == "Cerro"),
                    SucursalModel_ID = sucursales.FirstOrDefault(s => s.Nombre == "Cerro").ID,
                    Vehiculo = new VehiculoModel() {
                        Patente = "AE 477 IR",
                        AsientosLibres = 4,
                        FechaVencimientoCarnetConducir = DateTime.Today.AddDays(120),
                        ComprobanteCarnetValidado = true,
                        FechaVencimientoComprobantePoliza = DateTime.Today.AddDays(120),
                        ComprobantePolizaValidado = true,
                        EmpresaModel_ID = empresas.FirstOrDefault(e => e.Nombre == "Demo").ID
                    },
                    HorarioIngreso = new TimeSpan(9,0,0),
                    HorarioSalida = new TimeSpan(17,0,0),
                    Ubicacion = new UbicacionModel() {
                        UbicacionTexto = "Domingo Zípoli 940, Córdoba, Argentina",
                        Latitud = -31.393776,
                        LatitudTexto = "-31.393776",
                        Longitud = -64.221340,
                        LongitudTexto = "-64.221340"
                    }
                },
                new EmpleadoModel()  {
                    Nombre = "Lopez",
                    ClaveEncriptada = EncriptadoHandler.Encriptar("empleado5@cerro"),
                    CorreoElectronicoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("empleado5@cerro.com")),
                    Roles = RolesEmpleadoFlag.EMPLEADO,
                    TelefonoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("3515165507")),
                    EmpresaModel_ID = empresas.FirstOrDefault(e => e.Nombre == "Demo").ID,
                    Sucursal = sucursales.FirstOrDefault(s => s.Nombre == "Cerro"),
                    SucursalModel_ID = sucursales.FirstOrDefault(s => s.Nombre == "Cerro").ID,
                    HorarioIngreso = new TimeSpan(10,0,0),
                    HorarioSalida = new TimeSpan(18,0,0),
                    Ubicacion = new UbicacionModel() {
                        UbicacionTexto = "Mariano Larra 3417, Córdoba, Argentina",
                        Latitud = -31.385271,
                        LatitudTexto = "-31.385271",
                        Longitud = -64.227101,
                        LongitudTexto = "-64.227101"
                    }
                },
                new EmpleadoModel()  {
                    Nombre = "Topo",
                    ClaveEncriptada = EncriptadoHandler.Encriptar("empleado6@cerro"),
                    CorreoElectronicoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("empleado6@cerro.com")),
                    Roles = RolesEmpleadoFlag.EMPLEADO,
                    TelefonoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("3515165507")),
                    EmpresaModel_ID = empresas.FirstOrDefault(e => e.Nombre == "Demo").ID,
                    Sucursal = sucursales.FirstOrDefault(s => s.Nombre == "Cerro"),
                    SucursalModel_ID = sucursales.FirstOrDefault(s => s.Nombre == "Cerro").ID,
                    HorarioIngreso = new TimeSpan(9,0,0),
                    HorarioSalida = new TimeSpan(17,0,0),
                    Ubicacion = new UbicacionModel() {
                        UbicacionTexto = "Bv. Los Granaderos 1468, Córdoba, Argentina",
                        Latitud = -31.385831,
                        LatitudTexto = "-31.385831",
                        Longitud = -64.193818,
                        LongitudTexto = "-64.193818"
                    }
                },
                new EmpleadoModel()  {
                    Nombre = "Rios",
                    ClaveEncriptada = EncriptadoHandler.Encriptar("empleado7@cerro"),
                    CorreoElectronicoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("empleado7@cerro.com")),
                    Roles = RolesEmpleadoFlag.EMPLEADO,
                    TelefonoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("3515165507")),
                    EmpresaModel_ID = empresas.FirstOrDefault(e => e.Nombre == "Demo").ID,
                    Sucursal = sucursales.FirstOrDefault(s => s.Nombre == "Cerro"),
                    SucursalModel_ID = sucursales.FirstOrDefault(s => s.Nombre == "Cerro").ID,
                    HorarioIngreso = new TimeSpan(9,0,0),
                    HorarioSalida = new TimeSpan(17,0,0),
                    Ubicacion = new UbicacionModel() {
                        UbicacionTexto = "José Gigena 1900, Córdoba, Argentina",
                        Latitud = -31.371443,
                        LatitudTexto = "-31.371443",
                        Longitud = -64.236231,
                        LongitudTexto = "-64.236231"
                    }
                }, // input para que el empleado 4 pueda llevar al 7, ignorando al 5 por horario, 6 por ubicación. 
                new EmpleadoModel()  {
                    Nombre = "Lula",
                    ClaveEncriptada = EncriptadoHandler.Encriptar("empleado8@centro"),
                    CorreoElectronicoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("empleado8@centro.com")),
                    Roles = RolesEmpleadoFlag.CONDUCTOR,
                    TelefonoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("3515165507")),
                    EmpresaModel_ID = empresas.FirstOrDefault(e => e.Nombre == "Demo").ID,
                    Sucursal = sucursales.FirstOrDefault(s => s.Nombre == "Casa Central"),
                    SucursalModel_ID = sucursales.FirstOrDefault(s => s.Nombre == "Casa Central").ID,
                    Vehiculo = new VehiculoModel() {
                        Patente = "MOP237",
                        AsientosLibres = 1,
                        FechaVencimientoCarnetConducir = DateTime.Today.AddDays(120),
                        ComprobanteCarnetValidado = true,
                        FechaVencimientoComprobantePoliza = DateTime.Today.AddDays(120),
                        ComprobantePolizaValidado = true,
                        EmpresaModel_ID = empresas.FirstOrDefault(e => e.Nombre == "Demo").ID
                    },
                    HorarioIngreso = new TimeSpan(10,0,0),
                    HorarioSalida = new TimeSpan(18,0,0),
                    Ubicacion = new UbicacionModel() {
                        UbicacionTexto = "Genaro Pérez 2512, Córdoba, Argentina",
                        Latitud = -31.439718,
                        LatitudTexto = "-31.439718",
                        Longitud = -64.178710,
                        LongitudTexto = "-64.178710"
                    }
                },
                new EmpleadoModel()  {
                    Nombre = "Mara",
                    ClaveEncriptada = EncriptadoHandler.Encriptar("empleado9@centro"),
                    CorreoElectronicoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("empleado9@centro.com")),
                    Roles = RolesEmpleadoFlag.EMPLEADO,
                    TelefonoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("3515165507")),
                    EmpresaModel_ID = empresas.FirstOrDefault(e => e.Nombre == "Demo").ID,
                    Sucursal = sucursales.FirstOrDefault(s => s.Nombre == "Casa Central"),
                    SucursalModel_ID = sucursales.FirstOrDefault(s => s.Nombre == "Casa Central").ID,
                    HorarioIngreso = new TimeSpan(10,0,0),
                    HorarioSalida = new TimeSpan(18,0,0),
                    Ubicacion = new UbicacionModel() {
                        UbicacionTexto = "Monserrat 2413, Córdoba, Argentina",
                        Latitud = -31.437220,
                        LatitudTexto = "-31.437220",
                        Longitud = -64.167457,
                        LongitudTexto = "-64.167457"
                    }
                },
                new EmpleadoModel()  {
                    Nombre = "Zara",
                    ClaveEncriptada = EncriptadoHandler.Encriptar("empleado10@centro"),
                    CorreoElectronicoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("empleado10@centro.com")),
                    Roles = RolesEmpleadoFlag.EMPLEADO,
                    TelefonoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("3515165507")),
                    EmpresaModel_ID = empresas.FirstOrDefault(e => e.Nombre == "Demo").ID,
                    Sucursal = sucursales.FirstOrDefault(s => s.Nombre == "Casa Central"),
                    SucursalModel_ID = sucursales.FirstOrDefault(s => s.Nombre == "Casa Central").ID,
                    HorarioIngreso = new TimeSpan(10,0,0),
                    HorarioSalida = new TimeSpan(18,0,0),
                    Ubicacion = new UbicacionModel() {
                        UbicacionTexto = "Vigo 2139, Córdoba, Argentina",
                        Latitud = -31.432567,
                        LatitudTexto = "-31.432567",
                        Longitud = -64.166792,
                        LongitudTexto = "-64.166792"
                    }
                },
                new EmpleadoModel()  {
                    Nombre = "Zara",
                    ClaveEncriptada = EncriptadoHandler.Encriptar("empleado11@centro"),
                    CorreoElectronicoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("empleado11@centro.com")),
                    Roles = RolesEmpleadoFlag.EMPLEADO,
                    TelefonoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("3515165507")),
                    EmpresaModel_ID = empresas.FirstOrDefault(e => e.Nombre == "Demo").ID,
                    Sucursal = sucursales.FirstOrDefault(s => s.Nombre == "Casa Central"),
                    SucursalModel_ID = sucursales.FirstOrDefault(s => s.Nombre == "Casa Central").ID,
                    HorarioIngreso = new TimeSpan(10,0,0),
                    HorarioSalida = new TimeSpan(18,0,0),
                    Ubicacion = new UbicacionModel() {
                        UbicacionTexto = "C. San Luis 93, Córdoba, Argentina",
                        Latitud = -31.422838,
                        LatitudTexto = "-31.422838",
                        Longitud = -64.188344,
                        LongitudTexto = "-64.188344"
                    }
                // input para que el 8 ignore al 9 y 10 por quedarle más lejos del destino que su lugar actual, ruta para casa central
                },
                new EmpleadoModel()  {
                    Nombre = "ConductorNvaCba",
                    ClaveEncriptada = EncriptadoHandler.Encriptar("empleado12@nuevacba"),
                    CorreoElectronicoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("empleado12@nuevacba.com")),
                    Roles = RolesEmpleadoFlag.EMPLEADO,
                    TelefonoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("3515165507")),
                    EmpresaModel_ID = empresas.FirstOrDefault(e => e.Nombre == "Demo").ID,
                    Sucursal = sucursales.FirstOrDefault(s => s.Nombre == "Nueva Córdoba"),
                    SucursalModel_ID = sucursales.FirstOrDefault(s => s.Nombre == "Nueva Córdoba").ID,
                    HorarioIngreso = new TimeSpan(7,0,0),
                    HorarioSalida = new TimeSpan(16,0,0),
                    Vehiculo = new VehiculoModel() {
                        Patente = "LOS 404",
                        AsientosLibres = 3,
                        FechaVencimientoCarnetConducir = DateTime.Today.AddDays(120),
                        ComprobanteCarnetValidado = true,
                        FechaVencimientoComprobantePoliza = DateTime.Today.AddDays(120),
                        ComprobantePolizaValidado = true,
                        EmpresaModel_ID = empresas.FirstOrDefault(e => e.Nombre == "Demo").ID
                    },Ubicacion = new UbicacionModel() {
                        UbicacionTexto = "Camilo Isleño 4547, Córdoba, Argentina",
                        Latitud = -31.430517,
                        LatitudTexto = "-31.430517",
                        Longitud = -64.246280,
                        LongitudTexto = "-64.246280"
                    }
                },
                new EmpleadoModel()  {
                    Nombre = "CercanoHaciaNvaCba",
                    ClaveEncriptada = EncriptadoHandler.Encriptar("empleado13@nuevacba"),
                    CorreoElectronicoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("empleado13@nuevacba.com")),
                    Roles = RolesEmpleadoFlag.EMPLEADO,
                    TelefonoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("3515165507")),
                    EmpresaModel_ID = empresas.FirstOrDefault(e => e.Nombre == "Demo").ID,
                    Sucursal = sucursales.FirstOrDefault(s => s.Nombre == "Nueva Córdoba"),
                    SucursalModel_ID = sucursales.FirstOrDefault(s => s.Nombre == "Nueva Córdoba").ID,
                    HorarioIngreso = new TimeSpan(7,0,0),
                    HorarioSalida = new TimeSpan(16,0,0),
                    Ubicacion = new UbicacionModel() {
                        UbicacionTexto = "Francisco de Arteaga 1629, Córdoba, Argentina",
                        Latitud = -31.427212,
                        LatitudTexto = "-31.427212",
                        Longitud = -64.251562,
                        LongitudTexto = "-64.251562"
                    }
                },
                new EmpleadoModel()  {
                    Nombre = "CercanoHaciaNvaCba2",
                    ClaveEncriptada = EncriptadoHandler.Encriptar("empleado14@nuevacba"),
                    CorreoElectronicoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("empleado14@nuevacba.com")),
                    Roles = RolesEmpleadoFlag.EMPLEADO,
                    TelefonoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("3515165507")),
                    EmpresaModel_ID = empresas.FirstOrDefault(e => e.Nombre == "Demo").ID,
                    Sucursal = sucursales.FirstOrDefault(s => s.Nombre == "Nueva Córdoba"),
                    SucursalModel_ID = sucursales.FirstOrDefault(s => s.Nombre == "Nueva Córdoba").ID,
                    HorarioIngreso = new TimeSpan(7,0,0),
                    HorarioSalida = new TimeSpan(16,0,0),
                    Ubicacion = new UbicacionModel() {
                        UbicacionTexto = "Esmeralda 4631, Córdoba, Argentina",
                        Latitud = -31.438949,
                        LatitudTexto = "-31.438949",
                        Longitud = -64.247357,
                        LongitudTexto = "-64.247357"
                    }
                },
                new EmpleadoModel()  {
                    Nombre = "EstandarNvaCba1",
                    ClaveEncriptada = EncriptadoHandler.Encriptar("empleado15@nuevacba"),
                    CorreoElectronicoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("empleado15@nuevacba.com")),
                    Roles = RolesEmpleadoFlag.EMPLEADO,
                    TelefonoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("3515165507")),
                    EmpresaModel_ID = empresas.FirstOrDefault(e => e.Nombre == "Demo").ID,
                    Sucursal = sucursales.FirstOrDefault(s => s.Nombre == "Nueva Córdoba"),
                    SucursalModel_ID = sucursales.FirstOrDefault(s => s.Nombre == "Nueva Córdoba").ID,
                    HorarioIngreso = new TimeSpan(7,0,0),
                    HorarioSalida = new TimeSpan(16,0,0),
                    Ubicacion = new UbicacionModel() {
                        UbicacionTexto = "Sol de Mayo 1468, Córdoba, Argentina",
                        Latitud = -31.427871,
                        LatitudTexto = "-31.427871",
                        Longitud = -64.213450,
                        LongitudTexto = "-64.213450"
                    }
                },
                new EmpleadoModel()  {
                    Nombre = "EstandarNvaCba3",
                    ClaveEncriptada = EncriptadoHandler.Encriptar("empleado16@nuevacba"),
                    CorreoElectronicoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("empleado16@nuevacba.com")),
                    Roles = RolesEmpleadoFlag.EMPLEADO,
                    TelefonoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("3515165507")),
                    EmpresaModel_ID = empresas.FirstOrDefault(e => e.Nombre == "Demo").ID,
                    Sucursal = sucursales.FirstOrDefault(s => s.Nombre == "Nueva Córdoba"),
                    SucursalModel_ID = sucursales.FirstOrDefault(s => s.Nombre == "Nueva Córdoba").ID,
                    HorarioIngreso = new TimeSpan(7,0,0),
                    HorarioSalida = new TimeSpan(16,0,0),
                    Ubicacion = new UbicacionModel() {
                        UbicacionTexto = "Brasil 681, Córdoba, Argentina",
                        Latitud = -31.426351,
                        LatitudTexto = "-31.426351",
                        Longitud = -64.198358,
                        LongitudTexto = "-64.198358"
                    }
                },
                new EmpleadoModel()  {
                    Nombre = "EstandarNvaCba2ConAuto",
                    ClaveEncriptada = EncriptadoHandler.Encriptar("empleado17@nuevacba"),
                    CorreoElectronicoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("empleado17@nuevacba.com")),
                    Roles = RolesEmpleadoFlag.EMPLEADO,
                    TelefonoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("3515165507")),
                    EmpresaModel_ID = empresas.FirstOrDefault(e => e.Nombre == "Demo").ID,
                    Sucursal = sucursales.FirstOrDefault(s => s.Nombre == "Nueva Córdoba"),
                    SucursalModel_ID = sucursales.FirstOrDefault(s => s.Nombre == "Nueva Córdoba").ID,
                    HorarioIngreso = new TimeSpan(7,0,0),
                    HorarioSalida = new TimeSpan(16,0,0),
                    Vehiculo = new VehiculoModel() {
                        Patente = "AE 477 IRR",
                        AsientosLibres = 2,
                        FechaVencimientoCarnetConducir = DateTime.Today.AddDays(120),
                        ComprobanteCarnetValidado = true,
                        FechaVencimientoComprobantePoliza = DateTime.Today.AddDays(120),
                        ComprobantePolizaValidado = true,
                        EmpresaModel_ID = empresas.FirstOrDefault(e => e.Nombre == "Demo").ID
                    },
                    Ubicacion = new UbicacionModel() {
                        UbicacionTexto = "Emilio Achával 1315, Córdoba, Argentina",
                        Latitud = -31.427434,
                        LatitudTexto = "-31.427434",
                        Longitud = -64.206001,
                        LongitudTexto = "-64.206001"
                    }
                },
                new EmpleadoModel()  {
                    Nombre = "CercanoNuevaCba",
                    ClaveEncriptada = EncriptadoHandler.Encriptar("empleado18@nuevacba"),
                    CorreoElectronicoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("empleado18@nuevacba.com")),
                    Roles = RolesEmpleadoFlag.EMPLEADO,
                    TelefonoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("3515165507")),
                    EmpresaModel_ID = empresas.FirstOrDefault(e => e.Nombre == "Demo").ID,
                    Sucursal = sucursales.FirstOrDefault(s => s.Nombre == "Nueva Córdoba"),
                    SucursalModel_ID = sucursales.FirstOrDefault(s => s.Nombre == "Nueva Córdoba").ID,
                    HorarioIngreso = new TimeSpan(7,0,0),
                    HorarioSalida = new TimeSpan(16,0,0),
                    Ubicacion = new UbicacionModel() {
                        UbicacionTexto = "Fructuoso Rivera 181, Córdoba, Argentina",
                        Latitud = -31.426014,
                        LatitudTexto = "-31.426014",
                        Longitud = -64.190848,
                        LongitudTexto = "-64.190848"
                    }
                }
            };

            foreach (var item in empleados)
            {
                if (item.Vehiculo != null)
                    item.Vehiculo.Empleado_ID = item.ID;
                item.DistanciaSucursal = RecorridoHandler.CalcularDistancia(item.Ubicacion, item.Sucursal.Ubicacion);
            }

            empleados.ForEach(e => context.Empleados.Add(e));

            context.Empleados.Add(
                new EmpleadoModel()
                {
                    Nombre = "Admin",
                    ClaveEncriptada = EncriptadoHandler.Encriptar("123123Aa!"),
                    CorreoElectronicoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar("admin@admin.com")),
                    Roles = RolesEmpleadoFlag.ADMINISTRADOR,
                    EmpresaModel_ID = empresas.FirstOrDefault(e => e.Nombre == "Admin").ID,
                    SucursalModel_ID = sucursales.FirstOrDefault(s => s.Nombre == "Admin").ID,
                    DistanciaSucursal = 0,
                    HorarioIngreso = new TimeSpan(23, 0, 0),
                    HorarioSalida = new TimeSpan(23, 0, 0),
                    Ubicacion = new UbicacionModel()
                    {
                        UbicacionTexto = "Alfredo Garcia Voglino 5457, Cordoba, Argentina",
                        Latitud = -0.0,
                        LatitudTexto = "0.0",
                        Longitud = -0.0,
                        LongitudTexto = "0.0"
                    }
                });

            context.SaveChanges();
            #endregion
        }
    }
}