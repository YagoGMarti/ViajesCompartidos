using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ViajesCompartidos.Temporal
{
    public class InicioSesion
    {
        [DisplayName("Correo electrónico")]
        [Required(ErrorMessage = "Debe indicar su email", AllowEmptyStrings = false)]
        [EmailAddress(ErrorMessage = "El email no cumple con el formato esperado")]
        public string Email { get; set; }
        public string EmailEncriptado { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Debe ingresar una clave")]
        [DisplayName("Contraseña")]
        public string Clave { get; set; }
        public byte[] ClaveEncriptada { get; set; }
    }
}