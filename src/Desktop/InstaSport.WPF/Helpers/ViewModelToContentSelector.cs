using InstaSport.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace InstaSport.WPF.Helpers
{
    public class ViewModelToContentSelector : DataTemplateSelector
    {
        public DataTemplate GamesTemplate { get; set; }
        public DataTemplate SportsTemplate { get; set; }
        public DataTemplate LoginTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is SportsViewModel)
            {
                return this.SportsTemplate;
            }
            else if (item is LoginViewModel)
            {
                return this.LoginTemplate;
            }
            else if (item is GamesViewModel)
            {
                return this.GamesTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
