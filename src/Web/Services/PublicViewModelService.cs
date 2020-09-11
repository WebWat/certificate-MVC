using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly ICertificateRepository _repository;

        public PublicViewModelService(ICertificateRepository repository)
        {
            _repository = repository;
        }

        public PublicViewModel GetPublicViewModel(string year, string find, string userId, string name, string middleName, string surname, string code, byte[] photo)
        {
            var items = _repository.List(i => i.UserId == userId);

            if (year != null && year != "Все")
            {
                items = items.Where(i => i.Date.Year == int.Parse(year));
            }

            if (!string.IsNullOrEmpty(find))
            {
                items = items.Where(p => p.Title.ToLower().Contains(find.ToLower()));
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


            List<string> years = Enumerable.Range(2000, DateTime.Now.Year - 1999).OrderByDescending(i => i).Select(i => i.ToString()).ToList();
            years.Insert(0, "Все");

            PublicViewModel pvm = new PublicViewModel
            {
                Certificates = certificates,
                Years = new SelectList(years),
                Find = find,
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
            var certificate = await _repository.GetCertificateIncludeLinksAsync(i => i.Id == id && i.UserId == userId);

            return new CertificateViewModel
            {
                Id = certificate.Id,
                Title = certificate.Title,
                Description = certificate.Description,
                Links = certificate.Links,
                Rating = certificate.Rating,
                Date = certificate.Date,
                UniqueUrl = url,
                ImageData = certificate.File,
                UserId = certificate.UserId
            };
        }
    }
}
