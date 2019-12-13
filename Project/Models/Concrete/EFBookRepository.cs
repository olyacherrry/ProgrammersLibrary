using System.Collections.Generic;
using Project.Models.Abstract;
using Project.Models.Entities;

namespace Project.Models.Concrete
{
    public class EFBookRepository : IBookOrderRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Book> Books
        {
            get { return context.Books; }
        }

        public void SaveBook(Book book)
        {
            if (book.BookId == 0)
                context.Books.Add(book);
            else
            {
                Book dbEntry = context.Books.Find(book.BookId);
                if (dbEntry != null)
                {
                    dbEntry.Name = book.Name;
                    dbEntry.Description = book.Description;
                    dbEntry.Author = book.Author;
                    dbEntry.Category = book.Category;
                    dbEntry.ImageData = book.ImageData;
                    dbEntry.ImageMimeType = book.ImageMimeType;
                }
            }
            context.SaveChanges();
        }

        public Book DeleteBook(int bookId)
        {
            Book dbEntry = context.Books.Find(bookId);
            if (dbEntry != null)
            {
                context.Books.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

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

/*
 CREATE TABLE [dbo].[Book]
(
	[BookId] INT NOT NULL PRIMARY KEY IDENTITY,
	[Name]  NVARCHAR(MAX) NOT NULL, 
    [Author] NVARCHAR(MAX) NULL, 
    [Description] NVARCHAR(MAX) NULL, 
    [Category] NVARCHAR(MAX) NULL
)
 */
