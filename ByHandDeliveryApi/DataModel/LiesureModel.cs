using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ByHandDeliveryApi.DataModel
{
    public class LiesureModel
    {
      public  decimal? DeliveryPersonCharges { get; set; }
      public int? CashReceivedFromOrder{ get; set; }
      public decimal AmountRecievedByCompany { get; set; }
      public decimal CancellationFee { get; set; }
      public decimal? AmountPaidToCompany { get; set; }
      public decimal? BalanceAmount { get; set; }
    }
}
