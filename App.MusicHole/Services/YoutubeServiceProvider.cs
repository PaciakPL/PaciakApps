using System.Threading.Tasks;
using App.MusicHole.Configuration;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;

namespace App.MusicHole.Services
{
    public class YoutubeServiceProvider : IYoutubeServiceProvider
    {
        private readonly IYoutubeCredentialProvider credentialProvider;

        public YoutubeServiceProvider(IYoutubeCredentialProvider credentialProvider)
        {
            this.credentialProvider = credentialProvider;
        }

        public async Task<YouTubeService> GetService()
        {
            return new(new BaseClientService.Initializer()
            {
                HttpClientInitializer = await credentialProvider.GetCredential(),
                ApplicationName = this.GetType().ToString()
            });
        }
    }
}