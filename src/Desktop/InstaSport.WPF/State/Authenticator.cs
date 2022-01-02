using InstaSport.Data.Models;
using InstaSport.Services.Data;
using System;

namespace InstaSport.WPF.State
{
    public class Authenticator : IAuthenticator
    {
        private readonly IAuthenticationService authenticationService;

        public Authenticator(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        public User CurrentUser { get; private set; }

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

        public void Register(string username, string email, string password, string confirmPassword)
        {
            authenticationService.Register(username, email, password, confirmPassword);
        }
    }
}
