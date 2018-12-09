using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Flatbuilder.DAL.Interfaces;
using AutoMapper;
using Flatbuilder.DTO;

namespace Flatbuilder.WebAPI.Controllers
{
    [Route("api/Room")]
    //[ApiController]
    public class RoomController : Controller
    {
        private readonly IRoomManager _roomService;
        IMapper _mapper;

        public RoomController(IRoomManager roomService, IMapper mapper)
        {
            _roomService = roomService;
            _mapper = mapper;
        }

        [HttpGet("list")]
        [Produces(typeof(List<Room>))]
        public async Task<IActionResult> GetRoomsAsync()
        {
            var res = await _roomService.GetRoomsAsync();
            var mapped = _mapper.Map<List<Room>>(res);
            return Ok(mapped);
        }

        [HttpGet("get/{id}", Name = "GetRoomById")]
        [Produces(typeof(Room))]
        public async Task<IActionResult> GetRoomByIdAsync(int id)
        {
            var res = await _roomService.GetRoomByIdAsync(id);
            if(res == null)
            {
                return NotFound("Room not found");
            }

            var mapped = _mapper.Map<Room>(res);
            return Ok(mapped);
        }

        [HttpPost("create/kitchen")]
        public async Task<IActionResult> CreateKitchenAsync(Kitchen k) 
        {
            var mapped = _mapper.Map<DAL.Entities.Kitchen>(k);

            await _roomService.AddRoomAsync(mapped);

            return CreatedAtRoute("GetRoomById", new { id = mapped.Id }, mapped);
        }

        [HttpPost("create/bedroom")]
        public async Task<IActionResult> CreateBedroomAsync(Bedroom br)
        {
            var mapped = _mapper.Map<DAL.Entities.Bedroom>(br);

            await _roomService.AddRoomAsync(mapped);

            return CreatedAtRoute("GetRoomById", new { id = mapped.Id }, mapped);
        }

        [HttpPost("create/shower")]
        public async Task<IActionResult> CreateShowerAsync(Shower s)
        {
            var mapped = _mapper.Map<DAL.Entities.Shower>(s);

            await _roomService.AddRoomAsync(mapped);

            return CreatedAtRoute("GetRoomById", new { id = mapped.Id }, mapped);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateRoomAsync(int id, Room item)
        {
            var mapped = _mapper.Map<DAL.Entities.Room>(item);

            if(await _roomService.UpdateRoomAsync(id, mapped) == null)
            {
                return NotFound("Room not found");
            }

            return Ok("Successful update");
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteRoomAsync(int id)
        {
            var deleted = await _roomService.GetRoomByIdAsync(id);
            if (deleted == null)
            {
                return NotFound("Room not found");
            }

            await _roomService.DeletRoomAsync(deleted);

            return Ok("Successful delete");
        }
    }
}