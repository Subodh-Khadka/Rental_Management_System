using Microsoft.AspNetCore.Mvc;
using System.Reflection.Emit;
using static System.Net.Mime.MediaTypeNames;

namespace Rental_Management_System.Server
{
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }
}


/// Controllers->API endpoints(Presentation layer)
/// Services->Business logic, calculations (Application layer)
///Repositories    -> Data access (Infrastructure layer)
///Models          -> Entities: Tenant, RentPayment (Domain layer)
///Data            -> DbContext (Infrastructure layer)
