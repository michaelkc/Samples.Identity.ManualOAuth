namespace Seges.Samples.OAuth
{
    internal class ApplicationConfiguration
    {
        public string AuthorizeEndpoint { get; set; }
        public string TokenEndpoint { get; set; }
        public string ApiEndpoint { get; set; }
        public string RedirectUri { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
        public bool PerformTokenValidation { get; set; }
    }
}
