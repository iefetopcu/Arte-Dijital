﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class artedijitalEntities : DbContext
    {
        public artedijitalEntities()
            : base("name=artedijitalEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<category> categories { get; set; }
        public virtual DbSet<comenttable> comenttables { get; set; }
        public virtual DbSet<footermessage> footermessages { get; set; }
        public virtual DbSet<FreelanceJob> FreelanceJobs { get; set; }
        public virtual DbSet<Freelancer> Freelancers { get; set; }
        public virtual DbSet<menuler> menulers { get; set; }
        public virtual DbSet<posttable> posttables { get; set; }
        public virtual DbSet<projectstatu> projectstatus { get; set; }
        public virtual DbSet<ProjectTable> ProjectTables { get; set; }
        public virtual DbSet<responsibletable> responsibletables { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<taskstatu> taskstatus { get; set; }
        public virtual DbSet<TaskTable> TaskTables { get; set; }
        public virtual DbSet<userrole> userroles { get; set; }
        public virtual DbSet<usertable> usertables { get; set; }
    }
}