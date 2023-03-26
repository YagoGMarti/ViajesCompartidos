using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SistemaViajesCompartidos.Models
{
    public abstract class BaseModel
    {
        public BaseModel()
        {
            Activo = true;
            FechaAlta = DateTime.Now;
        }

        [Key] //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ID { get; set; } = Guid.NewGuid();

        public bool Activo { get; set; }

        [DisplayName("Alta")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaAlta { get; set; } = DateTime.Now;

        //[DisplayName("Baja")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        //public DateTime? FechaBaja { get; set; }
    }
}