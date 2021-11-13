namespace BusinessManagement.Models
{

using System;
    using System.Collections.Generic;
    
public partial class Account
{

    public string Username { get; set; }

    public string Password { get; set; }

    public string DisplayName { get; set; }

    public byte[] Image { get; set; }

    public string Location { get; set; }

    public string PhoneNumber { get; set; }

}

}
