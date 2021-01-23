using System;
using System.Collections.Generic;

namespace ByHandDeliveryApi.Models
{
    public partial class TblDdvalues
    {
        public int DdvalueId { get; set; }
        public int? DropDownId { get; set; }
        public string Ddkey { get; set; }
        public string Ddvalue { get; set; }
        public int? SortOrderNo { get; set; }
        public bool? IsActive { get; set; }
    }
}
