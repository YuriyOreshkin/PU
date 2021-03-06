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
    
    public partial class Staff
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Staff()
        {
            this.FormsADW_1 = new HashSet<FormsADW_1>();
            this.FormsADW_2 = new HashSet<FormsADW_2>();
            this.FormsADW_3 = new HashSet<FormsADW_3>();
            this.FormsDSW_3_Staff = new HashSet<FormsDSW_3_Staff>();
            this.FormsPredPens_Zapros_Staff = new HashSet<FormsPredPens_Zapros_Staff>();
            this.FormsRSW = new HashSet<FormsRSW>();
            this.FormsRSW2014_1_Razd_6_1 = new HashSet<FormsRSW2014_1_Razd_6_1>();
            this.FormsSPW2 = new HashSet<FormsSPW2>();
            this.FormsSZV_6 = new HashSet<FormsSZV_6>();
            this.FormsSZV_6_4 = new HashSet<FormsSZV_6_4>();
            this.FormsSZV_ISH_2017 = new HashSet<FormsSZV_ISH_2017>();
            this.FormsSZV_KORR_2017 = new HashSet<FormsSZV_KORR_2017>();
            this.FormsSZV_M_2016_Staff = new HashSet<FormsSZV_M_2016_Staff>();
            this.FormsSZV_STAJ_2017 = new HashSet<FormsSZV_STAJ_2017>();
            this.FormsSZV_TD_2020_Staff = new HashSet<FormsSZV_TD_2020_Staff>();
            this.StaffDateWork = new HashSet<StaffDateWork>();
            this.StaffInfo = new HashSet<StaffInfo>();
        }
    
        public long ID { get; set; }
        public Nullable<long> InsurerID { get; set; }
        public Nullable<long> DepartmentID { get; set; }
        public string InsuranceNumber { get; set; }
        public Nullable<byte> ControlNumber { get; set; }
        public Nullable<long> TabelNumber { get; set; }
        public string INN { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public Nullable<byte> Sex { get; set; }
        public Nullable<System.DateTime> DateBirth { get; set; }
        public Nullable<byte> Dismissed { get; set; }
        public Nullable<long> DolgnID { get; set; }
    
        public virtual Department Department { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormsADW_1> FormsADW_1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormsADW_2> FormsADW_2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormsADW_3> FormsADW_3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormsDSW_3_Staff> FormsDSW_3_Staff { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormsPredPens_Zapros_Staff> FormsPredPens_Zapros_Staff { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormsRSW> FormsRSW { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormsRSW2014_1_Razd_6_1> FormsRSW2014_1_Razd_6_1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormsSPW2> FormsSPW2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormsSZV_6> FormsSZV_6 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormsSZV_6_4> FormsSZV_6_4 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormsSZV_ISH_2017> FormsSZV_ISH_2017 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormsSZV_KORR_2017> FormsSZV_KORR_2017 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormsSZV_M_2016_Staff> FormsSZV_M_2016_Staff { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormsSZV_STAJ_2017> FormsSZV_STAJ_2017 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormsSZV_TD_2020_Staff> FormsSZV_TD_2020_Staff { get; set; }
        public virtual Insurer Insurer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StaffDateWork> StaffDateWork { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StaffInfo> StaffInfo { get; set; }
    }
}
