using ApplicationCore.Entities;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Services;

public class StageService : IStageService
{
    private readonly IStringLocalizer<SharedResource> _localizer;

    public StageService(IStringLocalizer<SharedResource> localizer)
    {
        _localizer = localizer;
    }


    public List<StageViewModel> GetStages(bool includeAllValue = false)
    {
        var list = new List<StageViewModel>
        {
            new StageViewModel { EnumName = nameof(Stage.School), Name = _localizer["School"].Value },
            new StageViewModel { EnumName = nameof(Stage.District), Name = _localizer["District"].Value },
            new StageViewModel { EnumName = nameof(Stage.Republican), Name = _localizer["Republican"].Value },
            new StageViewModel { EnumName = nameof(Stage.AllRussian), Name = _localizer["All-Russian"].Value },
            new StageViewModel { EnumName = nameof(Stage.International), Name = _localizer["International"].Value }
        };

        if (includeAllValue)
        {
            list.Insert(0, new() { EnumName = string.Empty, Name = _localizer["All"].Value });
        }

        return list;
    }


    public string GetNameOfStage(Stage stage) => stage switch
    {
        Stage.School => _localizer["School"],
        Stage.District => _localizer["District"],
        Stage.Republican => _localizer["Republican"],
        Stage.AllRussian => _localizer["All-Russian"],
        Stage.International => _localizer["International"],
        _ => string.Empty
    };
}
