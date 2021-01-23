using AutoMapper;
using ByHandDeliveryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ByHandDeliveryApi.DTO
{
    public class AutoMapper :Profile
    {
        public AutoMapper()
        {
            CreateMap<TblCustomers, CustomersDto>();
            CreateMap<CustomersDto, TblCustomers>()
                  .ForMember(c=>c.TblOrders,opt=>opt.Ignore());

            CreateMap<TblDeliveryPerson, DeliveryPersonDto>();
            CreateMap<DeliveryPersonDto, TblDeliveryPerson>()
                  .ForMember(c => c.TblOrders, opt => opt.Ignore());

            CreateMap<TblOrderDeliveryAddress, OrderDeliveryAddDto>();
            CreateMap<OrderDeliveryAddDto, TblOrderDeliveryAddress>()
                  .ForMember(c => c.Order, opt => opt.Ignore());

            CreateMap<TblOrders, OrderDto>();
            CreateMap<OrderDto, TblOrders>();

            CreateMap<DeliveryPersonAccountDetailsDto, TblDeliveryPersonAccountDetails>().ForMember(t=>t.DeliveryPerson,  opt => opt.Ignore());
            CreateMap<TblDeliveryPersonAccountDetails,DeliveryPersonAccountDetailsDto>();
        }
    }
}
