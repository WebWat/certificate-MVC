using ApplicationCore.Entities;
using System.Collections.Generic;

namespace IntegrationTests.Builders
{
    public class EventBuilder
    {
        public static List<Event> GetDefaultValues()
        {
            return new List<Event>
            {
                new Event
                {
                    Title = "Selection for November's chemistry programme has started"
                },
                new Event
                {
                    Title = "MIPT invites teachers for courses"
                }
            };
        }
    }
}
