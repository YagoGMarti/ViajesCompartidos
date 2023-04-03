using SistemaViajesCompartidos.Enums;
using System;

namespace ViajesCompartidos.Models.Temporal
{
    public class UsuarioFlattened
    {
        public UsuarioFlattened(Guid usuarioID, Guid empresaID)
        {
            UsuarioID = usuarioID;
            EmpresaID = empresaID;
        }

        public Guid UsuarioID { get; set; }
        public Guid EmpresaID { get; set; }
    }
}