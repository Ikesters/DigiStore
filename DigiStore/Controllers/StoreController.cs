using System.Web.Mvc;
using PayPalDGHelpers;

namespace DigiStore.Controllers
{
    public class StoreController : Controller
    {
        private readonly IPayPalDGService _payPalDGService;

        public StoreController()
        {
            _payPalDGService = new PayPalDGService();
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
