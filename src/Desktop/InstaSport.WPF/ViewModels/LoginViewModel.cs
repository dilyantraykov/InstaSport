using InstaSport.Services.Data;
using InstaSport.WPF.State;
using InstaSport.WPF.Views;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace InstaSport.WPF.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;
        private readonly IAuthenticator authenticator;
        private string email;

        public string Email
        {
            get { return this.email; }
            set
            {
                this.SetProperty(ref this.email, value);
            }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel(IRegionManager regionManager, IAuthenticator authenticator)
        {
            this.regionManager = regionManager;
            this.authenticator = authenticator;
            this.LoginCommand = new DelegateCommand(OnLogin);
        }

        private void OnLogin(object obj)
        {
            var success = this.authenticator.LogIn(this.email, obj.ToString());
            if (success)
            {
                this.regionManager.RequestNavigate("MainRegion", nameof(GamesView));
            }
        }
    }
}
