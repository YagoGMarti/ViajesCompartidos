using SistemaViajesCompartidos.Context;
using SistemaViajesCompartidos.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ViajesCompartidos.Handlers
{
    public class EmpresaHandler : BaseHandler
    {
        public static IEnumerable<EmpresaModel> GetEmpresas()
        {
            return ViajesCompartidosContext.GetEmpresas()
                .OrderBy(x => x.Activo).ThenBy(x => x.FechaAlta);
        }

        public static EmpresaModel GetEmpresa(Guid ID)
        {
            var empresa = ViajesCompartidosContext.GetEmpresa(ID);

            if (empresa == null)
                return null;

            empresa.CorreoElectronico = EncriptadoHandler.DesEncriptar(EncriptadoHandler.StringToBytes(empresa.CorreoElectronicoEncriptado));
            return empresa;
        }

        public static void CrearEmpresa(EmpresaModel empresaModel)
        {
            empresaModel.CorreoElectronicoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar(empresaModel.CorreoElectronico));
            ViajesCompartidosContext.CrearEmpresa(empresaModel);
            SesionHandler.RestablecerClaveEmpresa(empresaModel.ID);
        }

        public static void EditarEmpresa(EmpresaModel empresaModel)
        {
            var empresa = ViajesCompartidosContext.GetEmpresa(empresaModel.ID);
            empresaModel.CorreoElectronicoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar(empresaModel.CorreoElectronico));
            ViajesCompartidosContext.EditarEmpresa(empresaModel);

            if (empresa.CorreoElectronicoEncriptado != empresaModel.CorreoElectronicoEncriptado)
                SesionHandler.RestablecerClaveEmpresa(empresaModel.ID);
        }

        internal static void CambiarEstadoActivo(Guid ID, bool estado)
        {
            ViajesCompartidosContext.CambiarEstadoActivoEmpresa(ID, estado);
        }

        internal static Guid GetEmpresaBySucursal(Guid sucursalID)
        {
            return ViajesCompartidosContext.GetEmpresaBySucursal(sucursalID);
        }

        public static Tuple<string, string> ReiniciarClave(Guid empresaID)
        {
            string clave = Guid.NewGuid().ToString();
            return ReiniciarClave(empresaID, clave);
        }

        public static Tuple<string, string> ReiniciarClave(Guid empresaID, string clave)
        {
            var claveEncriptada = EncriptadoHandler.Encriptar(clave);
            var email = ViajesCompartidosContext.ReiniciarClaveEmpresa(empresaID, claveEncriptada);
            return new Tuple<string, string>(email, clave);
        }
    }
}