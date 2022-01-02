using InstaSport.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaSport.Services.Data
{
    public interface IAuthenticationService
    {
        User Login(string username, string password);
        User Register(string username, string email, string password, string confirmPassword);
    }
}
