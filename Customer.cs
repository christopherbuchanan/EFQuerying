namespace EFQuerying;

using System;
using System.Collections.Generic;

public class Customer
{
    public int CustomerID { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public DateTime RegistrationDate { get; set; }

    // Navigation property  
    public ICollection<Order> Orders { get; set; }
}
