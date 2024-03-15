namespace CLED.OnlineShop.API.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string Items { get; set; }
        public decimal Total { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
