//using Demo.DAL.Contexts;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Demo.DAL.Entities
//{
    
//        public class ShoppingCart
//        {
//            private readonly MvcProjectDbContext _projectDbContext; // context name
//            private ShoppingCart(MvcProjectDbContext projectDbContext)
//            {
//                _projectDbContext = projectDbContext;
//            }

//            public string ShoppingCartId { get; set; }
//            public List<ShoppingCartItem> ShoppingCartItems { get; set; }

//            public static ShoppingCart GetShoppingCart(IServiceProvider services)
//            {
//                ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
//                var context = services.GetService<MvcProjectDbContext>();

//                string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
//                session.SetString("CartId", cartId);

//                return new ShoppingCart(context) { ShoppingCartId = cartId };
//            }

//            public void AddItemToCart(Product product, int amount)
//            {
//                var shoppingCartItem = _projectDbContext.ShoppingCartItems.SingleOrDefault(n => n.Product.ProductId == product.ProductId && n.ShoppingCartId == ShoppingCartId);

//                if (shoppingCartItem == null)
//                {
//                    shoppingCartItem = new ShoppingCartItem()
//                    {
//                        ShoppingCartId = ShoppingCartId,
//                        Product = product,
//                        Amount = 1
//                    };

//                    _projectDbContext.ShoppingCartItems.Add(shoppingCartItem);
//                }
//                else
//                {
//                    shoppingCartItem.Amount++;
//                }
//                _projectDbContext.SaveChanges();
//            }

//            public void RemoveFromCart(Product product)
//            {
//                var shoppingCartItem = _projectDbContext.ShoppingCartItems.SingleOrDefault(n => n.Product.ProductId == product.ProductId && n.ShoppingCartId == ShoppingCartId);

//                var localAmount = 0;
//                if (shoppingCartItem != null)
//                {
//                    if (shoppingCartItem.Amount > 1)
//                    {
//                        shoppingCartItem.Amount--;
//                        localAmount = ShoppingCartItem.Amount;
//                    }
//                    else
//                    {
//                        _projectDbContext.ShoppingCartItems.Remove(shoppingCartItem);
//                    }
//                }
//                _projectDbContext.SaveChanges();
//                return localAmount;
//            }

//            public List<ShoppingCartItem> GetShoppingCartItems()
//            {
//                return ShoppingCartItems ?? (ShoppingCartItems = _projectDbContext.ShoppingCartItems.Where(n => n.ShoppingCartId == ShoppingCartId).Include(n => n.Product).ToList());
//            }

//            public double GetShoppingCartTotal() => _projectDbContext.ShoppingCartItems.Where(n => n.ShoppingCartId == ShoppingCartId).Select(n => n.Product.Price * n.Amount).Sum();

//            public async Task ClearShoppingCartAsync()
//            {
//                var items = await _projectDbContext.ShoppingCartItems.Where(n => n.ShoppingCartId == ShoppingCartId).ToListAsync();
//                _projectDbContext.ShoppingCartItems.RemoveRange(items);
//                await _projectDbContext.SaveChangesAsync();
//            }
//        }
//    }

