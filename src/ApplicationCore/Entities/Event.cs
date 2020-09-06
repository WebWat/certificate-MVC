using System;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Entities
{
    public class Event : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
