namespace MyMovie.Application.Dto.Config
{
    /// <summary>
    /// Provider config
    /// </summary>
    public class Provider
    {
        /// <summary>
        /// Name of the provider
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Vault key name of the secret
        /// </summary>
        public string VaultKey { get; set; }

        /// <summary>
        /// Auth header of the http call.
        /// </summary>
        public string AuthHeader { get; set; }

        /// <summary>
        /// Movie list url.
        /// </summary>
        public string MovieListUrl { get; set; }
        
        /// <summary>
        /// Movie details url.
        /// </summary>
        public string MovieUrl { get; set; }
    }
}