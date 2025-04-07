using System.ComponentModel.DataAnnotations;

namespace Assignment2.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return $"{LastName}, {FirstName}";
            }
        }
        // Navigation property for 1-N relationship with Subscription
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
