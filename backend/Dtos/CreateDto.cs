

namespace backend.Dtos{
public class UserDto
    {

    public int UserId { get; set; }
    public string UserName { get; set; }
    public string Address { get; set; }
    public string first_name{get;set;}
    public string last_name{get;set;}

    public string Password{get;set;}
    public byte[] PasswordHash{get;set;}
    public byte[] PasswordSat{get;set;}
    }

    public class productCreateDto{
    public string ProductName { get; set; }
    public string Price { get; set; }
    public int available_quantity{get;set;}
    public string available_sizes{get;set;}
    public string description{get;set;}
    public string Image{get;set;}
    public string imageName{get;set;}
    public string seceneName{get;set;}
    }

    public class cart_itemCreateDto{
     public int quantity{get;set;}
     public int price{get;set;}
     public int cartId{get;set;}
     public int ProductId{get;set;}

    }

    public class cartCreateDto{
    public int UserId{get;set;}
    }
    public class paymentCreateDto{
        public string paymentType{get;set;}
        public int amount{get;set;}
        public int cartId{get;set;}
    }
}