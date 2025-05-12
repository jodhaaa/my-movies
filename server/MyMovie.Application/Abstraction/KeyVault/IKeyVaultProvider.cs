namespace MyMovie.Application.Abstraction.KeyVault
{
    /// <summary>
    /// Interface for key vault provider
    /// </summary>
    public interface IKeyVaultProvider
    {
        /// <summary>
        /// Get secret.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> GetSecretAsync(string key);
    }
}
