﻿using SistemaViajesCompartidos.Context;
using SistemaViajesCompartidos.Enums;
using SistemaViajesCompartidos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using SistemaViajesCompartidos.Models.Transactional;

namespace ViajesCompartidos.Handlers
{
    public class RecorridoHandler : BaseHandler
    {
        private static CorreoElectronicoHandler _mails = new CorreoElectronicoHandler();

        public static void RecorridoAceptado(RecorridoModel recorrido)
        {
            recorrido.EstadoRecorrido = EstadoRecorridoEnum.ACEPTADO;
            recorrido.RecorridoEmpleado = recorrido.Pasajeros.Select((x, index) => new RecorridoEmpleado(recorrido.ID, x.ID, index)).ToList();
            recorrido.RecorridoUbicacion = recorrido.Ubicaciones.Select((x, index) => new RecorridoUbicacion(recorrido.ID, x.ID, index)).ToList();

            // sacarlo de otro recorrido, si tiene recorrido. 
            var conductor = EmpleadoHandler.GetEmpleado(recorrido.Conductor_ID);
            if (conductor.RecorridoModel_ID.HasValue && conductor.Recorrido != null)
                RemoverPasajero(conductor.RecorridoModel_ID, recorrido.Conductor_ID);

            ViajesCompartidosContext.GrabarRecorrido(recorrido);
            ViajesCompartidosContext.GrabarRecorrido(recorrido.Conductor_ID, recorrido.ID);

            foreach (var recorridoEmpleado in recorrido.RecorridoEmpleado)
            {
                ViajesCompartidosContext.GrabarRecorrido(recorridoEmpleado.Empleado_ID, recorridoEmpleado.Recorrido_ID);
                EmpleadoModel pasajero = EmpleadoHandler.GetEmpleado(recorridoEmpleado.Empleado_ID);
                _mails.EnviarCorreoElectronico(pasajero.CorreoElectronico, pasajero.Nombre, SistemaViajesCompartidos.Enums.TipoCorreoEnum.NuevaRuta);
            }
        }

        public static RecorridoModel GetRecorrido(Guid? recorrido_ID)
        {
            if (recorrido_ID.HasValue)
            {
                var recorrido = ViajesCompartidosContext.GetRecorrido(recorrido_ID.Value);
                return recorrido;
            }

            return null;
        }

        public static IEnumerable<RecorridoModel> GetRecorridosPorSucursal(Guid sucursal_ID)
        {
            var recorridos = ViajesCompartidosContext.GetRecorridosPorSucursal(sucursal_ID);
            return recorridos;
        }

        public static IEnumerable<RecorridoModel> GetRecorridosPorEmpresa(Guid empresa_ID)
        {
            var recorridos = ViajesCompartidosContext.GetRecorridosPorEmpresa(empresa_ID);
            return recorridos;
        }

        public static void RechazarRecorrido(Guid recorrido_ID, Guid empleado_ID)
        {
            RecorridoModel recorrido = GetRecorrido(recorrido_ID);

            ViajesCompartidosContext.BorrarRecorrido(recorrido.Conductor_ID);

            foreach (var pasajero in recorrido.Pasajeros)
            {
                ViajesCompartidosContext.BorrarRecorrido(pasajero.ID);
            }
        }

        public static RecorridoModel RemoverUbicacion(RecorridoModel recorrido, Guid removerUbicacion)
        {
            recorrido.Ubicaciones = recorrido.Ubicaciones.Where(x => x.ID != removerUbicacion).ToList();
            recorrido.Pasajeros = recorrido.Pasajeros.Where(x => x.Ubicacion.ID != removerUbicacion).ToList();

            return recorrido;
        }

