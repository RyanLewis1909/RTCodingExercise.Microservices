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

        /// <summary>
        /// The event bus connection.
        /// </summary>
        public string EventBusConnection { get; set; }

        /// <summary>
        /// The event bus username.
        /// </summary>
        public string EventBusUserName { get; set; }

        /// <summary>
        /// The event bus password.
        /// </summary>
        public string EventBusPassword { get; set; }

        public bool IsValid(out string reason)
        {
            if (string.IsNullOrWhiteSpace(WebAppBaseUrl))
            {
                reason = "WebAppBaseUrl";
                return false;
            };

            if (string.IsNullOrWhiteSpace(EventBusConnection))
            {
                reason = "EventBusConnection";
                return false;
            };

            if (string.IsNullOrWhiteSpace(EventBusUserName))
            {
                reason = "EventBusUserName";
                return false;
            };

            if (string.IsNullOrWhiteSpace(EventBusPassword))
            {
                reason = "EventBusPassword";
                return false;
            };

            reason = string.Empty;
            return true;
        }
    }
}
