using App.MusicHole.Services;
using Autofac;
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
            builder.RegisterType<MusicPostService>().AsImplementedInterfaces();
            builder.RegisterType<SongService>().AsImplementedInterfaces();

            return builder.Build();
        }
    }
}