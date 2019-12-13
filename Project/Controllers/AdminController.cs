using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Project.Models;
using Project.Models.Abstract;
using Project.Models.Entities;

namespace Project.Controllers
{
    public class AdminController : Controller
    {
        IBookOrderRepository repository;

        public AdminController(IBookOrderRepository repo)
        {

            repository = repo;
        }

        public ViewResult Index()
        {
            TempsData();
            return View(repository.Books);
        }
        public ViewResult Edit(int bookId)
        {
            Book book = repository.Books
                .FirstOrDefault(g => g.BookId == bookId);
            return View(book);
        }
        // Перегруженная версия Edit() для сохранения изменений
        [HttpPost]
        public ActionResult Edit(Book book, HttpPostedFileBase image = null)
        {
                if (ModelState.IsValid)
            {
                if (image != null)
                {
                    book.ImageMimeType = image.ContentType;
                    book.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(book.ImageData, 0, image.ContentLength);
                }
                repository.SaveBook(book);
                TempData["message"] = string.Format("Изменения в книге \"{0}\" были сохранены", book.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(book);
            }
        }
        public ViewResult Create()
        {
            return View("Edit", new Book());
        }

        [HttpPost]
        public ActionResult Delete(int bookId)
        {
            Book deletedBook = repository.DeleteBook(bookId);
            if (deletedBook != null)
            {
                TempData["messageDelete"] = string.Format("Книга \"{0}\" была удалена",
                    deletedBook.Name);
            }
            return RedirectToAction("Index");
        }

        public ViewResult Users()
        {
            TempsData();
            return View(UserManager);
        }

        public IQueryable<ApplicationUser> UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().Users;
            }
        }

        public ViewResult OrderList()
        {
            TempsData();
            return View(repository.Orders);
        }
        
        private void TempsData()
        {
            TempData["Book"] = repository.Books.Count();
            TempData["User"] = UserManager.Count();
            TempData["Order"] = repository.Orders.Count();
        }
        [HttpPost]
        public ActionResult DeleteOrder(int orderId)
        {
            Order deletedBook = repository.DeleteOrder(orderId);
            if (deletedBook != null)
            {

            }
            return RedirectToAction("OrderList");
        }
    }
}