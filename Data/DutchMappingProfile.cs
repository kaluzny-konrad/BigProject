using AutoMapper;
using BigProject.Data.Entities;
using BigProject.Models;

namespace BigProject.Data;

public class DutchMappingProfile : Profile
{
    public DutchMappingProfile()
    {
        CreateMap<Order, OrderModel>()
            .ForMember(o => o.OrderId, ex => ex.MapFrom(o => o.Id))
            .ReverseMap();

        CreateMap<OrderItem, OrderItemModel>()
            .ReverseMap()
            .ForMember(m => m.Product, opt => opt.Ignore());
    }
}