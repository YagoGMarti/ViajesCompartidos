using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;

namespace SistemaViajesCompartidos.Models
{
    [Table("Contactos")]
    public class ContactoModel
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public DateTime Fecha { get; set; } = DateTime.Now;
        public bool Procesado { get; set; } = false;

        [NotMapped]
        [DisplayName("Email")]
        [Required(ErrorMessage = "Es necesario un email", AllowEmptyStrings = false)]
        [EmailAddress(ErrorMessage = "El email no cumple con el formato esperado")]
        public string CorreoElectronico { get; set; }
        public string CorreoElectronicoEncriptado { get; set; }

        [Required(ErrorMessage = "Debe ingresar un mensaje", AllowEmptyStrings = false)]
        public string Mensaje { get; set; }
    }
}