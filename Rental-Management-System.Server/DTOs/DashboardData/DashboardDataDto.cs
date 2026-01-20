namespace Rental_Management_System.Server.DTOs.DashboardDataDto
{
    public class DashboardDataDto
    {
        public int TotalRooms { get; set; }
        public int OccupiedRooms { get; set; }
        public int PendingPayments { get; set; }
        public int TotalTransactions { get; set; }

        // Optional: chart data for frontend
        public IEnumerable<MonthlyPaymentData> MonthlyPayments { get; set; }
    }

    public class MonthlyPaymentData
    {
        public string Month { get; set; } // e.g. "Jan", "Feb"
        public int PaymentsCount { get; set; }
    }
}
