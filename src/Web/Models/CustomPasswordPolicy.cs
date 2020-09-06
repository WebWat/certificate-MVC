using ApplicationCore.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Web.Models
{
    public class CustomPasswordPolicy : IPasswordValidator<User>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
        {
            List<IdentityError> errors = new List<IdentityError>();

            if (password.ToLower().Contains("admin") || password.ToLower().Contains("user"))
            {
                errors.Add(new IdentityError
                {
                    Description = "Пароль не должен содержать слово admin или user"
                });
            }

            if (password.ToLower().Contains(user.UserName.ToLower()))
            {
                errors.Add(new IdentityError
                {
                    Description = "Пароль не должен содержать логин"
                });
            }

            if (password.Contains("123") || password.Contains("321"))
            {
                errors.Add(new IdentityError
                {
                    Description = "Пароль не должен содержать последовательность в виде 123 или 321"
                });
            }

            if (password.Length < 9)
            {
                errors.Add(new IdentityError
                {
                    Description = "Пароль должен быть больше 8 символов"
                });
            }

            Regex regex = new Regex(@"\d");
            if (!regex.IsMatch(password))
            {
                errors.Add(new IdentityError
                {
                    Description = "Пароль должен содержать десятичные числа"
                });
            }

            regex = new Regex(@"\D");
            if (!regex.IsMatch(password))
            {
                errors.Add(new IdentityError
                {
                    Description = "Пароль должен содержать алфавитные символы"
                });
            }

            return Task.FromResult(errors.Count == 0 ?
            IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }
    }
}
