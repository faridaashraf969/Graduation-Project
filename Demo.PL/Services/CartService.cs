using Demo.DAL.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

public class CartService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CartService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ISession Session => _httpContextAccessor.HttpContext.Session;

    public void AddToCart(Product product, int quantity)
    {
        var cartItems = Session.GetObjectFromJson<List<CartItem>>("CartItems") ?? new List<CartItem>();

        var cartItem = cartItems.FirstOrDefault(c => c.ProductID == product.Id);
        if (cartItem != null)
        {
            cartItem.Quantity += quantity;
        }
        else
        {
            cartItems.Add(new CartItem
            {
                ProductID = product.Id,
                ProductName = product.Name,
                Quantity = quantity,
                Price = product.Price
            });
        }

        Session.SetObjectAsJson("CartItems", cartItems);
    }

    public List<CartItem> GetCartItems()
    {
        return Session.GetObjectFromJson<List<CartItem>>("CartItems") ?? new List<CartItem>();
    }

    public void ClearCart()
    {
        Session.Remove("CartItems");
    }
}
