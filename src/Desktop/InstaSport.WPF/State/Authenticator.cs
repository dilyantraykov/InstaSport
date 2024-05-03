using InstaSport.Data.Models;
using InstaSport.Services.Data;
using Prism.Mvvm;
using System;

namespace InstaSport.WPF.State
{
    public class Authenticator : BindableBase, IAuthenticator
    {
        private readonly IAuthenticationService authenticationService;
        private User currentUser;

        public event EventHandler? CurrentUserChanged;

        public Authenticator(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        public User CurrentUser
        {
            get { return currentUser; }
            private set
            {
                this.SetProperty(ref currentUser, value);
                this.RaisePropertyChanged(nameof(IsLoggedIn));
                this.OnCurrentUserChanged();
            }
        }

        private void OnCurrentUserChanged()
        {
            this.CurrentUserChanged?.Invoke(this, new EventArgs());
        }

        public bool IsLoggedIn => this.CurrentUser != null;

        public bool LogIn(string username, string password)
        {
            try
            {
                this.CurrentUser = authenticationService.Login(username, password);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void LogOut()
        {
            this.CurrentUser = null;
        }

        public void Register(string username, string email, string firstName, string lastName, string password, string confirmPassword)
        {
            authenticationService.Register(username, email, firstName, lastName, password, confirmPassword);
            this.CurrentUser = authenticationService.Login(username, password);
        }
    }
}
