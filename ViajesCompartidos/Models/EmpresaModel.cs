using SistemaViajesCompartidos.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SistemaViajesCompartidos.Models
{
    [Table("Empresas")]
    public class EmpresaModel : BaseModel
    {
        [Required(ErrorMessage = "Debe indicar un nombre", AllowEmptyStrings = false)]
        public string Nombre { get; set; }
        
        [DisplayName("Razón social")]
        [Required(ErrorMessage = "Debe ingrear la razón social", AllowEmptyStrings = false)]
        public string RazonSocial { get; set; }

        [RegularExpression(@"^(\d{11})$", ErrorMessage = "11 digitos numéricos, completar con 0")]
        public Int64 CUIT { get; set; }
       
        [DisplayName("Tipo de sociedad")]
        public TipoEmpresaEnum TipoEmpresa { get; set; }

        //[NotMapped]
        //[DisplayName("Logo")]
        //public HttpPostedFileBase Imagen { get; set; }
        
        //public byte[] Logo { get; set; }
        //public string FormatoImagenLogo { get; set; }
        //public string NombreImagenLogo { get; set; }

        public List<SucursalModel> Sucursales { get; set; }
        public List<SucursalModel> GetSucursales() { return this.Sucursales; }

        public List<EmpleadoModel> Empleados { get; set; }
        public List<EmpleadoModel> GetEmpleados() { return this.Empleados; }

        internal void Update(EmpresaModel empresaModel)
        {
            this.Nombre = empresaModel.Nombre;
            this.RazonSocial = empresaModel.RazonSocial;
            this.CUIT = empresaModel.CUIT;
            this.TipoEmpresa = empresaModel.TipoEmpresa;
            
            //this.Logo = empresaModel.Logo;
            //this.FormatoImagenLogo = empresaModel.FormatoImagenLogo;
            //this.NombreImagenLogo = empresaModel.NombreImagenLogo;
        }
    }
}
