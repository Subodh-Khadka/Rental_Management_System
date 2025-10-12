using AutoMapper;
using Rental_Management_System.Server.DTOs;
using Rental_Management_System.Server.Repositories.Tenant;

namespace Rental_Management_System.Server.Services.Tenant
{
    using Rental_Management_System.Server.DTOs.Room;
    using Rental_Management_System.Server.DTOs.Tenant;
    using Rental_Management_System.Server.Models;
    using Rental_Management_System.Server.Repositories.Room;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class TenantService : ITenantService  
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public TenantService(ITenantRepository tenantRepository, IMapper mapper, IRoomRepository roomRepository)
        {
            _tenantRepository = tenantRepository;
            _mapper = mapper;
            _roomRepository = roomRepository;
        }

        public async Task<ApiResponse<TenantDto>> CreateTenantAsync(CreateTenantDto createTenantDto)
        {
            var room = await _roomRepository.GetByIdAsync(createTenantDto.RoomId);
            if(room == null)
            {
                return ApiResponse<TenantDto>.FailResponse("Invalid RoomId. The specified room doesnot exist.");
            }

            //maping input DTO to entity
            var tenant = _mapper.Map<Tenant> (createTenantDto);

            // Add to repository
            await _tenantRepository.AddAsync (tenant);
            await _tenantRepository.SavechangesAsync();

            //map entity back to DTO for response
            var tenantDto = _mapper.Map<TenantDto>(tenant);
            return ApiResponse<TenantDto>.SuccessResponse(tenantDto, "Tenanat Created Successfully");
        }

        public async Task<ApiResponse<bool>> DeleteTenantAsync(Guid tenantId)
        {
            var tenant = await _tenantRepository.GetByIdAsync(tenantId);
            if (tenant == null)
                return ApiResponse<bool>.FailResponse("Tenant Not Found!");

            await _tenantRepository.DeleteAsync(tenant);
            await _tenantRepository.SavechangesAsync();
            return ApiResponse<bool>.SuccessResponse(true, "Tenant Deleted Successfully");
        }

        public async Task<ApiResponse<IEnumerable<TenantDto>>> GetAllTenantsAsync()
        {
            var tenants = await _tenantRepository.GetAllAsync();
            if(tenants == null)
            {
                return ApiResponse<IEnumerable<TenantDto>>.FailResponse("No Tenants Data");
            }

            var tenantDtos = _mapper.Map<IEnumerable<TenantDto>>(tenants);
            return ApiResponse<IEnumerable<TenantDto>>.SuccessResponse(tenantDtos);
        }

        public async Task<ApiResponse<TenantDto>> GetTenantByIdAsync(Guid tenantId)
        {
            var tenant = await _tenantRepository.GetByIdAsync(tenantId);
            if(tenant == null)
            {
                return ApiResponse<TenantDto>.FailResponse("No tenant of the id provided was found");
            }

           var tenantDto = _mapper.Map<TenantDto>(tenant);
            return ApiResponse<TenantDto>.SuccessResponse(tenantDto, "Tenant Created Successfully");
        }

        public async Task<ApiResponse<TenantDto>> UpdateTenantAsync(Guid tenantId, UpdateTenantDto updateTenantDto)
        {
            var existingTenant = await _tenantRepository.GetByIdAsync(tenantId);

            if(existingTenant == null)
            {
                return ApiResponse<TenantDto>.FailResponse("Tenant not found.");
            }

            _mapper.Map(updateTenantDto, existingTenant);
            await _tenantRepository.UpdateAsync(existingTenant);
            await _tenantRepository.SavechangesAsync();

            var tenantDto = _mapper.Map<TenantDto>(existingTenant);
            return ApiResponse<TenantDto>.SuccessResponse(tenantDto, "Tenant Updated Successfully");
        }
    }
}
    