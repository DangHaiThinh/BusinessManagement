namespace BusinessManagement.Models
{

using System;
    using System.Collections.Generic;
    
public partial class Product
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Product()
    {

        this.InvoiceInfoes = new HashSet<InvoiceInfo>();

        this.StockReceiptInfoes = new HashSet<StockReceiptInfo>();

    }


    public int ID { get; set; }

    public string Name { get; set; }

    public Nullable<int> UnitsID { get; set; }

    public byte[] Image { get; set; }

    public Nullable<long> ImportPrice { get; set; }

    public Nullable<long> ExportPrice { get; set; }

    public Nullable<long> Count { get; set; }

    public Nullable<bool> IsDelete { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<InvoiceInfo> InvoiceInfoes { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<StockReceiptInfo> StockReceiptInfoes { get; set; }

    public virtual Unit Unit { get; set; }

}

}
