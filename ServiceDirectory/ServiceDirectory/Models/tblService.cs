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
    
    public partial class tblService
    {
        public tblService()
        {
            this.tblFundings = new HashSet<tblFunding>();
            this.tblOrganisationServices = new HashSet<tblOrganisationService>();
            this.tblServicePremises = new HashSet<tblServicePremise>();
            this.tblReferenceDatas = new HashSet<tblReferenceData>();
            this.tblReferenceDatas1 = new HashSet<tblReferenceData>();
        }
    
        public int ServiceID { get; set; }
        public Nullable<int> ProgrammeID { get; set; }
        public Nullable<int> ContactID { get; set; }
        public string ServiceName { get; set; }
        public string ShortDescription { get; set; }
        public string ClientDescription { get; set; }
        public Nullable<System.DateTime> StartExpected { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> ExtendableYears { get; set; }
        public Nullable<int> ExtendableMonths { get; set; }
        public string FullDescription { get; set; }
        public string DeptCode { get; set; }
        public string DescriptionDelivery { get; set; }
        public string ContractCode { get; set; }
        public string ContractValue { get; set; }
        public Nullable<bool> ContractPayment { get; set; }
        public Nullable<System.DateTime> TimeLimitedYears { get; set; }
        public Nullable<System.DateTime> TimeLimitedMonths { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string Participation { get; set; }
    
        public virtual tblContact tblContact { get; set; }
        public virtual ICollection<tblFunding> tblFundings { get; set; }
        public virtual ICollection<tblOrganisationService> tblOrganisationServices { get; set; }
        public virtual tblProgramme tblProgramme { get; set; }
        public virtual ICollection<tblServicePremise> tblServicePremises { get; set; }
        public virtual ICollection<tblReferenceData> tblReferenceDatas { get; set; }
        public virtual ICollection<tblReferenceData> tblReferenceDatas1 { get; set; }
    }
}
