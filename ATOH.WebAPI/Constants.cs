namespace ATOH.WebAPI
{
    /// <summary>
    /// Constants for Identity Server.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Audience.
        /// </summary>
        public const string Audience = "https://localhost:7021/";

        /// <summary>
        /// Issuer.
        /// </summary>
        public const string Issuer = Audience;

        /// <summary>
        /// User secret.
        /// </summary>
        public const string Secret = "some_random_text_for_secret_in_web_api";
    }
}
