using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using backend.classes.Data;
using backend.products;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Web;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Data
{
    public class SqlCommanderRepo : ICommanderRepo
    {
        private readonly Context _context;
        public SqlCommanderRepo(Context context)
        {
            _context = context;
        }
       
        public void CreateCart(cart command)
        {
             if(command==null){
                throw new ArgumentNullException(nameof(command));
            }
            _context.Carts.Add(command);
        }

        public void CreateCartItem(cart_item command)
        {
                if(command==null){
                throw new ArgumentNullException(nameof(command));
            }
            _context.Cart_Items.Add(command);
        }

        public void CreatePayment(payment command)
        {
                if(command==null){
                throw new ArgumentNullException(nameof(command));
            }
            _context.Payments.Add(command);
        }

        public void CreateProduct(Products prd)
        {
            
            if(prd==null){
                throw new ArgumentNullException(nameof(prd));
            }
            _context.Products.Add(prd);
        }

       

        public void CreateUser(User usr)
        {
            
            if(usr==null){
                throw new ArgumentNullException(nameof(usr));
            }
            _context.Users.Add(usr);
        }

        public void DeleteCart(cart command)
        {
            if(command==null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            _context.Carts.Remove(command);
        }

        public void DeleteCartItem(cart_item command)
        {
            if(command==null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            _context.Cart_Items.Remove(command);
        }

        public void DeletePayment(payment command)
        {
            if(command==null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            _context.Payments.Remove(command);
        }

        /////////////////////////////////////////////////////////////////////////////


        public void DeleteProduct(Products prd)
        {
             if(prd==null)
            {
                throw new ArgumentNullException(nameof(prd));
            }
            _context.Products.Remove(prd);
        }

       

        public void DeleteUser(User usr)
        {
             if(usr==null)
            {
                throw new ArgumentNullException(nameof(usr));
            }
            _context.Users.Remove(usr);
        }

        public IEnumerable<cart_item> GetAllCartItem()
        {
            return _context.Cart_Items.ToList();
        }

        public IEnumerable<cart> GetAllCarts()
        {
            return _context.Carts.ToList();
        }

        public IEnumerable<payment> getAllPayment()
        {
            return _context.Payments.ToList();
        }

        ////////////////////////////////////////////////////////////////////////////////////


        public IEnumerable<Products> GetAllProducts()
        {
             return _context.Products.ToList();
        }

       

        public IEnumerable<User> GetAllUsers()
        {
            
             return _context.Users.ToList();
        }

        public cart GetCartId(int id)
        {
            return _context.Carts.FirstOrDefault(p=>p.CartId==id);
        }

        public cart_item GetCart_ItemId(int id){
            return _context.Cart_Items.FirstOrDefault(p=>p.cart_item_id==id);
             
        }

        public IEnumerable <cart_item> GetCartItemId(int id)
        {
            
           return _context.Cart_Items.Where(s=>s.cart.CartId==id).ToList();
           
        }

        public payment GetPaymentId(int id)
        {
            _context.Payments.Include(p=>p.cart.CartId);
            return _context.Payments.FirstOrDefault(p=>p.paymentId==id);
        }

        /////////////////////////////////////////////////////////////////////////////////////////


        public Products GetProductsId(int id)
        {
            return _context.Products.FirstOrDefault(p=>p.ProductId==id);
        }

        

        public User GetUsersId(int id)
        {
            return _context.Users.FirstOrDefault(p=>p.UserId==id);
        }
////////////////////////////////////////////////////////////////////////////////////
        public bool SaveChanages()
        {
            return (_context.SaveChanges()>=0);
        }

        public void UpdateCart(cart command)
        {
            
        }

        public void UpdateCartItem(cart_item command)
        {
            
        }

        public void UpdatePayment(payment command)
        {
           
        }

        ///////////////////////////////////////////////////////////////////////////////////

        public void UpdateProduct(Products prd)
        {

        }

        
        public void UpdateUser(User usr)
        {
            
        }
    }
}