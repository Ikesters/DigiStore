using System;
using System.Web.Mvc;

namespace DigiStore.ActionResults
{
    public class PaypalDGExpressCheckoutResult : ActionResult
    {
        private readonly string _paypalUrl;
        private readonly string _token;

        public PaypalDGExpressCheckoutResult(string paypalUrl, string token)
        {
            if (string.IsNullOrEmpty(paypalUrl))
            {
                throw new ArgumentNullException("paypalUrl");
            }
            
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException("token");
            }

            _paypalUrl = paypalUrl;
            _token = token;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Redirect(GenerateUrl());
        }

        private string GenerateUrl()
        {
            return string.Format("{0}?token={1}&useraction=commit", _paypalUrl, _token);
        }
    }
}