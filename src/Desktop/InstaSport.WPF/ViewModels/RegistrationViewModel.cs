using InstaSport.Services.Data.Exceptions;
using InstaSport.Services.Data.Localization;
using InstaSport.WPF.Models;
using InstaSport.WPF.State;
using InstaSport.WPF.Views;
using Prism.Regions;
using System;
using System.Runtime.InteropServices;
using System.Security;
using Telerik.Windows.Controls;

namespace InstaSport.WPF.ViewModels
{
    public class RegistrationViewModel : ValidatableBindableBase
    {
        private readonly IRegionManager regionManager;
        private readonly IAuthenticator authenticator;

        private string username;
        private string firstName;
        private string lastName;
        private string email;
        private SecureString password = new SecureString();
        private SecureString confirmPassword = new SecureString();

        public string UserName
        {
            get { return this.username; }
            set
            {
                this.ValidateUserName(value);
                this.SetProperty(ref this.username, value);
            }
        }

        public string FirstName
        {
            get { return this.firstName; }
            set
            {
                this.SetProperty(ref this.firstName, value);
            }
        }

        public string LastName
        {
            get { return this.lastName; }
            set
            {
                this.SetProperty(ref this.lastName, value);
            }
        }

        public string Email
        {
            get { return this.email; }
            set
            {
                if (this.email != value)
                {
                    ClearErrors(nameof(Email));
                    ValidateEmail(value);
                    this.SetProperty(ref this.email, value);
                }
            }
        }

        public SecureString Password
        {
            get { return this.password; }
            set
            {
                this.ValidatePassword(value);
                this.SetProperty(ref this.password, value);
            }
        }

        public SecureString ConfirmPassword
        {
            get { return this.confirmPassword; }
            set
            {
                this.ValidateConfirmPassword(value);
                this.SetProperty(ref this.confirmPassword, value);
            }
        }

        public DelegateCommand RegisterCommand { get; }

        public RegistrationViewModel(IRegionManager regionManager, IAuthenticator authenticator)
        {
            this.regionManager = regionManager;
            this.authenticator = authenticator;
            this.RegisterCommand = new DelegateCommand(OnRegistration);
        }

        private void OnRegistration(object obj)
        {
            try
            {
                if (this.HasErrors)
                {
                    return;
                }

                string password = ConvertToUnsecureString(this.Password);
                string confirmPassword = ConvertToUnsecureString(this.ConfirmPassword);

                this.authenticator.Register(this.UserName, this.Email, this.FirstName, this.LastName, password, confirmPassword);
                this.regionManager.RequestNavigate("MainRegion", nameof(GamesView));
            }
            catch (InvalidPropertyException ex)
            {
                this.AddError(ex.Property, ex.Message);
            }
        }

        private void ValidateUserName(string name)
        {
            ClearErrors(nameof(UserName));
            if (string.IsNullOrWhiteSpace(name))
                AddError(nameof(UserName), Strings.UsernameEmptyError);
            if (string.Equals(name, "Admin", StringComparison.OrdinalIgnoreCase))
                AddError(nameof(UserName), Strings.UsernameInvalidError);
            if (name == null || name?.Length <= 3)
                AddError(nameof(UserName), Strings.UsernameLengthError);
        }

        private void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                AddError(nameof(Email), Strings.EmailEmptyError);
            }
            else if (!IsValidEmail(email))
            {
                AddError(nameof(Email), Strings.EmailInvalidError);
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void ValidatePassword(SecureString password)
        {
            ClearErrors(nameof(ConfirmPassword));
            if (!SecureStringEqual(this.ConfirmPassword, password))
            {
                AddError(nameof(ConfirmPassword), Strings.PasswordMismatchError);
            }
        }

        private void ValidateConfirmPassword(SecureString confirmPassword)
        {
            ClearErrors(nameof(ConfirmPassword));
            if (!SecureStringEqual(this.Password, confirmPassword))
            {
                AddError(nameof(ConfirmPassword), Strings.PasswordMismatchError);
            }
        }

        private bool SecureStringEqual(SecureString ss1, SecureString ss2)
        {
            if (ss1.Length != ss2.Length)
                return false;

            IntPtr bstr1 = IntPtr.Zero;
            IntPtr bstr2 = IntPtr.Zero;

            try
            {
                bstr1 = Marshal.SecureStringToBSTR(ss1);
                bstr2 = Marshal.SecureStringToBSTR(ss2);

                for (int i = 0; i < ss1.Length; i++)
                {
                    byte b1 = Marshal.ReadByte(bstr1, i);
                    byte b2 = Marshal.ReadByte(bstr2, i);

                    if (b1 != b2)
                        return false;
                }

                return true;
            }
            finally
            {
                if (bstr1 != IntPtr.Zero)
                    Marshal.ZeroFreeBSTR(bstr1);

                if (bstr2 != IntPtr.Zero)
                    Marshal.ZeroFreeBSTR(bstr2);
            }
        }

        private string ConvertToUnsecureString(SecureString secureString)
        {
            if (secureString == null)
                throw new ArgumentNullException(nameof(secureString));

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}
