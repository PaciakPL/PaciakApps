using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;

namespace App.MusicHole.Configuration
{
    public interface IYoutubeCredentialProvider
    {
        Task<UserCredential> GetCredential();
    }
}