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
    
    public partial class tblGovOfficeRegion
    {
        public System.Guid GovOfficeRegionID { get; set; }
        public Nullable<System.Guid> CountyID { get; set; }
        public string GovOfficeRegionName { get; set; }
        public string GovOfficeRegionDescription { get; set; }
        public bool IsActive { get; set; }
    
        public virtual tblCounty tblCounty { get; set; }
    }
}
