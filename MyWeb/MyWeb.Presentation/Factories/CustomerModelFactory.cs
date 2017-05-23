using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyWeb.Presentation.Areas.Admin.Models;
using MyWeb.Data;

namespace MyWeb.Presentation.Factories
{
    public partial class CustomerModelFactory : ICustomerModelFactory
    {
        private readonly CustomerSettings _customerSettings;

        public CustomerModelFactory(CustomerSettings customerSettings)
        {
            this._customerSettings = customerSettings;
        }

        public LoginViewModel PrepareLoginModel(bool? checkoutAsGuest)
        {
            var model = new LoginViewModel();
            model.UsernamesEnabled = _customerSettings.UsernamesEnabled;
            model.CheckoutAsGuest = checkoutAsGuest.GetValueOrDefault();
            //model.DisplayCaptcha = _captchaSettings.Enabled && _captchaSettings.ShowOnLoginPage;
            return model;
        }
    }
}