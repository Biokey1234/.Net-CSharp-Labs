using System.ComponentModel.DataAnnotations;
using Assignment2.Models;

namespace Assignment2.Models
{
    public class Deal
    {
        public int Id { get; set; }

        [Required]
        [Url(ErrorMessage = "Please enter a valid path.")]
        public string ImagePath { get; set; }

        [Required]
        public string FoodDeliveryServiceId { get; set; }
        

        public FoodDeliveryService FoodDeliveryService { get; set; }
    }
}
