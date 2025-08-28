namespace Rental_Management_System.Server.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) { }
    }

    public class BadRequestException: Exception
    {
        public BadRequestException(string message) { }
    }
}
