using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Lab7.Models
{
    public class Student
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-generates ID in the database
        [SwaggerSchema(ReadOnly = true)] // Ensures ID is read-only in Swagger documentation
        public Guid Id { get; set; }

        [Required] // Marks the field as mandatory
        [StringLength(75)] 
        public string FirstName { get; set; }

        [Required]
        [StringLength(75)]
        public string LastName { get; set; }

        [Required]
        [StringLength(75)] 
        public string Program { get; set; }

    }
}
