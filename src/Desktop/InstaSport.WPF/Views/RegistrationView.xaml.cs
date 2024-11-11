using InstaSport.Services.Data.Exceptions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace InstaSport.WPF.Views
{
    /// <summary>
    /// Interaction logic for LocationsViewModel.xaml
    /// </summary>
    public partial class RegistrationView : UserControl
    {
        public ICommand RegisterCommand
        {
            get { return (ICommand)GetValue(RegisterCommandProperty); }
            set { SetValue(RegisterCommandProperty, value); }
        }

        public static readonly DependencyProperty RegisterCommandProperty =
            DependencyProperty.Register("RegisterCommand", typeof(ICommand), typeof(RegistrationView), new PropertyMetadata(null));

        public RegistrationView()
        {
            InitializeComponent();
            this.SetBinding(RegistrationView.RegisterCommandProperty, new Binding("RegisterCommand"));
        }

        private void OnRegisterClick(object sender, System.Windows.RoutedEventArgs e)
        {
            var passwords = new Tuple<string, string>(this.PasswordBox.Password, this.ConfirmPasswordBox.Password);
            this.RegisterCommand.Execute(passwords);
        }
    }
}
