namespace StoreManagement.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class STOREMANAGEMENTEntities : DbContext
    {
        public STOREMANAGEMENTEntities()
            : base("name=STOREMANAGEMENTEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ACCOUNT> ACCOUNTs { get; set; }
        public virtual DbSet<BILL> BILLs { get; set; }
        public virtual DbSet<BILLINFO> BILLINFOes { get; set; }
        public virtual DbSet<GOOD> GOODS { get; set; }
        public virtual DbSet<STORE> STOREs { get; set; }
        public virtual DbSet<TYPEOFSTORE> TYPEOFSTOREs { get; set; }
    }
}
