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
    
    public partial class xmlInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public xmlInfo()
        {
            this.rsw2014 = new HashSet<rsw2014>();
            this.StaffList = new HashSet<StaffList>();
            this.xmlFile = new HashSet<xmlFile>();
            this.xmlInfo1 = new HashSet<xmlInfo>();
        }
    
        public long ID { get; set; }
        public Nullable<long> Num { get; set; }
        public Nullable<long> CountDoc { get; set; }
        public Nullable<long> CountStaff { get; set; }
        public string DocType { get; set; }
        public Nullable<short> Year { get; set; }
        public Nullable<byte> Quarter { get; set; }
        public Nullable<short> YearKorr { get; set; }
        public Nullable<byte> QuarterKorr { get; set; }
        public string UserName { get; set; }
        public Nullable<System.DateTime> DateCreate { get; set; }
        public string FileName { get; set; }
        public Nullable<long> ParentID { get; set; }
        public Nullable<long> SourceID { get; set; }
        public Nullable<System.Guid> UniqGUID { get; set; }
        public Nullable<long> InsurerID { get; set; }
        public string FormatType { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rsw2014> rsw2014 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StaffList> StaffList { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<xmlFile> xmlFile { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<xmlInfo> xmlInfo1 { get; set; }
        public virtual xmlInfo xmlInfo2 { get; set; }
    }
}