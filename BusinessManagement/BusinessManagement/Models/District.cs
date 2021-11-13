namespace BusinessManagement.Models
{

using System;
    using System.Collections.Generic;
    
public partial class District
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public District()
    {

        this.Agencies = new HashSet<Agency>();

    }


    public string Name { get; set; }

    public Nullable<int> NumberAgencyInDistrict { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Agency> Agencies { get; set; }

}

}
