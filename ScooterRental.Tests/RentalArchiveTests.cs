using FluentAssertions;
using Moq.AutoMock;
using ScooterRental.Exceptions;
using ScooterRental.Interfaces;
using ScooterRental.Models;
using ScooterRental.Services;

namespace ScooterRental.Tests
{
    public class RentalArchiveTests
    {
        private IRentalArchive _archive;
        private IList<RentedScooter> _rentedScooterList;

        [SetUp]
        public void Setup()
        {
            _rentedScooterList = new List<RentedScooter>();
            _archive = new RentalArchive(_rentedScooterList);
        }

        [Test]
        public void AddRent_ValidIdAndValidPriceAndStartDateProvided_RentalAdded()
        {
            _archive.AddRent("1", 1m, DateTime.Now);

            _rentedScooterList.Count.Should().Be(1);
        }

        [Test]
        public void AddRent_EmptyIdAndValidPriceAndStartDateProvided_ThrowsInvalidIdException()
        {
            Action act = () => _archive.AddRent("", 1m, DateTime.Now);

            act.Should().Throw<InvalidIdException>();
        }

        [Test]
        public void AddRent_NullIdAndValidPriceAndStartDateProvided_ThrowsInvalidIdException()
        {
            Action act = () => _archive.AddRent(null, 1m, DateTime.Now);

            act.Should().Throw<InvalidIdException>();
        }

        [Test]
        public void AddRent_ValidIdAndNegativePriceAndStartDateProvided_ThrowsInvalidPriceException()
        {
            Action act = () => _archive.AddRent("1", -1m, DateTime.Now);

            act.Should().Throw<InvalidPriceException>();
        }

        [Test]
        public void EndRent_ValidIdAndEndDateProvided_RentalStopped()
        {
            var rental = new RentedScooter("1", 1m, DateTime.Now);
            _rentedScooterList.Add(rental);

            _archive.EndRent("1", DateTime.Now).Should().Be(rental);
        }

        [Test]
        public void EndRent_EmptyIdAndEndDateProvided_ThrowsInvalidIdException()
        {
            Action act = () => _archive.EndRent("",  DateTime.Now);

            act.Should().Throw<InvalidIdException>();
        }

        [Test]
        public void EndRent_NullIdAndEndDateProvided_ThrowsInvalidIdException()
        {
            Action act = () => _archive.EndRent(null, DateTime.Now);

            act.Should().Throw<InvalidIdException>();
        }

        [Test]
        public void EndRent_InvalidIdAndEndDateProvided_ThrowsScooterNotFoundException()
        {
            var rental = new RentedScooter("1", 1m, DateTime.Now);
            _rentedScooterList.Add(rental);

            Action act = () => _archive.EndRent("2", DateTime.Now);

            act.Should().Throw<ScooterNotFoundException>();
        }

        [Test]
        public void GetRentedScooters_ReturnsEmptyListOfRentedScooters()
        {
            _archive.GetRentedScooters().Count.Should().Be(0);
        }

        [Test]
        public void GetRentedScooters_ReturnsListOfRentedScooters()
        {
            var rental = new RentedScooter("1", 1m, DateTime.Now);
            _rentedScooterList.Add(rental);

            _archive.GetRentedScooters().Count.Should().Be(1);
        }
    }
}