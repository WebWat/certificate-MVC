using System.Collections.Generic;

namespace Web.ViewModels
{
    public class EventListViewModel
    {
        public IEnumerable<EventViewModel> Events { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
