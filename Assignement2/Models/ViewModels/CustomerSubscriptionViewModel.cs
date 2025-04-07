namespace Assignment2.Models.ViewModels
{
    public class CustomerSubscriptionViewModel
    {
        public Customer Customer { get; set; }
        public IEnumerable<FoodDeliveryServiceSubscriptionViewModel> Subscriptions { get; set; }
    }

}
