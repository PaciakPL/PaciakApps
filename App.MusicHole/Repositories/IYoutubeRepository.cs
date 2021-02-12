using System.Threading.Tasks;

namespace App.MusicHole.Repositories
{
    public interface IYoutubeRepository
    {
        Task<bool> InsertVideoToPlaylist(string videoId, string playlistId);
    }
}