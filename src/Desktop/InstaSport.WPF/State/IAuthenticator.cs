using InstaSport.Data.Models;
using System;

namespace InstaSport.WPF.State
{
    public interface IAuthenticator
    {
        User CurrentUser { get; }

        bool IsLoggedIn { get; }

        event EventHandler? CurrentUserChanged;

        bool LogIn(string username, string password);
        void LogOut();
        void Register(string username, string email, string firstName, string lastName, string password, string confirmPassword);
    }
}