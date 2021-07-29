using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ByHandDeliveryApi.DTO
{
    public class DDValueDTO
    {
        public int DdvalueId { get; set; }
        public int? DropDownId { get; set; }
        public string Ddkey { get; set; }
        public string Ddvalue { get; set; }
        public int? SortOrderNo { get; set; }
        public bool? IsActive { get; set; }
    }
}
