using Project.Models.Entities;
using System.Collections.Generic;

namespace Project.Models.Abstract
{
    public interface IBookOrderRepository
    {
        IEnumerable<Book> Books { get; }
        void SaveBook(Book book);
        Book DeleteBook(int bookId);

        IEnumerable<Order> Orders { get; }
        void SaveOrder(Order order);
        Order DeleteOrder(int orderId);
    }
}
