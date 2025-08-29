namespace Rental_Management_System.Server.Services.Tenant
{
    using Rental_Management_System.Server.DTOs;
    using Rental_Management_System.Server.DTOs.Tenant;
    using Rental_Management_System.Server.Models;
    public interface ITenantService
    {
        Task<ApiResponse<IEnumerable<TenantDto>>> GetAllTenantsAsync();
        Task<ApiResponse<TenantDto>> GetTenantByIdAsync(Guid tenantId);
        Task<ApiResponse<TenantDto>> CreateTenantAsync(CreateTenantDto tenant);
        Task<ApiResponse<TenantDto>> UpdateTenantAsync(Guid tenantId, UpdateTenantDto tenant);
        Task<ApiResponse<bool>> DeleteTenantAsync(Guid tenantId);
    }
}
