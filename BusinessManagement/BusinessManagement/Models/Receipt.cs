
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
    
public partial class Receipt
{

    public int ID { get; set; }

    public Nullable<int> AgencyID { get; set; }

    public Nullable<System.DateTime> Date { get; set; }

    public Nullable<long> Amount { get; set; }

    public string Message { get; set; }



    public virtual Agency Agency { get; set; }

}

}
