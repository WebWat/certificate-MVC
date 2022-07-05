using ApplicationCore.Entities;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using Web.Interfaces;

namespace Web.Services;

public class FilterService : IFilterService
{
    private readonly IStringLocalizer<SharedResource> _localizer;

    public FilterService(IStringLocalizer<SharedResource> localizer)
    {
        _localizer = localizer;
    }


    public IEnumerable<Certificate> FilterOut(IEnumerable<Certificate> list, string? year, string? find, Stage? stage)
    {
        // Sort by date.
        if (year != null && year != _localizer["All"] && int.TryParse(year, out int result))
        {
            list = list.Where(i => i.Date.Year == result);
        }

        // Sort by title.
        if (!string.IsNullOrEmpty(find))
        {
            list = list.Where(p => p.Title.ToLower().Contains(find.Trim().ToLower()));
        }

        // Sort by stage.
        if (stage != null)
        {
            list = list.Where(p => p.Stage == stage);
        }

        return list;
    }
}