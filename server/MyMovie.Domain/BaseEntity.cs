namespace MyMovie.Domain
{
    /// <summary>
    /// Base entity for DB models
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// Id for entity
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// CreatedDate for audit
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// LastModifiedDate for audit
        /// </summary>
        public DateTime? LastModifiedDate { get; set; }
    }
}
