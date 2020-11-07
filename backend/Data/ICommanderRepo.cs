using System.Collections.Generic;
using backend.products;
using Microsoft.AspNetCore.Http;

namespace backend.Data
{
    public interface ICommanderRepo{
       
        bool SaveChanages();
        IEnumerable <Products> GetAllProducts();
        IEnumerable <User> GetAllUsers();
        IEnumerable<cart> GetAllCarts();
        IEnumerable<cart_item> GetAllCartItem();
        IEnumerable<payment> getAllPayment();

        Products GetProductsId(int id);
        User GetUsersId(int id);
        cart GetCartId(int id);
        IEnumerable <cart_item> GetCartItemId(int id);
        payment GetPaymentId(int id);
        cart_item GetCart_ItemId(int id);
        void CreateUser(User usr);
        void CreateProduct(Products prd);
        void CreateCart(cart command);
        void CreateCartItem(cart_item command);
        void CreatePayment(payment command);
        
        void UpdateUser(User usr);
        void UpdateProduct(Products prd);
        void UpdateCart(cart command);
        void UpdateCartItem(cart_item command);
        void UpdatePayment(payment command);
      


        void DeleteUser(User  usr);
        void DeleteProduct(Products products);
        void DeleteCart(cart command);
        void DeleteCartItem(cart_item command);
        void DeletePayment(payment command);
        
    }

}