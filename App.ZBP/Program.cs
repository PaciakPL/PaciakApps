using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using App.ZBP.Helpers;
using App.ZBP.Services;
using DAL.Configuration;
using DAL.Paciak;

namespace App.ZBP
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var from = Convert.ToDateTime(ConfigurationManager.AppSettings["from"]);
            var to = Convert.ToDateTime(ConfigurationManager.AppSettings["to"]);

            var configurationProvider = new DbConfigurationProvider();
            var dbProvider = new DbProvider(configurationProvider);
            var postsRepository = new PostsRepository(dbProvider);
            var usersRepository = new UserRepository(dbProvider);
            var eventService = new EventService(postsRepository, usersRepository);

            var events = await eventService.GetEventsFromTopic(ConfigurationManager.AppSettings["eventTopicId"]);
            var qualifiedEVents = events.Select(e => e).Where(e => PostEventHelpers.EventQualifies(e, from, to));
            
            Console.WriteLine($"There are {qualifiedEVents.Count()} step posts with sum of {qualifiedEVents.Select(p => p.Steps).Sum()} steps, and {qualifiedEVents.Select(p => p.Diff).Sum()} error steps");
            Console.WriteLine($"The event took {to.Subtract(from).TotalDays} days to finish");
            
            var csv = PersistanceHelper.GetCsvFromResults(qualifiedEVents);
            var usersCsv = PersistanceHelper.GetUserResultsCsv(PostEventHelpers.GroupEventsByUid(qualifiedEVents));

            var outputFile = ConfigurationManager.AppSettings["outputFile"];
            var outputUsersFile = ConfigurationManager.AppSettings["outputUsersFile"];
            await File.WriteAllTextAsync(outputFile, csv);
            await File.WriteAllTextAsync(outputUsersFile, usersCsv);
            Console.WriteLine($"Output saved to ${outputFile} and {outputUsersFile}");
        }
    }
}