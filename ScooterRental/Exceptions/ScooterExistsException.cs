namespace ScooterRental.Exceptions
{
    public class ScooterExistsException : Exception
    {
        public ScooterExistsException() : base("Provided scooter already exists.")
        {

        }
    }
}
