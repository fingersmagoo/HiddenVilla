using AutoMapper;
using Business.Repository.IRepository;
using DataAccess.Data;
using Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository
{
    public class HotelRoomRepository : IHotelRoomRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public HotelRoomRepository(ApplicationDbContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }

        public async Task<HotelRoomDTO> CreateHotelRoom(HotelRoomDTO hotelRoomDTO)
        {
            HotelRoom room = _mapper.Map<HotelRoomDTO, HotelRoom>(hotelRoomDTO);
            room.CreatedDate = DateTime.Now;
            room.CreatedBy = "";

            var addedHotelRoom = await _db.HotelRooms.AddAsync(room);
            await _db.SaveChangesAsync();
            return _mapper.Map<HotelRoom, HotelRoomDTO>(addedHotelRoom.Entity);
        }

        public async Task<int> DeleteHotelRoom(int roomId)
        {
            var room = await _db.HotelRooms.FindAsync(roomId);
            if (room != null)
            {
                _db.HotelRooms.Remove(room);
                return await _db.SaveChangesAsync();
            }
            return 0;            
        }

        public async Task<IEnumerable<HotelRoomDTO>> GetAllHotelRooms()
        {
            try
            {
                IEnumerable<HotelRoomDTO> hotelRoomsDTOS = _mapper.Map <IEnumerable<HotelRoom>, IEnumerable<HotelRoomDTO>>(_db.HotelRooms);
                return hotelRoomsDTOS;
            } 
            catch(Exception e)
            {
                return null;
            }
        }

        public async Task<HotelRoomDTO> GetHotelRoom(int roomId)
        {
            try
            {
                HotelRoomDTO room = _mapper.Map<HotelRoom, HotelRoomDTO>(_db.HotelRooms.FirstOrDefault(x => x.Id == roomId));
                return room;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<HotelRoomDTO> IsRoomUnique(string name)
        {
            try
            {
                HotelRoomDTO room = _mapper.Map<HotelRoom, HotelRoomDTO>(_db.HotelRooms.FirstOrDefault(x => x.Name.ToLower().Equals(name.ToLower())));
                return room;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<HotelRoomDTO> UpdateHotelRoom(int roomId, HotelRoomDTO hotelRoomDTO)
        {
            try
            {
                if (roomId == hotelRoomDTO.Id)
                {
                    HotelRoom roomDetails = await _db.HotelRooms.FindAsync(roomId);
                    HotelRoom room = _mapper.Map<HotelRoomDTO, HotelRoom>(hotelRoomDTO, roomDetails);
                    room.UpdatedBy = "";
                    room.UpdatedDate = DateTime.Now;
                    var updatedRoom = _db.HotelRooms.Update(room);
                    await _db.SaveChangesAsync();
                    return _mapper.Map<HotelRoom, HotelRoomDTO>(updatedRoom.Entity);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
