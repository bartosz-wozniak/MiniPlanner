//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class Preferences
    {
        public int id { get; set; }
        public int courseId { get; set; }
        public int userId { get; set; }
        public int scheduleId { get; set; }
    
        public virtual Courses Courses { get; set; }
        public virtual Users Users { get; set; }
    }
}
