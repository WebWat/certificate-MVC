using ApplicationCore.Entities.Identity;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Services;
using Web.ViewModels;
using Xunit;

namespace UnitTests.Web.Services
{
    public class AdminViewModelServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;

        public AdminViewModelServiceTests()
        {
            _mockUserRepository = new();

            var store = new Mock<IUserStore<ApplicationUser>>();
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(store.Object,
                                                                      null, null, null,
                                                                      null, null, null,
                                                                      null, null);

            _mockUserRepository.Setup(f => f.List(It.IsAny<Func<ApplicationUser, bool>>()))
                               .Returns(GetUsers);

            _mockUserManager.Setup(f => f.GetRolesAsync(It.IsAny<ApplicationUser>()))
                            .ReturnsAsync(GetRoles);
        }


        [Fact]
        public async Task ReturnAdminViewModelList()
        {
            // Arrange
            var service = new AdminViewModelService(_mockUserRepository.Object,
                                                    _mockUserManager.Object);

            // Act
            var result = await service.GetIndexAdminViewModelListAsync();

            // Assert
            _mockUserRepository.Verify(f => f.List(It.IsAny<Func<ApplicationUser, bool>>()));
            _mockUserManager.Verify(f => f.GetRolesAsync(It.IsAny<ApplicationUser>()));
            Assert.Equal(2, (result as List<AdminViewModel>).Count);
        }


        private List<ApplicationUser> GetUsers() =>
            new()
            {
                new ApplicationUser { UserName = "user1" },
                new ApplicationUser { UserName = "user2" }
            };


        private List<string> GetRoles() =>
            new()
            {
                "user"
            };
    }
}
