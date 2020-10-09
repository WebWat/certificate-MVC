using ApplicationCore.Entities.Identity;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Services
{
    public class AdminViewModelService : IAdminViewModelService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;

        public AdminViewModelService(IUserRepository userRepository, UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public async Task DeleteUserAsync(string id)
        {
            var _user = await _userManager.FindByIdAsync(id);

            if(_user == null)
            {
                return;
            }

            //Сheck the main user
            if (_user.UserName == "admin")
            {
                return;
            }

            await _userRepository.DeleteUserAsync(id);
        }

        public async Task EditUserRoleAsync(string login, string role)
        {
            //Сheck the main user
            if (login == "admin")
            {
                return;
            }

            var user = await _userManager.FindByNameAsync(login);

            var roles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, roles);

            await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<IEnumerable<AdminViewModel>> GetIndexAdminViewModelListAsync()
        {
            var users = _userRepository.List(i => i.UserName != "admin");

            List<AdminViewModel> result = new List<AdminViewModel>();

            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);

                result.Add(new AdminViewModel
                {
                    Id = u.Id,
                    Login = u.UserName,
                    Email = u.Email,
                    EmailConfirmed = u.EmailConfirmed,
                    Role = roles.First()
                });
            }

            return result;
        }


        public async Task<AdminViewModel> GetUserAsync(string login)
        {
            var user = await _userRepository.GetAsync(i => i.UserName == login);
            var roles = await _userManager.GetRolesAsync(user);

            return new AdminViewModel
            {
                Login = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                Role = roles.First()
            };
        }
    }
}
