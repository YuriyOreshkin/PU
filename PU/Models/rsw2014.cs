//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PU.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class rsw2014
    {
        public long ID { get; set; }
        public Nullable<long> xmlInfo_ID { get; set; }
        public Nullable<decimal> RSW_2_5_1_2 { get; set; }
        public Nullable<decimal> RSW_2_5_1_3 { get; set; }
        public Nullable<decimal> RSW_2_5_2_4 { get; set; }
        public Nullable<decimal> RSW_2_5_2_5 { get; set; }
        public Nullable<decimal> RSW_2_5_2_6 { get; set; }
    
        public virtual xmlInfo xmlInfo { get; set; }
    }
}
