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

            List<AdminViewModel> result = new List<AdminViewModel>();

            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);

                result.Add(new AdminViewModel
                {
                    Login = u.UserName,
                    Email = u.Email,
                    EmailConfirmed = u.EmailConfirmed,
                    RegistrationDate = u.RegistrationDate,
                    Role = roles.First()
                });
            }

            return result;
        }
    }
}
