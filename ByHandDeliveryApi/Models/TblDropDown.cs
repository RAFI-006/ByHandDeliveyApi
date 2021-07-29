using System;
using System.Collections.Generic;

namespace ByHandDeliveryApi.Models
{
    public partial class TblDropDown
    {
        public TblDropDown()
        {
            TblDdvalues = new HashSet<TblDdvalues>();
        }

        public int DropDownID { get; set; }
        public string Ddname { get; set; }
        public string DropDownKey { get; set; }
        public bool? IsActive { get; set; }

        public ICollection<TblDdvalues> TblDdvalues { get; set; }
    }
}
