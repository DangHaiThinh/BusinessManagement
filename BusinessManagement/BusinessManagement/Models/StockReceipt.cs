namespace BusinessManagement.Models
{

using System;
    using System.Collections.Generic;
    
public partial class StockReceipt
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public StockReceipt()
    {

        this.StockReceiptInfoes = new HashSet<StockReceiptInfo>();

    }


    public int ID { get; set; }

    public Nullable<System.DateTime> CheckIn { get; set; }

    public Nullable<long> Total { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<StockReceiptInfo> StockReceiptInfoes { get; set; }

}

}
