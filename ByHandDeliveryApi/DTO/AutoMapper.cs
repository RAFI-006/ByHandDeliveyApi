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

            CreateMap<TblOrderDeliveryAddress, OrderDeliveryAddDto>().ForMember(c => c.FromTime, opt => opt.Ignore()).ForMember(c => c.ToTime, opt => opt.Ignore());
            CreateMap<OrderDeliveryAddDto, TblOrderDeliveryAddress>()
                  .ForMember(c => c.Order, opt => opt.Ignore());

            CreateMap<TblOrders, OrderDto>();
            CreateMap<OrderDto, TblOrders>();
            //CreateMap<TblOrders, OrderRequest>().ForMember(c=> c.OrderDeliveryAdd , opt=>opt.Ignore());
            
          //  CreateMap<OrderRequest,TblOrders>().ForMember(c => c.TblOrderDeliveryAddress, opt => opt.Ignore());


            CreateMap<DeliveryPersonAccountDetailsDto, TblDeliveryPersonAccountDetails>().ForMember(t=>t.DeliveryPerson,  opt => opt.Ignore());
            CreateMap<TblDeliveryPersonAccountDetails,DeliveryPersonAccountDetailsDto>();
        }
    }
}
