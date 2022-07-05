using ApplicationCore.Entities.Identity;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OfficeOpenXml;
using System.IO;
using System.IO.Compression;
using System.Net.Mime;
using System.Threading.Tasks;
using Web.Interfaces;


namespace Web.Controllers;

[Authorize]
[ApiController]
[Route("[controller]/[action]")]
public class DownloadController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICertificateRepository _repository;
    private readonly IStringLocalizer<SharedResource> _localizer;
    private readonly IStageService _stage;
    private readonly IWebHostEnvironment _appEnvironment;


    public DownloadController(UserManager<ApplicationUser> userManager,
                              ICertificateRepository repository,
                              IStringLocalizer<SharedResource> localizer,
                              IStageService stage,
                              IWebHostEnvironment appEnvironment)
    {
        _userManager = userManager;
        _repository = repository;
        _localizer = localizer;
        _stage = stage;
        _appEnvironment = appEnvironment;
    }


    [HttpGet("{id}")]
    public async Task<FileResult?> Jpg(int id)
    {
        var _user = await _userManager.GetUserAsync(User);

        var certificate = await _repository.GetByUserIdAsync(id, _user.Id);

        if (certificate is null)
        {
            return default;
        }

        return File(certificate.Path, MediaTypeNames.Image.Jpeg, certificate.Title + ".jpg");
    }


    [HttpGet]
    public async Task<FileResult?> Zip()
    {
        var _user = await _userManager.GetUserAsync(User);

        var list = _repository.ListByUserId(_user.Id);

        if (list is null)
        {
            return default;
        }

        // Creating an archive. 
        var compressedFileStream = new MemoryStream();
        (int count, string lastName) itemInfo = (0, string.Empty);

        using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Create, false))
        {
            foreach (var item in list)
            {
                string fileName = string.Empty;

                if (itemInfo.count == 0)
                {
                    // For the first time, we simply name the itemInfo.lastName.
                    itemInfo.lastName = item.Title;
                    fileName = itemInfo.lastName;
                }
                else
                {
                    // If identical names are found, then add a serial number.
                    fileName = itemInfo.lastName == item.Title ? item.Title + "_" + itemInfo.count : item.Title;
                }

                itemInfo.count++;

                var zipEntry = zipArchive.CreateEntry(fileName + ".jpg");

                using var originalFileStream = new MemoryStream(
                    System.IO.File.ReadAllBytes(_appEnvironment.WebRootPath + item.Path));

                using var zipEntryStream = zipEntry.Open();
                originalFileStream.CopyTo(zipEntryStream);
            }
        }

        return File(compressedFileStream.ToArray(),
                    MediaTypeNames.Application.Zip,
                    _localizer["Achievements"] + ".zip");
    }


    [HttpGet]
    public async Task<FileResult?> Excel()
    {
        var _user = await _userManager.GetUserAsync(User);
        var certificates = _repository.ListByUserId(_user.Id);
        byte[]? data = default;
        int column = 2;
        int count = 1;

        // https://epplussoftware.com/developers/licenseexception
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Sheet1");

            // Insert column names.
            worksheet.Cells[1, 1].Value = "№";
            worksheet.Cells[1, 2].Value = _localizer["AwardTitle"];
            worksheet.Cells[1, 3].Value = _localizer["Description"];
            worksheet.Cells[1, 4].Value = _localizer["Stage"];
            worksheet.Cells[1, 5].Value = _localizer["Date"];

            foreach (var c in certificates)
            {
                // Insert data.
                worksheet.Cells[column, 1].Value = count.ToString();
                worksheet.Cells[column, 2].Value = c.Title;
                worksheet.Cells[column, 3].Value = c.Description;
                worksheet.Cells[column, 4].Value = _stage.GetNameOfStage(c.Stage);
                worksheet.Cells[column, 5].Value = c.Date.ToShortDateString();

                column++;
                count++;
            }

            data = package.GetAsByteArray();
        }

        if (data is null || data.Length == 0)
        {
            return default;
        }

        return File(data,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    _localizer["Achievements"] + ".xlsx");
    }
}
