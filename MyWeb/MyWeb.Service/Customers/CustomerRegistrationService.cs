using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWeb.Data;
using MyWeb.Services.Security;

namespace MyWeb.Services.Customers
{
    public partial class CustomerRegistrationService : ICustomerRegistrationService
    {
        #region Fields

        private readonly ICustomerService _customerService;
        //private readonly CustomerSettings _customerSettings;
        private readonly IEncryptionService _encryptionService;

        #endregion

        #region Ctors

        public CustomerRegistrationService(
            ICustomerService customerService,
            //, CustomerSettings customerSettings
            IEncryptionService encryptionService
            )
        {
            _customerService = customerService;
            //_customerSettings = customerSettings;
            _encryptionService = encryptionService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Check whether the entered password matches with saved one
        /// </summary>
        /// <param name="customerPassword">Customer password</param>
        /// <param name="enteredPassword">The entered password</param>
        /// <returns>True if passwords match; otherwise false</returns>
        protected bool PasswordsMatch(CustomerPassword customerPassword, string enteredPassword)
        {
            if (customerPassword == null || string.IsNullOrEmpty(enteredPassword))
                return false;

            var savedPassword = string.Empty;
            //switch (customerPassword.PasswordFormat)
            //{
            //    case PasswordFormat.Clear:
            //        savedPassword = enteredPassword;
            //        break;
            //    case PasswordFormat.Encrypted:
            //        //savedPassword = _encryptionService.EncryptText(enteredPassword);
            //        break;
            //    case PasswordFormat.Hashed:
            //        //savedPassword = _encryptionService.CreatePasswordHash(enteredPassword, customerPassword.PasswordSalt, _customerSettings.HashedPasswordFormat);
            //        break;
            //}

            savedPassword = _encryptionService.CreatePasswordHash(enteredPassword, customerPassword.PasswordSalt, "SHA1");

            return customerPassword.Password.Equals(savedPassword);
        }

        #endregion

        public ChangePasswordResult ChangePassword(ChangePasswordRequest request)
        {
            throw new NotImplementedException();
        }

        public CustomerRegistrationResult RegisterCustomer(CustomerRegistrationRequest request)
        {
            throw new NotImplementedException();
        }

        public void SetEmail(Customer customer, string newEmail, bool requireValidation)
        {
            throw new NotImplementedException();
        }

        public void SetUsername(Customer customer, string newUsername)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Validate customer
        /// </summary>
        /// <param name="usernameOrEmail">Username or email</param>
        /// <param name="password">Password</param>
        /// <returns>Result</returns>
        public virtual CustomerLoginResults ValidateCustomer(string usernameOrEmail, string password)
        {
            var customer =
                //_customerSettings.UsernamesEnabled ?
                //_customerService.GetCustomerByUsername(usernameOrEmail) :
                _customerService.GetCustomerByEmail(usernameOrEmail);

            if (customer == null)
                return CustomerLoginResults.CustomerNotExist;
            if (customer.Deleted)
                return CustomerLoginResults.Deleted;
            if (!customer.Active)
                return CustomerLoginResults.NotActive;
            //only registered can login
            if (!customer.IsRegistered())
                return CustomerLoginResults.NotRegistered;
            //check whether a customer is locked out
            if (customer.CannotLoginUntilDateUtc.HasValue && customer.CannotLoginUntilDateUtc.Value > DateTime.UtcNow)
                return CustomerLoginResults.LockedOut;

            if (!PasswordsMatch(_customerService.GetCurrentPassword(customer.Id), password))
            {
                //wrong password
                customer.FailedLoginAttempts++;
                //if (_customerSettings.FailedPasswordAllowedAttempts > 0 &&
                //    customer.FailedLoginAttempts >= _customerSettings.FailedPasswordAllowedAttempts)
                //{
                //    //lock out
                //    customer.CannotLoginUntilDateUtc = DateTime.UtcNow.AddMinutes(_customerSettings.FailedPasswordLockoutMinutes);
                //    //reset the counter
                //    customer.FailedLoginAttempts = 0;
                //}
                _customerService.UpdateCustomer(customer);

                return CustomerLoginResults.WrongPassword;
            }

            //update login details
            customer.FailedLoginAttempts = 0;
            customer.CannotLoginUntilDateUtc = null;
            customer.RequireReLogin = false;
            customer.LastLoginDateUtc = DateTime.UtcNow;
            _customerService.UpdateCustomer(customer);

            return CustomerLoginResults.Successful;
        }
    }
}
