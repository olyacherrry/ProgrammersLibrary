using Project.Models.Abstract;
using Project.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models.Concrete
{
    public class EFOrderRepository : IOrderRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Order> Orders
        {
            get { return context.Orders; }
        }

        public void SaveOrder(Order order)
        {
            if (order.OrderId == 0)
                context.Orders.Add(order);
            else
            {
                Order dbEntry = context.Orders.Find(order.OrderId);
                if (dbEntry != null)
                {
                    dbEntry.BookId = order.BookId;
                    dbEntry.UserId = order.UserId;
                    dbEntry.DateStarting = order.DateStarting;
                    dbEntry.DateEnding = order.DateEnding;
                    dbEntry.Phone = order.Phone;
                    dbEntry.Adress = order.Adress;
                }
            }
            context.SaveChanges();
        }

        public Order DeleteOrder(int orderId)
        {
            Order dbEntry = context.Orders.Find(orderId);
            if (dbEntry != null)
            {
                context.Orders.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}