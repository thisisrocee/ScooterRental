using ScooterRental.Models;

namespace ScooterRental.Interfaces
{
    public interface IRentalCalculator
    {
        decimal CalculateRent(RentedScooter rent);
        decimal CalculateIncome(IList<RentedScooter> rentedScooters);
    }
}
