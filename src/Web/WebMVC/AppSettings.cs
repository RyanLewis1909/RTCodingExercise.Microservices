using System.ComponentModel.DataAnnotations;

namespace WebMVC
{
    public class AppSettings
    {
        /// <summary>
        /// The catalog api base url.
        /// </summary>
        public string CatalogBaseUrl { get; set; }

        public bool IsValid(out string reason)
        {
            if (string.IsNullOrWhiteSpace(CatalogBaseUrl))
            {
                reason = "CatalogBaseUrl";
                return false;
            };

            reason = string.Empty;
            return true;
        }
    }
}
