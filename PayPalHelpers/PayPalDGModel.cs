using System;
using System.Collections.Generic;
using System.Linq;
using PayPal.PayPalAPIInterfaceService.Model;

namespace PayPalDGHelpers
{
    public class PayPalDGModel
    {
        public bool SupportCreditCardPayment { get; private set; }
        public CurrencyCodeType CurrencyCodeType { get; private set; }
        public decimal TaxTotalAmount { get; private set; }
        public List<PayPalDGProductItem> ProductItems { get; private set; }
        public string DoExpressCheckoutCallbackUrl { get; private set; }
        public string CancelCallbackUrl { get; private set; }
        public List<PayPalDGAdditionalCallbackParam> AdditionalCallbackParams { get; set; }

        public static PayPalDGModel Create()
        {
            return new PayPalDGModel {SupportCreditCardPayment = true};
        }

        public PayPalDGModel IncludeSupportForCreditCardPayment(bool support = true)
        {
            SupportCreditCardPayment = support;
            return this;
        }

        public PayPalDGModel WithCurrencyCode(CurrencyCodeType currencyCode)
        {
            CurrencyCodeType = currencyCode;
            return this;
        }

        public PayPalDGModel WithTaxTotalAmountOf(decimal amount)
        {
            TaxTotalAmount = amount;
            return this;
        }

        public PayPalDGModel WithProductItems(List<PayPalDGProductItem> productItems)
        {
            if((productItems == null) || (productItems.Count == 0))
            {
                throw new PayPalDGException("At one product is required.");
            }

            ProductItems = productItems;
            return this;
        }

        public PayPalDGModel WithDoExpressCheckoutCallbackUrl(string url)
        {
            if(string.IsNullOrEmpty(url))
            {
                throw new PayPalDGException("Do Express Checkout Callback Url is required.");
            }

            if (!IsValidUrl(url))
            {
                throw new PayPalDGException("Invalid Url");
            }

            DoExpressCheckoutCallbackUrl = url;
            return this;
        }

        public PayPalDGModel WithCancelCallbackUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new PayPalDGException("Cancel Callback Url is required.");
            }

            if (!IsValidUrl(url))
            {
                throw new PayPalDGException("Invalid Url");
            }

            CancelCallbackUrl = url;
            return this;
        }

        public PayPalDGModel WithAdditionalCallbackParams(List<PayPalDGAdditionalCallbackParam> @params)
        {
            AdditionalCallbackParams = @params;
            return this;
        }

        public void AssertRequiredParameters()
        {
            if (string.IsNullOrEmpty(DoExpressCheckoutCallbackUrl))
            {
                throw new PayPalDGException("Do Express Checkout Callback Url is required.");
            }

            if (string.IsNullOrEmpty(CancelCallbackUrl))
            {
                throw new PayPalDGException("Cancel Callback Url is required.");
            }

            if ((ProductItems == null) || (ProductItems.Count == 0))
            {
                throw new PayPalDGException("At least one product is required.");
            }
        }

        public decimal GetItemTotalAmount()
        {
            return ProductItems.Sum(p => p.Amount);
        }

        public decimal GetOrderTotalAmount()
        {
            return TaxTotalAmount + GetItemTotalAmount();
        }

        public string BuildAdditionalCallbackParams()
        {
            if ((AdditionalCallbackParams == null) || AdditionalCallbackParams.Count == 0)
            {
                return string.Empty;
            }

            var additionalParams = "?";

            AdditionalCallbackParams.ForEach(p => { additionalParams += string.Format("{0}={1}&", p.Name, p.Value); });

            return additionalParams.TrimEnd('&');
        }

        private bool IsValidUrl(string url)
        {
            Uri result;

            return Uri.TryCreate(url, UriKind.Absolute, out result);
        }
    }
}