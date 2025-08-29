namespace Rental_Management_System.Server.Services.Room
{
    using AutoMapper;
    using Rental_Management_System.Server.DTOs;
    using Rental_Management_System.Server.DTOs.Room;
    using Rental_Management_System.Server.Models;
    using Rental_Management_System.Server.Repositories.Room;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class RoomService : IRoomService
    {
        private readonly IMapper _mapper;
        private readonly IRoomRepository _roomRepository;

        public RoomService(IMapper mapper, IRoomRepository roomRepository)
        {
            _mapper = mapper;
            _roomRepository = roomRepository;
        }

        public async Task<ApiResponse<IEnumerable<RoomDto>>> GetAllRoomsAsync()
        {
            var rooms = await _roomRepository.GetAllAsync();
            var roomDtos = _mapper.Map<IEnumerable<RoomDto>>(rooms);
            return ApiResponse<IEnumerable<RoomDto>>.SuccessResponse(roomDtos);
        }   

        public async Task<ApiResponse<RoomDto>> GetRoomByIdAsync(Guid roomId)
        {
            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room == null)
                return ApiResponse<RoomDto>.FailResponse("Room not found");

            var roomDto = _mapper.Map<RoomDto>(room);
            return ApiResponse<RoomDto>.SuccessResponse(roomDto);
        }

        public async Task<ApiResponse<RoomDto>> CreateRoomAsync(CreateRoomDto createRoomDto)
        {
            var room = _mapper.Map<Room>(createRoomDto);
            await _roomRepository.AddAsync(room);
            await _roomRepository.SaveChangesAsync();

            var roomDto = _mapper.Map<RoomDto>(room);
            return ApiResponse<RoomDto>.SuccessResponse(roomDto, "Room created successfully");
        }

        public async Task<ApiResponse<RoomDto>> UpdateRoomAsync(Guid roomId, UpdateRoomDto updateRoomDto)
        {
            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room == null)
                return ApiResponse<RoomDto>.FailResponse("Room not found");

            _mapper.Map(updateRoomDto, room);
            await _roomRepository.UpdateAsync(room);
            await _roomRepository.SaveChangesAsync();

            var roomDto = _mapper.Map<RoomDto>(room);
            return ApiResponse<RoomDto>.SuccessResponse(roomDto, "Room updated successfully");
        }

        public async Task<ApiResponse<bool>> DeleteRoomAsync(Guid roomId)
        {
            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room == null)
                return ApiResponse<bool>.FailResponse("Room not found");

            await _roomRepository.DeleteAsync(room);
            await _roomRepository.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Room deleted successfully");
        }
    }
}
