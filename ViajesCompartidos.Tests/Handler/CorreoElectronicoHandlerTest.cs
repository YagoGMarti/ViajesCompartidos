using NUnit.Framework;
using ViajesCompartidos.Handlers;

namespace ViajesCompartidos.Tests.Controllers
{
    [TestFixture]
    public class CorreoElectronicoHandlerTest
    {
        CorreoElectronicoHandler handler;

        [SetUp] 
        public void Start() {
            handler = new Handlers.CorreoElectronicoHandler();
        }

        [Test]
        [TestCase("viajescompartidosmails@gmail.com", "Clave123123")]
        public void Index(string cuenta, string clave)
        {
            Assert.AreEqual(handler.GetMailsEnabled, true);
            Assert.AreEqual(handler.GetCuenta, cuenta);
            Assert.AreEqual(handler.GetClave, clave);
        }
    }
}

