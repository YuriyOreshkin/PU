using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PU.Models.ModelViews
{
    public class LookUp
    {
        public object DataSource { get; set; }
        public string ValueMember { get; set; }
        public string DisplayMember { get; set; }
        public object Value { get; set; }
    }
}
