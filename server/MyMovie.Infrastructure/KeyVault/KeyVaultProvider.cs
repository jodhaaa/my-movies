using Microsoft.Extensions.Configuration;
using MyMovie.Application.Abstraction.KeyVault;
using MyMovie.Application.Dto.Config;

namespace MyMovie.Infrastructure.KeyVault
{
    /// <summary>
    /// IMPORTANT: This is for local demonstration only.
    /// Need to implement proper key vault solution here.
    /// </summary>
    public class KeyVaultProvider(Provider[] providers, IConfiguration config): IKeyVaultProvider
    {
        private readonly Provider[] _providers = providers;

        /// <summary>
        /// IMPORTANT: This is for local demonstration only.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> GetSecretAsync(string key)
        {
            // IMPORTANT: This is for local demonstration only.
            if (_providers.Any(p => p.VaultKey == key))
            {
                return config["ApiKey"];
                // return "<setup user secret or hard code here> ";
                // https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-9.0&tabs=windows#use-visual-studio
            }

            return string.Empty;
        }
    }
}
