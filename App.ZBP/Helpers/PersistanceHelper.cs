using System.Collections.Generic;
using System.Linq;
using DAL.Entities;
using static System.String;

namespace App.ZBP.Helpers
{
    public static class PersistanceHelper
    {
        public static string GetCsvFromResults(IEnumerable<Event> events)
        {
            return Join("\r\n", events.Select(x => $"{x.Date};{x.User};{x.Steps};{x.NewValue};{x.LastValue};{x.Ok};{x.Diff}").ToArray());;
        }

        public static string GetUserResultsCsv(IEnumerable<Event> events)
        {
            return Join("\r\n", events.Select(x => $"{x.User};{x.Steps}"));
        }
    }
}