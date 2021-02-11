using System.Threading.Tasks;
using App.MusicHole.Configuration;
using App.MusicHole.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace App.MusicHole.Repositories
{
    public class YoutubeRepository : IYoutubeRepository
    {
        private readonly IYoutubeServiceProvider youtubeServiceProvider;
        private YouTubeService youTubeService;

        public YoutubeRepository(IYoutubeServiceProvider youtubeServiceProvider)
        {
            this.youtubeServiceProvider = youtubeServiceProvider;
        }

        public async Task<bool> InsertVideoToPlaylist(string videoId, string playlistId)
        {
            youTubeService ??= await youtubeServiceProvider.GetService();

            var playlistItem = new PlaylistItem
            {
                Snippet = new PlaylistItemSnippet
                {
                    PlaylistId = playlistId,
                    ResourceId = new ResourceId {Kind = "youtube#video", VideoId = videoId}
                }
            };

            var request = youTubeService.PlaylistItems.Insert(playlistItem, "snippet");
            var response = await request.ExecuteAsync();

            return response != null;
        }
    }
}