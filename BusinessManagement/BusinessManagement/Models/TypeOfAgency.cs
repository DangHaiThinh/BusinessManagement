namespace BusinessManagement.Models
{

using System;
    using System.Collections.Generic;
    
public partial class TypeOfAgency
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public TypeOfAgency()
    {

        this.Agencies = new HashSet<Agency>();

    }


    public int ID { get; set; }

    public string Name { get; set; }

    public Nullable<long> MaxOfDebt { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Agency> Agencies { get; set; }

}

}
