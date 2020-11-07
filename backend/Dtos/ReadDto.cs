namespace backend.Dtos{
    public class ProductsReadDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string Price { get; set; }
    public int available_quantity{get;set;}
    public string available_sizes{get;set;}
    public string description{get;set;}
    public string Image{get;set;}
    public string imageName{get;set;}
    public string seceneName{get;set;}
    
}
public class cart_itemReadDto{
  
  public int cart_item_id{get;set;}
   public int quantity{get;set;}
   public int price{get;set;}
   public int CartId{get;set;}
   public int ProductId{get;set;}
   

}

public class cartReadDto{

    public int CartId{get;set;}
    public int UserId{get;set;}

}


public class paymentReadDto{
 public int paymentId{get;set;}
 public string paymentType{get;set;}
public int amount{get;set;}
}
}