using MyWeb.Presentation.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWeb.Presentation.Factories
{
    public partial interface ICustomerModelFactory
    {
        LoginViewModel PrepareLoginModel(bool? checkoutAsGuest);
    }
}