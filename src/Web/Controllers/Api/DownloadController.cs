using ApplicationCore.Entities.Identity;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class DownloadController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ICertificateRepository _repository;

        public DownloadController(UserManager<User> userManager, ICertificateRepository repository)
        {
            _userManager = userManager;
            _repository = repository;
        }

        [HttpGet("{id}")]
        public async Task<FileResult> Jpg(int id)
        {
            var _user = await _userManager.GetUserAsync(User);

            var certificate = await _repository.GetAsync(i => i.Id == id && i.UserId == _user.Id);

            return File(certificate.File, "image/jpeg", certificate.Title + ".jpg");
        }

        [HttpGet]
        public async Task<FileResult> Zip()
        {
            var _user = await _userManager.GetUserAsync(User);

            var list = _repository.List(i => i.UserId == _user.Id);

            if (list == null)
            {
                return null;
            }

            var compressedFileStream = new MemoryStream();

            using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Create, false))
            {
                foreach (var item in list)
                {
                    var zipEntry = zipArchive.CreateEntry(item.Title + ".jpg");

                    using (var originalFileStream = new MemoryStream(item.File))
                    using (var zipEntryStream = zipEntry.Open())
                    {
                        originalFileStream.CopyTo(zipEntryStream);
                    }
                }
            }

            return File(compressedFileStream.ToArray(), "application/zip", "Достижения.zip");
        }

        [HttpGet]
        public async Task<FileResult> Excel()
        {
            var _user = await _userManager.GetUserAsync(User);
            var certificates = _repository.List(i => i.UserId == _user.Id);
            byte[] data = default;
            int column = 2;
            int count = 1;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; //https://epplussoftware.com/developers/licenseexception

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells[1, 1].Value = "№";
                worksheet.Cells[1, 2].Value = "Название";
                worksheet.Cells[1, 3].Value = "Описание";
                worksheet.Cells[1, 4].Value = "Этап";
                worksheet.Cells[1, 5].Value = "Дата";

                foreach (var c in certificates)
                {
                    worksheet.Cells[column, 1].Value = count.ToString();
                    worksheet.Cells[column, 2].Value = c.Title;
                    worksheet.Cells[column, 3].Value = c.Description;
                    worksheet.Cells[column, 4].Value = GetRating(c.Rating);
                    worksheet.Cells[column, 5].Value = c.Date.ToShortDateString();

                    column++;
                    count++;
                }

                data = package.GetAsByteArray();
            }

            if (data == null || data.Length == 0)
            {
                return null;
            }

            return File(
                data,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Достижения.xlsx"
            );
        }

        private string GetRating(int id) => id switch
        {
            1 => "Школьный",
            2 => "Районный",
            3 => "Республиканский",
            4 => "Всероссийский",
            5 => "Международный",
            _ => ""
        };
    }
}
