using InstaSport.WPF.ViewModels;
using Prism.Mvvm;

namespace InstaSport.WPF.Models
{
    public class NavigationItem
    {
        public NavigationItem(string title, string iconGlyph, BindableBase viewModel)
        {
            this.Title = title;
            this.IconGlyph = iconGlyph;
            this.ViewModel = viewModel;
        }

        public string Title { get; set; }
        public string IconGlyph { get; set; }
        public BindableBase ViewModel { get; set; }
    }
}
