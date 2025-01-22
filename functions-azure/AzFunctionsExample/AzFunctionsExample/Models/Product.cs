
namespace AzFunctionsExample.Models;

public class Product
{
    public string id { get; set; }
    public string name { get; set; }
    public string price { get; set; }
    public string description { get; set; }
    public int amount { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public string category_id { get; set; }
}
