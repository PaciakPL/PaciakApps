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
            if (await songRepository.Exists(song))
            {
                return false;
            }
            
            await songRepository.AddSong(song);

            return true;
        }

        public IEnumerable<Song> GetOrphanedSongs()
        {
            throw new System.NotImplementedException();
        }
    }
}