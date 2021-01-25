using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZdarteButyPaciaka.Entities;
using ZdrateButyPaciaka.Extensions;
using static System.String;

namespace ZdrateButyPaciaka.Helpers
{
    public static class PostEventHelpers
    {
        private static readonly Regex _lastValueRegex = new Regex(@"([0-9\s]+)(−|-)", RegexOptions.Multiline);
        private static readonly Regex _newValueRegex = new Regex(@"=([\-0-9\s]+)", RegexOptions.Multiline);
        private static readonly Regex _stepRegex = new Regex(@"((−|-)\s?([0-9\s]+))", RegexOptions.Multiline);

        public static (int, int) GetGlobalEventValues(string data)
        {
            var lastValue = 0;
            var newValue = 0;

            var lastValueMatches = _lastValueRegex.Matches(data ?? Empty);
            var newValueMatches = _newValueRegex.Matches(data ?? Empty);

            if (lastValueMatches.Count > 0)
            {
                Int32.TryParse(lastValueMatches[0]?.Value.JustNumbers(), out lastValue);
            }

            if (newValueMatches.Count > 0)
            {
                Int32.TryParse(newValueMatches[0]?.Value.JustNumbers(), out newValue);
            }

            return (lastValue, newValue);
        }

        public static MatchCollection GetStepMatchesFromPostContent(string content)
        {
            var steps = _stepRegex.Matches(content);
            return steps;
        }
        
        public static bool EventQualifies(Event e, DateTime from, DateTime to)
        {
            return e.Date >= from.Date && e.Date <= to.Date && e.Steps > 0;
        }

        public static IEnumerable<Event> GroupEventsByUid(IEnumerable<Event> events)
        {
            return events.Select(e => e).GroupBy(e => e.User).Select(x => new Event
            {
                Date = null,
                Diff = 0,
                LastValue = 0,
                NewValue = 0,
                Ok = true,
                User = x.Key,
                Steps = x.Select(p => p.Steps).Sum()
            }).ToList();
        }
    }
}