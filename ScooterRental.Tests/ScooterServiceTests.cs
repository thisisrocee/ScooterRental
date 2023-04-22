using FluentAssertions;
using Moq.AutoMock;
using ScooterRental.Exceptions;
using ScooterRental.Interfaces;
using ScooterRental.Models;
using ScooterRental.Services;

namespace ScooterRental.Tests
{
    public class ScooterRentalTests
    {
        private IScooterService _scooterService;
        private List<Scooter> _scooters;
        private AutoMocker _mocker;
        private const string scooterId = "1";
        private const decimal scooterPricePerMinute = 1m;

        [SetUp]
        public void Setup()
        {
            _scooters = new List<Scooter>();
            _scooterService = new ScooterService(_scooters);
            _mocker = new AutoMocker();
        }

        [Test]
        public void AddScooter_ValidIdAndPriceProvided_ScooterAdded()
        {
            _scooterService.AddScooter(scooterId, scooterPricePerMinute);
        }

        [Test]
        public void AddScooter_AddingDuplicateScooter_ThrowsScooterExistsException()
        {
            _scooterService.AddScooter(scooterId, scooterPricePerMinute);

            Action act = () => _scooterService.AddScooter(scooterId, scooterPricePerMinute);

            act.Should().Throw<ScooterExistsException>();
        }

        [Test]
        public void AddScooter_EmptyIdProvided_ThrowsInvalidIdException()
        {
            Action act = () => _scooterService.AddScooter("", scooterPricePerMinute);

            act.Should().Throw<InvalidIdException>();
        }

        [Test]
        public void AddScooter_NullIdProvided_ThrowsInvalidIdException()
        {
            Action act = () => _scooterService.AddScooter(null, scooterPricePerMinute);

            act.Should().Throw<InvalidIdException>();
        }

        [Test]
        public void AddScooter_NegativePriceProvided_ThrowsInvalidPriceException()
        {
            Action act = () => _scooterService.AddScooter(scooterId, -1m);

            act.Should().Throw<InvalidPriceException>();
        }

        [Test]
        public void RemoveScooter_ScooterIdProvided_ScooterRemoved()
        {
            _scooters.Add(new Scooter(scooterId, scooterPricePerMinute));
            _scooterService.RemoveScooter(scooterId);
            _scooters.Count.Should().Be(0);
        }

        [Test]
        public void RemoveScooter_InvalidScooterIdProvided_ThrowsScooterNotFoundException()
        {
            _scooters.Add(new Scooter(scooterId, scooterPricePerMinute));
            Action act = () => _scooterService.RemoveScooter("3");

            act.Should().Throw<ScooterNotFoundException>();
        }

        [Test]
        public void RemoveScooter_EmptyScooterIdProvided_ThrowsInvalidIdException()
        {
            Action act = () => _scooterService.RemoveScooter("");

            act.Should().Throw<InvalidIdException>();
        }

        [Test]
        public void RemoveScooter_NullScooterIdProvided_ThrowsInvalidIdException()
        {
            Action act = () => _scooterService.RemoveScooter(null);

            act.Should().Throw<InvalidIdException>();
        }

        [Test]
        public void RemoveScooter_ScooterInRent_ThrowsScooterRentedException()
        {
            _scooters.Add(new Scooter(scooterId, scooterPricePerMinute) {IsRented = true});
            Action act = () => _scooterService.RemoveScooter(scooterId);

            act.Should().Throw<ScooterRentedException>();
        }

        [Test]
        public void GetScooters_EmptyScooterListReturned()
        {
            _scooterService.GetScooters().Count.Should().Be(0);
        }

        [Test]
        public void GetScooters_NonEmptyScooterListReturned()
        {
            _scooterService.AddScooter(scooterId, scooterPricePerMinute);
            _scooterService.GetScooters().Count.Should().Be(1);
        }

        [Test]
        public void GetScootersById_EmptyScooterIdProvided_ThrowsInvalidIdException()
        {
            Action act = () => _scooterService.GetScooterById("");

            act.Should().Throw<InvalidIdException>();
        }

        [Test]
        public void GetScootersById_NullScooterIdProvided_ThrowsInvalidIdException()
        {
            Action act = () => _scooterService.GetScooterById(null);

            act.Should().Throw<InvalidIdException>();
        }

        [Test]
        public void GetScootersById_Id1Provided_ScooterWithRequestedId1Returned()
        {
            _scooters.Add(new Scooter(scooterId, scooterPricePerMinute));
            var scooter = _scooterService.GetScooterById(scooterId);

            scooter.Id.Should().Be(scooterId);
        }

        [Test]
        public void GetScootersById_Id2Provided_ScooterWithRequestedId2Returned()
        {
            _scooters.Add(new Scooter("2", scooterPricePerMinute));
            var scooter = _scooterService.GetScooterById("2");
            scooter.Id.Should().Be("2");
        }

        [Test]
        public void GetScootersById_NonExistingIdProvided_ThrowsScooterNotFoundException()
        {
            Action act = () => _scooterService.GetScooterById("5");

            act.Should().Throw<ScooterNotFoundException>();
        }
    }
}