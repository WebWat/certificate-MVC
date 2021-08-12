using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Entities
{
    public class Certificate : BaseEntity
    {
        public string Title { get; private set; }
        public byte[] File { get; private set; }
        public string Description { get; private set; }
        public Stage Stage { get; private set; }
        public List<Link> Links { get; private set; }
        public string UserId { get; private set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; private set; }

        public Certificate(string userId, 
                           string title, 
                           byte[] file, 
                           string description, 
                           Stage stage, 
                           DateTime date)
        {
            UserId = userId;
            Title = title;
            File = file;
            Description = description;
            Stage = stage;
            Date = date;
        }


        public Certificate SetId(int id)
        {
            Id = id;
            return this;
        }
    }
}
