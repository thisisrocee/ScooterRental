using FluentAssertions;
using Moq;
using Moq.AutoMock;
using ScooterRental.Exceptions;
using ScooterRental.Interfaces;
using ScooterRental.Models;
using ScooterRental.Services;

namespace ScooterRental.Tests
{
    public class RentalCompanyTests
    {
        private IRentalCompany _rentalCompany;
        private AutoMocker _mocker;

        [SetUp]
        public void Setup()
        {
            _mocker = new AutoMocker();
            _rentalCompany = new RentalCompany("Bolt", _mocker.GetMock<IScooterService>().Object, _mocker.GetMock<IRentalArchive>().Object);
        }

        [Test]
        public void Name_CompanyNameReturned()
        {
            _rentalCompany.Name.Should().Be("Bolt");
        }

        [Test]
        public void StartRent_EmptyIdProvided_ThrowsInvalidIdException()
        {
            Action act = () => _rentalCompany.StartRent("");

            act.Should().Throw<InvalidIdException>();
        }

        [Test]
        public void StartRent_NullIdProvided_ThrowsInvalidIdException()
        {
            Action act = () => _rentalCompany.StartRent(null);

            act.Should().Throw<InvalidIdException>();
        }

        [Test]
        public void StartRent_NonExistingIdProvided_ThrowsScooterNotFoundException()
        {
            var mock = _mocker.GetMock<IScooterService>();
            mock.Setup(x => x.AddScooter("1", 1m));

            Action act = () => _rentalCompany.StartRent("2");

            act.Should().Throw<ScooterNotFoundException>();
        }

        [Test]
        public void StartRent_ValidIdProvided_ScooterIsRented()
        {
            var scooter = new Scooter("1", 1m);

            var mock = _mocker.GetMock<IScooterService>();
            mock.Setup(x => x.GetScooterById("1")).Returns(scooter);

            _rentalCompany.StartRent("1");

            var mock1 = _mocker.GetMock<IRentalArchive>();
            mock1.Verify(s => s.AddRent("1", 1m, It.IsAny<DateTime>()), Times.Once);

            scooter.IsRented.Should().BeTrue();
        }

        [Test]
        public void EndRent_ValidIdProvided_ScooterRentEnded()
        {
            var scooter = new Scooter("1", 1m) { IsRented = true };

            var rentedScooter = new RentedScooter(scooter.Id, scooter.PricePerMinute, DateTime.Now.AddMinutes(-25))
            {
                RentCompleted = DateTime.Now
            };

            _mocker.GetMock<IScooterService>()
                .Setup(s => s.GetScooterById(scooter.Id)).Returns(scooter);

            _mocker.GetMock<IRentalArchive>()
                .Setup(s => s.EndRent(scooter.Id, It.IsAny<DateTime>())).Returns(rentedScooter);

            _mocker.GetMock<IRentalCalculator>().Setup(s => s.CalculateRent(rentedScooter)).Returns(20m);

            _rentalCompany.EndRent(scooter.Id).Should().Be(20m);
            scooter.IsRented.Should().BeFalse();
        }

        [Test]
        public void EndRent_EmptyIdProvided_ThrowsInvalidIdException()
        {
            Action act = () => _rentalCompany.EndRent("");

            act.Should().Throw<InvalidIdException>();
        }

        [Test]
        public void EndRent_NullIdProvided_ThrowsInvalidIdException()
        {
            Action act = () => _rentalCompany.EndRent(null);

            act.Should().Throw<InvalidIdException>();
        }

        [Test]
        public void EndRent_NonExistingIdProvided_ThrowsScooterNotFoundException()
        {
            var mock = _mocker.GetMock<IScooterService>();
            mock.Setup(x => x.AddScooter("1", 1m));

            Action act = () => _rentalCompany.EndRent("2");

            act.Should().Throw<ScooterNotFoundException>();
        }

        [Test]
        public void CalculateIncome_YearNotProvidedAndNonCompletedRentalsNotIncluded_ReturnsTotalIncome()
        {
            var rentals = new List<RentedScooter>();

            _mocker.GetMock<IRentalArchive>().Setup(s => s.GetRentedScooters()).Returns(rentals);

            _mocker.GetMock<IRentalCalculator>().Setup(s => s.CalculateIncome(rentals)).Returns(0);

            _rentalCompany.CalculateIncome(null, false).Should().Be(0);
        }

        [Test]
        public void CalculateIncome_YearNotProvidedAndNonCompletedRentalsIncluded_ReturnsTotalIncome()
        {
            var rentals = new List<RentedScooter>();

            var rent = new RentedScooter("1", 1m, DateTime.Now.AddMinutes(-5)) { RentCompleted = DateTime.Now };
            var rent1 = new RentedScooter("1", 1m, new DateTime(2022, 10, 2, 2, 0, 0))
            {
                RentCompleted = new DateTime(2022, 10, 4, 2, 0, 0)
            };
            rentals.Add(rent);
            rentals.Add(rent1);

            _mocker.GetMock<IRentalArchive>().Setup(s => s.GetRentedScooters()).Returns(rentals);

            _mocker.GetMock<IRentalCalculator>().Setup(s => s.CalculateIncome(rentals)).Returns(65m);

            _rentalCompany.CalculateIncome(null, true).Should().Be(65m);
        }

        [Test]
        public void CalculateIncome_YearProvidedAndNonCompletedRentalsNotIncluded_ReturnsTotalIncome()
        {
            var rentals = new List<RentedScooter>();

            var rent = new RentedScooter("1", 1m, DateTime.Now.AddMinutes(-5)) { RentCompleted = DateTime.Now };
            var rent1 = new RentedScooter("1", 1m, DateTime.Now.AddMinutes(-5)) { RentCompleted = DateTime.Now };
            rentals.Add(rent);
            rentals.Add(rent1);

            _mocker.GetMock<IRentalArchive>().Setup(s => s.GetRentedScooters()).Returns(rentals);

            _mocker.GetMock<IRentalCalculator>().Setup(s => s.CalculateIncome(rentals)).Returns(10m);

            _rentalCompany.CalculateIncome(2023, false).Should().Be(10m);
        }

        [Test]
        public void CalculateIncome_YearProvidedAndNonCompletedRentalsIncluded_ReturnsTotalIncome()
        {
            var rentals = new List<RentedScooter>();

            var rent = new RentedScooter("1", 1m, DateTime.Now.AddMinutes(-5)) { RentCompleted = DateTime.Now };
            var rent1 = new RentedScooter("1", 1m, DateTime.Now.AddMinutes(-5));
            rentals.Add(rent);
            rentals.Add(rent1);

            _mocker.GetMock<IRentalArchive>().Setup(s => s.GetRentedScooters()).Returns(rentals);

            _mocker.GetMock<IRentalCalculator>().Setup(s => s.CalculateIncome(rentals)).Returns(10m);

            _rentalCompany.CalculateIncome(2023, true).Should().Be(10m);
        }

        [Test]
        public void CalculateIncome_NegativeYearProvided_ThrowsInvalidYearException()
        {
            Action act = () => _rentalCompany.CalculateIncome(-1, false);

            act.Should().Throw<InvalidYearException>();
        }
    }
}