using System;

namespace PayPalDGHelpers
{
    [Serializable]
    public class PayPalDGException : Exception
    {
        public PayPalDGException(string message)
            : base(message)
        {
        }
    }
}