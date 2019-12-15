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
using Project.Models;
using Project.App_Start;
using System.ComponentModel.DataAnnotations;

namespace Project.Tests
{
    [TestClass]
    public class AdminTest
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
            byte[] imageData = { 0x36, 0x31};
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
        public void save_canSaveWithZeroBookId()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IBookOrderRepository> mock = new Mock<IBookOrderRepository>();

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Организация - создание объекта book
            Book book = new Book { Name = "Test", BookId = 0 };

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(book);

            // Утверждение - проверка типа результата метода
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
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

        [TestMethod]
        public void delete_Can_Delete_Null_Books()
        {
            // Организация - создание объекта book
            Book book = new Book ();

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

        [TestMethod]
        public void saveOrder_canSaveOrderOrderIsBiggerZero()
        {
            Order order1 = new Order { UserId = "1", Adress = "ddcd", BookId = 1, OrderId = 0, Phone = "375336349609", DateStarting = DateTime.Parse("01.01.2019"), DateEnding = DateTime.Parse("02.02.2019")  };
            Mock<IBookOrderRepository> mock = new Mock<IBookOrderRepository>();
            OrderController controller = new OrderController(mock.Object);
            
            ActionResult result = controller.Edit(order1);
            mock.Verify(m => m.SaveOrder(order1));
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void saveOrder_canSaveOrderOrderIsBiggerZerodbEntryIsNull()
        {
            Order order1 = new Order { UserId = "1", Adress = "ddcd", BookId = 1, OrderId = 0, Phone = "375336349609", DateStarting = DateTime.Parse("01.01.2019"), DateEnding = DateTime.Parse("02.02.2019") };
            Mock<IBookOrderRepository> mock = new Mock<IBookOrderRepository>();
            OrderController controller = new OrderController(mock.Object);

            ActionResult result = controller.Edit(order1);
            mock.Verify(m => m.SaveOrder(order1));
            result = controller.Edit(order1);
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void saveOrder_canSaveOrderOrderIsZero()
        {
            Order order1 = new Order { UserId = "1", Adress = "ddcd", BookId = 1, OrderId = 2, Phone = "375336349609", DateStarting = DateTime.Parse("01.01.2019"), DateEnding = DateTime.Parse("02.02.2019") };
            Mock<IBookOrderRepository> mock = new Mock<IBookOrderRepository>();
            OrderController controller = new OrderController(mock.Object);

            ActionResult result = controller.Edit(order1);
            mock.Verify(m => m.SaveOrder(order1));
            Assert.IsNotNull(result);
        }

        [TestMethod] 
        public void deleteOrder_CanDeleteOrder()
        {
            Order order1 = new Order { UserId = "1", Adress = "ddcd", BookId = 1, OrderId = 2, Phone = "375336349609", DateStarting = DateTime.Parse("01.01.2019"), DateEnding = DateTime.Parse("02.02.2019") };
            Mock<IBookOrderRepository> mock = new Mock<IBookOrderRepository>();
            OrderController controller = new OrderController(mock.Object);
            AdminController controller1 = new AdminController(mock.Object);

            ActionResult result = controller.Edit(order1);
            controller1.DeleteOrder(order1.OrderId);
            mock.Verify(m => m.DeleteOrder(order1.OrderId));
        }
        [TestMethod]
        public void deleteOrder_CanDeleteUnexcitingOrder()
        {
            Order order1 = new Order { UserId = "1", Adress = "ddcd", BookId = 1, OrderId = 2, Phone = "375336349609", DateStarting = DateTime.Parse("01.01.2019"), DateEnding = DateTime.Parse("02.02.2019") };
            Mock<IBookOrderRepository> mock = new Mock<IBookOrderRepository>();
            OrderController controller = new OrderController(mock.Object);
            AdminController controller1 = new AdminController(mock.Object);

            ActionResult result = controller.Edit(order1);
            controller1.DeleteOrder(order1.OrderId);
            mock.Verify(m => m.DeleteOrder(order1.OrderId));
            controller1.DeleteOrder(order1.OrderId);
            mock.Verify(m => m.DeleteOrder(order1.OrderId));
        }


        //[TestMethod] 
        public void register_canRegister()
        {
            var accountController = new AccountController();
            var registerViewModel = new RegisterViewModel
            {
                Login = "olya",
                FirsName =  "olya",
                LastName = "berestneva",
                Group ="10701117",
                Email = "test@gmail.com",
                Password = "123456",
                ConfirmPassword = "123456"
            };
           
            var result = accountController.Register(registerViewModel).Result;
            Assert.IsNotNull(result);
        }

        //[TestMethod]
        public void register_cantRegister_invalidModelState()
        {
            var accountController = new AccountController();
            var registerViewModel = new RegisterViewModel
            {
                Login = "olya",
                FirsName = "olya",
                LastName = "berestneva",
                Group = "10701117",
                Email = "test@spam.com",
                Password = "123456",
                ConfirmPassword = "123456"
            };

            var result = accountController.Register(registerViewModel).Result;
            Assert.IsNotNull(result);
        }

        public void register_cantRegister_invalidModelStateresultIsntSucced()
        {
            var accountController = new AccountController();
            var registerViewModel = new RegisterViewModel
            {
                Login = "olya",
                FirsName = "olya",
                LastName = "berestneva",
                Group = "10701117",
                Email = "test@spam.com",
                Password = "1234561",
                ConfirmPassword = "123456"
            };

            var result = accountController.Register(registerViewModel).Result;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void isValid_returnTrue()
        {
            Order order1 = new Order { UserId = "1", Adress = "ddcd", BookId = 1, OrderId = 2, Phone = "375336349609", DateStarting = DateTime.Parse("01.01.2019"), DateEnding = DateTime.Parse("02.02.2019") };
            Mock<IBookOrderRepository> mock = new Mock<IBookOrderRepository>();
            OrderController controller = new OrderController(mock.Object);
            AdminController controller1 = new AdminController(mock.Object);
            NotAllowedAttribute notAllowedAttribute = new NotAllowedAttribute();

            ActionResult result = controller.Edit(order1);
            bool boolRes = notAllowedAttribute.IsValid(order1);
            Assert.IsTrue(boolRes);
        }
        [TestMethod]
        public void isValid_returnFalse()
        {
            Order order1 = new Order { UserId = "1", Adress = "ddcd", BookId = 1, OrderId = 2, Phone = "375336349609", DateStarting = DateTime.Parse("01.01.2029"), DateEnding = DateTime.Parse("02.02.2019") };
            Mock<IBookOrderRepository> mock = new Mock<IBookOrderRepository>();
            OrderController controller = new OrderController(mock.Object);
            AdminController controller1 = new AdminController(mock.Object);
            NotAllowedAttribute notAllowedAttribute = new NotAllowedAttribute();

            bool boolRes = notAllowedAttribute.IsValid(order1);
            Assert.IsFalse(boolRes);
        }

        [TestMethod]
        public void validate_validateOrderDateLess1990()
        {
            Order order1 = new Order { UserId = "1", Adress = "ddcd", BookId = 1, OrderId = 2, Phone = "375336349609", DateStarting = DateTime.Parse("01.01.2019"), DateEnding = DateTime.Parse("02.02.2019") };
            Mock<IBookOrderRepository> mock = new Mock<IBookOrderRepository>();
            OrderController controller = new OrderController(mock.Object);
            AdminController controller1 = new AdminController(mock.Object);
            List<ValidationResult> errors = new List<ValidationResult>();
            var valid = new ValidationContext(order1);
            order1.Validate(valid);
            Assert.IsNotNull(errors);
        }
        [TestMethod]
        public void validate_validateDateEndingLess1990()
        {
            Order order1 = new Order { UserId = "1", Adress = "ddcd", BookId = 1, OrderId = 2, Phone = "375336349609", DateStarting = DateTime.Parse("01.01.1800"), DateEnding = DateTime.Parse("02.02.2019") };
            Mock<IBookOrderRepository> mock = new Mock<IBookOrderRepository>();
            OrderController controller = new OrderController(mock.Object);
            AdminController controller1 = new AdminController(mock.Object);
            List<ValidationResult> errors = new List<ValidationResult>();
            var valid = new ValidationContext(order1);
            order1.Validate(valid);
            Assert.IsNotNull(errors);
        }
        [TestMethod]
        public void validate_validatePhoneNumberIsNot13Numbers()
        {
            Order order1 = new Order { UserId = "1", Adress = "ddcd", BookId = 1, OrderId = 2, Phone = "37533634960911", DateStarting = DateTime.Parse("01.01.2019"), DateEnding = DateTime.Parse("02.02.2019") };
            Mock<IBookOrderRepository> mock = new Mock<IBookOrderRepository>();
            OrderController controller = new OrderController(mock.Object);
            AdminController controller1 = new AdminController(mock.Object);
            List<ValidationResult> errors = new List<ValidationResult>();
            var valid = new ValidationContext(order1);
            order1.Validate(valid);
            Assert.IsNotNull(errors);
        }
        [TestMethod]
        public void validate_validateAllisGood()
        {
            Order order1 = new Order { UserId = "1", Adress = "ddcd", BookId = 1, OrderId = 2, Phone = "375336349609", DateStarting = DateTime.Parse("01.01.2019"), DateEnding = DateTime.Parse("02.02.2019") };
            Mock<IBookOrderRepository> mock = new Mock<IBookOrderRepository>();
            OrderController controller = new OrderController(mock.Object);
            AdminController controller1 = new AdminController(mock.Object);
            List<ValidationResult> errors = new List<ValidationResult>();
            var valid = new ValidationContext(order1);
            order1.Validate(valid);
            Assert.IsNotNull(errors);
        }


        //public void validateAsunc_EmailContainsSpam()
        //{
        //    //Arrange
        //    var userManager = new Mock<ApplicationUserManager>();
        //    Mock<CustomUserValidator> customVal = new Mock<CustomUserValidator>();
        //    var registerViewModel = new RegisterViewModel
        //    {
        //        Login = "olya",
        //        FirsName = "olya",
        //        LastName = "berestneva",
        //        Group = "10701117",
        //        Email = "test@spam.com",
        //        Password = "1234561",
        //        ConfirmPassword = "123456"
        //    };


        //    //Act
        //    var result = await customVal.ValidateAsync(userManager);

        //    //Assert
        //    List<ErrorEventArgs> errors = result.Succeeded ? new List<ErrorEventArgs>() : result.Errors.ToList();
        //    Assert.AreEqual(errors.Count, 2);
        //}

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

        //[TestMethod]
        //public void Index_Contains_Books_In_Right_Order()
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
        //    Assert.AreEqual("Книга1", result[0].Name);
        //}
    }

   

