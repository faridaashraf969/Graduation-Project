﻿using Demo.DAL.Entities;
using System.Collections.Generic;
using System;
using System.Linq;
using Demo.DAL.Contexts;

public class OrderService
{
    private readonly MvcProjectDbContext _context;

    public OrderService(MvcProjectDbContext context)
    {
        _context = context;
    }

    public Order CreateOrder(string applicationUserId, List<CartItem> cartItems, string shippingAddress, string billingAddress)
    {
        var order = new Order
        {
            UserId = applicationUserId,
            OrderDate = DateTime.UtcNow,
            TotalAmount = cartItems.Sum(item => item.Quantity * item.Price),
            ShippingAddress = shippingAddress,
            
            OrderItems = cartItems.Select(item => new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.Price
            }).ToList()
        };

        _context.Orders.Add(order);
        _context.SaveChanges();

        return order;
    }
}
