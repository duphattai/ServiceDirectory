//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServiceDirectory.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblVolunteering
    {
        public System.Guid ContactID { get; set; }
        public System.Guid PremisesID { get; set; }
        public string Purpose { get; set; }
        public string Detail { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> VolunteerNos { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        public virtual tblContact tblContact { get; set; }
        public virtual tblPremis tblPremis { get; set; }
    }
}
