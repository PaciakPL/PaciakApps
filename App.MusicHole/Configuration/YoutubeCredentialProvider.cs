using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.YouTube.v3;

namespace App.MusicHole.Configuration
{
    public class YoutubeCredentialProvider : IYoutubeCredentialProvider
    {
        public async Task<UserCredential> GetCredential()
        {
            await using var stream = new FileStream(ConfigurationManager.AppSettings["googleSecretFile"], FileMode.Open,
                FileAccess.Read);
            return await GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                new[] {YouTubeService.Scope.Youtube},
                "user",
                CancellationToken.None
            );
        }
    }
}