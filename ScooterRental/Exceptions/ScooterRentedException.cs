namespace ScooterRental.Exceptions
{
    public class ScooterRentedException : Exception
    {
        public ScooterRentedException() : base("Provided scooter is rented.")
        {

        }
    }
}
