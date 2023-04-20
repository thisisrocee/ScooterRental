using ScooterRental.Exceptions;
using ScooterRental.Interfaces;
using ScooterRental.Models;

namespace ScooterRental.Services
{
    public class RentalArchive : IRentalArchive
    {
        private readonly IList<RentedScooter> _rentedScooters;
        public RentalArchive(IList<RentedScooter> rentedScooters)
        {
            _rentedScooters = rentedScooters;
        }
        public void AddRent(string id, decimal pricePerMinute, DateTime rentStart)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new InvalidIdException();
            }

            if (pricePerMinute < 0)
            {
                throw new InvalidPriceException();
            }

            _rentedScooters.Add(new RentedScooter(id, pricePerMinute, rentStart));
        }

        public RentedScooter EndRent(string id, DateTime rentEnd)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new InvalidIdException();
            }

            var rental = _rentedScooters.SingleOrDefault(s => s.Id == id && !s.RentCompleted.HasValue) ?? throw new ScooterNotFoundException();
            rental.RentCompleted = rentEnd;
            return rental;
        }

        public IList<RentedScooter> GetRentedScooters()
        {
            return _rentedScooters.ToList();
        }
    }
}