namespace Assignment2.Models
{
    public class Subscription
    {
        // Foreign key to Customer
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        // Foreign key to FooddeliveryService
        public string ServiceId { get; set; }
        public FoodDeliveryService Service { get; set; }
    }
}
