using System.Threading.Tasks;
using Google.Apis.YouTube.v3;

namespace App.MusicHole.Services
{
    public interface IYoutubeServiceProvider
    {
        Task<YouTubeService> GetService();
    }
}