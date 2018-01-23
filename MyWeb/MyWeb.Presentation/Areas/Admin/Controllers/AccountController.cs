using MyWeb.Data;
using MyWeb.Presentation.Areas.Admin.Models;
using MyWeb.Presentation.Factories;
using MyWeb.Services.Authentication;
using MyWeb.Services.Customers;
using System;
using System.Web.Mvc;
using System.Web.Security;

namespace MyWeb.Presentation.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        private readonly CustomerSettings _customerSettings;
        private readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly ICustomerService _customerService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICustomerModelFactory _customerModelFactory;

        public AccountController(
            CustomerSettings customerSettings,
            ICustomerRegistrationService customerRegistrationService,
            ICustomerService customerService,
            IAuthenticationService authenticationService,
            ICustomerModelFactory customerModelFactory
            )
        {
            _customerSettings = customerSettings;
            _customerRegistrationService = customerRegistrationService;
            _customerService = customerService;
            _authenticationService = authenticationService;
            _customerModelFactory = customerModelFactory;
        }

        // GET: Admin/Account
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Login(LoginViewModel model, string returnUrl/*, bool captchaValid*/)
        {
            if (ModelState.IsValid)
            {
                //if (_customerSettings.UsernamesEnabled && model.Username != null)
                //{
                //    model.Username = model.Username.Trim();
                //}
                var loginResult =
                    _customerRegistrationService.ValidateCustomer(
                        //_customerSettings.UsernamesEnabled ? model.Username :
                        model.Username, model.Password);
                switch (loginResult)
                {
                    case CustomerLoginResults.Successful:
                        {
                            var customer =
                                //_customerSettings.UsernamesEnabled
                                //? _customerService.GetCustomerByUsername(model.Username):
                                _customerService.GetCustomerByEmail(model.Username);

                            //sign in new customer
                            _authenticationService.SignIn(customer, model.RememberMe);
                            //activity log
                            //_customerActivityService.InsertActivity(customer, "PublicStore.Login", _localizationService.GetResource("ActivityLog.PublicStore.Login"));

                            if (String.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                                //return RedirectToRoute("/Admin/News/List");
                                return Redirect("/Admin/News/List");

                            return Redirect(returnUrl);
                        }
                    case CustomerLoginResults.CustomerNotExist:
                        ModelState.AddModelError("", "CustomerNotExist");
                        break;
                    case CustomerLoginResults.Deleted:
                        ModelState.AddModelError("", "Deleted");
                        break;
                    case CustomerLoginResults.NotActive:
                        ModelState.AddModelError("", "NotActive");
                        break;
                    case CustomerLoginResults.NotRegistered:
                        ModelState.AddModelError("", "NotRegistered");
                        break;
                    case CustomerLoginResults.LockedOut:
                        ModelState.AddModelError("", "LockedOut");
                        break;
                    case CustomerLoginResults.WrongPassword:
                    default:
                        ModelState.AddModelError("", "WrongPassword");
                        break;
                }
            }

            //If we got this far, something failed, redisplay form
            model = _customerModelFactory.PrepareLoginModel(model.CheckoutAsGuest);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            //_cachedCustomer = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}