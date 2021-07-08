using ApplicationCore.Constants;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminViewModelService(IUserRepository userRepository, UserManager<ApplicationUser> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }


        public async Task<IEnumerable<AdminViewModel>> GetIndexAdminViewModelListAsync()
        {
            var users = _userRepository.List(i => i.UserName != AuthorizationConstants.UserName);

            var result = new List<AdminViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                result.Add(new AdminViewModel
                {
                    Login = user.UserName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    RegistrationDate = user.RegistrationDate,
                    Role = roles.First()
                });
            }

            return result;
        }
    }
}
