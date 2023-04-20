using ScooterRental.Exceptions;
using ScooterRental.Interfaces;
using ScooterRental.Models;

namespace ScooterRental.Services
{
    public class ScooterService : IScooterService
    {
        private readonly List<Scooter> _scooters;

        public ScooterService(List<Scooter> scooters)
        {
            _scooters = scooters;
        }

        public void AddScooter(string id, decimal pricePerMinute)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new InvalidIdException();
            }

            if (_scooters.Any(x => x.Id == id))
            {
                throw new ScooterExistsException();
            }

            if (pricePerMinute < 0)
            {
                throw new InvalidPriceException();
            }

            _scooters.Add(new Scooter(id, pricePerMinute));
        }

        public void RemoveScooter(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new InvalidIdException();
            }

            var scooter = _scooters.SingleOrDefault(x => x.Id == id);

            if (scooter == null)
            {
                throw new ScooterNotFoundException();
            }

            if (scooter.IsRented)
            {
                throw new ScooterRentedException();
            }

            _scooters.Remove(scooter);
        }

        public IList<Scooter> GetScooters()
        {
            return _scooters.ToList();
        }

        public Scooter GetScooterById(string scooterId)
        {
            if (string.IsNullOrEmpty(scooterId))
            {
                throw new InvalidIdException();
            }

            var scooter = _scooters.SingleOrDefault(s => s.Id == scooterId);

            if (scooter == null)
            {
                throw new ScooterNotFoundException();
            }

            return scooter;
        }
    }
}
