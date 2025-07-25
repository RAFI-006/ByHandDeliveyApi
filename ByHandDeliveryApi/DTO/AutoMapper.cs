﻿using AutoMapper;
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

            CreateMap<TblDeliveryPersonDetails, DeliveryPersonDetailDto>();
            CreateMap<DeliveryPersonDetailDto, TblDeliveryPersonDetails>();

            CreateMap<TblDeliveryPersonWallet, DeliveryPersonWalletDto>();
            CreateMap<DeliveryPersonWalletDto, TblDeliveryPersonWallet>();

            CreateMap<TblOrderDeliveryAddress, OrderDeliveryAddDto>();
            CreateMap<OrderDeliveryAddDto, TblOrderDeliveryAddress>()
                  .ForMember(c => c.Order, opt => opt.Ignore());

            CreateMap<DDValueDTO, TblDdvalues>().ForMember(c => c.DropDown, opt => opt.Ignore());
            CreateMap<TblDdvalues, DDValueDTO>();


            CreateMap<TblOrders, OrderDto>();
            CreateMap<OrderDto, TblOrders>();
            //CreateMap<TblOrders, OrderRequest>().ForMember(c=> c.OrderDeliveryAdd , opt=>opt.Ignore());
            
          //  CreateMap<OrderRequest,TblOrders>().ForMember(c => c.TblOrderDeliveryAddress, opt => opt.Ignore());


            CreateMap<TblDeliveryPersonCancelOrderDetails,DeliveryPersonCancelOrderDetailsDTO>();
            CreateMap<DeliveryPersonCancelOrderDetailsDTO, TblDeliveryPersonCancelOrderDetails>().ForMember(t => t.DeliveryPerson, opt => opt.Ignore()).ForMember(t => t.Orders, opt => opt.Ignore());

            CreateMap<DeliveryPersonPaymentTransactionDetailsDTO, TblDeliveryPersonPaymentTransactionDetails>().ForMember(t => t.DeliveryPerson, opt => opt.Ignore());
            CreateMap<TblDeliveryPersonPaymentTransactionDetails, DeliveryPersonPaymentTransactionDetailsDTO>();

        }
    }
}
