using InstaSport.WPF.ViewModels;
using Prism.Mvvm;

namespace InstaSport.WPF.Models
{
    public class NavigationItem : BindableBase
    {
        private bool isVisible;

        public NavigationItem(string title, string iconGlyph, string view)
        {
            this.Title = title;
            this.IconGlyph = iconGlyph;
            this.View = view;
            this.IsVisible = true;
        }

        public string Title { get; set; }
        public string IconGlyph { get; set; }
        public string View { get; set; }

        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                this.SetProperty(ref isVisible, value);
            }
        }
    }
}
