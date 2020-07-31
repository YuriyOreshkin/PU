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
    
    public partial class FormsSZV_6
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FormsSZV_6()
        {
            this.StajOsn = new HashSet<StajOsn>();
        }
    
        public long ID { get; set; }
        public Nullable<long> InsurerID { get; set; }
        public long StaffID { get; set; }
        public long TypeInfoID { get; set; }
        public long PlatCategoryID { get; set; }
        public short Year { get; set; }
        public byte Quarter { get; set; }
        public Nullable<short> YearKorr { get; set; }
        public Nullable<byte> QuarterKorr { get; set; }
        public Nullable<decimal> SUMTAXYEAR { get; set; }
        public Nullable<bool> AutoCalc { get; set; }
        public Nullable<decimal> s_1_0 { get; set; }
        public Nullable<decimal> s_1_1 { get; set; }
        public Nullable<decimal> s_2_0 { get; set; }
        public Nullable<decimal> s_2_1 { get; set; }
        public Nullable<decimal> s_3_0 { get; set; }
        public Nullable<decimal> s_3_1 { get; set; }
        public Nullable<decimal> s_4_0 { get; set; }
        public Nullable<decimal> s_4_1 { get; set; }
        public Nullable<decimal> s_5_0 { get; set; }
        public Nullable<decimal> s_5_1 { get; set; }
        public Nullable<decimal> s_6_0 { get; set; }
        public Nullable<decimal> s_6_1 { get; set; }
        public Nullable<decimal> s_7_0 { get; set; }
        public Nullable<decimal> s_7_1 { get; set; }
        public Nullable<decimal> s_8_0 { get; set; }
        public Nullable<decimal> s_8_1 { get; set; }
        public Nullable<decimal> s_9_0 { get; set; }
        public Nullable<decimal> s_9_1 { get; set; }
        public Nullable<decimal> s_10_0 { get; set; }
        public Nullable<decimal> s_10_1 { get; set; }
        public Nullable<decimal> s_11_0 { get; set; }
        public Nullable<decimal> s_11_1 { get; set; }
        public Nullable<decimal> s_12_0 { get; set; }
        public Nullable<decimal> s_12_1 { get; set; }
        public Nullable<decimal> SumFeePFR_Strah { get; set; }
        public Nullable<decimal> SumPayPFR_Strah { get; set; }
        public Nullable<decimal> SumPayPFR_Nakop { get; set; }
        public Nullable<decimal> SumFeePFR_Nakop { get; set; }
        public Nullable<decimal> SumFeePFR_Strah_D { get; set; }
        public Nullable<decimal> SumFeePFR_Nakop_D { get; set; }
        public System.DateTime DateFilling { get; set; }
        public Nullable<byte> DispatchPFR { get; set; }
    
        public virtual PlatCategory PlatCategory { get; set; }
        public virtual TypeInfo TypeInfo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StajOsn> StajOsn { get; set; }
        public virtual Staff Staff { get; set; }
    }
}