namespace OfficeBite.Core.Models.StaffModels
{
    public class StaffAllOrdersViewModel
    {
        public int OrderId { get; set; }

        public string OrderName { get; set; } = string.Empty;
        public int MenuToOrderId { get; set; }
        public string CustomerIdentifier { get; set; } = string.Empty;
        public string CustomerFirstName { get; set; } = string.Empty;

        public string CustomerLastName { get; set; } = string.Empty;

        public string CustomerUsername { get; set; } = string.Empty;

        public decimal TotalPrice { get; set; }
        public DateTime LunchDate { get; set; }

        public DateTime DateOrderCreated { get; set; }

        public bool IsEaten { get; set; }

    }
}
