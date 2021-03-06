﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Services
{
    public class PublicViewModelService : IPublicViewModelService
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ICachedPublicViewModelService _cacheService;

        public PublicViewModelService(ICachedPublicViewModelService cacheService, IStringLocalizer<SharedResource> localizer)
        {
            _cacheService = cacheService;
            _localizer = localizer;
        }

        public PublicViewModel GetPublicViewModel(string year, string find, string userId, string name, string middleName, string surname, string code, byte[] photo)
        {
            var items = _cacheService.GetList(userId);

            //Sort by date
            if (year != null && year != _localizer["All"])
            {
                items = items.Where(i => i.Date.Year == int.Parse(year)).ToList();
            }

            //Sort by title
            if (!string.IsNullOrEmpty(find))
            {
                items = items.Where(p => p.Title.ToLower().Contains(find.Trim().ToLower())).ToList();
            }

            var certificates = items.Select(i =>
            {
                var certificateViewModel = new CertificateViewModel
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description,
                    Date = i.Date,
                    ImageData = i.File,
                    UserId = i.UserId
                };
                return certificateViewModel;
            });

            List<string> years = Enumerable.Range(2000, DateTime.Now.Year - 1999).Reverse().Select(i => i.ToString()).ToList();
            years.Insert(0, _localizer["All"].Value);

            PublicViewModel pvm = new PublicViewModel
            {
                Certificates = certificates,
                Find = find,
                Years = new SelectList(years),
                Year = year,
                Name = name,
                Surname = surname,
                UniqueUrl = code,
                MiddleName = middleName,
                ImageData = photo
            };

            return pvm;
        }

        public async Task<CertificateViewModel> GetCertificateByIdIncludeLinksAsync(int id, string userId, string url)
        {
            var certificate = await _cacheService.GetItemAsync(id, userId);

            return new CertificateViewModel
            {
                Id = certificate.Id,
                Title = certificate.Title,
                Description = certificate.Description,
                Links = certificate.Links,
                Stage = certificate.Stage,
                Date = certificate.Date,
                UniqueUrl = url,
                ImageData = certificate.File,
                UserId = certificate.UserId
            };
        }
    }
}
