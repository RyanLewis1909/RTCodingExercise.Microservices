using AutoMapper;
using Catalog.API.Data;
using Catalog.API.Messages.Request;
using WebMVC.Models;

namespace WebMVC
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Plate, PlateModel>().ReverseMap();
            CreateMap<PlateModel, UpdatePlateRequest>().ReverseMap();
            CreateMap<PlateModel, CreatePlateRequest>().ReverseMap();
        }
    }
}
