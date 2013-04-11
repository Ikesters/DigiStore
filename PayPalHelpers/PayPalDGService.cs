using System;
using System.Collections.Generic;
using System.Linq;
using PayPal.PayPalAPIInterfaceService;
using PayPal.PayPalAPIInterfaceService.Model;

namespace PayPalDGHelpers
{
    public class PayPalDGService : IPayPalDGService
    {
        private PayPalDGModel _model;

        public string SetExpressCheckout(PayPalDGModel model)
        {
            if(model == null)
            {
                throw new ArgumentNullException("model");
            }

            _model.AssertRequiredParameters();
            _model = model;

            var paymentDetails = new PaymentDetailsType();

            AddProductsToDetails(paymentDetails);

            paymentDetails.PaymentAction = PaymentActionCodeType.SALE;
            paymentDetails.ItemTotal = new BasicAmountType(_model.CurrencyCodeType, _model.GetItemTotalAmount().ToString());
            paymentDetails.OrderTotal = new BasicAmountType(_model.CurrencyCodeType, _model.GetOrderTotalAmount().ToString());
            paymentDetails.TaxTotal = new BasicAmountType(_model.CurrencyCodeType, _model.TaxTotalAmount.ToString());
            
            var ecDetails = CheckoutRequestDetails();
            ecDetails.PaymentDetails.Add(paymentDetails);

            if (_model.SupportCreditCardPayment)
            {
                ecDetails.SolutionType = SolutionTypeType.SOLE;
            }

            var request = new SetExpressCheckoutRequestType { SetExpressCheckoutRequestDetails = ecDetails };
            var checkoutReq = new SetExpressCheckoutReq { SetExpressCheckoutRequest = request };

            var service = new PayPalAPIInterfaceServiceService();
            var setECResponse = service.SetExpressCheckout(checkoutReq);

            var ack = setECResponse.Ack.HasValue ? setECResponse.Ack.Value : AckCodeType.FAILURE;
            AssertCheckoutResponse(ack, setECResponse.Errors);
            
            return setECResponse.Token;
        }

        public void DoExpressCheckoutPayment(string token, string payerId)
        {
            var service = new PayPalAPIInterfaceServiceService();
            var getECWrapper = new GetExpressCheckoutDetailsReq { GetExpressCheckoutDetailsRequest = new GetExpressCheckoutDetailsRequestType(token) };

            var request = new DoExpressCheckoutPaymentRequestType();
            var requestDetails = new DoExpressCheckoutPaymentRequestDetailsType();
            request.DoExpressCheckoutPaymentRequestDetails = requestDetails;

            var getECResponse = service.GetExpressCheckoutDetails(getECWrapper);
            requestDetails.PaymentDetails = getECResponse.GetExpressCheckoutDetailsResponseDetails.PaymentDetails;
            requestDetails.Token = token;
            requestDetails.PayerID = payerId;
            requestDetails.PaymentAction = PaymentActionCodeType.SALE;

            var wrapper = new DoExpressCheckoutPaymentReq { DoExpressCheckoutPaymentRequest = request };

            var doECResponse = service.DoExpressCheckoutPayment(wrapper);

            var ack = doECResponse.Ack.HasValue ? doECResponse.Ack.Value : AckCodeType.FAILURE;
            AssertCheckoutResponse(ack, doECResponse.Errors);
        }

        private void AddProductsToDetails(PaymentDetailsType detailsType)
        {
            _model.ProductItems.ForEach(p =>
            {
                var assignmentDetails = new PaymentDetailsItemType
                {
                    Name = p.Title,
                    Description = p.Description,
                    Amount = new BasicAmountType(_model.CurrencyCodeType, p.Amount.ToString()),
                    Quantity = 1,
                    ItemCategory = ItemCategoryType.DIGITAL
                };

                detailsType.PaymentDetailsItem.Add(assignmentDetails);
            });
        }

        private SetExpressCheckoutRequestDetailsType CheckoutRequestDetails()
        {
            return new SetExpressCheckoutRequestDetailsType
            {
                ReturnURL = _model.DoExpressCheckoutCallbackUrl + _model.BuildAdditionalCallbackParams(),
                CancelURL = _model.CancelCallbackUrl
            };
        }

        private void AssertCheckoutResponse(AckCodeType codeType, List<ErrorType> errorTypes)
        {
            if (codeType.Equals(AckCodeType.FAILURE) || (errorTypes != null && errorTypes.Count > 0))
            {
                var errMsg = string.Join(",", errorTypes.Select(e => e.ShortMessage + ":" + e.ErrorCode));

                throw new PayPalDGException(string.Format("An error occurred while processing payment to PayPal. (Error Details: {0})", errMsg));
            }
        }
    }
}