//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Blog
    {
        public int BlogID { get; set; }
        public string BlogTitle { get; set; }
        public string BlogSubTitle { get; set; }
        public string BlogText { get; set; }
        public Nullable<System.DateTime> BlogCreatedDate { get; set; }
        public Nullable<bool> isActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public string IP { get; set; }
        public Nullable<System.DateTime> BlogUpdatedDate { get; set; }
        public Nullable<int> BlogUpdatedBy { get; set; }
    
        public virtual User User { get; set; }
    }
}