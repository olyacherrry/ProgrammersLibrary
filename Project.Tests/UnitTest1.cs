using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Project.Models.Abstract;
using Project.App_Start;
using Project.Controllers;
using Project.Models.Entities;
using Project.HtmlHelpers;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using System.Web.Mvc;

namespace Project.Tests
{
    [TestClass]
    public class BookTest
    {
        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            // Организация - определение вспомогательного метода HTML - это необходимо
            // для применения расширяющего метода
            HtmlHelper myHelper = null;

            // Организация - создание объекта PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Организация - настройка делегата с помощью лямбда-выражения
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Действие
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Утверждение
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary active"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Организация (arrange)
            Mock<IBookOrderRepository> mock = new Mock<IBookOrderRepository>(); 
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { BookId = 1, Name = "Книга1"},
                new Book { BookId = 2, Name = "Книга2"},
                new Book { BookId = 3, Name = "Книга3"},
                new Book { BookId = 4, Name = "Книга4"},
                new Book { BookId = 5, Name = "Книга5"}
            });
            BookController controller = new BookController(mock.Object);
            controller.pageSize = 3;

            // Act
            BookListViewModel result
                = (BookListViewModel)controller.List(null, 2).Model;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Paginate()
        {
            // Организация (arrange)
            Mock<IBookOrderRepository> mock = new Mock<IBookOrderRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
    {
        new Book { BookId = 1, Name = "Книга1"},
                new Book { BookId = 2, Name = "Книга2"},
                new Book { BookId = 3, Name = "Книга3"},
                new Book { BookId = 4, Name = "Книга4"},
                new Book { BookId = 5, Name = "Книга5"}
    });
            BookController controller = new BookController(mock.Object);
            controller.pageSize = 3;

            // Act
            BookListViewModel result
                = (BookListViewModel)controller.List(null, 2).Model;

            // Утверждение
            List<Book> books = result.Books.ToList();
            Assert.IsTrue(books.Count == 2);
            Assert.AreEqual(books[0].Name, "Книга4");
            Assert.AreEqual(books[1].Name, "Книга5");
        }

        [TestMethod]
        public void Can_Filter_Books()
        {
            // Организация (arrange)
            // Организация (arrange)
            Mock<IBookOrderRepository> mock = new Mock<IBookOrderRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
    {
        new Book { BookId = 1, Name = "Книга1", Category="Cat1"},
                new Book { BookId = 2, Name = "Книга2", Category="Cat2"},
                new Book { BookId = 3, Name = "Книга3", Category="Cat3"},
                new Book { BookId = 4, Name = "Книга4", Category="Cat2"},
                new Book { BookId = 5, Name = "Книга5", Category="Cat1"}
    });
            BookController controller = new BookController(mock.Object);
            controller.pageSize = 3;

            // Action
            List<Book> result = ((BookListViewModel)controller.List("Cat2", 1).Model)
                .Books.ToList();

            // Assert
            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Name == "Книга2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "Книга4" && result[1].Category == "Cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            // Организация - создание имитированного хранилища
            Mock<IBookOrderRepository> mock = new Mock<IBookOrderRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book> {
        new Book { BookId = 1, Name = "Книга1", Category="JS"},
        new Book { BookId = 2, Name = "Книга2", Category="JS"},
        new Book { BookId = 3, Name = "Книга3", Category="Java"},
        new Book { BookId = 4, Name = "Книга4", Category="C"},
    });

            // Организация - создание контроллера
            NavigationController target = new NavigationController(mock.Object);

            // Действие - получение набора категорий
            List<string> results = ((IEnumerable<string>)target.Menu().Model).ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 3);
            Assert.AreEqual(results[2], "JS");
            Assert.AreEqual(results[1], "Java");
            Assert.AreEqual(results[0], "C");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            // Организация - создание имитированного хранилища
            Mock<IBookOrderRepository> mock = new Mock<IBookOrderRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book> {
        new Book { BookId = 2, Name = "Игра2", Category="JS"},
        new Book { BookId = 3, Name = "Игра3", Category="Java"}
    });

            // Организация - создание контроллера
            NavigationController target = new NavigationController(mock.Object);

            // Организация - определение выбранной категории
            string categoryToSelect = "JS";

            // Действие
            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            // Утверждение
            Assert.AreEqual(categoryToSelect, result);
        }
        [TestMethod]
        public void Generate_Category_Specific_Game_Count()
        {
            /// Организация (arrange)
            Mock<IBookOrderRepository> mock = new Mock<IBookOrderRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
    {
        new Book { BookId = 1, Name = "Игра1", Category="Cat1"},
        new Book { BookId = 2, Name = "Игра2", Category="Cat2"},
        new Book { BookId = 3, Name = "Игра3", Category="Cat1"},
        new Book { BookId = 4, Name = "Игра4", Category="Cat2"},
        new Book { BookId = 5, Name = "Игра5", Category="Cat3"}
    });
            BookController controller = new BookController(mock.Object);
            controller.pageSize = 3;

            // Действие - тестирование счетчиков товаров для различных категорий
            int res1 = ((BookListViewModel)controller.List("Cat1").Model).PagingInfo.TotalItems;
            int res2 = ((BookListViewModel)controller.List("Cat2").Model).PagingInfo.TotalItems;
            int res3 = ((BookListViewModel)controller.List("Cat3").Model).PagingInfo.TotalItems;
            int resAll = ((BookListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            // Утверждение
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
    }
}
