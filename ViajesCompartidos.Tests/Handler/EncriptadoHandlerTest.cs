using NUnit.Framework;
using ViajesCompartidos.Handlers;

namespace ViajesCompartidos.Tests.Controllers
{
    [TestFixture]
    public class EncriptadoHandlerTest
    {
        EncriptadoHandler _encriptadoHandler = new EncriptadoHandler();
        
        [Test]
        [TestCase("correo@prueba.com", "AxokAf6CYUeRMogrwQE5cQ==")]
        public void Index(string texto, string IV)
        {
            byte[] IVbytes = EncriptadoHandler.StringToBytes(IV);
            byte[] textoEncriptado = EncriptadoHandler.Encriptar(texto);

            string textoDesencriptado = EncriptadoHandler.DesEncriptar(textoEncriptado);
            string IVstring = EncriptadoHandler.BytesToString(IVbytes);

            Assert.AreEqual(texto, textoDesencriptado);
            Assert.AreEqual(IV, IVstring);
        }
    }
}