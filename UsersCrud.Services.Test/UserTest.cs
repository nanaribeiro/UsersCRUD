using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;
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

            var user = new UserDTO() { Email = "alana@a.com", Password = "123456", PhoneNumber = "6298746521", UserName = "arana."};
            var exception = Assert.Throws<Exception>(() => x.ValidateUserData(user));
            Assert.Equal("O nome de usuário deve ter entre 5 a 50 caracteres alfanúmericos", exception.Message);
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

            var user = new UserDTO() { Email = "alana@a.com", Password = "123456", PhoneNumber = "6298746521", UserName = "arana" };
            x.ValidateUserData(user);
            Assert.True(true);
        }

        [Fact]
        public void InvalidEmail()
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

            var user = new UserDTO() { Email = "global@", Password = "123456", PhoneNumber = "6298746521", UserName = "muitod4" };
            var exception = Assert.Throws<Exception>(() => x.ValidateUserData(user));
            Assert.Equal("O e-mail deve ser um e-mail válido", exception.Message);
        }

        [Fact]
        public void InvalidPhoneNumber()
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

            var user = new UserDTO() { Email = "alana@a.com", Password = "123456", PhoneNumber = "(62)99122-929", UserName = "muitod4" };
            var exception = Assert.Throws<Exception>(() => x.ValidateUserData(user));
            Assert.Equal("O número de telefone deve ser um telefone válido", exception.Message);
        }

        [Fact]
        public void NewValidUser()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MapData>());
            var mapper = config.CreateMapper();
            var ddd = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase("teste");
            var dv = new DataContext(ddd.Options);
            var e = new BaseRepository<UserEntity>(dv);
            var settings = new AppSettings()
            {
                Secret = "Asls-nsns-TERKRKS-**sm"
            };
            IOptions<AppSettings> appSettingsOptions = Options.Create(settings);
            var x = new UserService(e, mapper, appSettingsOptions);

            var user = new UserDTO() { Email = "alana@a.com", Password = "123456", PhoneNumber = "(62)99122-9290", UserName = "muitod4" };

            var result = x.AddNewUser(user).Result;
            Assert.True(result!=null);

        }

        /// <summary>
        /// Executar teste junto com NewValidUser
        /// </summary>
        [Fact]
        public void AddingExistingUserName()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MapData>());
            var mapper = config.CreateMapper();
            var ddd = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase("teste");
            var dv = new DataContext(ddd.Options);
            var e = new BaseRepository<UserEntity>(dv);
            var settings = new AppSettings()
            {
                Secret = "Asls-nsns-TERKRKS-**sm"
            };
            IOptions<AppSettings> appSettingsOptions = Options.Create(settings);
            var x = new UserService(e, mapper, appSettingsOptions);

            var user = new UserDTO() { Email = "alana@a.com", Password = "123456", PhoneNumber = "(62)99122-9290", UserName = "muitod4" };

            user.Email = "local@ema.jp";

            var dynMethod = typeof(UserService).GetMethod("VerifyExistingUser", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.Throws<TargetInvocationException>(() => dynMethod.Invoke(x, new object[] { user }));
        }

    }
}
