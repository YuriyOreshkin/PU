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
    
    public partial class FormsODV_1_5_2017
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FormsODV_1_5_2017()
        {
            this.FormsODV_1_5_2017_OUT = new HashSet<FormsODV_1_5_2017_OUT>();
        }
    
        public long ID { get; set; }
        public Nullable<short> Num { get; set; }
        public long FormsODV_1_2017_ID { get; set; }
        public string Department { get; set; }
        public string Profession { get; set; }
        public Nullable<decimal> StaffCountShtat { get; set; }
        public Nullable<long> StaffCountFakt { get; set; }
        public string VidRabotFakt { get; set; }
        public string DocsName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormsODV_1_5_2017_OUT> FormsODV_1_5_2017_OUT { get; set; }
        public virtual FormsODV_1_2017 FormsODV_1_2017 { get; set; }
    }
}
