using System;
using System.ComponentModel.DataAnnotations;

namespace SistemaViajesCompartidos.Models
{
    public abstract class BaseModel
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        //public int CourseID { get; set; }

        Boolean Activo = true;
        public DateTime FechaAlta { get; set; } = DateTime.Now;
        public DateTime? FechaBaja { get; set; }
    }
}