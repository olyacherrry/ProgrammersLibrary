using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Project.Controllers;
using Project.Models.Entities;
using Project.Models.Abstract;
using System.IO;

namespace Project.Tests
{
    [TestClass]
    public class IntegratedTest
    {
        [TestMethod]
        public void edit_can_Edit_Exsisted_Book_return_true()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IBookOrderRepository> mock = new Mock<IBookOrderRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
    {
        new Book { BookId = 1, Name = "Книга1"}
    });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие
            Book book1 = controller.Edit(1).ViewData.Model as Book;

            // Assert
            Assert.AreEqual(1, book1.BookId);
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

            // Assert
            Assert.IsNull(book6);
        }

        [TestMethod]
        public void edit_can_add_image()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IBookOrderRepository> mock = new Mock<IBookOrderRepository>();
            byte[] imageData = { 0x36, 0x31 };
            mock.Setup(m => m.Books).Returns(new List<Book>
    {
        new Book { BookId = 1, Name = "Книга1",  ImageData = imageData}
    });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие
            Book result = controller.Edit(1).ViewData.Model as Book;

            // Assert
            Assert.IsNotNull(result.ImageData);
        }

        [TestMethod]
        public void save_Can_Save_Valid_Changes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IBookOrderRepository> mock = new Mock<IBookOrderRepository>();

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Организация - создание объекта Book
            Book book = new Book { Name = "Test", Description = "Test description" };

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(book);

            // Утверждение - проверка того, что к хранилищу производится обращение
            mock.Verify(m => m.SaveBook(book));

            // Утверждение - проверка типа результата метода
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
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


        [TestMethod]
        public void delete_Can_Delete_Valid_Books()
        {
            // Организация - создание объекта book
            Book book = new Book { BookId = 2, Name = "книга2" };

            // Организация - создание имитированного хранилища данных
            Mock<IBookOrderRepository> mock = new Mock<IBookOrderRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
    {
        new Book { BookId = 1, Name = "книга1"},
        new Book { BookId = 2, Name = "книга2"},
        new Book { BookId = 3, Name = "книга3"},
        new Book { BookId = 4, Name = "книга4"},
        new Book { BookId = 5, Name = "книга5"}
    });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие - удаление book
            controller.Delete(book.BookId);

            // Утверждение - проверка того, что метод удаления в хранилище
            // вызывается для корректного объекта Book
            mock.Verify(m => m.DeleteBook(book.BookId));
        }
    }
}
