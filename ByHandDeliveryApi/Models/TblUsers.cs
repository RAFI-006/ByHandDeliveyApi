using System;
using System.Collections.Generic;

namespace ByHandDeliveryApi.Models
{
    public partial class TblUsers
    {
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
    }
}
