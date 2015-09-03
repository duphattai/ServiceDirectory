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
    
    public partial class tblAddress
    {
        public tblAddress()
        {
            this.tblDepartments = new HashSet<tblDepartment>();
            this.tblDirectorates = new HashSet<tblDirectorate>();
            this.tblOrganisations = new HashSet<tblOrganisation>();
            this.tblPremises = new HashSet<tblPremis>();
            this.tblTeams = new HashSet<tblTeam>();
        }
    
        public int AddressID { get; set; }
        public string PostCode { get; set; }
        public Nullable<int> TownID { get; set; }
        public string AddressDescription { get; set; }
        public string AddressName { get; set; }
    
        public virtual tblTown tblTown { get; set; }
        public virtual ICollection<tblDepartment> tblDepartments { get; set; }
        public virtual ICollection<tblDirectorate> tblDirectorates { get; set; }
        public virtual ICollection<tblOrganisation> tblOrganisations { get; set; }
        public virtual ICollection<tblPremis> tblPremises { get; set; }
        public virtual ICollection<tblTeam> tblTeams { get; set; }
    }
}
