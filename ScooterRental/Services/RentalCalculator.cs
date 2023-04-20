using ScooterRental.Exceptions;
using ScooterRental.Interfaces;
using ScooterRental.Models;

namespace ScooterRental.Services
{
    public class RentalCalculator : IRentalCalculator
    {
        public decimal CalculateRent(RentedScooter rent)
        {
            if (rent.RentCompleted == null)
            {
                rent.RentCompleted = DateTime.Now;
            }

            var daySplittedList = SplitDateTime(rent.RentStarted, rent.RentCompleted);

            var result = 0m;

            foreach (var day in daySplittedList)
            {
                var total = (decimal)day.TotalMinutes;

                if (total > 20)
                {
                    result += 20;
                    continue;
                }

                result += total * rent.PricePerMinute;
            }

            return Math.Round(result);
        }

        public decimal CalculateIncome(IList<RentedScooter> rentedScooters)
        {
            var result = 0m;

            foreach (var s in rentedScooters)
            {
                result += CalculateRent(s);
            }

            return Math.Round(result);
        }

        private List<TimeSpan> SplitDateTime(DateTime start, DateTime? end)
        {
            var timeSpans = new List<TimeSpan>();

            if (end.Value.Date == start.Date)
            {
                timeSpans.Add(end.Value - start);
                return timeSpans;
            }

            timeSpans.Add(start.Date.AddDays(1) - start);
            timeSpans.Add(end.Value - end.Value.Date);

            var currentDay = start.Date.AddDays(1);

            if (end.Value.Date == currentDay || end.Value.Date < currentDay)
            {
                return timeSpans;
            }

            while (currentDay < end.Value.Date)
            {
                timeSpans.Add(currentDay.Date.AddDays(1) - currentDay);

                currentDay = currentDay.AddDays(1);
            }

            return timeSpans;
        }
    }
}
