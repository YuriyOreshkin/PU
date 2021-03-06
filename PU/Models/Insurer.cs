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
    
    public partial class Insurer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Insurer()
        {
            this.Department = new HashSet<Department>();
            this.FormsDSW_3 = new HashSet<FormsDSW_3>();
            this.FormsRSW2014_1_1 = new HashSet<FormsRSW2014_1_1>();
            this.FormsRSW2014_2_1 = new HashSet<FormsRSW2014_2_1>();
            this.FormsRW3_2015 = new HashSet<FormsRW3_2015>();
            this.FormsSZV_M_2016 = new HashSet<FormsSZV_M_2016>();
            this.FormsPredPens_Zapros = new HashSet<FormsPredPens_Zapros>();
            this.FormsSZV_TD_2020 = new HashSet<FormsSZV_TD_2020>();
            this.Staff = new HashSet<Staff>();
        }
    
        public long ID { get; set; }
        public Nullable<short> CodeRegion { get; set; }
        public Nullable<short> CodeRaion { get; set; }
        public string RegNum { get; set; }
        public Nullable<byte> TypePayer { get; set; }
        public string Name { get; set; }
        public string NameShort { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string InsuranceNumber { get; set; }
        public Nullable<byte> ControlNumber { get; set; }
        public Nullable<short> YearBirth { get; set; }
        public string PhoneContact { get; set; }
        public string BossFIO { get; set; }
        public string BossDolgn { get; set; }
        public Nullable<bool> BossPrint { get; set; }
        public string BossFIODop { get; set; }
        public string BossDolgnDop { get; set; }
        public Nullable<bool> BossDopPrint { get; set; }
        public string BuchgFIO { get; set; }
        public Nullable<bool> BuchgPrint { get; set; }
        public string INN { get; set; }
        public string KPP { get; set; }
        public string EGRIP { get; set; }
        public string EGRUL { get; set; }
        public string OKTMO { get; set; }
        public string OKWED { get; set; }
        public string OKPO { get; set; }
        public string OrgLegalForm { get; set; }
        public string PerformerDolgn { get; set; }
        public string PerformerFIO { get; set; }
        public Nullable<bool> PerformerPrint { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Department> Department { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormsDSW_3> FormsDSW_3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormsRSW2014_1_1> FormsRSW2014_1_1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormsRSW2014_2_1> FormsRSW2014_2_1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormsRW3_2015> FormsRW3_2015 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormsSZV_M_2016> FormsSZV_M_2016 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormsPredPens_Zapros> FormsPredPens_Zapros { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormsSZV_TD_2020> FormsSZV_TD_2020 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Staff> Staff { get; set; }
    }
}
