using AutoMapper;
using HotelProject.DtoLayer.Dtos.RoomDto;
using HotelProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelProject.WebApi.Mapping
{
    public class AutoMapperConfig : Profile
    {
        // Dto larımız ile entitylerimizi bağlayacağımız sınıf.

        public AutoMapperConfig()
        {
            // İlgili automapper metotları constructor içine yazılır.
            // Mapleme işlemi sayesinde dto sınıflarında geçmiş olduğumuz propertyleri entity sınıfındaki propertylerimiz birbirleriyle eşleşmiş oldu.

            CreateMap<RoomAddDto, Room>();
            CreateMap<Room, RoomAddDto>();

            CreateMap<UpdateRoomDto, Room>().ReverseMap(); // Tersini yazmama gerek kalmadı reversemap ile.
        }

    }
}
