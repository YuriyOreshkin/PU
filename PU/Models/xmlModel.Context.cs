﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class pfrXMLEntities : DbContext
    {
        public pfrXMLEntities()
            : base("name=pfrXMLEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<StaffList> StaffList { get; set; }
        public virtual DbSet<xmlFile> xmlFile { get; set; }
        public virtual DbSet<rsw2014> rsw2014 { get; set; }
        public virtual DbSet<xmlInfo> xmlInfo { get; set; }
    }
}
