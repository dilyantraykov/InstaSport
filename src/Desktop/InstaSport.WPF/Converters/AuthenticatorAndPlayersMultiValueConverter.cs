using InstaSport.Data.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace InstaSport.WPF.Converters
{
    public class AuthenticatorAndPlayersMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
                return Visibility.Collapsed;

            var currentUser = values[0] as User;
            var players = values[1] as IList<UserDto>;

            if (currentUser == null || players == null)
                return Visibility.Collapsed;

            return players.Any(p => p.Id == currentUser.Id) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
