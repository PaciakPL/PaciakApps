using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Configuration;
using DAL.Entities;

namespace DAL.Paciak
{
    public interface ISongRepository
    {
        Task<bool> Exists(Song song);
        Task AddSong(Song song);
        Task<bool> Upsert(Song song);
        Task<bool> Delete(Song song);
        Task<IEnumerable<Song>> GetOrphanedSongs();
    }
}