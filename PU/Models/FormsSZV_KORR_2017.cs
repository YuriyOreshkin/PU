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
    
    public partial class FormsSZV_KORR_2017
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FormsSZV_KORR_2017()
        {
            this.FormsSZV_KORR_4_2017 = new HashSet<FormsSZV_KORR_4_2017>();
            this.FormsSZV_KORR_5_2017 = new HashSet<FormsSZV_KORR_5_2017>();
            this.StajOsn = new HashSet<StajOsn>();
        }
    
        public long ID { get; set; }
        public short Year { get; set; }
        public byte Code { get; set; }
        public Nullable<short> YearKorr { get; set; }
        public Nullable<byte> CodeKorr { get; set; }
        public string RegNumKorr { get; set; }
        public string INNKorr { get; set; }
        public string KPPKorr { get; set; }
        public Nullable<long> InsurerID { get; set; }
        public long StaffID { get; set; }
        public Nullable<long> FormsODV_1_2017_ID { get; set; }
        public Nullable<byte> TypeInfo { get; set; }
        public Nullable<byte> ContractType { get; set; }
        public Nullable<System.DateTime> ContractDate { get; set; }
        public string ContractNum { get; set; }
        public Nullable<long> PlatCategoryID { get; set; }
        public string DopTarCode { get; set; }
        public System.DateTime DateFilling { get; set; }
        public string ShortNameKorr { get; set; }
        public Nullable<bool> Dismissed { get; set; }
    
        public virtual PlatCategory PlatCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormsSZV_KORR_4_2017> FormsSZV_KORR_4_2017 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormsSZV_KORR_5_2017> FormsSZV_KORR_5_2017 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StajOsn> StajOsn { get; set; }
        public virtual FormsODV_1_2017 FormsODV_1_2017 { get; set; }
        public virtual Staff Staff { get; set; }
    }
}
