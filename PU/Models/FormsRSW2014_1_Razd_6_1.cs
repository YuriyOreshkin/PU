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
    
    public partial class FormsRSW2014_1_Razd_6_1
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FormsRSW2014_1_Razd_6_1()
        {
            this.FormsRSW2014_1_Razd_6_4 = new HashSet<FormsRSW2014_1_Razd_6_4>();
            this.FormsRSW2014_1_Razd_6_6 = new HashSet<FormsRSW2014_1_Razd_6_6>();
            this.FormsRSW2014_1_Razd_6_7 = new HashSet<FormsRSW2014_1_Razd_6_7>();
            this.StajOsn = new HashSet<StajOsn>();
        }
    
        public long ID { get; set; }
        public long StaffID { get; set; }
        public long TypeInfoID { get; set; }
        public Nullable<short> YearKorr { get; set; }
        public Nullable<byte> QuarterKorr { get; set; }
        public string RegNumKorr { get; set; }
        public Nullable<bool> AutoCalc { get; set; }
        public Nullable<decimal> SumFeePFR { get; set; }
        public System.DateTime DateFilling { get; set; }
        public Nullable<byte> DispatchPFR { get; set; }
        public short Year { get; set; }
        public byte Quarter { get; set; }
        public Nullable<long> InsurerID { get; set; }
        public Nullable<byte> CorrectionNum { get; set; }
    
        public virtual TypeInfo TypeInfo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormsRSW2014_1_Razd_6_4> FormsRSW2014_1_Razd_6_4 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormsRSW2014_1_Razd_6_6> FormsRSW2014_1_Razd_6_6 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormsRSW2014_1_Razd_6_7> FormsRSW2014_1_Razd_6_7 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StajOsn> StajOsn { get; set; }
        public virtual Staff Staff { get; set; }
    }
}