        public static RecorridoModel ObtenerRecorrido(Guid Empleado_ID, EstrategiaRutaEnum estrategiaRutaEnum)
        {
            EmpleadoModel conductor = EmpleadoHandler.GetEmpleado(Empleado_ID);

            if (conductor.Vehiculo == null || !conductor.Vehiculo.ValidoRuta)
                return null;

            int asientos = conductor.Vehiculo.AsientosLibres;

            var sucursal = SucursalHandler.GetSucursal(conductor.SucursalModel_ID);
            var pasajeros = EmpleadoHandler.GetEmpleados(null, conductor.SucursalModel_ID);
            pasajeros = pasajeros.Where(x => x.Ubicacion != null && x.DistanciaSucursal > 0 && x.Activo);

            pasajeros = pasajeros.Where(x => x.Recorrido == null || !x.Recorrido.Activo);
            pasajeros = pasajeros.Where(x => x.Recorrido == null || x.Recorrido.Conductor_ID == Empleado_ID);

            pasajeros = pasajeros.Where(x => x.Horario == conductor.Horario).OrderByDescending(x => x.DistanciaSucursal);
            pasajeros = pasajeros.Where(x => x.ID != Empleado_ID);

            if (estrategiaRutaEnum == EstrategiaRutaEnum.SinConductores)
            {
                // descarta a quienes tienen un vehículo. 
                pasajeros = pasajeros.Where(x => !x.TieneVehiculo);
            }

            // TODO : Ver de variar a polígonos más pequeños y acotados. 
            // TODO : sumar un primer ciclo de búsqueda a N cuadras a la redonda del conductor con prioridad "vecinos" 

            int signoCordenadaX = (conductor.Ubicacion.Longitud - sucursal.Ubicacion.Longitud) > 0 ? 1 : -1;
            int signoCordenadaY = (conductor.Ubicacion.Latitud - sucursal.Ubicacion.Latitud) > 0 ? 1 : -1;

            RecorridoModel recorrido = new RecorridoModel()
            {
                Empresa_ID = conductor.EmpresaModel_ID,
                EstadoRecorrido = EstadoRecorridoEnum.CREADO,
                Sucursal_ID = sucursal.ID,
                Conductor_ID = conductor.ID
            };

            conductor.Ubicacion.TipoUbicacion = Enums.TipoUbicacionEnum.Inicio;
            recorrido.Ubicaciones.Add(conductor.Ubicacion);

            if (estrategiaRutaEnum == EstrategiaRutaEnum.SoloCercanosDomicilio)
            {
                // carga todos los cercanos, en proximidades, luego remueve por ID. 
                double deltaDistancia = 0.0075;
                var cercanos = BuscarCercanos(pasajeros, conductor.Ubicacion, deltaDistancia).ToList();
                cercanos.AddRange(BuscarCercanos(pasajeros, conductor.Ubicacion, deltaDistancia * 2).ToList());
                //cercanos.AddRange(BuscarCercanos(pasajeros, conductor.Ubicacion, deltaDistancia * 4, signoCordenadaX, signoCordenadaY).ToList());

                while (asientos > 0 && cercanos.Any())
                {
                    var ultimoPasajero = cercanos.FirstOrDefault();
                    recorrido.Ubicaciones.Add(ultimoPasajero.Ubicacion);
                    recorrido.Pasajeros.Add(ultimoPasajero);
                    cercanos = cercanos.Where(x => x.ID != ultimoPasajero.ID).ToList();
                    asientos--;
                }
            }
            else
            {
                pasajeros = ReducirLista(pasajeros, conductor.Ubicacion, sucursal.Ubicacion, signoCordenadaX, signoCordenadaY);
                if (pasajeros.Any())
                {
                    var ultimoPasajero = pasajeros.FirstOrDefault();

                    if (estrategiaRutaEnum == EstrategiaRutaEnum.SoloMasCercano)
                    {
                        // sólo obtener al mas cercano a la sucursal compatible. 
                        ultimoPasajero = pasajeros.OrderByDescending(x => x.DistanciaSucursal).LastOrDefault();
                        asientos = 1;
                    }

                    recorrido.Ubicaciones.Add(ultimoPasajero.Ubicacion);
                    recorrido.Pasajeros.Add(ultimoPasajero);
                    asientos--;
                    pasajeros = ReducirLista(pasajeros, ultimoPasajero.Ubicacion, sucursal.Ubicacion, signoCordenadaX, signoCordenadaY);

                    while (asientos > 0 && pasajeros.Any())
                    {
                        ultimoPasajero = pasajeros.FirstOrDefault();
                        recorrido.Ubicaciones.Add(ultimoPasajero.Ubicacion);
                        recorrido.Pasajeros.Add(ultimoPasajero);
                        asientos--;
                        pasajeros = ReducirLista(pasajeros, ultimoPasajero.Ubicacion, sucursal.Ubicacion, signoCordenadaX, signoCordenadaY);
                    }
                }
            }

            sucursal.Ubicacion.TipoUbicacion = Enums.TipoUbicacionEnum.Destino;
            recorrido.Ubicaciones.Add(sucursal.Ubicacion);
            recorrido.LatitudCentro = (conductor.Ubicacion.Latitud - sucursal.Ubicacion.Latitud) / 2;
            recorrido.LongitudCentro = (conductor.Ubicacion.Longitud - sucursal.Ubicacion.Longitud) / 2;
            recorrido.EstrategiaRecorrido = estrategiaRutaEnum;

            return recorrido;
        }

        internal static void CancelarRuta(Guid? recorrido_ID)
        {
            RecorridoModel recorrido = GetRecorrido(recorrido_ID);
            foreach (var pasajero in recorrido.Pasajeros)
            {
                pasajero.CorreoElectronico = EncriptadoHandler.DesEncriptar(EncriptadoHandler.StringToBytes(pasajero.CorreoElectronicoEncriptado));
                _mails.EnviarCorreoElectronico(pasajero.CorreoElectronico, pasajero.Nombre, SistemaViajesCompartidos.Enums.TipoCorreoEnum.RutaCancelada);
            }

            {
                var conductor = EmpleadoHandler.GetEmpleado(recorrido.Conductor_ID);
                _mails.EnviarCorreoElectronico(conductor.CorreoElectronico, conductor.Nombre, SistemaViajesCompartidos.Enums.TipoCorreoEnum.RutaCancelada);
            }

            ViajesCompartidosContext.CancelarRuta(recorrido_ID.Value);
        }

