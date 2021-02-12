using System.Threading.Tasks;
using App.MusicHole.Repositories;

namespace App.MusicHole.Services
{
    public class YoutubePlaylistService : IYoutubePlaylistService
    {
        private readonly IYoutubeRepository youtubeRepository;

        public YoutubePlaylistService(IYoutubeRepository youtubeRepository)
        {
            this.youtubeRepository = youtubeRepository;
        }

        public async Task<bool> InsertVideoToPlaylist(string videoId, string playlistId)
        {
            return await youtubeRepository.InsertVideoToPlaylist(videoId, playlistId);
        }
    }
}