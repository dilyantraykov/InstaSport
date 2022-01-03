using InstaSport.Data.Common;
using InstaSport.Data.Models;
using InstaSport.Services.Data.Constants;
using InstaSport.Services.Data.Exceptions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaSport.Services.Data
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IDbRepository<User> users;
        private readonly IPasswordHasher<User> passwordHasher;

        public AuthenticationService(IDbRepository<User> users, IPasswordHasher<User> passwordHasher)
        {
            this.users = users;
            this.passwordHasher = passwordHasher;

            if (!this.users.All().Any())
            {
                this.Register("dtraykov", "dtraykov94@gmail.com", "123456", "123456");
            }
        }

        public User Login(string identicator, string password)
        {
            var user = this.GetByEmail(identicator);

            if (user == null)
            {
                user = this.GetByUserName(identicator);
            }

            if (user == null)
            {
                throw new ArgumentException("User not found!");
            }

            PasswordVerificationResult passwordResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            if (passwordResult != PasswordVerificationResult.Success)
            {
                throw new ArgumentException("Password is invalid!");
            }

            return user;
        }

        public User Register(string username, string email, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                throw new InvalidPropertyException(StringConstants.Password, "Passwords do not match!");
            }

            var user = this.GetByUserName(username);
            if (user != null)
            {
                throw new InvalidPropertyException(StringConstants.UserName, "This username is already registered!");
            }

            user = this.GetByEmail(email);
            if (user != null)
            {
                throw new InvalidPropertyException(StringConstants.Email, "This email is already registered!");
            }

            string hashedPassword = passwordHasher.HashPassword(user, password);

            user = new User()
            {
                UserName = username,
                Email = email,
                PasswordHash = hashedPassword,
                DateJoined = DateTime.Now
            };

            this.users.Add(user);
            this.users.Save();

            return user;
        }

        private User? GetByEmail(string email)
        {
            return this.users.All().FirstOrDefault(x => x.Email == email);
        }

        private User? GetByUserName(string userName)
        {
            return this.users.All().FirstOrDefault(x => x.UserName == userName);
        }
    }
}
