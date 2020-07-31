using System;

namespace PU.Models
{
    
    public partial class BackupDB_Info
    {
        public long ID { get; set; }
        public Nullable<long> UserID { get; set; }
        public Nullable<System.DateTime> DateCreate { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
    
        public virtual Users Users { get; set; }
    }
}
