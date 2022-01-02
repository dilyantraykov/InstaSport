using InstaSport.Services.Data;
using InstaSport.WPF.State;
using Prism.Mvvm;
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
        private readonly IAuthenticator authenticator;
        private string email;

        public string Email
        {
            get { return this.email; }
            set
            {
                if (this.email != value)
                {
                    this.email = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel(IAuthenticator authenticator)
        {
            this.authenticator = authenticator;
            this.LoginCommand = new DelegateCommand(OnLogin);
        }

        private void OnLogin(object obj)
        {
            var success = this.authenticator.LogIn(this.email, obj.ToString());
        }
    }
}
