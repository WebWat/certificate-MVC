using ApplicationCore.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels;

public class SearchViewModel
{
    public Dictionary<string, string> Parameters { get; set; } = new();

    public string Controller { get; set; } = string.Empty;

    public SelectList Stages { get; set; } = default!;

    public SelectList Years { get; set; } = default!;

    [MaxLength(100)]
    public string? Find { get; set; }

    public string? Year { get; set; }

    public Stage? Stage { get; set; }
}
