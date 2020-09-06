using ApplicationCore.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Entities
{
    public class Certificate : BaseEntity
    {
        public string Title { get; set; }

        public byte[] File { get; set; }

        public string Description { get; set; }

        public int Rating { get; set; }

        public List<Link> Links { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
