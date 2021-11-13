namespace BusinessManagement.Models
{

using System;
    using System.Collections.Generic;
    
public partial class InvoiceInfo
{

    public int InvoiceID { get; set; }

    public int ProductID { get; set; }

    public Nullable<int> Amount { get; set; }

    public Nullable<long> Total { get; set; }



    public virtual Invoice Invoice { get; set; }

    public virtual Product Product { get; set; }

}

}
