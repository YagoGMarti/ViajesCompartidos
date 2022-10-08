using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SistemaViajesCompartidos.Models
{
    [Table("Empresas")]
    public class EmpresaModel : BaseModel
    {
        public string Nombre { get; set; }
        public string RazonSocial { get; set; }
        public string CUIT { get; set; }
        public TipoEmpresaEnum TipoEmpresa { get; set; }

        [NotMapped]
        public HttpPostedFileBase Imagen { get; set; }
        public byte[] Logo { get; set; }
        public string FormatoImagenLogo { get; set; }

        private List<SucursalModel> Sucursales { get; set; }
        private List<EmpleadoModel> Empleados { get; set; }

        public List<SucursalModel> GetSucursales() { return this.Sucursales; }
        public List<EmpleadoModel> GetEmpleados() { return this.Empleados; }

    }
}
