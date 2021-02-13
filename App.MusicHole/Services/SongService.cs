using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Paciak;
using Microsoft.Extensions.Logging;

namespace App.MusicHole.Services
{
    public class SongService : ISongService
    {
        private readonly ILogger<SongService> logger;
        private readonly ISongRepository songRepository;

        public SongService(ILogger<SongService> logger, ISongRepository songRepository)
        {
            this.logger = logger;
            this.songRepository = songRepository;
        }

        public async Task<bool> UpsertSong(Song song)
        {
            logger.LogTrace($"Updating song {song.VideoId}");

            return await songRepository.Upsert(song);
        }

        public async Task<bool> Delete(Song song)
        {
            logger.LogTrace($"Deleting song {song.VideoId}");

            return await songRepository.Delete(song);
        }

        public async Task<IEnumerable<Song>> GetOrphanedSongs()
        {
            return await songRepository.GetOrphanedSongs();
        }
    }
}