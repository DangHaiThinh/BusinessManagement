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
    
    public partial class StockReceiptInfo
    {
        public int StockReceiptID { get; set; }
        public int ProductID { get; set; }
        public Nullable<long> Amount { get; set; }
        public Nullable<long> Price { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual StockReceipt StockReceipt { get; set; }
    }
}
