namespace PayPalDGHelpers
{
    public interface IPayPalDGService
    {
        string SetExpressCheckout(PayPalDGModel model);
        void DoExpressCheckoutPayment(string token, string payerId);
    }
}