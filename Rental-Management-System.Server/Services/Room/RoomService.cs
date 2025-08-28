namespace Rental_Management_System.Server.Services.Room
{
    using AutoMapper;
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

        public async Task<IEnumerable<RoomDto>> GetAllRoomsAsync()
        {
            var rooms = await _roomRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<RoomDto>>(rooms);
        }

        public async Task<RoomDto> GetRoomByIdAsync(Guid roomId)
        {
            var room = await  _roomRepository.GetByIdAsync(roomId);
            return _mapper.Map<RoomDto>(room);
        }

        public async Task<RoomDto> CreateRoomAsync(CreateRoomDto createRoomDto)
        {
            var room = _mapper.Map<Room>(createRoomDto);
            await _roomRepository.AddAsync(room);
            await _roomRepository.SaveChangesAsync();
            return _mapper.Map<RoomDto>(room);
        }

        public async Task<RoomDto> UpdateRoomAsync(Guid roomId, UpdateRoomDto updateRoomDto)
        {
            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room == null) return null;

            _mapper.Map(updateRoomDto, room);
            await _roomRepository.UpdateAsync(room);
            await _roomRepository.SaveChangesAsync();

            return _mapper.Map<RoomDto>(room);
        }

        public async Task DeleteRoomAsync(Guid roomId)
        {
            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room == null) return;

            await _roomRepository.DeleteAsync(room);
            await _roomRepository.SaveChangesAsync();
        }

    }
}
