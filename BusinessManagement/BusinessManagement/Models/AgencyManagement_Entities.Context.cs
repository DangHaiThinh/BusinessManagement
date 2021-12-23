﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BusinessManagement.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class AgencyManagementEntities : DbContext
    {
        public AgencyManagementEntities()
            : base("name=AgencyManagementEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Agency> Agencies { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceInfo> InvoiceInfoes { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Receipt> Receipts { get; set; }
        public virtual DbSet<StockReceipt> StockReceipts { get; set; }
        public virtual DbSet<StockReceiptInfo> StockReceiptInfoes { get; set; }
        public virtual DbSet<TypeOfAgency> TypeOfAgencies { get; set; }
        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<AutoLogin> AutoLogins { get; set; }
    }
}
