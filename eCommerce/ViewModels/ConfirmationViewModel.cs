namespace eCommerce.ViewModels
{
    public class ConfirmationViewModel
    {
        public int OrderId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShippingName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Payment { get; set; }
        public List<OrderItemViewModel> Items { get; set; }
    }
}
