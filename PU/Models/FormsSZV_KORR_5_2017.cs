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
    
    public partial class FormsSZV_KORR_5_2017
    {
        public long ID { get; set; }
        public long FormsSZV_KORR_2017_ID { get; set; }
        public Nullable<byte> Month { get; set; }
        public Nullable<long> SpecOcenkaUslTrudaID { get; set; }
        public Nullable<decimal> s_0 { get; set; }
        public Nullable<decimal> s_1 { get; set; }
    
        public virtual SpecOcenkaUslTruda SpecOcenkaUslTruda { get; set; }
        public virtual FormsSZV_KORR_2017 FormsSZV_KORR_2017 { get; set; }
    }
}
