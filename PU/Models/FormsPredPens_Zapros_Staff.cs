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
    
    public partial class FormsPredPens_Zapros_Staff
    {
        public long ID { get; set; }
        public long FormsPredPens_ZaprosID { get; set; }
        public long StaffID { get; set; }
    
        public virtual FormsPredPens_Zapros FormsPredPens_Zapros { get; set; }
        public virtual Staff Staff { get; set; }
    }
}
