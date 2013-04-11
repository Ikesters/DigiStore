using System;
using PayPal.PayPalAPIInterfaceService.Model;

namespace PayPalDGHelpers
{
    public class PayPalDGUtils
    {
        public static CurrencyCodeType ToCurrencyCodeType(string currencyCode)
        {
            if (string.IsNullOrEmpty(currencyCode))
            {
                throw new ArgumentNullException("currencyCode");
            }

            try
            {
                return (CurrencyCodeType)Enum.Parse(typeof(CurrencyCodeType), currencyCode.ToUpper());
            }
            catch
            {
                throw new PayPalDGException("Invalid Currency ISO Code");
            }
        }
         
    }
}