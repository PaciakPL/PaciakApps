using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities;

namespace App.ZBP.Services
{
    public interface IEventsService
    {
        Task<IEnumerable<Event>> GetEventsFromTopic(string topicId);
    }
}