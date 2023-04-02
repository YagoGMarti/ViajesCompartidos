using NUnit.Framework;
using SistemaViajesCompartidos.Enums;
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
        [TestCase("viajescompartidosmails@gmail.com", "rdqfnpuakmfiprtm")]
        public void PruebaConfiguracion(string cuenta, string clave)
        {
            Assert.AreEqual(handler.GetMailsEnabled, true);
            Assert.AreEqual(handler.GetCuenta, cuenta);
            Assert.AreEqual(handler.GetClave, clave);
        }

        [Test]
        [TestCase("mail de prueba", TipoCorreoEnum.NuevaRuta)]
        public void PruebaEnvio(string destinatario, TipoCorreoEnum tipoCorreoEnum)
        {
            handler.EnviarCorreoElectronico(handler.GetCuenta, destinatario, tipoCorreoEnum);
        }
    }
}

