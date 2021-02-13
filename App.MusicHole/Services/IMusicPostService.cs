using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.MusicHole.Services
{
    public interface IMusicPostService
    {
        Task<IEnumerable<string>> GetVideoIdsFromTopic(string topicId);
        Task<IEnumerable<string>> GetVideoIdsFromTopicByDateOffset(string topicId, DateTime offset);
    }
}