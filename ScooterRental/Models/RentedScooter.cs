namespace ScooterRental.Models
{
    public class RentedScooter
    {
        public RentedScooter(string id, decimal pricePerMinute, DateTime rentStarted)
        {
            Id = id;
            PricePerMinute = pricePerMinute;
            RentStarted = rentStarted;
        }

        public string Id { get; }
        public decimal PricePerMinute { get; }
        public DateTime RentStarted { get; }
        public DateTime? RentCompleted { get; set; }
    }
}