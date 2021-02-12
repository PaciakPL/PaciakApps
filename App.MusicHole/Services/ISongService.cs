using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities;

namespace App.MusicHole.Services
{
    public interface ISongService
    {
        Task<bool> UpsertSong(Song song);
        Task<bool> Delete(Song song);
        Task<IEnumerable<Song>> GetOrphanedSongs();
    }
}