using AutoMapper;
using Rental_Management_System.Server.DTOs;
using Rental_Management_System.Server.DTOs.RentalContract;
using Rental_Management_System.Server.Repositories.RentalContract;
using Rental_Management_System.Server.Repositories.Room;
using Rental_Management_System.Server.Repositories.Tenant;

namespace Rental_Management_System.Server.Services.RentalContract
{
    using Rental_Management_System.Server.DTOs;
    using Rental_Management_System.Server.Models;
    public class RentalContractService : IRentalContractService
    {
        private readonly IRentalContractRepository _rentalContractRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly ITenantRepository _tenantRepository;
        private readonly IMapper _mapper;
        public RentalContractService(IRentalContractRepository rentalContractRepository, IMapper mapper, ITenantRepository tenant, IRoomRepository roomRepository)
        {
            _mapper = mapper;
            _rentalContractRepository = rentalContractRepository;
            _roomRepository = roomRepository;
            _tenantRepository = tenant;
        }
        public async Task<ApiResponse<RentalContractDto>> CreateRentalContractAsync(CreateRentalContractDto createRentalContractDto)
        {
            var tenant = await _tenantRepository.GetByIdAsync(createRentalContractDto.TenantId);
            if (tenant == null) return ApiResponse<RentalContractDto>.FailResponse("Invalid tenant Id");

            var room = await _roomRepository.GetByIdAsync(createRentalContractDto.RoomId);
            if (room == null) return ApiResponse<RentalContractDto>.FailResponse("Invalid room Id");

            var rentalContract = _mapper.Map<RentalContract>(createRentalContractDto);

            await _rentalContractRepository.AddAsync(rentalContract);
            await _rentalContractRepository.SaveChangesAsync();

            var rentalContractDto = _mapper.Map<RentalContractDto>(rentalContract);
            return ApiResponse<RentalContractDto>.SuccessResponse(rentalContractDto, "Contract created successfully");

        }

        public async Task<ApiResponse<bool>> DeleteRentalContractAsync(Guid rentalContractId)
        {
            var rentalContract = await _rentalContractRepository.GetByIdAsync(rentalContractId);
            if (rentalContract == null) return ApiResponse<bool>.FailResponse("Invalid rental Contract Id");

            await _rentalContractRepository.DeleteAsync(rentalContract);
            await _rentalContractRepository.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(true,"Rental contract deleted successfully");
        }

        public async Task<ApiResponse<IEnumerable<RentalContractDto>>> GetAllRentalContractsAsync()
        {
            var rentalContracts = await _rentalContractRepository.GetAllAsync();
            if (rentalContracts == null) return ApiResponse<IEnumerable<RentalContractDto>>.FailResponse("No rental contracts found");
            
            var rentalContractDtos =  _mapper.Map<IEnumerable<RentalContractDto>>(rentalContracts);
            return ApiResponse<IEnumerable<RentalContractDto>>.SuccessResponse(rentalContractDtos, "Data fetched successfully");
        }

        public async Task<ApiResponse<RentalContractDto>> GetRentalContractByIdAsync(Guid rentalContractId)
        {
            var rentalContract = await _rentalContractRepository.GetByIdAsync(rentalContractId);
            if (rentalContract == null) return ApiResponse<RentalContractDto>.FailResponse("No rental contract found");

            var rentalContractDto = _mapper.Map<RentalContractDto>(rentalContract);
            return ApiResponse<RentalContractDto>.SuccessResponse(rentalContractDto, "data fetched successfully");
        }

        public async Task<ApiResponse<RentalContractDto>> UpdateRentalContractAsync(Guid rentalContractId, UpdateRentalContractDto updateRentalContractDto)
        {
            var rentalContract = await _rentalContractRepository.GetByIdAsync(rentalContractId);
            if (rentalContract == null) return ApiResponse<RentalContractDto>.FailResponse("Invalid rental Id");

            var tenant = await _tenantRepository.GetByIdAsync(updateRentalContractDto.TenantId);
            if (tenant == null) return ApiResponse<RentalContractDto>.FailResponse("Invalid tenant Id");

            var room = await _roomRepository.GetByIdAsync(updateRentalContractDto.RoomId);
            if (room == null) return ApiResponse<RentalContractDto>.FailResponse("Invalid room Id");


            _mapper.Map(updateRentalContractDto, rentalContract);
            await _rentalContractRepository.UpdateAsync(rentalContract);
            await _rentalContractRepository.SaveChangesAsync();

            var rentalContractDto = _mapper.Map<RentalContractDto>(rentalContract);
            return ApiResponse<RentalContractDto>.SuccessResponse(rentalContractDto, "Rental contract updated successfully");

        }
    }
}
