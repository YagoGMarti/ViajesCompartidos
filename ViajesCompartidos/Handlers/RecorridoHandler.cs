using SistemaViajesCompartidos.Context;
using SistemaViajesCompartidos.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViajesCompartidos.Models.Transactional;

namespace ViajesCompartidos.Handlers
{
    public class RecorridoHandler : BaseHandler
    {
        public static void RecorridoAceptado(RecorridoModel recorrido)
        {
            recorrido.EstadoRecorrido = SistemaViajesCompartidos.Enums.EstadoRecorridoEnum.ACEPTADO;
            recorrido.RecorridoEmpleado = recorrido.Pasajeros.Select((x,index) => new RecorridoEmpleado(recorrido.ID, x.ID, index)).ToList();
            recorrido.RecorridoUbicacion = recorrido.Ubicaciones.Select((x, index) => new RecorridoUbicacion(recorrido.ID, x.ID, index)).ToList();

            ViajesCompartidosContext.GrabarRecorrido(recorrido);
            ViajesCompartidosContext.GrabarRecorrido(recorrido.Conductor_ID, recorrido.ID);

            foreach (var recorridoEmpleado in recorrido.RecorridoEmpleado)
            {
                ViajesCompartidosContext.GrabarRecorrido(recorridoEmpleado.Empleado_ID, recorridoEmpleado.Recorrido_ID);
            }
        }

        public static RecorridoModel GetRecorrido(Guid recorrido_ID)
        {
            var recorrido = ViajesCompartidosContext.GetRecorrido(recorrido_ID);

            //recorrido.Emps_ID.ForEach(x => recorrido.Pasajeros.Add(EmpleadoHandler.GetEmpleado(x)));
            //recorrido.Ubis_ID.ForEach(x => recorrido.Ubicaciones.Add(GetUbicacion(x)));
            return recorrido;
        }

        public static UbicacionModel GetUbicacion(Guid ID)
        {
            return ViajesCompartidosContext.GetUbicacion(ID);
        }


        public static void RechazarRecorrido(Guid recorrido_ID, Guid empleado_ID)
        {
            RecorridoModel recorrido = GetRecorrido(recorrido_ID);

            // TODO : lógica de rechazo 
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

        public static RecorridoModel ObtenerRecorrido(Guid Empleado_ID)
        {
            EmpleadoModel conductor = EmpleadoHandler.GetEmpleado(Empleado_ID);

            if (conductor.Vehiculo == null || !conductor.Vehiculo.ValidoRuta)
                return null;

            int asientos = conductor.Vehiculo.AsientosLibres;

            var sucursal = SucursalHandler.GetSucursal(conductor.SucursalModel_ID);
            // TODO : primer búsqueda traer sólo aquellos a menor distancia que el conductor 
            // TODO : primer búsqueda, acotar a sólo aquellos en la región buscada, o al menos descartar más items. 
            var pasajeros = EmpleadoHandler.GetEmpleados(conductor.EmpresaModel_ID, conductor.SucursalModel_ID);
            pasajeros = pasajeros.Where(x => x.Ubicacion != null && x.DistanciaSucursal > 0 && x.Activo);
            pasajeros = pasajeros.Where(x => x.Horario == conductor.Horario).OrderByDescending(x => x.DistanciaSucursal);

            // TODO : Ver de variar a polígonos más pequeños y acotados, la mitad del cuadrado buscado se desperdicia. 
            // TODO : sumar un primer ciclo de búsqueda a N cuadras a la redonda del conductor con prioridad "vecinos" 

            int signoCordenadaX = (conductor.Ubicacion.Longitud - sucursal.Ubicacion.Longitud) > 0 ? 1 : -1;
            int signoCordenadaY = (conductor.Ubicacion.Latitud - sucursal.Ubicacion.Latitud) > 0 ? 1 : -1;

            pasajeros = ReducirLista(pasajeros, conductor.Ubicacion, sucursal.Ubicacion, signoCordenadaX, signoCordenadaY);

            RecorridoModel recorrido = new RecorridoModel()
            {
                Empresa_ID = conductor.EmpresaModel_ID,
                EstadoRecorrido = SistemaViajesCompartidos.Enums.EstadoRecorridoEnum.CREADO,
                Sucursal_ID = sucursal.ID,
                Conductor_ID = conductor.ID
            };

            if (pasajeros.Any())
            {
                conductor.Ubicacion.TipoUbicacion = Enums.TipoUbicacionEnum.Inicio;
                recorrido.Ubicaciones.Add(conductor.Ubicacion);

                var ultimoPasajero = pasajeros.FirstOrDefault();
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

                sucursal.Ubicacion.TipoUbicacion = Enums.TipoUbicacionEnum.Destino;
                recorrido.Ubicaciones.Add(sucursal.Ubicacion);
                recorrido.LatitudCentro = (conductor.Ubicacion.Latitud - sucursal.Ubicacion.Latitud) / 2;
                recorrido.LongitudCentro = (conductor.Ubicacion.Longitud - sucursal.Ubicacion.Longitud) / 2;
            }

            return recorrido;
        }

        internal static void CancelarRuta(Guid recorrido_ID)
        {
            ViajesCompartidosContext.CancelarRuta(recorrido_ID);
        }

        internal static void RemoverPasajero(Guid recorrido_ID, Guid pasajero_ID)
        {
            EmpleadoModel pasajero = EmpleadoHandler.GetEmpleado(pasajero_ID);
            ViajesCompartidosContext.RemoverPasajero(recorrido_ID, pasajero_ID);
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