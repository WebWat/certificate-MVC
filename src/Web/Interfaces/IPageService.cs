using ApplicationCore.Entities;
using System.Collections.Generic;

namespace Web.Interfaces;

public interface IPageService
{
    IEnumerable<Certificate> GetDataToPage(IEnumerable<Certificate> list,
                                           int numberOfItems,
                                           ref int page,
                                           bool includeCheck = false);
}
