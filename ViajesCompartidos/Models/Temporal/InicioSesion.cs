using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SistemaViajesCompartidos.Temporal
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

        [DataType(DataType.Password)]
        [DisplayName("Nueva contraseña")]
        [RegularExpressionAttribute(@"^(?=.*?[A-Za-z])(?=.*?[0-9])(?=.*?[#?|!@$%^&*-/_]).{8,}$", ErrorMessage = "La clave debe tener una letra, un número y un carácter especial. Longitud mínima = 8")]
        public string ClaveNueva { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Confirmar nueva contraseña")]
        [RegularExpressionAttribute(@"^(?=.*?[A-Za-z])(?=.*?[0-9])(?=.*?[#?|!@$%^&*-/_]).{8,}$", ErrorMessage = "La clave debe tener una letra, un número y un carácter especial. Longitud mínima = 8")]
        public string ConfirmarClaveNueva { get; set; }

    }
}