        internal static void RemoverPasajero(Guid? recorrido_ID, Guid pasajero_ID)
        {
            RecorridoModel recorrido = GetRecorrido(recorrido_ID);
            if (recorrido == null)
            {
                return;
            }

            if (recorrido.Conductor_ID == pasajero_ID
                || recorrido.Pasajeros.All(x => x.ID == pasajero_ID))
            {
                CancelarRuta(recorrido_ID);
            }
            else
            {
                EmpleadoModel pasajero = EmpleadoHandler.GetEmpleado(pasajero_ID);
                _mails.EnviarCorreoElectronico(pasajero.CorreoElectronico, pasajero.Nombre, SistemaViajesCompartidos.Enums.TipoCorreoEnum.DesasociadoRuta);
                ViajesCompartidosContext.RemoverPasajero(recorrido_ID.Value, pasajero_ID);
            }
        }

        internal static IEnumerable<EmpleadoModel> ReducirLista(IEnumerable<EmpleadoModel> pasajeros, UbicacionModel ultimoPuntoUbicacion, UbicacionModel sucursalUbicacion
            , int signoCordenadaX, int signoCordenadaY)
        {
            if (signoCordenadaX > 0)
            {
                // como X es positivo, descarta todo lo que está en el rango negativo usando a la sucursal como coordenada {0,0} de referencia
                pasajeros = pasajeros.Where(x => x.Ubicacion.Longitud > sucursalUbicacion.Longitud).ToList();
                // descarta todo en X más lejos de la sucursal que el conductor
                pasajeros = pasajeros.Where(x => x.Ubicacion.Longitud < ultimoPuntoUbicacion.Longitud).ToList();
            }
            else
            {
                // como X es negativo, descarta todo lo que está en el rango positivo usando a la sucursal como coordenada {0,0} de referencia
                pasajeros = pasajeros.Where(x => x.Ubicacion.Longitud < sucursalUbicacion.Longitud).ToList();
                // descarta todo en X más lejos de la sucursal que el conductor
                pasajeros = pasajeros.Where(x => x.Ubicacion.Longitud > ultimoPuntoUbicacion.Longitud).ToList();
            }

            if (signoCordenadaY > 0)
            {
                // como Y es positivo, descarta todo lo que está en el rango negativo usando a la sucursal como coordenada {0,0} de referencia
                pasajeros = pasajeros.Where(x => x.Ubicacion.Latitud > sucursalUbicacion.Latitud).ToList();
                // descarta todo en Y más lejos de la sucursal que el conductor
                pasajeros = pasajeros.Where(x => x.Ubicacion.Latitud < ultimoPuntoUbicacion.Latitud).ToList();
            }
            else
            {
                // como Y es negativo, descarta todo lo que está en el rango positivo usando a la sucursal como coordenada {0,0} de referencia
                pasajeros = pasajeros.Where(x => x.Ubicacion.Latitud < sucursalUbicacion.Latitud).ToList();
                // descarta todo en Y más lejos de la sucursal que el conductor
                pasajeros = pasajeros.Where(x => x.Ubicacion.Latitud > ultimoPuntoUbicacion.Latitud).ToList();
            }
            return pasajeros;
        }

        internal static IEnumerable<EmpleadoModel> BuscarCercanos(IEnumerable<EmpleadoModel> pasajeros, UbicacionModel ubicacionConductor, double deltaDistancia)
        {
            // descarta todo en X fuera del rango aceptable por el conductor
            pasajeros = pasajeros.Where(x => x.Ubicacion.Longitud > ubicacionConductor.Longitud - deltaDistancia).ToList();
            pasajeros = pasajeros.Where(x => x.Ubicacion.Longitud < ubicacionConductor.Longitud + deltaDistancia).ToList();

            // descarta todo en Y fuera del rango aceptable por el conductor
            pasajeros = pasajeros.Where(x => x.Ubicacion.Latitud > ubicacionConductor.Latitud - deltaDistancia).ToList();
            pasajeros = pasajeros.Where(x => x.Ubicacion.Latitud < ubicacionConductor.Latitud + deltaDistancia).ToList();

            return pasajeros;
        }

        public static double CalcularDistancia(UbicacionModel origen, UbicacionModel destino)
        {
            // se usa Math.Abs por si en algún momento se considera el sistema en otro lugar. 
            // para sólo Argentina, siempre Lat y Lang son negativas, por lo cual negativo - negativo va a dar un valor lógico aceptable. 
            double Base = Math.Pow(Math.Abs(origen.Longitud) - Math.Abs(destino.Longitud), 2);
            double Altura = Math.Pow(Math.Abs(origen.Latitud) - Math.Abs(destino.Latitud), 2);
            double Distancia = Math.Sqrt(Base + Altura); // hipotenusa
            return Distancia;
        }
    }
}