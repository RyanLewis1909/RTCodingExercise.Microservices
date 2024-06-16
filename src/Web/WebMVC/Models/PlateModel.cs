using System.ComponentModel.DataAnnotations;

namespace WebMVC.Models
{
    public class PlateModel
    {
        public Guid Id { get; set; }

        [Required]
        public string? Registration { get; set; }

        private decimal _purchasePrice { get; set; }

        [Required]
        public decimal PurchasePrice
        {
            get { return _purchasePrice; }
            set
            {
                if (PromoCodeDiscount)
                {
                    var amount = value - PromoAmount;
                    if (amount < 0)
                    {
                        _purchasePrice = value;
                    }
                    else
                    {
                        _purchasePrice = amount;
                    }
                }
                else if (PromoCodePercentOff)
                {
                    var amount = value - Math.Round(value * PromoAmount, 2);
                    if (amount < 0)
                    {
                        _purchasePrice = value;
                    }
                    else
                    {
                        _purchasePrice = amount;
                    }
                }
                else
                {
                    _purchasePrice = value;
                }
            }
        }

        public decimal SalePrice => Math.Round(PurchasePrice * 1.2m, 2);

        [Required]
        public bool IsReserved { get; set; }

        public bool OriginalIsReserved { get; set; }

        [Required]
        public bool IsSold { get; set; }

        public string Status => IsSold
                                    ? "Sold"
                                    : IsReserved
                                        ? "Reserved"
                                        : "For Sale";
        public string? PromoCode { get; set; }
        public bool PromoCodeDiscount => !string.IsNullOrEmpty(PromoCode) && PromoCode.Equals("DISCOUNT", StringComparison.InvariantCultureIgnoreCase);
        public bool PromoCodePercentOff => !string.IsNullOrEmpty(PromoCode) && PromoCode.Equals("PERCENTOFF", StringComparison.InvariantCultureIgnoreCase);
        public decimal PromoAmount => PromoCodeDiscount ? 25m : 0.15m;
    }
}
