using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ByHandDeliveryApi.Models
{
    public partial class TblDeliveryPersonWallet
    {
        public int DeliveryPersonWalletID { get; set; }
        public int DeliveryPersonID { get; set; }
        public int Wallet { get; set; }
        public TblDeliveryPerson DeliveryPerson { get; set; }
    }
}
