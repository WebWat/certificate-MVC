﻿using ApplicationCore.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels;

public class CertificateViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Required")]
    [Display(Name = "AwardTitle")]
    [MaxLength(100)]
    [RegularExpression(@"[^&<>\/]*$", ErrorMessage = "RegularExpression")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Required")]
    [Display(Name = "AwardDescription")]
    [MaxLength(200)]
    [RegularExpression(@"[^&<>\/]*$", ErrorMessage = "RegularExpression")]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Date")]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }

    public List<Link>? Links { get; set; }

    [Display(Name = "File")]
    [DataType(DataType.Upload)]
    public IFormFile? File { get; set; }

    public string Path { get; set; } = string.Empty;

    public string UniqueUrl { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;

    [Display(Name = "Stage")]
    public Stage Stage { get; set; }

    public int Page { get; set; }
}
