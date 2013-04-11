using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayPal.PayPalAPIInterfaceService.Model;
using PayPalDGHelpers;

namespace UnitTests.PayPalHelpers
{
    public class PayPalDGModelUnitTests
    {
        [TestClass]
        public class TheDefaultValues
        {
            [TestMethod]
            public void ShouldSetTheSupportForCreditCardPaymentToTrue()
            {
                var model = PayPalDGModel.Create();

                Assert.IsTrue(model.SupportCreditCardPayment);
            }
        }

        [TestClass]
        public class TheIncludeSupportForCreditCardPaymentMethod
        {
            [TestMethod]
            public void ShouldBeAbleToSetToFalse()
            {
                var model = PayPalDGModel.Create()
                    .IncludeSupportForCreditCardPayment(false);

                Assert.IsFalse(model.SupportCreditCardPayment);
            }
        }

        [TestClass]
        public class TheWithCurrencyCodeMethod
        {
            [TestMethod]
            public void ShouldSetTheCurrencyCodeTypeToUSD()
            {
                var model = PayPalDGModel.Create()
                    .WithCurrencyCode(CurrencyCodeType.USD);

                Assert.AreEqual(CurrencyCodeType.USD, model.CurrencyCodeType);
            }

            [TestMethod]
            public void ShouldSetTheCurrencyCodeTypeToPHP()
            {
                var model = PayPalDGModel.Create()
                    .WithCurrencyCode(CurrencyCodeType.PHP);

                Assert.AreEqual(CurrencyCodeType.PHP, model.CurrencyCodeType);
            }
        }

        [TestClass]
        public class TheWithTaxTotalAmountOfMethod
        {
            [TestMethod]
            public void ShouldSetTheTaxTotalAmountToZero()
            {
                var model = PayPalDGModel.Create()
                    .WithTaxTotalAmountOf(0);

                Assert.AreEqual(0, model.TaxTotalAmount);
            }

            [TestMethod]
            public void ShouldSetTheTaxTotalAmountTo15()
            {
                var model = PayPalDGModel.Create()
                    .WithTaxTotalAmountOf(15);

                Assert.AreEqual(15, model.TaxTotalAmount);
            }
        }

        [TestClass]
        public class TheWithProductItemsMethod
        {
            [TestMethod]
            [ExpectedException(typeof(PayPalDGException))]
            public void ShouldThrowPayPalDGExceptionWhenPassingNullItems()
            {
                PayPalDGModel.Create().WithProductItems(null);
            }

            [TestMethod]
            [ExpectedException(typeof(PayPalDGException))]
            public void ShouldThrowPayPalDGExceptionWhenPassing0Item()
            {
                PayPalDGModel.Create().WithProductItems(new List<PayPalDGProductItem>());
            }
        }

        [TestClass]
        public class TheWithDoExpressCheckoutCallbackUrlMethod
        {
            [TestMethod]
            [ExpectedException(typeof(PayPalDGException))]
            public void ShouldThrowPayPalDGExceptionWhenNoCallbackUrlSpecified()
            {
                PayPalDGModel.Create().WithDoExpressCheckoutCallbackUrl(null);
            }

            [TestMethod]
            [ExpectedException(typeof(PayPalDGException))]
            public void ShouldThrowPayPalDGExceptionWhenPassingInvalidUrl()
            {
                PayPalDGModel.Create().WithDoExpressCheckoutCallbackUrl("/test/url");
            }

            [TestMethod]
            public void ShouldSetTheCallbackUrl()
            {
                var model = PayPalDGModel.Create().WithDoExpressCheckoutCallbackUrl("http://localhost:1234/test");

                Assert.AreEqual("http://localhost:1234/test", model.DoExpressCheckoutCallbackUrl);
            }
        }

        [TestClass]
        public class TheWithCancelCallbackUrlMethod
        {
            [TestMethod]
            [ExpectedException(typeof(PayPalDGException))]
            public void ShouldThrowPayPalDGExceptionWhenNoCallbackUrlSpecified()
            {
                PayPalDGModel.Create().WithCancelCallbackUrl(null);
            }

            [TestMethod]
            [ExpectedException(typeof(PayPalDGException))]
            public void ShouldThrowPayPalDGExceptionWhenPassingInvalidUrl()
            {
                PayPalDGModel.Create().WithCancelCallbackUrl("/test/url");
            }

            [TestMethod]
            public void ShouldSetTheCallbackUrl()
            {
                var model = PayPalDGModel.Create().WithCancelCallbackUrl("http://localhost:1234/test");

                Assert.AreEqual("http://localhost:1234/test", model.CancelCallbackUrl);
            }
        }

        [TestClass]
        public class TheComputationsMethod
        {
            [TestMethod]
            public void ShouldReturnTheTotalItemAmount()
            {
                var model = PayPalDGModel.Create()
                    .WithProductItems(new List<PayPalDGProductItem>
                                          {
                                              new PayPalDGProductItem{Amount = 15.22m},
                                              new PayPalDGProductItem{Amount = 11.01m}
                                          });

                Assert.AreEqual(26.23m, model.GetItemTotalAmount());
            }

            [TestMethod]
            public void ShouldReturnTheOrderTotalAmount()
            {
                var model = PayPalDGModel.Create()
                    .WithTaxTotalAmountOf(50)
                    .WithProductItems(new List<PayPalDGProductItem>
                                          {
                                              new PayPalDGProductItem{Amount = 15.22m},
                                              new PayPalDGProductItem{Amount = 11.01m}
                                          });

                Assert.AreEqual(76.23m, model.GetOrderTotalAmount());
            }
        }

        [TestClass]
        public class TheBuildAdditionalParamsQueryStringMethod
        {
            [TestMethod]
            public void WithOneParameter()
            {
                var model = PayPalDGModel.Create()
                    .WithAdditionalCallbackParams(new List<PayPalDGAdditionalCallbackParam>
                                                           {
                                                               new PayPalDGAdditionalCallbackParam{Name = "p1", Value = "v1"}
                                                           });

                Assert.AreEqual("?p1=v1", model.BuildAdditionalCallbackParams());
            }

            [TestMethod]
            public void WithTwoParameters()
            {
                var model = PayPalDGModel.Create()
                    .WithAdditionalCallbackParams(new List<PayPalDGAdditionalCallbackParam>
                                                           {
                                                               new PayPalDGAdditionalCallbackParam{Name = "p1", Value = "v1"},
                                                               new PayPalDGAdditionalCallbackParam{Name = "p2", Value = "v2"}
                                                           });

                Assert.AreEqual("?p1=v1&p2=v2", model.BuildAdditionalCallbackParams());
            }
        }
    }
}