using ScooterRental.Exceptions;
using ScooterRental.Interfaces;

namespace ScooterRental.Services
{
    public class RentalCompany : IRentalCompany
    {
        private readonly IScooterService _scooterService;
        private readonly IRentalArchive _archive;
        private readonly IRentalCalculator _calculator;
        public string Name { get; }

        public RentalCompany(string name, IScooterService scooterService, IRentalArchive archive)
        {
            Name = name;
            _scooterService = scooterService;
            _archive = archive;
            _calculator = new RentalCalculator();
        }

        public void StartRent(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new InvalidIdException();
            }

            var scooter = _scooterService.GetScooterById(id) ?? throw new ScooterNotFoundException();

            _archive.AddRent(scooter.Id, scooter.PricePerMinute, DateTime.UtcNow);

            scooter.IsRented = true;
        }

        public decimal EndRent(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new InvalidIdException();
            }

            var scooter = _scooterService.GetScooterById(id) ?? throw new ScooterNotFoundException();
            var rental = _archive.EndRent(id, DateTime.Now);

            scooter.IsRented = false;

            return _calculator.CalculateRent(rental);
        }

        public decimal CalculateIncome(int? year, bool includeNotCompletedRentals)
        {
            if (year < 0)
            {
                throw new InvalidYearException();
            }

            var result = 0m;

            var allScooters = _archive.GetRentedScooters();

            if (year.HasValue && includeNotCompletedRentals)
            {
                var scoot = allScooters.Where(x => x.RentStarted.Year == year).ToList();

                if (scoot.Count == 0)
                {
                    return 0m;
                }

                result += _calculator.CalculateIncome(scoot);
            }
            else if (year.HasValue && !includeNotCompletedRentals)
            {
                var scoot = allScooters.Where(x => x.RentStarted.Year == year && x.RentCompleted.HasValue).ToList();

                if (scoot.Count == 0)
                {
                    return 0m;
                }

                result += _calculator.CalculateIncome(scoot);
            }
            else if (!year.HasValue && !includeNotCompletedRentals)
            {
                var scoot = allScooters.Where(x => x.RentCompleted.HasValue).ToList();

                result += _calculator.CalculateIncome(scoot);
            }
            else
            {
                result += _calculator.CalculateIncome(allScooters);
            }

            return result;
        }
    }
}