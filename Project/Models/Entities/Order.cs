using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Models.Entities
{
    [NotAllowedAttribute(ErrorMessage = "Даты начало не должна быть больше, чем дата окончания" )]
    public class Order : IValidatableObject
    {
        [HiddenInput(DisplayValue = false)]
        public int OrderId { get; set; }

        //[HiddenInput(DisplayValue = false)]
        public string UserId { get; set; }

        //[HiddenInput(DisplayValue = false)]
        public int BookId { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Дата начала")]
        [Required(ErrorMessage = "Пожалуйста, введите дату взятия книги")]
        public DateTime DateStarting { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Дата окончания")]
        [Required(ErrorMessage = "Пожалуйста, введите дату возврта книги")]
        public DateTime DateEnding { get; set; }

        [Display(Name = "Телефон")]
        [StringLength(13, ErrorMessage = "Значение должно содержать {2} символов.", MinimumLength = 13)]
        public string Phone { get; set; }

        [Display(Name = "Адрес")]
        [Required(ErrorMessage = "Пожалуйста, введите адрес проживания")]
        public string Adress { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (this.DateStarting == null || this.DateStarting.Year < 1990 || DateTime.Today.Year.Equals(this.DateStarting))
            {
                errors.Add(new ValidationResult("Недопустимый год"));
            }
            if (this.DateEnding == null || this.DateEnding.Year < 1990 || DateTime.Today.Year.Equals(this.DateEnding))
            {
                errors.Add(new ValidationResult("Недопустимый год"));
            }
            if(this.Phone.Length != 13)
            {
                errors.Add(new ValidationResult("Введите правильно номер телефона"));
            }

            return errors;
        }
    }
}
/*
 CREATE TABLE [dbo].[Orders]
(
	[OrderId] INT NOT NULL PRIMARY KEY IDENTITY,
    [UserId] INT NOT NULL,
	[DateStarting]  NVARCHAR(MAX) NOT NULL, 
    [DateEnding] NVARCHAR(MAX) NULL, 
    [Phone] NVARCHAR(MAX) NULL, 
    [Adress] NVARCHAR(MAX) NULL
)
 */
