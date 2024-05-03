using InstaSport.Services.Data.Exceptions;
using InstaSport.WPF.Models;
using InstaSport.WPF.State;
using InstaSport.WPF.Views;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

        [EmailAddress(ErrorMessage = "Email address is invalid!")]
        public string Email
        {
            get { return this.email; }
            set
            {
                if (this.email != value)
                {
                    ClearErrors(nameof(Email));
                    Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = nameof(Email) });
                    this.SetProperty(ref this.email, value);
                }
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
            var passwords = obj as Tuple<string, string>;
            try
            {
                this.authenticator.Register(this.UserName, this.Email, this.FirstName, this.LastName, passwords.Item1, passwords.Item2);
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
                AddError(nameof(UserName), "Username cannot be empty.");
            if (string.Equals(name, "Admin", StringComparison.OrdinalIgnoreCase))
                AddError(nameof(UserName), "Admin is not valid username.");
            if (name == null || name?.Length <= 3)
                AddError(nameof(UserName), "Username must be at least 4 characters long.");
        }
    }
}
