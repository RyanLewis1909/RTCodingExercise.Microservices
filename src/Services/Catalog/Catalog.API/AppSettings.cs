namespace Catalog.API
{
    public class AppSettings
    {
        /// <summary>
        /// The page split for plates.
        /// </summary>
        public int? PlatePageSplit { get; set; }

        /// <summary>
        /// The web app base url.
        /// </summary>
        public string WebAppBaseUrl { get; set; }

        public bool IsValid(out string reason)
        {
            if (string.IsNullOrWhiteSpace(WebAppBaseUrl))
            {
                reason = "WebAppBaseUrl";
                return false;
            };

            reason = string.Empty;
            return true;
        }
    }
}
