using SistemaViajesCompartidos.Context;
using SistemaViajesCompartidos.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViajesCompartidos.Handlers
{
    public class RutaHandler : BaseHandler
    {
        public static RecorridoModel ObtenerRecorrido(Guid Empleado_ID)
        {
            EmpleadoModel conductor = EmpleadoHandler.GetEmpleado(Empleado_ID);

            if (conductor.Vehiculo == null || !conductor.Vehiculo.ValidoRuta)
                return null;

            int asientos = conductor.Vehiculo.AsientosLibres;

            var sucursal = SucursalHandler.GetSucursal(conductor.SucursalModel_ID);
            var pasajeros = EmpleadoHandler.GetEmpleados(conductor.EmpresaModel_ID, conductor.SucursalModel_ID);

            int signoCordenadaX = (conductor.Ubicacion.Longitud - sucursal.Ubicacion.Longitud) > 0 ? 1 : -1;
            int signoCordenadaY = (conductor.Ubicacion.Latitud - sucursal.Ubicacion.Latitud) > 0 ? 1 : -1;

            pasajeros = pasajeros.Where(x => x.Ubicacion != null && x.DistanciaSucursal > 0).OrderByDescending(x => x.DistanciaSucursal);
            pasajeros = ReducirLista(pasajeros, conductor.Ubicacion, sucursal.Ubicacion, signoCordenadaX, signoCordenadaY);

            RecorridoModel recorrido = new RecorridoModel()
            {
                EmpresaId = conductor.EmpresaModel_ID,
                Conductor = conductor,
                Sucursal = sucursal,
                EstadoRecorrido = SistemaViajesCompartidos.Enums.EstadoRecorridoEnum.CREADO
            };


            if (pasajeros.Any())
            {
                recorrido.Ubicaciones.Add(conductor.Ubicacion);

                var ultimoPasajero = pasajeros.FirstOrDefault();
                recorrido.Pasajeros.Add(ultimoPasajero);
                asientos--;

                while (asientos > 0)
                {
                    pasajeros = ReducirLista(pasajeros, ultimoPasajero.Ubicacion, sucursal.Ubicacion, signoCordenadaX, signoCordenadaY);

                    ultimoPasajero = pasajeros.FirstOrDefault();
                    recorrido.Pasajeros.Add(ultimoPasajero);
                    asientos--;
                }

                recorrido.Ubicaciones.Add(sucursal.Ubicacion);
            }


            return recorrido;
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