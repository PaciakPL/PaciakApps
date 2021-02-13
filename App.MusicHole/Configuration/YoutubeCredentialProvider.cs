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
            await using var stream = new MemoryStream();
            await using var streamWriter = new StreamWriter(stream);

            await streamWriter.WriteAsync(ConfigurationManager.AppSettings["googleOathSecret"]);
            await streamWriter.FlushAsync();
            stream.Position = 0;

            var scopes = new[] {YouTubeService.Scope.Youtube};
            var secrets = GoogleClientSecrets.Load(stream).Secrets;
            
            return await GoogleWebAuthorizationBroker.AuthorizeAsync(
                secrets,
                scopes,
                "user",
                CancellationToken.None,
                null,
                new PromptCodeReceiver()
            );
        }
    }
}