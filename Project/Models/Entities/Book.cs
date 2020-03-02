using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Project.Models.Entities
{
    public class Book
    {
        [HiddenInput(DisplayValue = false)]
        public int BookId { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Пожалуйста, введите название книги")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Недопустимая длина поля")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Пожалуйста, введите описание")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Недопустимая длина поля")]
        public string Description { get; set; }

        [Display(Name = "Категория")]
        [Required(ErrorMessage = "Пожалуйста, введите категирию")]
        [StringLength(32, MinimumLength = 2, ErrorMessage = "Недопустимая длина поля")]
        public string Category { get; set; }

        [Display(Name = "Автор")]
        [Required(ErrorMessage = "Пожалуйста, введите автора книги")]
        [StringLength(32, MinimumLength = 2, ErrorMessage = "Недопустимая длина поля")]
        public string Author { get; set; }

        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
    }
}