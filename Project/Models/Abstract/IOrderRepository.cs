using Project.Models.Entities;
using System.Collections.Generic;

namespace Project.Models.Abstract
{
    public interface IOrderRepository
    {
        IEnumerable<Order> Orders { get; }
        void SaveOrder(Order order);
        Order DeleteOrder(int orderId);
    }
}
