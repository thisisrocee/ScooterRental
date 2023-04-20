using System.Diagnostics;

namespace ScooterRental.Exceptions
{
    public class InvalidYearException : Exception
    {
        public InvalidYearException() : base("Provided negative year.")
        {

        }
    }
}
