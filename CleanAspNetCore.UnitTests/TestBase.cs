using AutoMapper;
using CleanAspNetCoreWithFreeMetronic.Data;
using CleanAspNetCoreWithFreeMetronic.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using Xunit;

namespace CleanAspNetCore.UnitTests
{
    public abstract class TestBase
    {
        
        protected ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                                  .UseInMemoryDatabase(Guid.NewGuid().ToString())
                                  .Options;
            return new ApplicationDbContext(options);

        }

        //https://stackoverflow.com/questions/49708895/how-to-write-xunit-test-for-net-core-2-0-service-that-uses-automapper-and-depen?rq=1
        protected IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapping());
            });
            return config.CreateMapper();
        }

        ////https://dejanstojanovic.net/aspnet/2019/september/unit-testing-repositories-in-aspnet-core-with-xunit-and-moq/
        public Mock<UserManager<TIDentityUser>> GetUserManagerMock<TIDentityUser>() where TIDentityUser : IdentityUser
        {
            return new Mock<UserManager<TIDentityUser>>(
                    new Mock<IUserStore<TIDentityUser>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<TIDentityUser>>().Object,
                    new IUserValidator<TIDentityUser>[0],
                    new IPasswordValidator<TIDentityUser>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<TIDentityUser>>>().Object);
        }

        //public Mock<RoleManager<TIdentityRole>> GetRoleManagerMock<TIdentityRole>() where TIdentityRole : IdentityRole
        //{
        //    return new Mock<RoleManager<TIdentityRole>>(
        //            new Mock<IRoleStore<TIdentityRole>>().Object,
        //            new IRoleValidator<TIdentityRole>[0],
        //            new Mock<ILookupNormalizer>().Object,
        //            new Mock<IdentityErrorDescriber>().Object,
        //            new Mock<ILogger<RoleManager<TIdentityRole>>>().Object);
        //}

    }
}
