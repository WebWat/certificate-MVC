using ApplicationCore.Constants;
using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Linq;
using Web.Interfaces;

namespace Web.Services;

public class PageService : IPageService
{
    public IEnumerable<Certificate> GetDataToPage(IEnumerable<Certificate> list,
                                                  int numberOfItems,
                                                  ref int page,
                                                  bool includeCheck = false)
    {
        page = page <= 0 ? 1 : page;

        var skipped = (page - 1) * Common.PageSize;

        // Сheck if we deleted the last element on
        // the page to return to the previous one.
        if (includeCheck)
        {
            // If the page is blank, return to the previous page.
            if (skipped >= numberOfItems && numberOfItems != 0)
            {
                page -= 1;
                skipped -= Common.PageSize;
            }
        }

        return list.Skip(skipped).Take(Common.PageSize);
    }
}
