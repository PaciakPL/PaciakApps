using System.Threading.Tasks;

namespace App.MusicHole.Services
{
    public interface IYoutubePlaylistService
    {
        Task<bool> InsertVideoToPlaylist(string videoId, string playlistId);
    }
}