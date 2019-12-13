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
using Project.Models.Concrete;

namespace Project.Tests
{
    public class IntegratedTests
    {

        EFBookRepository bookDataBase;
        Book testBook1;
        Book testBook2;
        Book testBook3;
        byte[] imageData = { 0x36, 0x31 };

        [TestInitialize()]
        public void Startup()
        {
            // arrange - database
            EFBookRepository bookDataBase = new EFBookRepository();

            Book testBook1 = new Book
            {
                BookId = 1,
                Author = "Test Author",
                Name = "Book1"
            };
            Book testBook2 = new Book
            {
                BookId = 2,
                Author = "Test Author",
                Name = "Book2"
            };
            Book testBook3 = new Book
            {
                BookId = 3,
                Author = "Test Author",
                Name = "Book3",
                ImageData = imageData
            };
        }


        [TestMethod]
        public void edit_can_Edit_Exsisted_Book_return_true()
        {
            //arrange - сохраниение книги1
            bookDataBase.SaveBook(testBook1);

            // Организация - создание контроллера
            AdminController controller = new AdminController(bookDataBase);

            // Действие
            Book book1 = controller.Edit(1).ViewData.Model as Book;

            // Assert
            Assert.AreEqual(1, book1.BookId);
        }

        [TestMethod]
        public void edit_cannot_Edit_Nonexsisted_Book()
        {
            //организация - сохрание в бд 3 книг
            bookDataBase.SaveBook(testBook1);
            bookDataBase.SaveBook(testBook2);
            bookDataBase.SaveBook(testBook3);


            // Организация - создание контроллера
            AdminController controller = new AdminController(bookDataBase);

            // Действие
            Book book6 = controller.Edit(6).ViewData.Model as Book;

            // Assert
            Assert.IsNull(book6);
        }

        [TestMethod]
        public void edit_can_add_image()
        {
            
            // Организация - создание контроллера
            AdminController controller = new AdminController(bookDataBase);
            bookDataBase.SaveBook(testBook1);

            // Действие
            Book result = controller.Edit(1).ViewData.Model as Book;

            // Assert
            Assert.IsNotNull(result.ImageData);
        }

        [TestMethod]
        public void save_Can_Save_Valid_Changes()
        {
            // Организация - создание контроллера
            AdminController controller = new AdminController(bookDataBase);

            // Организация - создание объекта Book
            Book book = new Book { Name = "Test", Description = "Test description" };

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(book);

            // Утверждение - проверка типа результата метода
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void save_Cannot_Save_Invalid_Changes()
        {
            // Организация - создание контроллера
            AdminController controller = new AdminController(bookDataBase);

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
            // Организация - создание контроллера
            AdminController controller = new AdminController(bookDataBase);

            // Действие - удаление book
            ActionResult ar = controller.Delete(testBook1.BookId);

            // Утверждение - проверка того, что метод удаления в хранилище
            // вызывается для объекта Book
            Assert.IsNotNull(ar);
        }
    }
}
