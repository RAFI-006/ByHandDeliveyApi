using System;
using System.Collections.Generic;

namespace ByHandDeliveryApi.Models
{
    public partial class TblDropDown
    {
        public int DropDownId { get; set; }
        public string Ddname { get; set; }
        public string DropDownKey { get; set; }
        public bool? IsActive { get; set; }
    }
}
