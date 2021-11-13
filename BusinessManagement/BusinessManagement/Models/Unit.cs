namespace BusinessManagement.Models
{

using System;
    using System.Collections.Generic;
    
public partial class Unit
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Unit()
    {

        this.Products = new HashSet<Product>();

    }


    public int ID { get; set; }

    public string Name { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Product> Products { get; set; }

}

}
