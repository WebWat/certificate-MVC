﻿using ApplicationCore.Entities;
using System.Collections.Generic;
using Web.ViewModels;

namespace Web.Interfaces
{
    public interface IStageService
    {
        List<StageViewModel> GetStages();
        string GetNameOfStage(Stage stage);
    }
}
