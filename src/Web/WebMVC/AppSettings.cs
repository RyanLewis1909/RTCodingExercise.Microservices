using System.ComponentModel.DataAnnotations;

namespace WebMVC
{
    public class AppSettings
    {
        /// <summary>
        /// The catalog api base url.
        /// </summary>
        public string CatalogBaseUrl { get; set; }

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
            if (string.IsNullOrWhiteSpace(CatalogBaseUrl))
            {
                reason = "CatalogBaseUrl";
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
