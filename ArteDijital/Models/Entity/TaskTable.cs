//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ArteDijital.Models.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class TaskTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TaskTable()
        {
            this.responsibletables = new HashSet<responsibletable>();
        }
    
        public long id { get; set; }
        public string taskname { get; set; }
        public string taskexplanation { get; set; }
        public Nullable<System.DateTime> deadline { get; set; }
        public Nullable<System.DateTime> createtime { get; set; }
        public Nullable<long> taskstatus { get; set; }
        public Nullable<long> creatorid { get; set; }
        public Nullable<long> projectid { get; set; }
    
        public virtual ProjectTable ProjectTable { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<responsibletable> responsibletables { get; set; }
        public virtual taskstatu taskstatu { get; set; }
        public virtual usertable usertable { get; set; }
    }
}