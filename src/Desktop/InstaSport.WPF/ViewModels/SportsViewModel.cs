using InstaSport.Data.Models;
using InstaSport.Services.Data;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using Telerik.Windows.Controls;

namespace InstaSport.WPF.ViewModels
{
    public class SportsViewModel : BindableBase
    {
        private ISportsService sportsService;
        private ObservableCollection<Sport> sports;

        public ObservableCollection<Sport> Sports
        {
            get { return this.sports; }
        }

        public SportsViewModel(ISportsService sportsService)
        {
            this.sportsService = sportsService;
            this.sports = new ObservableCollection<Sport>(this.sportsService.GetAll());
        }
    }
}
