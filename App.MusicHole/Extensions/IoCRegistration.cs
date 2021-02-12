using App.MusicHole.Configuration;
using App.MusicHole.Repositories;
using App.MusicHole.Services;
using Autofac;
using Common.Services;
using DAL.Configuration;
using DAL.Paciak;
using Serilog;
using Serilog.Extensions.Autofac.DependencyInjection;

namespace App.MusicHole.Extensions
{
    public static class IoCRegistration
    {
        public static IContainer RegisterIoC(this ContainerBuilder builder)
        {
            var loggerConfiguration = new LoggerConfiguration().ReadFrom.AppSettings();
            builder.RegisterSerilog(loggerConfiguration);
            
            builder.RegisterType<Startup>().AsImplementedInterfaces();

            builder.RegisterType<DbConfigurationProvider>().AsImplementedInterfaces();
            builder.RegisterType<DbProvider>().AsImplementedInterfaces();
            builder.RegisterType<YoutubeCredentialProvider>().AsImplementedInterfaces();

            builder.RegisterType<PostsRepository>().AsImplementedInterfaces();
            builder.RegisterType<SongRepository>().AsImplementedInterfaces();
            builder.RegisterType<SettingsRepository>().AsImplementedInterfaces();
            builder.RegisterType<MusicPostService>().AsImplementedInterfaces();
            builder.RegisterType<YoutubeRepository>().AsImplementedInterfaces();

            builder.RegisterType<SongService>().AsImplementedInterfaces();
            builder.RegisterType<SettingsService>().AsImplementedInterfaces();
            builder.RegisterType<YoutubeServiceProvider>().AsImplementedInterfaces();
            builder.RegisterType<YoutubePlaylistService>().AsImplementedInterfaces();

            return builder.Build();
        }
    }
}