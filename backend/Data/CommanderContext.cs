using backend.products;
using Microsoft.EntityFrameworkCore;


namespace backend.classes.Data
{
    public class Context:DbContext
    {
        public Context(DbContextOptions<Context>opt):base(opt)
        {

        }
        public DbSet<Products> Products{get;set;}
        public DbSet<User> Users{get;set;}
        public DbSet<cart> Carts{get;set;}
        public DbSet<cart_item> Cart_Items{get;set;}
        public DbSet<payment> Payments{get;set;}
      
    }
}