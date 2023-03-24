using NUnit.Framework;
using ViajesCompartidos.Handlers;

namespace ViajesCompartidos.Tests.Controllers
{
    [TestFixture]
    public class EncriptadoHandlerTest
    {
        [Test]
        [TestCase("correo@prueba.com")]
        [TestCase("viajescompartidosmails@gmail.com")]
        [TestCase("Clave123123")]
        public void Index(string texto)
        {
            byte[] bytesEncriptado = EncriptadoHandler.Encriptar(texto);
            string textoEncriptado = EncriptadoHandler.BytesToString(bytesEncriptado);
            byte[] bytesDesencriptado = EncriptadoHandler.StringToBytes(textoEncriptado);
            string textoDesencriptado = EncriptadoHandler.DesEncriptar(bytesDesencriptado);
            Assert.AreEqual(texto, textoDesencriptado);
        }
    }
}

