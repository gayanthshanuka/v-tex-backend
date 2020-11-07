using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.products{
[Table("Products")]
public class Products
{
    [Key]
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string Price { get; set; }
    public int available_quantity{get;set;}
    public string available_sizes{get;set;}
    public string description{get;set;}
    public string seceneName{get;set;}
    public string Image{get;set;}
    public string imageName{get;set;}
    
    public List<cart_item> cart_Items{get;set;}
   

}
[Table("cart_item")]
public class cart_item{
  [Key]
  public int cart_item_id{get;set;}
   public int quantity{get;set;}
   public int price{get;set;}
   public int CartId{get;set;}
   public int ProductId{get;set;}
   [ForeignKey("CartId")]
   public cart cart{get;set;}
   [ForeignKey("ProductId")]
   public Products products{get;set;}
}

[Table("cart")]
public class cart{
        [Key]
        public int CartId { get; set; }
        public int UserId{get;set;}
        [ForeignKey("UserId")]
        public User user{get;set;}

 
    public List<payment> payments{get;set;}
    public List<cart_item> cart_Items{get;set;}

}

[Table("payment")]
public class payment{
 [Key]
 public int paymentId{get;set;}
 public string paymentType{get;set;}
 public int CartId{get;set;}
public int amount{get;set;}
[ForeignKey("CartId")]
public cart cart{get;set;}

}

[Table("Users")]
public class User
{ 
    [Key]
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string Address { get; set; }
    public string first_name{get;set;}
    public string last_name{get;set;}
    public string Password{get;set;}
    public byte[] PasswordHash{get;set;}
    public byte[] PasswordSat{get;set;}
    public List<cart> carts{get;set;}
}
}