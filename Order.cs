namespace EFQuerying;

public class Order
{
    public int OrderID { get; set; }

    public int CustomerID { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    // Navigation properties  
    public Customer Customer { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; }
}

public class ExpeditedOrder : Order
{
    public string? ShippingMethod { get; set; }
    public virtual int? DaysToDeliver { get; set; }
}

public class NextDayOrder : ExpeditedOrder
{
    public override int? DaysToDeliver { get => 1; set => throw new InvalidOperationException(); }
    public int? AdditionalCharge { get; set; }
}