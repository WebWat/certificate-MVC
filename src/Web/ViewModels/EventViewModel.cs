using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class EventViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Это обязательное поле")]
        [Display(Name = "Название")]
        [MaxLength(150)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Это обязательное поле")]
        [Display(Name = "Описание")]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Это обязательное поле")]
        [Url(ErrorMessage = "Некорректный url")]
        [Display(Name = "Ссылка")]
        [MaxLength(300)]
        public string Url { get; set; }

        [Required(ErrorMessage = "Это обязательное поле")]
        [Display(Name = "Дата")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } 

        public int Page { get; set; }
    }
}
