using ApplicationCore.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels;

public class SearchViewModel
{
    public Dictionary<string, string> Parameters { get; set; } = new();

    public string Controller { get; set; }

    public SelectList Stages { get; set; }

    public SelectList Years { get; set; }

    [MaxLength(100)]
    public string Find { get; set; }

    public string Year { get; set; }

    public Stage? Stage { get; set; }
}
