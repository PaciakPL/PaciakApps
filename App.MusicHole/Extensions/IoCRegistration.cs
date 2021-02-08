using App.MusicHole.Services;
using Autofac;
using Common.Services;
using DAL.Configuration;
using DAL.Paciak;

namespace App.MusicHole.Extensions
{
    public static class IoCRegistration
    {
        public static IContainer RegisterIoC(this ContainerBuilder builder)
        {
            builder.RegisterType<Startup>().AsImplementedInterfaces();

            builder.RegisterType<DbConfigurationProvider>().AsImplementedInterfaces();
            builder.RegisterType<DbProvider>().AsImplementedInterfaces();
            builder.RegisterType<PostsRepository>().AsImplementedInterfaces();
            builder.RegisterType<SongRepository>().AsImplementedInterfaces();
            builder.RegisterType<SettingsRepository>().AsImplementedInterfaces();
            builder.RegisterType<MusicPostService>().AsImplementedInterfaces();
            builder.RegisterType<SongService>().AsImplementedInterfaces();
            builder.RegisterType<SettingsService>().AsImplementedInterfaces();

            return builder.Build();
        }
    }
}