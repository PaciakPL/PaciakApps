using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Paciak;

namespace App.MusicHole.Services
{
    public class SongService : ISongService
    {
        private readonly ISongRepository songRepository;

        public SongService(ISongRepository songRepository)
        {
            this.songRepository = songRepository;
        }

        public async Task<bool> UpsertSong(Song song)
        {
            return await songRepository.Upsert(song);
        }

        public async Task<bool> Delete(Song song)
        {
            return await songRepository.Delete(song);
        }

        public async Task<IEnumerable<Song>> GetOrphanedSongs()
        {
            return await songRepository.GetOrphanedSongs();
        }
    }
}