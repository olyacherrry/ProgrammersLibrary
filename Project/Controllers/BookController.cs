using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project.Models.Abstract;
using Project.Models.Entities;
using Project.Models;

namespace Project.Controllers
{
    public class BookController : Controller
    {
        private IBookOrderRepository repository;
        public int pageSize = 3;

        public BookController(IBookOrderRepository repo)
        {
            repository = repo;
        }
        public ViewResult List(string category, int page = 1)
        {
            BookListViewModel model = new BookListViewModel
            {
                Books = repository.Books
                    .Where(p => category == null || p.Category == category)
                    .OrderBy(book => book.BookId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ?
                repository.Books.Count() :
                repository.Books.Where(game => game.Category == category).Count()
                },
                CurrentCategory = category
            };
            return View(model);
        }
        public FileContentResult GetImage(int bookId)
        {
            Book book = repository.Books
                .FirstOrDefault(g => g.BookId == bookId);

            if (book != null)
            {
                return File(book.ImageData, book.ImageMimeType);
            }
            else
            {
                return null;
            }
        }

        public ActionResult Details(int bookId)
        {
            Book book = repository.Books
                .FirstOrDefault(g => g.BookId == bookId);
            if (book != null)
                return PartialView(book);
            return HttpNotFound();
        }

    }
}
