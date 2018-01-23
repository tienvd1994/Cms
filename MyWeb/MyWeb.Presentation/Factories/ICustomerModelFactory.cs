using MyWeb.Presentation.Areas.Admin.Models;

namespace MyWeb.Presentation.Factories
{
    public partial interface ICustomerModelFactory
    {
        LoginViewModel PrepareLoginModel(bool? checkoutAsGuest);
    }
}