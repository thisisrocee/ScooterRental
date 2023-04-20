using ScooterRental.Models;

namespace ScooterRental.Interfaces
{
    public interface IRentalArchive
    {
        void AddRent(string id, decimal pricePerMinute, DateTime rentStart);
        RentedScooter EndRent(string id,  DateTime rentEnd);
        IList<RentedScooter> GetRentedScooters();
    }
}
