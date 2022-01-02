using InstaSport.Data.Models;

namespace InstaSport.WPF.State
{
    public interface IAuthenticator
    {
        User CurrentUser { get; }
        bool IsLoggedIn { get; }

        bool LogIn(string username, string password);
        void LogOut();
        void Register(string username, string email, string password, string confirmPassword);
    }
}