namespace StoreManagement.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class GOOD
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GOOD()
        {
            this.BILLINFOes = new HashSet<BILLINFO>();
        }
    
        public int ID { get; set; }
        public string NAME { get; set; }
        public string UNIT { get; set; }
        public Nullable<long> COSTPRICE { get; set; }
        public Nullable<long> PRICE { get; set; }
        public Nullable<int> COUNT { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BILLINFO> BILLINFOes { get; set; }
    }
}
