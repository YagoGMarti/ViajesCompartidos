using NUnit.Framework;
using SistemaViajesCompartidos.Enums;
using SistemaViajesCompartidos.Models;
using System;
using ViajesCompartidos.Handlers;

namespace ViajesCompartidos.Tests.Handler
{
    internal class RecorridoHandlerTest
    {
        Guid empleado_ID;

        [SetUp]
        public void Start()
        {
            //empleado_ID = new Handlers.CorreoElectronicoHandler();
        }

        [Test]
        [TestCase(EstrategiaRutaEnum.Estandar)]
        [TestCase(EstrategiaRutaEnum.SoloCercanosDomicilio)]
        [TestCase(EstrategiaRutaEnum.SoloMasCercano)]
        [TestCase(EstrategiaRutaEnum.SinConductores)]
        public void PruebaEstrategia(EstrategiaRutaEnum estrategia)
        {
            RecorridoModel recorrido;
            recorrido = RecorridoHandler.ObtenerRecorrido(empleado_ID, estrategia);
        }
    }
}
