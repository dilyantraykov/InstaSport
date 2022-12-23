using InstaSport.Data.Models;
using InstaSport.Services.Data;
using InstaSport.WPF.State;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace InstaSport.WPF.ViewModels
{
    public class UserDetailsViewModel : BindableBase, INavigationAware
    {
        private readonly IAuthenticator authenticator;
        private readonly IAuthenticationService authenticationService;
        private User user;

        public User User
        {
            get { return this.user; }
            set
            {
                this.SetProperty(ref this.user, value);
                this.RaisePropertyChanged(nameof(IsRatingEnabled));
            }
        }

        public bool IsRatingEnabled { get { return this.authenticator.CurrentUser != this.User; } }

        public ICommand RatedCommand { get; }

        public UserDetailsViewModel(IAuthenticationService authenticationService, IAuthenticator authenticator)
        {
            this.authenticationService = authenticationService;
            this.authenticator = authenticator;
            this.RatedCommand = new DelegateCommand(OnRated);
        }

        private void OnRated(object obj)
        {
            var args = obj as RoutedPropertyChangedEventArgs<double?>;
            this.authenticationService.Rate(User, this.authenticator.CurrentUser.Id, (int)args.NewValue);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var username = (string)navigationContext.Parameters["Username"];
            this.User = this.authenticationService.GetByUserName(username);
        }
    }
}
