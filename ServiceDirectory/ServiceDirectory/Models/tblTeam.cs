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
    
    public partial class tblTeam
    {
        public System.Guid TeamID { get; set; }
        public Nullable<System.Guid> DepartmentID { get; set; }
        public Nullable<System.Guid> ContactID { get; set; }
        public Nullable<System.Guid> AddressID { get; set; }
        public Nullable<System.Guid> BusinessID { get; set; }
        public string TeamName { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public Nullable<int> PhoneNumber { get; set; }
        public Nullable<int> Fax { get; set; }
        public string Email { get; set; }
        public string WebAddress { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        public virtual tblAddress tblAddress { get; set; }
        public virtual tblBusinessType tblBusinessType { get; set; }
        public virtual tblContact tblContact { get; set; }
        public virtual tblDepartment tblDepartment { get; set; }
    }
}
