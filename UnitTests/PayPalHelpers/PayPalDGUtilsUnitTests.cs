using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayPal.PayPalAPIInterfaceService.Model;
using PayPalDGHelpers;

namespace UnitTests.PayPalHelpers
{
    [TestClass]
    public class PayPalDGUtilsUnitTests
    {
        [TestMethod]
        public void ShouldConvertUSDStringToCurrencyCodeType()
        {
            var usd = PayPalDGUtils.ToCurrencyCodeType("usd");

            Assert.AreEqual(CurrencyCodeType.USD, usd);
        }

        [TestMethod]
        public void ShouldConvertEURStringToCurrencyCodeType()
        {
            var eur = PayPalDGUtils.ToCurrencyCodeType("EUR");

            Assert.AreEqual(CurrencyCodeType.EUR, eur);
        }

        [TestMethod]
        public void ShouldConvertGBPStringToCurrencyCodeType()
        {
            var gbp = PayPalDGUtils.ToCurrencyCodeType("gBp");

            Assert.AreEqual(CurrencyCodeType.GBP, gbp);
        }

        [TestMethod]
        [ExpectedException(typeof(PayPalDGException))]
        public void ShouldThrowExceptionForInvalidCurrencyISOCode()
        {
            PayPalDGUtils.ToCurrencyCodeType("abc");
        }
    }
}