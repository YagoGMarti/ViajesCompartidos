using SistemaViajesCompartidos.Context;
using SistemaViajesCompartidos.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ViajesCompartidos.Handlers
{
    public class ContactoHandler : BaseHandler
    {
        public static bool GrabarContacto(ContactoModel contactoModel)
        {
            try
            {
                contactoModel.CorreoElectronicoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar(contactoModel.CorreoElectronico));
                ViajesCompartidosContext.GrabarContacto(contactoModel);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void MarcarContactoRealizado(Guid ID)
        {
            ViajesCompartidosContext.MarcarContactoRealizado(ID);
        }

        public static IEnumerable<ContactoModel> GetContactos(bool filtrarProcesados)
        {
            var contactos = ViajesCompartidosContext.GetContactos(filtrarProcesados);
            foreach (var contacto in contactos)
            {
                contacto.CorreoElectronico = EncriptadoHandler.DesEncriptar(EncriptadoHandler.StringToBytes(contacto.CorreoElectronicoEncriptado));
            }
            return contactos.OrderBy(x => x.Procesado).ThenBy(x => x.Fecha);
        }
    }
}