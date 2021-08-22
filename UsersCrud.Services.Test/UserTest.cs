using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
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
        Guid userId = Guid.Empty;
        Guid adminId = Guid.Empty;

        /// <summary>
        /// Testar a valida��o do nome de usu�rio inv�lido
        /// </summary>
        [Fact]
        public void InvalidUserName()
        {
            var mapperconfig = new MapperConfiguration(cfg => cfg.AddProfile<MapData>());
            var mapper = mapperconfig.CreateMapper();
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase("teste");
            var dataContext = new DataContext(dbContextOptions.Options);
            var repository = new BaseRepository<UserEntity>(dataContext);

            var settings = new AppSettings()
            {
                Secret = "Test Project"
            };

            IOptions<AppSettings> appSettingsOptions = Options.Create(settings);
            var userService = new UserService(repository, mapper, appSettingsOptions);

            var user = new UserDTO() { Email = "alana@a.com", Password = "123456", PhoneNumber = "6298746521", UserName = "arana."};

            var invokeMethod = typeof(UserService).GetMethod("ValidateUserData", BindingFlags.NonPublic | BindingFlags.Instance);
            var exception = Assert.Throws<TargetInvocationException>(() => invokeMethod.Invoke(userService, new object[] { user }));
        }

        /// <summary>
        /// Testar a valida��o de nome de usu�rio v�lido
        /// </summary>
        [Fact]
        public void ValidUserName()
        {
            var mapperconfig = new MapperConfiguration(cfg => cfg.AddProfile<MapData>());
            var mapper = mapperconfig.CreateMapper();
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase("teste");
            var dataContext = new DataContext(dbContextOptions.Options);
            var repository = new BaseRepository<UserEntity>(dataContext);

            var settings = new AppSettings()
            {
                Secret = "Test Project"
            };
            IOptions<AppSettings> appSettingsOptions = Options.Create(settings);
            var userService = new UserService(repository, mapper, appSettingsOptions);

            var user = new UserDTO() { Email = "alana@a.com", Password = "123456", PhoneNumber = "6298746521", UserName = "arana" };

            var invokeMethod = typeof(UserService).GetMethod("ValidateUserData", BindingFlags.NonPublic | BindingFlags.Instance);
            invokeMethod.Invoke(userService, new object[] { user });
            Assert.True(true);
        }

        /// <summary>
        /// Testar a valida��o de email inv�lido
        /// </summary>
        [Fact]
        public void InvalidEmail()
        {
            var mapperconfig = new MapperConfiguration(cfg => cfg.AddProfile<MapData>());
            var mapper = mapperconfig.CreateMapper();
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase("teste");
            var dataContext = new DataContext(dbContextOptions.Options);
            var repository = new BaseRepository<UserEntity>(dataContext);
            var settings = new AppSettings()
            {
                Secret = "Test Project"
            };
            IOptions<AppSettings> appSettingsOptions = Options.Create(settings);
            var userService = new UserService(repository, mapper, appSettingsOptions);

            var user = new UserDTO() { Email = "global@", Password = "123456", PhoneNumber = "6298746521", UserName = "muitod4" };

            var invokeMethod = typeof(UserService).GetMethod("ValidateUserData", BindingFlags.NonPublic | BindingFlags.Instance);

            var exception = Assert.Throws<TargetInvocationException>(() => invokeMethod.Invoke(userService, new object[] { user }));
        }

        /// <summary>
        /// Testar a valida�ao de n�mero de telefone inv�lido
        /// </summary>
        [Fact]
        public void InvalidPhoneNumber()
        {
            var mapperconfig = new MapperConfiguration(cfg => cfg.AddProfile<MapData>());
            var mapper = mapperconfig.CreateMapper();
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase("teste");
            var dataContext = new DataContext(dbContextOptions.Options);
            var repository = new BaseRepository<UserEntity>(dataContext);
            var settings = new AppSettings()
            {
                Secret = "Test Project"
            };
            IOptions<AppSettings> appSettingsOptions = Options.Create(settings);
            var userService = new UserService(repository, mapper, appSettingsOptions);

            var user = new UserDTO() { Email = "alana@a.com", Password = "123456", PhoneNumber = "(62)99122-929", UserName = "muitod4" };

            var invokeMethod = typeof(UserService).GetMethod("ValidateUserData", BindingFlags.NonPublic | BindingFlags.Instance);

            var exception = Assert.Throws<TargetInvocationException>(() => invokeMethod.Invoke(userService, new object[] { user }));
        }

        /// <summary>
        /// Testar a adi��o de um novo usu�rio com dados v�lidos
        /// </summary>
        [Fact]
        public void NewValidUser()
        {
            var mapperconfig = new MapperConfiguration(cfg => cfg.AddProfile<MapData>());
            var mapper = mapperconfig.CreateMapper();
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase("teste");
            var dataContext = new DataContext(dbContextOptions.Options);
            var repository = new BaseRepository<UserEntity>(dataContext);
            var settings = new AppSettings()
            {
                Secret = "Asls-nsns-TERKRKS-**sm"
            };
            IOptions<AppSettings> appSettingsOptions = Options.Create(settings);
            var userService = new UserService(repository, mapper, appSettingsOptions);

            var user = new UserDTO() { Email = "alana@a.com", Password = "123456", PhoneNumber = "(62)99122-9290", UserName = "muitod4" };

            var result = userService.AddNewUser(user).Result;
            userId = result.Id;
            Assert.True(result!=null);

        }

        /// <summary>
        /// Testar a valida��o d� adi��o de um usu�rio existente
        /// </summary>
        [Fact]
        public void AddingExistingUserName()
        {
            var mapperconfig = new MapperConfiguration(cfg => cfg.AddProfile<MapData>());
            var mapper = mapperconfig.CreateMapper();
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase("teste");
            var dataContext = new DataContext(dbContextOptions.Options);
            var repository = new BaseRepository<UserEntity>(dataContext);
            var settings = new AppSettings()
            {
                Secret = "Asls-nsns-TERKRKS-**sm"
            };
            IOptions<AppSettings> appSettingsOptions = Options.Create(settings);
            var userService = new UserService(repository, mapper, appSettingsOptions);

            var user = new UserDTO() { Email = "alana@a.com", Password = "123456", PhoneNumber = "(62)99122-9290", UserName = "muitod4" };

            var result = userService.AddNewUser(user).Result;

            var invokeMethod = typeof(UserService).GetMethod("VerifyExistingUser", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.Throws<TargetInvocationException>(() => invokeMethod.Invoke(userService, new object[] { user }));
        }

        /// <summary>
        /// Testar a atualiza��o de um usu�rio com e-mail inv�lido
        /// </summary>
        [Fact]
        public void UpdateUserWithInvalidEmail()
        {
            var mapperconfig = new MapperConfiguration(cfg => cfg.AddProfile<MapData>());
            var mapper = mapperconfig.CreateMapper();
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase("teste");
            var dataContext = new DataContext(dbContextOptions.Options);
            var repository = new BaseRepository<UserEntity>(dataContext);
            var settings = new AppSettings()
            {
                Secret = "Asls-nsns-TERKRKS-**sm"
            };
            IOptions<AppSettings> appSettingsOptions = Options.Create(settings);
            var userService = new UserService(repository, mapper, appSettingsOptions);

            var user = new UserDTO() { Email = "alana@a.com", Password = "123456", PhoneNumber = "(62)99122-9290", UserName = "muitod4" };

            user.Email = "local@repository";

            var exception = Assert.ThrowsAsync<Exception>(() => userService.UpdateUser(userId, user));
            Assert.Equal("O repository-mail deve ser um repository-mail v�lido", exception.Result.Message);
        }

        /// <summary>
        /// Testar a atualiza��o de um usu�rio com dados v�lidos
        /// </summary>
        [Fact]
        public void UpdateUserWithValidData()
        {
            var mapperconfig = new MapperConfiguration(cfg => cfg.AddProfile<MapData>());
            var mapper = mapperconfig.CreateMapper();
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase("teste");
            var dataContext = new DataContext(dbContextOptions.Options);
            var repository = new BaseRepository<UserEntity>(dataContext);
            var settings = new AppSettings()
            {
                Secret = "Asls-nsns-TERKRKS-**sm"
            };
            IOptions<AppSettings> appSettingsOptions = Options.Create(settings);
            var userService = new UserService(repository, mapper, appSettingsOptions);

            var users = (List<UserEntity>)userService.GetAllUsers().Result;            

            var user = users.Find(userService => userService.UserName == "muitod4");

            var result = userService.UpdateUser(user.Id, new UserDTO { Email = "local@repository.com", UserName = "radical8", Password = "novasenha", PhoneNumber = user.PhoneNumber}).Result;
            Assert.True(result != null);
        }

        /// <summary>
        /// Testar a obten��o da lista de usu�rios do sistema
        /// </summary>
        [Fact]
        public void GetAllUsers()
        {
            var mapperconfig = new MapperConfiguration(cfg => cfg.AddProfile<MapData>());
            var mapper = mapperconfig.CreateMapper();
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase("teste");
            var dataContext = new DataContext(dbContextOptions.Options);
            var repository = new BaseRepository<UserEntity>(dataContext);
            var settings = new AppSettings()
            {
                Secret = "Asls-nsns-TERKRKS-**sm"
            };
            IOptions<AppSettings> appSettingsOptions = Options.Create(settings);
            var userService = new UserService(repository, mapper, appSettingsOptions);

            var result = (List<UserEntity>)userService.GetAllUsers().Result;
            Assert.True(result.Count> 0);
        }

        /// <summary>
        /// Testar o m�todo de autentica��o com usu�rio inv�lido
        /// </summary>
        [Fact]
        public void AuthenticateWithInvalidUser()
        {
            var mapperconfig = new MapperConfiguration(cfg => cfg.AddProfile<MapData>());
            var mapper = mapperconfig.CreateMapper();
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase("teste");
            var dataContext = new DataContext(dbContextOptions.Options);
            var repository = new BaseRepository<UserEntity>(dataContext);
            var settings = new AppSettings()
            {
                Secret = "Asls-nsns-TERKRKS-**sm"
            };
            IOptions<AppSettings> appSettingsOptions = Options.Create(settings);
            var userService = new UserService(repository, mapper, appSettingsOptions);

            var user = new UserDTO() { Email = "alana@a.com", Password = "12345", PhoneNumber = "(62)99122-9290", UserName = "muitod4" };

            var exception = Assert.ThrowsAsync<Exception>(() => userService.Authenticate(user));
            Assert.Equal("Usu�rio ou senha inv�lido", exception.Result.Message);            
        }

        /// <summary>
        /// Dados de admin para testar m�todos de valida��o de usu�rio padr�o
        /// </summary>
        [Fact]
        public void AddAdminUser()
        {
            var mapperconfig = new MapperConfiguration(cfg => cfg.AddProfile<MapData>());
            var mapper = mapperconfig.CreateMapper();
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase("teste");
            var dataContext = new DataContext(dbContextOptions.Options);
            var repository = new BaseRepository<UserEntity>(dataContext);
            var settings = new AppSettings()
            {
                Secret = "Asls-nsns-TERKRKS-**sm"
            };
            IOptions<AppSettings> appSettingsOptions = Options.Create(settings);
            var userService = new UserService(repository, mapper, appSettingsOptions);

            var user = new UserDTO() { Email = "admin@admin.com", UserName = "4dmin21", Password = "Junt0Segur0s@2021", PhoneNumber = "21 9875-9854" };

            var result = userService.AddNewUser(user).Result;
            adminId = result.Id;
            Assert.True(result != null);
        }

        /// <summary>
        /// Testar a valida��o ]da atualiza��o do usu�rio padr�o 
        /// </summary>
        [Fact]
        public void ValidateAdminUserUpdate()
        {
            var mapperconfig = new MapperConfiguration(cfg => cfg.AddProfile<MapData>());
            var mapper = mapperconfig.CreateMapper();
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase("teste");
            var dataContext = new DataContext(dbContextOptions.Options);
            var repository = new BaseRepository<UserEntity>(dataContext);
            var settings = new AppSettings()
            {
                Secret = "Asls-nsns-TERKRKS-**sm"
            };
            IOptions<AppSettings> appSettingsOptions = Options.Create(settings);
            var userService = new UserService(repository, mapper, appSettingsOptions);

            var invokeMethod = typeof(UserService).GetMethod("ValidateAdminUserUpdate", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.Throws<TargetInvocationException>(() => invokeMethod.Invoke(userService, new object[] { "4dmin21", "eumudooadmin" }));
        }

        /// <summary>
        /// Testar valida��o de exclus�o do usu�rio padr�o do sistema
        /// </summary>
        [Fact]
        public void ValidateAdminUserDelete()
        {
            var mapperconfig = new MapperConfiguration(cfg => cfg.AddProfile<MapData>());
            var mapper = mapperconfig.CreateMapper();
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase("teste");
            var dataContext = new DataContext(dbContextOptions.Options);
            var repository = new BaseRepository<UserEntity>(dataContext);
            var settings = new AppSettings()
            {
                Secret = "Asls-nsns-TERKRKS-**sm"
            };
            IOptions<AppSettings> appSettingsOptions = Options.Create(settings);
            var userService = new UserService(repository, mapper, appSettingsOptions);

            var invokeMethod = typeof(UserService).GetMethod("ValidateAdminUserDelete", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.Throws<TargetInvocationException>(() => invokeMethod.Invoke(userService, new object[] { adminId }));
        }

        /// <summary>
        /// Testar a dele��o de um usu�rio inv�lido
        /// </summary>
        [Fact]
        public void DeleteInvalidUser()
        {
            var mapperconfig = new MapperConfiguration(cfg => cfg.AddProfile<MapData>());
            var mapper = mapperconfig.CreateMapper();
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase("teste");
            var dataContext = new DataContext(dbContextOptions.Options);
            var repository = new BaseRepository<UserEntity>(dataContext);
            var settings = new AppSettings()
            {
                Secret = "Asls-nsns-TERKRKS-**sm"
            };
            IOptions<AppSettings> appSettingsOptions = Options.Create(settings);
            var userService = new UserService(repository, mapper, appSettingsOptions);

            var result = Assert.ThrowsAsync<Exception>(() => userService.DeleteUser(new Guid()));
            Assert.Equal("Usu�rio inexistente", result.Result.Message);
        }

    }
}
