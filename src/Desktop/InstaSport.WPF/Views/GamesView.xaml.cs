﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace InstaSport.WPF.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class GamesView : UserControl
    {
        public GamesView()
        {
            InitializeComponent();

            this.GamesCarousel.Loaded += (s, e) =>
            {
                var panel = this.GamesCarousel.FindCarouselPanel();
                if (this.GamesCarousel.Items.Count < panel.ItemsPerPage)
                {
                    panel.ItemsPerPage = this.GamesCarousel.Items.Count;
                }
            };
        }
    }
}
