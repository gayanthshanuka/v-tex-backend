namespace backend.Dtos{
         public class productUpdateDto{
        public string ProductName { get; set; }
    public string Price { get; set; }
    
    public int available_quantity{get;set;}
    public string available_sizes{get;set;}
    public string description{get;set;}
    public string Image{get;set;}
    public string imageName{get;set;}
    public string seceneName{get;set;}
    }

    public class cart_itemUpdateDto{
     public int quantity{get;set;}
     public int price{get;set;}
     public int cartId{get;set;}
     public int ProductId{get;set;}

    }

    public class cartUpdateDto{
    public int UserId{get;set;}
    }
    public class paymentUpdateDto{
        public string paymentType{get;set;}
        public int amount{get;set;}
        public int cartId{get;set;}
    }
    
}