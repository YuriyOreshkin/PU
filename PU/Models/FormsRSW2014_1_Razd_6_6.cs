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
    
    public partial class FormsRSW2014_1_Razd_6_6
    {
        public long ID { get; set; }
        public long FormsRSW2014_1_Razd_6_1_ID { get; set; }
        public Nullable<short> AccountPeriodYear { get; set; }
        public Nullable<byte> AccountPeriodQuarter { get; set; }
        public Nullable<decimal> SumFeePFR_D { get; set; }
        public Nullable<decimal> SumFeePFR_StrahD { get; set; }
        public Nullable<decimal> SumFeePFR_NakopD { get; set; }
    
        public virtual FormsRSW2014_1_Razd_6_1 FormsRSW2014_1_Razd_6_1 { get; set; }
    }
}