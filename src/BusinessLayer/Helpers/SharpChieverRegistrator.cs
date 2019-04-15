using System;
using System.Collections.Generic;
using System.Reflection;
using AutoMapper;
using BusinessLayer.Config;
using BusinessLayer.Facades;
using BusinessLayer.QueryObjects;
using BusinessLayer.QueryObjects.Base;
using BusinessLayer.Repository;
using BusinessLayer.Services.Generic.Achievement;
using BusinessLayer.Services.Generic.AchievementGroup;
using BusinessLayer.Services.Generic.Notification;
using BusinessLayer.Services.Generic.Reward;
using BusinessLayer.Services.Generic.SubTask;
using BusinessLayer.Services.Generic.User;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLayer.Helpers
{
    public static class SharpChieverRegistrator
    {

        private static Types _actualTypes;
        
        public static MapperConfiguration InitializeAutoMapper(IEnumerable<Profile> mappingProfiles = null)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapperConfig(_actualTypes));

                if (mappingProfiles == null) return;
                
                foreach (var profile in mappingProfiles)
                {
                    cfg.AddProfile(profile);
                }
            });
            return config;
        }
        public static void RegisterSharpChiever<TAchievementDbContext>(this IServiceCollection services, Profile[] mappingProfiles = null)
            where TAchievementDbContext : DbContext
        {
           services
               .Scan(scan => scan
                   .FromAssembliesOf(typeof(QueryBase<,,>))
                                .AddClasses()
                                .AsImplementedInterfaces()
                                .AsSelf()
                                .WithScopedLifetime());
           
            DetectGenericParametersOfDbContext<TAchievementDbContext>();
            services.AddScoped(typeof(INotificationService<,>), typeof(NotificationService<,>));
            services.AddScoped(typeof(IAchievementService<,,>), typeof(AchievementService<,,>));
            services.AddScoped(typeof(IAchievementGroupService<,,>), typeof(AchievementGroupService<,,>));
            services.AddScoped(typeof(IRewardService<,>), typeof(RewardService<,>));
            services.AddScoped(typeof(IUserService<,,,>), typeof(UserService<,,,>));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(ISubTaskService<,>), typeof(SubTaskService<,>));
            services.AddScoped(typeof(AchievementFacade<,,,,,>));
            services.AddScoped(typeof(AchievementsQuery<,>));
            services.AddScoped(typeof(AchievementGroupQuery<,>));
            services.AddScoped(typeof(NotificationQuery<,>));
            services.AddScoped(typeof(SubTaskFacade<,>));
            services.AddScoped<DbContext, TAchievementDbContext>();
            services.AddSingleton(typeof(Types), _actualTypes);
            
            services.AddSingleton(typeof(IMapper), new Mapper(InitializeAutoMapper(mappingProfiles)));
        }

        private static void DetectGenericParametersOfDbContext<TAchievementDbContext>()
            where TAchievementDbContext : DbContext
        {
            var context = FindGenericBaseType(typeof(TAchievementDbContext), typeof(AchievementDbContext<,,,,,,,,,,>));
            if (context == null) return;
            
            _actualTypes = new Types(context.GenericTypeArguments[0], context.GenericTypeArguments[1],
                context.GenericTypeArguments[2], context.GenericTypeArguments[3], context.GenericTypeArguments[4],
                context.GenericTypeArguments[5], context.GenericTypeArguments[6], context.GenericTypeArguments[7],
                context.GenericTypeArguments[8], context.GenericTypeArguments[9], context.GenericTypeArguments[10]);
        }

        private static TypeInfo FindGenericBaseType(Type currentType, Type genericBaseType)
        {
            var type = currentType;
            while (type != null)
            {
                var typeInfo = type.GetTypeInfo();
                var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
                if (genericType != null && genericType == genericBaseType)
                {
                    return typeInfo;
                }

                type = type.BaseType;
            }

            return null;
        }
        
        
    }
}