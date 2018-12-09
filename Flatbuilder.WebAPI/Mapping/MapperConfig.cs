using AutoMapper;
using Flatbuilder.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flatbuilder.WebAPI.Mapping
{
    public class MapperConfig
    {
        public static IMapper Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, Flatbuilder.DTO.Order>()
                    .ForMember(dto => dto.Rooms, opt => opt.Ignore())
                    .AfterMap((o, dto, ctx) => dto.Rooms = o.OrderRooms.Select(or => ctx.Mapper.Map<Flatbuilder.DTO.Room>(or.Room)).ToList());
                cfg.CreateMap<Flatbuilder.DTO.Order, Order>()
                   .AfterMap((dto, o, ctx) =>
                   {
                       o.OrderRooms = dto.Rooms.Select(r => new OrderRoom { Room = ctx.Mapper.Map<Room>(r), Order = o }).ToList();
                   })
                ;

                cfg.CreateMap<Room, Flatbuilder.DTO.Room>()
                .Include<Shower, Flatbuilder.DTO.Shower>()
                .Include<Bedroom, Flatbuilder.DTO.Bedroom>()
                .Include<Kitchen, Flatbuilder.DTO.Kitchen>();

                cfg.CreateMap<Flatbuilder.DTO.Room, Room>()
                .Include<Flatbuilder.DTO.Shower, Shower>()
                .Include<Flatbuilder.DTO.Bedroom, Bedroom>()
                .Include<Flatbuilder.DTO.Kitchen, Kitchen>();

                cfg.CreateMap<Flatbuilder.DTO.Bedroom, Bedroom>().ReverseMap();
                cfg.CreateMap<Flatbuilder.DTO.Shower, Shower>().ReverseMap();
                cfg.CreateMap<Flatbuilder.DTO.Kitchen, Kitchen>().ReverseMap();

                cfg.CreateMap<Flatbuilder.DTO.Costumer, Costumer>().ReverseMap();

            });

            return config.CreateMapper();
        }
    }
}
