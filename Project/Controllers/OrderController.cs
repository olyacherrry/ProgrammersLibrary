using Project.Models.Abstract;
using Project.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Project.Controllers
{
    public class OrderController : Controller
    {
        private IBookOrderRepository repository;

        public OrderController(IBookOrderRepository repo)
        {
            repository = repo;
        }

        public int BookId;
        public string UserId;

        public ViewResult Edit(int bookId)
        {
            Book book = repository.Books.FirstOrDefault(g => g.BookId == bookId);
            TempData["BookName"] = string.Format("Заказ книги \"{0}\"", book.Name);
            Order order = new Order();
            BookId = bookId;
            UserId = User.Identity.GetUserId();
            return View(order);
        }

        // Перегруженная версия Edit() для сохранения изменений
        [HttpPost]
        public ActionResult Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                if (order.BookId == 0)
                    order.BookId = BookId;
                if (order.UserId == null)
                    order.UserId = User.Identity.GetUserId();
                repository.SaveOrder(order);
                return RedirectToAction("List", "Book");
            }
            else
            {
                BookId = BookId;
                UserId = UserId;
                Book book = repository.Books.FirstOrDefault(g => g.BookId == order.BookId);
                TempData["BookName"] = string.Format("Заказ книги \"{0}\"", book.Name);
                return View(order);
            }
        }

    }
}