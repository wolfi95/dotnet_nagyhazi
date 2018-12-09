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
    [Route("api/Order")]
//    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderManager _orderService;
        IMapper _mapper;

        public OrderController(IOrderManager orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpGet("list")]
        [Produces(typeof(List<Order>))]
        public async Task<IActionResult> GetOrdersAsync()
        {
            var res = await _orderService.GetOrdersAsync();
            var mapped = _mapper.Map<List<Order>>(res);
            return Ok(mapped);
        }

        [HttpGet("list/{name}")]
        [Produces(typeof(List<Order>))]
        public async Task<IActionResult> GetOrdersByNameAsync(string name)
        {
            var res = await _orderService.GetOrdersByNameAsync(name);
            if (res == null)
            {
                return NotFound();
            }
            var mapped = _mapper.Map<List<Order>>(res);
            mapped.ForEach(o => o.Rooms.ForEach(or => or.Type = or.GetType()));
            return Ok(mapped);
        }

        [HttpGet("get/{id}", Name = "GetOrderById")]
        [Produces(typeof(Order))]
        public async Task<IActionResult> GetOrderByIdAsync(int id)
        {
            var res = await _orderService.GetOrderByIdAsync(id);
            if(res == null)
            {
                return NotFound("Order not found!");
            }
            var mapped = _mapper.Map<Order>(res);
            return Ok(mapped);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteOrderAsync(int id)
        {
            var toDelete = await _orderService.GetOrderByIdAsync(id);
            if (toDelete == null)
            {
                return NotFound("Order not found");
            }

            await _orderService.DeleteOrderAsync(id);

            return Ok("Successful delete");
        }

        [HttpGet("{start}/{end}")]
        public async Task<IActionResult> GetFreeRoomsAsync(string start,string end)
        {
            var sd = DateTime.ParseExact(start, "MM-dd-yyyy",null);
            var ed = DateTime.ParseExact(end, "MM-dd-yyyy", null);

            var res = await _orderService.GetFreeRoomsAsync(sd, ed);
            if (res == null)
            {
                return NotFound("No free Rooms");
            }
            var mapped = _mapper.Map<List<Room>>(res);
            mapped.ForEach(r => r.Type = r.GetType());
            return Ok(mapped);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrderAsync([FromBody]Order o)
        {

            var mappedorder = _mapper.Map<DAL.Entities.Order>(o);

            //o = new Order
            //{
            //    Costumer = new Costumer { Name = "Name" },
            //    StartDate = DateTime.Now.AddDays(-1),
            //    EndDate = DateTime.Now.AddDays(1),
            //    Rooms = new List<Room>
            //    {
            //       new Kitchen(){ Price = 100  },
            //       new Bedroom(){ Price = 100  },
            //       new Shower(){ Price = 100  }
            //    }

            //};

            //var mappedorder = new DAL.Entities.Order
            //{
            //    Costumer = new DAL.Entities.Costumer { Name = o.Costumer.Name },
            //    StartDate = o.StartDate,
            //    EndDate = o.EndDate
            //};

            List<DAL.Entities.Room> mappedrooms = new List<DAL.Entities.Room>();
            mappedrooms = _mapper.Map<List <DAL.Entities.Room>>(o.Rooms);

            var neworder = await _orderService.AddOrderAsync(mappedorder,mappedrooms);
            if(neworder == null)
            {
                return NotFound("No free room in the time interval");
            }

            var mappedneworder = _mapper.Map<Order>(neworder);

            return CreatedAtRoute("GetOrderById", new { id = mappedneworder.Id }, mappedneworder);
        }

        [HttpGet]
        public async Task<IActionResult> InsertAsync()
        {
            await _orderService.InsertAsync();
            return Ok();
        }
    }
}