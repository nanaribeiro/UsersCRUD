using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using UsersCrud.CrossCutting.Helpers;
using UsersCrud.Domain;
using UsersCrud.Domain.DTO;
using UsersCrud.Domain.Entities;
using UsersCrud.Infra.Data.Contexts;
using UsersCrud.Infra.Data.Repository;
using UsersCrud.Services.Services;
using Xunit;


namespace UsersCrud.Services.Test
{
    public class UserTest
    {
        [Fact]
        public void InvalidUserName()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MapData>());
            var mapper = config.CreateMapper();
            var ddd = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase("teste");
            var dv = new DataContext(ddd.Options);
            var e = new BaseRepository<UserEntity>(dv);
            var settings = new AppSettings()
            {
                Secret = "Test Project"
            };
            IOptions<AppSettings> appSettingsOptions = Options.Create(settings);
            var x = new UserService(e, mapper, appSettingsOptions);

            var user = new UserDTO() { Email = "alana", Password = "123456", PhoneNumber = "062", UserName = "arana."};
            Assert.Throws<Exception>(() => x.ValidateUserData(user));
        }

        [Fact]
        public void ValidUserName()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MapData>());
            var mapper = config.CreateMapper();
            var ddd = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase("teste");
            var dv = new DataContext(ddd.Options);
            var e = new BaseRepository<UserEntity>(dv);
            var settings = new AppSettings()
            {
                Secret = "Test Project"
            };
            IOptions<AppSettings> appSettingsOptions = Options.Create(settings);
            var x = new UserService(e, mapper, appSettingsOptions);

            var user = new UserDTO() { Email = "alana", Password = "123456", PhoneNumber = "062", UserName = "arana" };
            x.ValidateUserData(user);
            Assert.True(true);
        }
    }
}
