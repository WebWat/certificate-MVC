using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.ViewComponents;

public class UsersCount : ViewComponent
{
    private readonly IUserRepository _repository;

    public UsersCount(IUserRepository repository)
    {
        _repository = repository;
    }


    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View(await _repository.GetCountAsync());
    }
}