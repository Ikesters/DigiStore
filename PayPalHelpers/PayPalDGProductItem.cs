namespace PayPalDGHelpers
{
    public class PayPalDGProductItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public int Quantity { get; set; }

        public PayPalDGProductItem()
        {
            Quantity = 1;
        }
    }
}