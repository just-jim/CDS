namespace Mock.Order.Models;

public class Order {
    public string OrderNumber { get; set; }
    public string CustomerName { get; set; }
    public string OrderDate { get; set; }
    public int TotalAssets { get; set; }
    public List<Asset> Assets { get; set; }
}