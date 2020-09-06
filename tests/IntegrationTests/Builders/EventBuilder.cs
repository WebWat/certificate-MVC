using ApplicationCore.Entities;
using System;
using System.Collections.Generic;

namespace IntegrationTests.Builders
{
    public class EventBuilder
    {
        public static List<Event> GetDefaultValues()
        {
            return new List<Event>
            {
                new Event { Title = "Test Event", Description = "Test Event Description", Date = DateTime.Now, Url = "Test Event Link" },
                new Event { Title = "Test Event 2", Description = "Test Event Description 2", Date = DateTime.Now, Url = "Test Event Link 2" },
            };
        }
    }
}
