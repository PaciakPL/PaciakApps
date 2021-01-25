using System.Collections.Generic;
using System.Threading.Tasks;
using ZdarteButyPaciaka.Entities;

namespace ZdrateButyPaciaka.Services
{
    public interface IEventsService
    {
        Task<IEnumerable<Event>> GetEventsFromTopic(string topicId);
    }
}