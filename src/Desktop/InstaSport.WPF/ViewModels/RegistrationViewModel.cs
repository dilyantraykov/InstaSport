using InstaSport.Services.Data.Exceptions;
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
    public class RegistrationViewModel : BindableBase, INotifyDataErrorInfo
    {
        private readonly IRegionManager regionManager;
        private readonly IAuthenticator authenticator;

        private string username;
        private string email;
        private readonly Dictionary<string, List<string>> errorsByPropertyName = new Dictionary<string, List<string>>();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public string UserName
        {
            get { return this.username; }
            set
            {
                this.ValidateUserName();
                this.SetProperty(ref this.username, value);
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
                    Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = nameof(Email) });
                    this.SetProperty(ref this.email, value);
                }
            }
        }

        public DelegateCommand RegisterCommand { get; }

        public bool HasErrors => errorsByPropertyName.Any();

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
                this.authenticator.Register(this.UserName, this.Email, passwords.Item1, passwords.Item2);
                this.regionManager.RequestNavigate("MainRegion", nameof(GamesView));
            }
            catch (InvalidPropertyException ex)
            {
                this.AddError(ex.Property, ex.Message);
            }
        }

        private void ValidateUserName()
        {
            ClearErrors(nameof(UserName));
            if (string.IsNullOrWhiteSpace(UserName))
                AddError(nameof(UserName), "Username cannot be empty.");
            if (string.Equals(UserName, "Admin", StringComparison.OrdinalIgnoreCase))
                AddError(nameof(UserName), "Admin is not valid username.");
            if (UserName == null || UserName?.Length <= 5)
                AddError(nameof(UserName), "Username must be at least 6 characters long.");
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            return errorsByPropertyName.ContainsKey(propertyName) ? errorsByPropertyName[propertyName] : null;
        }

        private void OnErrorsChanged(string propertyName)
        {
            this.RaisePropertyChanged(nameof(HasErrors));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private void AddError(string propertyName, string error)
        {
            if (!errorsByPropertyName.ContainsKey(propertyName))
                errorsByPropertyName[propertyName] = new List<string>();

            if (!errorsByPropertyName[propertyName].Contains(error))
            {
                errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        private void ClearErrors(string propertyName)
        {
            if (errorsByPropertyName.ContainsKey(propertyName))
            {
                errorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }
    }
}
