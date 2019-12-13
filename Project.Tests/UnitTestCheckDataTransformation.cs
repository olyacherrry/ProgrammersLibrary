using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Web.Mvc;
using Moq;
using Project.Controllers;
using Project.Models.Entities;
using Project.Models.Abstract;

namespace Project.Tests
{
    [TestClass]
    public class UnitTestCheckDataTransformation
    {
        [TestMethod]
        public void edit_can_Edit_Exsisted_Book_return_true()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IBookOrderRepository> mock = new Mock<IBookOrderRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
    {
        new Book { BookId = 1, Name = "Книга1"},
        new Book { BookId = 2, Name = "Книга2"},
        new Book { BookId = 3, Name = "Книга3"},
        new Book { BookId = 4, Name = "Книга4"},
        new Book { BookId = 5, Name = "Книга5"}
    });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие
            Book book1 = controller.Edit(1).ViewData.Model as Book;
            Book book2 = controller.Edit(2).ViewData.Model as Book;
            Book book3 = controller.Edit(3).ViewData.Model as Book;
            Book book4 = controller.Edit(4).ViewData.Model as Book;
            Book book5 = controller.Edit(5).ViewData.Model as Book;

            // Assert
            Assert.AreEqual(1, book1.BookId);
            Assert.AreEqual(3, book3.BookId);
            Assert.AreEqual(5, book5.BookId);
        }

        [TestMethod]
        public void edit_cannot_Edit_Nonexsisted_Book()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IBookOrderRepository> mock = new Mock<IBookOrderRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
    {
        new Book { BookId = 1, Name = "Книга1"},
        new Book { BookId = 2, Name = "Книга2"},
        new Book { BookId = 3, Name = "Книга3"},
        new Book { BookId = 4, Name = "Книга4"},
        new Book { BookId = 5, Name = "Книга5"}
    });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие

            Book book6 = controller.Edit(6).ViewData.Model as Book;
            Book book100 = controller.Edit(100).ViewData.Model as Book;

            // Assert
            Assert.IsNull(book6);
            Assert.IsNull(book100);
        }

        [TestMethod]
        public void edit_can_Edit_book_with_image()
        {

            // Организация - создание имитированного хранилища данных
            Mock<IBookOrderRepository> mock = new Mock<IBookOrderRepository>();
            byte[] imageData1 = { 0x36, 0x31 };
            mock.Setup(m => m.Books).Returns(new List<Book>
    {
        new Book { BookId = 1, Name = "Книга1", ImageData =imageData1 }
    });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие

            Book book1 = controller.Edit(1).ViewData.Model as Book;

            // Assert
            Assert.IsNotNull(book1.ImageData);
        }


        [TestMethod]
        public void delete_Can_Delete_Valid_Books_And_Check_Null_Books()
        {
            // Организация - создание объекта book
            Book book1 = new Book { BookId = 1, Name = "книга1" };
            Book book2 = new Book { BookId = 2, Name = "книга2" };
            Book book3 = new Book { BookId = 3, Name = "книга3" };
            Book book4 = null;

            // Организация - создание имитированного хранилища данных
            Mock<IBookOrderRepository> mock = new Mock<IBookOrderRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                book1,
                book2,
                book3
            });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие - удаление book
            controller.Delete(book2.BookId);

            // Утверждение - проверка того, что метод удаления в хранилище
            // вызывается для корректного объекта Book
            mock.Verify(m => m.DeleteBook(book2.BookId));

            // Действие - удаление book
            controller.Delete(book3.BookId);
            mock.Verify(m => m.DeleteBook(book3.BookId));

            // Утверждение - проверка того, что пустой обьект book не содержить bookId,
            // то есть, его невозможно удалить 
            Assert.IsNull(book4);
        }

        [TestMethod]
        public void save_Can_Save_Valid_Changes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IBookOrderRepository> mock = new Mock<IBookOrderRepository>();

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Организация - создание объекта Book
            Book book = new Book { Name = "Test", Description = "Test desccription" };

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(book);

            // Утверждение - проверка того, что к хранилищу производится обращение
            mock.Verify(m => m.SaveBook(book));

            // Утверждение - проверка типа результата метода
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
            Assert.IsNotNull(book.Description);
        }

        [TestMethod]
        public void save_Cannot_Save_Invalid_Changes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IBookOrderRepository> mock = new Mock<IBookOrderRepository>();

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Организация - создание объекта book
            Book book = new Book { Name = "Test" };

            // Организация - добавление ошибки в состояние модели
            controller.ModelState.AddModelError("error", "error");

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(book);


            // Утверждение - проверка типа результата метода
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }


        //[TestMethod]
        //public void Index_Contains_All_Books()
        //{
        //    // Организация - создание имитированного хранилища данных
        //    Mock<IBookOrderRepository> mock = new Mock<IBookOrderRepository>();
        //    mock.Setup(m => m.Books).Returns(new List<Book>
        //    {
        //        new Book { BookId = 1, Name = "Книга1"},
        //        new Book { BookId = 2, Name = "Книга2"},
        //        new Book { BookId = 3, Name = "Книга3"},
        //        new Book { BookId = 4, Name = "Книга4"},
        //        new Book { BookId = 5, Name = "Книга5"}
        //    });

        //    // Организация - создание контроллера
        //    AdminController controller = new AdminController(mock.Object);

        //    // Действие
        //    List<Book> result = ((IEnumerable<Book>)controller.Index().
        //        ViewData.Model).ToList();

        //    // Утверждение
        //    Assert.AreEqual(result.Count(), 5);
        //}

    }
}
