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
    
    public partial class tblGroupReference
    {
        public tblGroupReference()
        {
            this.tblReferenceDatas = new HashSet<tblReferenceData>();
        }
    
        public int GroupReferenceID { get; set; }
        public string GroupValue { get; set; }
    
        public virtual ICollection<tblReferenceData> tblReferenceDatas { get; set; }
    }
}
