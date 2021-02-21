using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Services
{
    public class StageService : IStageService
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        public StageService(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        public List<StageViewModel> GetStages()
        {
            return new List<StageViewModel>
            {
                new StageViewModel { Id = 1, Name = _localizer["School"].Value },
                new StageViewModel { Id = 2, Name = _localizer["District"].Value },
                new StageViewModel { Id = 3, Name = _localizer["Republican"].Value },
                new StageViewModel { Id = 4, Name = _localizer["All-Russian"].Value },
                new StageViewModel { Id = 5, Name = _localizer["International"].Value }
            };
        }

        public string GetNameOfStage(int stage) => stage switch
        {
            1 => _localizer["School"],
            2 => _localizer["District"],
            3 => _localizer["Republican"],
            4 => _localizer["All-Russian"],
            5 => _localizer["International"],
            _ => string.Empty
        };
    }
}
