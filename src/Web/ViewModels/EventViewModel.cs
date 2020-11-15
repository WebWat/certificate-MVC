using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class EventViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "EventTitle")]
        [MaxLength(150)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "EventDescription")]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Required")]
        [Url(ErrorMessage = "IncorrectUrl")]
        [Display(Name = "Url")]
        [MaxLength(300)]
        public string Url { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int Page { get; set; }
    }
}
