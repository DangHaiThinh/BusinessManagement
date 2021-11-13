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


    public virtual DbSet<sysdiagram> sysdiagrams { get; set; }

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

}

}

