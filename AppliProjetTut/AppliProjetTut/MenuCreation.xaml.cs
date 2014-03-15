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

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="parent"></param>
        public MenuCreation(SurfaceWindow1 parent)
        {
            InitializeComponent();

            parentWindow = parent;
        }

        /// <summary>
        /// Ferme le menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseMenuClick(object sender, RoutedEventArgs e)
        {
            parentWindow.MenuIsClicked(this, "Close");
        }

        /// <summary>
        /// Crée un NodeText
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextButtonClick(object sender, RoutedEventArgs e)
        {
            parentWindow.MenuIsClicked(this, "Text");
        }

        /// <summary>
        /// Crée un NodeImage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageButtonClick(object sender, RoutedEventArgs e)
        {
            parentWindow.MenuIsClicked(this, "Image");
        }
    }
}
