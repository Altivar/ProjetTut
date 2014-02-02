using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Surface.Presentation.Controls;

namespace AppliProjetTut
{
    /// <summary>
    /// Logique d'interaction pour MenuCreation.xaml
    /// </summary>
    public partial class MenuCreation : ScatterViewItem
    {


        // surface window parent
        SurfaceWindow1 parentWindow;

        public MenuCreation(SurfaceWindow1 parent)
        {
            InitializeComponent();

            parentWindow = parent;
        }

        private void CloseMenuClick(object sender, RoutedEventArgs e)
        {
            parentWindow.MenuIsClicked(this, "Close");
        }

        private void TextButtonClick(object sender, RoutedEventArgs e)
        {
            parentWindow.MenuIsClicked(this, "Text");
        }
    }
}
