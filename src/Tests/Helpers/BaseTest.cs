using System;
using System.Collections.Generic;
using AutoMapper;
using BusinessLayer.Config;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Common;
using BusinessLayer.Helpers;
using BusinessLayer.QueryObjects.Base;
using BusinessLayer.Repository;
using DAL;
using DAL.BaHuEntities;
using DAL.BaHuEntities.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Moq;
using NUnit.Framework;
using RandomNameGenerator;
using RandomStringGenerator;

namespace Tests.Helpers
{
    public class BaseTest<T, TDto, TFilter>
        where T : class, IEntity, new()
        where TDto : DtoBase
        where TFilter : FilterDtoBase
    {
        #region AttributesAboutDI
        private DbContextOptions<BadgerHunterDbContextBase> _dbContextOptions { get; }
        protected DbContext DbContext { get; set; }
        protected Types ActualTypes { get; set;}
        protected Repository<T> Repository { get; set; }
        protected QueryBase<T,TDto, TFilter> QueryBase { get; set; }
        protected IList<User> UsersInDb { get; set; }

        #endregion

        #region RandomListsOfDtos

        protected IList<BaHuAchievementDto> RandomAchievements;
        
        protected IList<BaHuAchievementGroupDto> RandomAchievementGroups;
        
        protected IList<BaHuRewardDto> RandomRewards;
        
        protected IList<BaHuNotificationDto> RandomNotifications;
        
        protected IList<BaHuSubTaskDto> RandomSubTasks;

        #endregion
        
        private StringGenerator _stringGenerator { get; } = new StringGenerator();
        protected Mapper AutoMapper { get; } =
            new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new MapperConfig(new Types()))));

        public BaseTest()
        {
            _dbContextOptions = new DbContextOptionsBuilder<BadgerHunterDbContextBase>()
                .UseInMemoryDatabase(databaseName: "test")
                .Options;
        }

        [SetUp]
        public void SetUp()
        {
            DbContext = new BadgerHunterDbContextBase(_dbContextOptions);
            Repository = new Repository<T>(AutoMapper, DbContext, ActualTypes);
            var tmpQuery = typeof(QueryBase<,,>).MakeGenericType(typeof(T), typeof(TDto), typeof(TFilter));
            var query = Activator.CreateInstance(tmpQuery, DbContext, AutoMapper, ActualTypes);
            QueryBase = query as QueryBase<T,TDto,TFilter>;
            InsertUsersIntoDb();
            GenerateAllRandomList();
        }

        [TearDown]
        public void TearDown()
        {
            DbContext.Dispose();
            Repository = null;
            QueryBase = null;
        }

        public IList<User> GetListOfRandomUsers()
        {
            var list = new List<User>(8);
            for (var i = 0; i < 8; ++i)
            {
                var gender = i % 2 == 0 ? Gender.Male : Gender.Female;
                var name = NameGenerator.GenerateFirstName(gender);
                list.Add(new User
                {
                    FirstName = name,
                    LastName = NameGenerator.GenerateLastName(),
                    Email = $"{name}@gmail.com"
                });
            }
            return list;
        }

        private void InsertUsersIntoDb()
        {
            UsersInDb = GetListOfRandomUsers();
            foreach (var user in UsersInDb)
            {
                DbContext.Set<User>()
                    .Add(user);
            }
        }

        private void GenerateAchievements()
        {
            RandomAchievements = new List<BaHuAchievementDto>(10);
            for (var i = 0; i < 10; i++)
            {
                var eva = i % 2 == 0 ? BaHuEvaluationsDto.Progress : BaHuEvaluationsDto.YesNo;
                RandomAchievements.Add(new BaHuAchievementDto
                {
                    Name = NameGenerator.GenerateLastName() + NameGenerator.GenerateLastName(),
                    Evaluation = eva,
                    Description = _stringGenerator.GenerateString(i * 8)
                });
            }
        }

        private void GenerateRewards()
        {
            RandomRewards = new List<BaHuRewardDto>(10);
            for (int i = 0; i < 10; i++)
            {
                RandomRewards.Add(new BaHuRewardDto
                {
                    Name = NameGenerator.GenerateLastName() + NameGenerator.GenerateLastName()
                });
            }
        }

        private void GenerateSubTasks()
        {
            RandomSubTasks = new List<BaHuSubTaskDto>(10);
            for (int i = 0; i < 10; ++i)
            {
                RandomSubTasks.Add(new BaHuSubTaskDto
                {
                    Name = NameGenerator.GenerateLastName() + NameGenerator.GenerateLastName(),
                    Description = _stringGenerator.GenerateString(i * 8)
                });
            }
        }

        private void GenerateAchievementGroups()
        {
            RandomAchievementGroups = new List<BaHuAchievementGroupDto>();
            for (int i = 0; i < 10; i++)
            {
                RandomAchievementGroups.Add(new BaHuAchievementGroupDto
                {
                    Name = NameGenerator.GenerateLastName() + NameGenerator.GenerateLastName(),
                    ExpiredIn = DateTime.Now.AddDays(i * 8)
                });
            }
        }

        private void GenerateNotifications()
        {
            RandomNotifications = new List<BaHuNotificationDto>();
            for (int i = 0; i < 10; i++)
            {
                RandomNotifications.Add(new BaHuNotificationDto
                {
                    Message = _stringGenerator.GenerateString(i * 10),
                    Created = DateTime.Now.AddDays(i * 8 - 1)
                });
            }
        }

        private void GenerateAllRandomList()
        {
            GenerateAchievements();
            GenerateRewards();
            GenerateAchievementGroups();
            GenerateSubTasks();  
            GenerateNotifications();
        }
        
        public Repository<T> GetRepository<T>()
        where T : class, IEntity, new()
        {
            return new Repository<T>(AutoMapper, DbContext, ActualTypes);
        }
    }
}