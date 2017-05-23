using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWeb.Data;
using MyWeb.Services.Customers;
using System.Web;
using System.Web.Security;

namespace MyWeb.Services.Authentication
{
    public partial class FormsAuthenticationService : IAuthenticationService
    {
        #region Fields

        private readonly HttpContextBase _httpContext;
        private readonly ICustomerService _customerService;
        //private readonly CustomerSettings _customerSettings;
        //private readonly TimeSpan _expirationTimeSpan;

        private Customer _cachedCustomer;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="httpContext">HTTP context</param>
        /// <param name="customerService">Customer service</param>
        /// <param name="customerSettings">Customer settings</param>
        public FormsAuthenticationService(
            HttpContextBase httpContext,
            ICustomerService customerService
            //, CustomerSettings customerSettings
            )
        {
            this._httpContext = httpContext;
            this._customerService = customerService;
            //this._customerSettings = customerSettings;
            //this._expirationTimeSpan = FormsAuthentication.Timeout;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get authenticated customer
        /// </summary>
        /// <param name="ticket">Ticket</param>
        /// <returns>Customer</returns>
        protected virtual Customer GetAuthenticatedCustomerFromTicket(FormsAuthenticationTicket ticket)
        {
            //if (ticket == null)
            //    throw new ArgumentNullException("ticket");

            //var usernameOrEmail = ticket.UserData;

            //if (String.IsNullOrWhiteSpace(usernameOrEmail))
            //    return null;
            //var customer = _customerSettings.UsernamesEnabled
            //    ? _customerService.GetCustomerByUsername(usernameOrEmail)
            //    : _customerService.GetCustomerByEmail(usernameOrEmail);
            //return customer;
            throw new NotImplementedException();
        }

        #endregion

        public Customer GetAuthenticatedCustomer()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sign in
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="createPersistentCookie">A value indicating whether to create a persistent cookie</param>
        public void SignIn(Customer customer, bool createPersistentCookie)
        {
            var now = DateTime.UtcNow.ToLocalTime();

            var ticket = new FormsAuthenticationTicket(
                1 /*version*/,
                 customer.Email,
                now,
                now.Add(FormsAuthentication.Timeout),
                createPersistentCookie,
                customer.Email,
                FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            cookie.HttpOnly = true;
            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            _httpContext.Response.Cookies.Add(cookie);
            _cachedCustomer = customer;
        }

        public void SignOut()
        {
            throw new NotImplementedException();
        }
    }
}
