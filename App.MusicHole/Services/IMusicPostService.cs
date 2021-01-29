﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.MusicHole.Services
{
    public interface IMusicPostService
    {
        Task<IEnumerable<string>> GetMusicUrlsFromTopic(string topicId);
    }
}