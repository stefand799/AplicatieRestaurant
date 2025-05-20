namespace AppRestaurant.Config
{
    public class AppSettings
    {
        public decimal MenuDefaultDiscountPercentageX { get; set; }
        public decimal OrderDiscountThresholdY { get; set; }
        public int LoyaltyOrderCountZ { get; set; }
        public int LoyaltyTimeIntervalTInDays { get; set; }
        public decimal OrderGlobalDiscountPercentageW { get; set; }
        public decimal ShippingThresholdA { get; set; }
        public decimal ShippingCostB { get; set; }
        public int LowStockThresholdC { get; set; }
    }
}