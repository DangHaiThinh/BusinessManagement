//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Agency
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Agency()
        {
            this.Invoices = new HashSet<Invoice>();
            this.Receipts = new HashSet<Receipt>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> TypeOfAgency { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string District { get; set; }
        public Nullable<System.DateTime> CheckIn { get; set; }
        public string Email { get; set; }
        public Nullable<long> Debt { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Receipt> Receipts { get; set; }
        public virtual TypeOfAgency TypeOfAgency1 { get; set; }
        public virtual District District1 { get; set; }
    }
}
