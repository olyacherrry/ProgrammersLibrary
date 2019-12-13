using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project.Models.Abstract;

namespace Project.Controllers
{
    public class NavigationController : Controller
    {
        private IBookOrderRepository repository;

        public NavigationController(IBookOrderRepository repo)
        {
            repository = repo;
        }

        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;

            IEnumerable<string> categories = repository.Books
                .Select(book => book.Category)
                .Distinct()
                .OrderBy(x => x);
            return PartialView(categories);
        }
    }
}