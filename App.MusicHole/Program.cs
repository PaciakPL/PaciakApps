using System;
using System.Threading.Tasks;
using App.MusicHole.Extensions;
using Autofac;

namespace App.MusicHole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ContainerBuilder();
            await builder.RegisterIoC().Resolve<IStartup>().Run();
        }
    }
}