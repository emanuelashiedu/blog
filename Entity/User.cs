//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.Blogs = new HashSet<Blog>();
        }
    
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserPassword { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string IP { get; set; }
        public Nullable<bool> isActive { get; set; }
        public Nullable<short> UserRole { get; set; }
    
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual Role Role { get; set; }
    }
}
