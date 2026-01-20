namespace Rental_Management_System.Server.Services.DashboardService
{
    using AutoMapper;
    using Rental_Management_System.Server.DTOs;
    using Rental_Management_System.Server.DTOs.DashboardDataDto;
    using Rental_Management_System.Server.DTOs.DashboardDataDto;
    using Rental_Management_System.Server.Repositories.PaymentTransaction;
    using Rental_Management_System.Server.Repositories.Room;

    public class DashboardService : IDashboardService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IPaymentTransactionRepository _transactionRepository;

        public DashboardService(IRoomRepository roomRepo, IPaymentTransactionRepository txRepo)
        {
            _roomRepository = roomRepo;
            _transactionRepository = txRepo;
        }

        public async Task<ApiResponse<DashboardDataDto>> GetDashboardDataAsync()
        {
            var rooms = await _roomRepository.GetAllAsync();
            var transactions = await _transactionRepository.GetAllAsync();

            var totalRooms = rooms.Count();
            var occupiedRooms = rooms.Count(r => r.IsActive == true);
            var pendingPayments = transactions.Count(t => !t.AmountPaid.HasValue || t.AmountPaid == 0);
            var totalTransactions = transactions.Count();

            // Prepare monthly chart data
            var monthlyPayments = Enumerable.Range(1, 12).Select(month =>
            {
                var count = transactions.Count(t => t.PaymentDate?.Month == month);
                return new MonthlyPaymentData
                {
                    Month = new DateTime(2024, month, 1).ToString("MMM"),
                    PaymentsCount = count
                };
            }).ToList();

            var dashboardData = new DashboardDataDto
            {
                TotalRooms = totalRooms,
                OccupiedRooms = occupiedRooms,
                PendingPayments = pendingPayments,
                TotalTransactions = totalTransactions,
                MonthlyPayments = monthlyPayments
            };

            return ApiResponse<DashboardDataDto>.SuccessResponse(dashboardData);
        }
    }
}
