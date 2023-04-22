using FluentAssertions;
using Moq.AutoMock;
using ScooterRental.Exceptions;
using ScooterRental.Interfaces;
using ScooterRental.Models;
using ScooterRental.Services;

namespace ScooterRental.Tests
{
    public class RentalCalculatorTests
    {
        private AutoMocker _mocker;
        private IRentalCalculator _calculator;
        
        [SetUp]
        public void Setup()
        {
            _mocker = new AutoMocker();
            _calculator = new RentalCalculator();
        }

        [Test]
        public void CalculateRent_RentalScooterProvided_ReturnedCalculatedRent()
        {
            var rental = new RentedScooter("1", 1m, DateTime.Now.AddMinutes(-30)) {RentCompleted = DateTime.Now};

            _calculator.CalculateRent(rental).Should().Be(20m);
        }

        [Test]
        public void CalculateIncome_RentalScootersProvided_ReturnedCalculatedIncome()
        {
            var rental = new RentedScooter("1", 1m, DateTime.Now.AddMinutes(-30)) { RentCompleted = DateTime.Now };
            var rental1 = new RentedScooter("1", 1m, DateTime.Now.AddMinutes(-30)) { RentCompleted = DateTime.Now };

            _calculator.CalculateIncome(new List<RentedScooter>{rental, rental1}).Should().Be(40m);
        }
    }
}
