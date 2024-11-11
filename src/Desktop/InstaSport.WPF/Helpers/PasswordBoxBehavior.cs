using System.Security;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace InstaSport.WPF.Helpers
{
    public static class PasswordBoxBehavior
    {
        public static readonly DependencyProperty AttachProperty =
            DependencyProperty.RegisterAttached("Attach",
            typeof(bool), typeof(PasswordBoxBehavior), new PropertyMetadata(false, Attach));

        public static void SetAttach(DependencyObject dp, bool value)
        {
            dp.SetValue(AttachProperty, value);
        }

        public static bool GetAttach(DependencyObject dp)
        {
            return (bool)dp.GetValue(AttachProperty);
        }

        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached("BoundPassword", typeof(SecureString), typeof(PasswordBoxBehavior), new PropertyMetadata(null, OnBoundPasswordChanged));

        public static SecureString GetBoundPassword(DependencyObject d)
        {
            return (SecureString)d.GetValue(BoundPasswordProperty);
        }

        public static void SetBoundPassword(DependencyObject d, SecureString value)
        {
            d.SetValue(BoundPasswordProperty, value);
        }


        private static void Attach(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            RadPasswordBox passwordBox = sender as RadPasswordBox;

            if (passwordBox == null)
                return;

            if ((bool)e.OldValue)
            {
                passwordBox.PasswordChanged -= OnPasswordChanged;
            }

            if ((bool)e.NewValue)
            {
                passwordBox.PasswordChanged += OnPasswordChanged;
            }
        }

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RadPasswordBox passwordBox)
            {
                passwordBox.PasswordChanged -= OnPasswordChanged;
                passwordBox.PasswordChanged += OnPasswordChanged;
            }
        }

        private static void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is RadPasswordBox passwordBox)
            {
                SetBoundPassword(passwordBox, passwordBox.SecurePassword);
            }
        }
    }
}
